using System;
using System.Collections.Generic;
using System.IO;
using Common;

namespace Interpreter
{
	//
	// IContents for Language constructs
	//
	internal class Branch<T> : INode<T> where T : IContent
	{
		public override bool Eval(T content)
		{ 
			return Content.Eval(content);
		}
	}

	internal class Result<T> : INode<T> where T : IContent
	{
		public override bool Eval(T content) { return true; }
	}

	internal class Goto<T> : INode<T> where T : IContent
	{
		public Goto(Id toid)
		{
			Next = toid;
		}

		public Id Next { get; set; }

		public override bool Eval(T content) { return true; }
	}

	//
	// Scripting host
	//
	public delegate void ScriptHostHandler(object sender, Id parentId, Id id, bool parentBranch);
	public delegate void NodeHandler(object sender, int level, Id id);

	public enum NodeType {NONE, Branch, Result, Goto};

	public class ScriptHost<T> where T : IContent
	{
		private List<ContentTree<T>> Trees;
		private bool Dirty;
		private const int Capacity = 256;

		public ScriptHost()
		{
			// init trees
			Trees = new List<ContentTree<T>>();

			// allocate the first nodes
			Trees.Add(new ContentTree<T>(Capacity, 0));

			// load defaults
			Dirty = true;
			IsError = false;
			ErrorInfo = Id.Identity;
		}

		//
		// Private helpers
		//

		// return the corresponding Tree for this level
		private ContentTree<T> GetTree(Id id)
		{
			if (id.Level < 0 || id.Level >= Trees.Count) return default(ContentTree<T>);
			else return Trees[ id.Level ];
		}

		private Id TryGetNextHead(Id id)
		{
			ContentTree<T> tree;
			Id tmpId;

			// return the head of the next level
			tmpId = new Id(id.Index, id.Level+1);
			tree = GetTree(tmpId);
			if (tree != default(ContentTree<T>)) return tree.Head;
			else return Id.Identity;
		}

		//
		// Public surface area
		//

		// load and store
		public void Load(Stream stream) { throw new NotImplementedException(); }
		public Stream Store() { throw new NotImplementedException(); }

		// execution
		public T Execute(T content)
		{
			// start at head
			Reset();

			// execute until reach a result or gotonext
			while (!IsDone()) Step(content);

			// return the final result
			return GetResult();
		}

		// debug APIs
		public void Reset()
		{
			Current = Head;

			// verify first
			if (!Verify())
			{
				throw new Exception("Error at node " + ErrorInfo.Index);
			}
		}

		public void Step(T content)
		{
			if (!ContentTree<T>.IsValid(Current)) throw new Exception("Invalid current node: " + Current.Index);

			if (GetTree(Current)[Current].GetType() == typeof(Result<T>)) throw new Exception("Unexpected node type " + typeof(Result<T>));

			// advance current
			if (GetTree(Current)[Current].GetType() == typeof(Goto<T>))
			{
				Current = (GetTree(Current)[Current] as Goto<T>).Next;
			}
			else if (GetTree(Current)[Current].Eval(content))
			{
				// move to TRUE
				Current = GetTree(Current)[Current].RightBranch;
			}
			else
			{
				// move to FALSE
				Current = GetTree(Current)[Current].LeftBranch;
			}
		}
		public Id Current { get; set; }
		public bool IsDone()
		{
			if (!ContentTree<T>.IsValid(Current)) throw new Exception("Invalid current node: " + Current.Index);

			return (GetTree(Current)[Current].GetType() == typeof(Result<T>));
		}

		public T GetResult()
		{
			if (!IsDone()) throw new Exception("Invalid access to a non-completed execution");

			return GetTree(Current)[Current].Content;
		}

		// results
		public Id Head
		{
			get
			{
				return Trees[0].Head; // true head
			}
		}
		public Id GetNextHead(Id head)
		{
			Id nextHead;
			ContentTree<T> newTree;

			// grab the next head down
			nextHead = TryGetNextHead(head);

			if (nextHead.Equals(Id.Identity))
			{
				// allocate a new Tree
				newTree = new ContentTree<T>(Capacity, head.Level+1);
				nextHead = newTree.Head;
				Trees.Add(newTree);

				// verify that the inserted index matches the level
				if (Trees[head.Level+1].Head.Level != nextHead.Level) throw new Exception("Invalid insert into Trees list: Expected(" + nextHead.Level + ") Actual(" + Trees[head.Level+1].Head.Level + ")");
			}

			return nextHead;
		}
		public bool IsError { get; private set; }
		public Id ErrorInfo { get; private set; }

		private INode<T> GetNode(Id id)
		{
			return GetTree(id)[id];
		}

		public T GetContent(Id id)
		{ 
			if (!ContentTree<T>.IsValid(id)) throw new Exception("Invalid id passed to GetContent: " + id.Index);
			return GetNode(id).Content;
		}

		public void SetContent(Id id, T content)
		{
			if (!ContentTree<T>.IsValid(id)) throw new Exception("Invalid id passed to SetContent: " + id.Index);
			GetNode(id).Content = content;
		}

		public T ClearContent(Id id)
		{
			T content;

			if (!ContentTree<T>.IsValid(id)) throw new Exception("Invalid id passed to ClearContent: " + id.Index);
			
			content = GetContent(id);
			SetContent(id, default(T));

			return content;
		}

		public NodeType GetNodeType(Id id)
		{
			if (GetNode(id).GetType() == typeof(Branch<T>)) return NodeType.Branch;
			else if (GetNode(id).GetType() == typeof(Result<T>)) return NodeType.Result;
			else if (GetNode(id).GetType() == typeof(Goto<T>)) return NodeType.Goto;
			else throw new Exception("Invalid NodeType: " + GetNode(id).GetType());
		}

		public Id GetGotoDestination(Id id)
		{
			if (GetNodeType(id) != NodeType.Goto) return Id.Identity;
			return (GetNode(id) as Goto<T>).Next;
		}

		public Id GetTrueBranch(Id id)
		{
			return GetTree(id)[id].RightBranch;
		}

		public Id GetFalseBranch(Id id)
		{
			return GetTree(id)[id].LeftBranch;
		}

		// contruction
		public event ScriptHostHandler OnInsert;
		public event ScriptHostHandler OnRemove;

		public Id InsertBranch(Id id)
		{
			return Insert(id, new Branch<T>());
		}

		public Id InsertResult(Id id)
		{
			return Insert(id, new Result<T>());
		}

		public Id InsertGoto(Id id, Id toid)
		{
			return Insert(id, new Goto<T>(toid));
		}

		private Id Insert(Id id, INode<T> content)
		{
			if (content == default(INode<T>)) throw new Exception("Cannot send in default content");
			
			// insert
			InsertAndNotify(id, content);

			// add the appropriate branches
			if (content.GetType() == typeof(Branch<T>))
			{
				Id tmpId;

				// add the true branches
				tmpId = GetTree(id).GetNext();
				content.RightBranch = tmpId;
				InsertGoto(tmpId, GetNextHead(tmpId));


				// add the false branch
				tmpId = GetTree(id).GetNext();
				content.LeftBranch = tmpId;
				InsertGoto(tmpId, GetNextHead(tmpId));
			}

			return id;
		}

		private void InsertAndNotify(Id id, INode<T> content)
		{
			// invalidate verify
			Dirty = true;

			// find parent info - VERY EXPENSIVE!
			Id p = GetTree(id).FindParent(id);
			if (!p.Equals(Id.Identity))
			{
				content.Parent = p;
				content.ParentBranch = GetTree(p)[p].RightBranch.Equals(id);
			}

			// insert
			GetTree(id).Insert(id, content);

			if (OnInsert != null) OnInsert(this, content.Parent, id, content.ParentBranch);
		}

		public void Remove(Id id)
		{ 
			Stack<Id> removals = new Stack<Id>();

			removals.Push(id);

			// remove the whole subtree
			while(removals.Count > 0)
			{
				Id tmpId = removals.Pop();

				if (ContentTree<T>.IsValid(tmpId))
				{
					Id parentId = Id.Identity;
					bool parentBranch = false;

					// push children
					if (GetTree(tmpId)[tmpId] != default(INode<T>))
					{
						removals.Push(GetTree(tmpId)[tmpId].RightBranch);
						removals.Push(GetTree(tmpId)[tmpId].LeftBranch);

						// capture parent info
						parentId = GetTree(tmpId)[tmpId].Parent;
						parentBranch = GetTree(tmpId)[tmpId].ParentBranch;
					}
	
					// invalidate verify
					Dirty = true;

					// delete
					GetTree(tmpId).Delete(tmpId);
	
					if (OnRemove != null) OnRemove(this, parentId, tmpId, parentBranch);
				}
			}
		}

		public bool Verify()
		{
			Id retId;
			bool gotonext;
			Id gotoId;
			Id headId;

			// Rule any tree with a Gotonext must have 

			// exit early if we have checked since the last add/remove
			if (!Dirty) return !IsError;

			// verify one tree at a time
			//  continue as long as there is a reference to a GOTONEXT and there is no error
			headId = Head;
			do
			{
				gotoId = Id.Identity;
				gotonext = false;

				// verify tree
				retId = Verify(headId, ref gotonext, ref gotoId);

				// store the results
				IsError = !retId.Equals(Id.Identity);
				ErrorInfo = retId;

				// check if there is a next tree to verify
				if (!IsError && gotonext)
				{
					// grab the next head
					headId = TryGetNextHead(headId);

					if (headId.Equals(Id.Identity))
					{
						// any tree with a gotonext must have another tree to back it up
						IsError = true;
						ErrorInfo = gotoId;
					}
				}
			}
			while(gotonext && !IsError);

			// once verified it is no longer Dirty (even if there is an error)
			Dirty = false;

			// true means it is verifiable
			return !IsError;
		}

		private Id Verify(Id id, ref bool gotonext, ref Id gotoId)
		{
			// The RULES
			//  All Result,Gotonext nodes must have Identify children
			//  All Branch nodes must have 2 non-Identity children
			//  All Result,Branch must have content
			//  Empty trees are not valid
			//  All nodes must verify

			if (id.Equals(Id.Identity))
			{
				return Id.Identity;
			}
			else if (GetNode(id) == null || GetNode(id) == default(INode<T>))
			{
				return id; // empty trees are not valid
			}
			else
			{
				if (GetNode(id).GetType() == typeof(Branch<T>))
				{
					if (GetTrueBranch(id).Equals(Id.Identity)
						|| GetFalseBranch(id).Equals(Id.Identity)
						|| GetContent(id) == null
						|| !GetContent(id).Verify())
					{
						return id;
					}
				}
				else if (GetNode(id).GetType() == typeof(Result<T>))
				{
					if (!GetTrueBranch(id).Equals(Id.Identity)
						|| !GetFalseBranch(id).Equals(Id.Identity)
						|| GetContent(id) == null
						|| !GetContent(id).Verify())
					{
						return id;
					}
				}
				else if (GetNode(id).GetType() == typeof(Goto<T>))
				{
					// indicate next if goto level is not the same as the current id
					gotonext = true;
					if (id.Level >= (GetNode(id) as Goto<T>).Next.Level) gotonext = false;
					gotoId = id;

					if (!GetTrueBranch(id).Equals(Id.Identity)
						|| !GetFalseBranch(id).Equals(Id.Identity))
					{
						return id;
					}
				}
				else
				{
					throw new Exception("Unknown INode type: " + GetNode(id).GetType());
				}
				
				Id nId;
				
				// check the True side
				nId = Verify(GetTrueBranch(id), ref gotonext, ref gotoId);
				if (!nId.Equals(Id.Identity)) return nId;

				// check the False side
				return Verify(GetFalseBranch(id), ref gotonext, ref gotoId);
			}
		}

		// depth first search
		public event NodeHandler OnSearch;

		public void DepthSearch()
		{
			for(int i=0; i<Trees.Count; i++)
			{
				DepthSearch(Trees[i].Head, 0);
			}
		}

		private void DepthSearch(Id id, int level)
		{
			if (id.Equals(Id.Identity))
			{

			}
			else if (GetTree(id)[id] == default(INode<T>))
			{

			}
			else
			{
				if (OnSearch != null) OnSearch(this, level, id);

				DepthSearch(GetTree(id)[id].RightBranch, level+1);
				DepthSearch(GetTree(id)[id].LeftBranch, level+1);
			}
		}

		// debug
		public void DEBUG_Print()
		{
			for(int i=0; i<Trees.Count; i++)
			{
				Trees[i].DEBUG_PrintTree();
			}
		}
	}

	//
	// Expression host
	//

	internal class ExprNode<T> where T : IContent
	{
		public BooleanOp Bop { get; set; }
		public ExpressionOp Eop { get; set; }
		public T Content { get; set; }
		public Id Group { get; set; }

		public ExprNode()
		{
			Bop = BooleanOp.NONE;
			Eop = ExpressionOp.NONE;
			Content = default(T);
			Group = Id.Identity;
		}
	}

	public enum BooleanOp {NONE, AND, OR};
	public enum ExpressionOp {NONE, EQ, NE};

	public class ExpressionTree<T> where T : IContent
	{
		private List<ExprNode<T>> Tree;
		private bool Dirty;
		private int CurrentGroup;
		private int Index;

		public ExpressionTree()
		{
			Tree = new List<ExprNode<T>>();
			
			// clear enumeration
			Reset();

			// start fresh
			Clear();
		}

		public void Reset()
		{
			Index = 0;
		}

		public bool Enumerate(out BooleanOp bop, out ExpressionOp eop, out T content)
		{
			// set defaults
			bop = BooleanOp.NONE;
			eop = ExpressionOp.NONE;
			content = default(T);

			if (Index >= Tree.Count) return false;

			// find next suitable expression
			while(Index < Tree.Count && Tree[Index] == null) Index++;

			// return details
			bop = GetNode(Index).Bop;
			eop = GetNode(Index).Eop;
			content = GetNode(Index).Content;

			// advance
			Index++;

			return true;
		}

		private void Validate(Id id)
		{
			if (id.Index < 0 || id.Index >= Tree.Count) throw new Exception("Invalid index: Min(0) Max(" + Tree.Count + ") Actual(" + id.Index + ")");
		}

		private ExprNode<T> GetNode(Id id)
		{
			return GetNode(id.Index);
		}

		private ExprNode<T> GetNode(int index)
		{
			return Tree[index];
		}

		private Id GetNextGroup()
		{
			return new Id(CurrentGroup++);
		}

		// load and store
		public void Load(Stream stream) { throw new NotImplementedException(); }
		public Stream Store() { throw new NotImplementedException(); }

		// evaluate
		public bool Eval(T content)
		{
			Dictionary<int, Id> map;
			Dictionary<Id, bool> results;
			int group;
			Id curgroup;
			Id prevgroup;
			ExprNode<T> node;
			bool result;

			if (!Verify()) throw new Exception("Failed to verify");	

			map = new Dictionary<int, Id>();
			results = new Dictionary<Id, bool>();
			group = CurrentGroup+1;

			// 2 phases
			//  Phase 1 - evaluate and assign groups (of 1)
			//  Phase 2 - combine		
			
			// Phase 1
			curgroup = Id.Identity;
			for(int i=0; i<Tree.Count; i++)
			{
				node = GetNode(i);

				if (node != null)
				{
					// assign group
					if (node.Group == Id.Identity) curgroup = new Id(group++);
					else curgroup = node.Group;

					// associate group
					map.Add(i, curgroup);

					// evaluate
					result = node.Content.Eval(content);

					// flip the result for NE
					if (node.Eop == ExpressionOp.NE) result = !result;

					// store the result
					if (!results.ContainsKey(curgroup)) 
					{
						results.Add(curgroup, result);
					}
					else
					{
						switch (node.Bop)
						{
							case BooleanOp.AND: results[curgroup] = results[curgroup] && result; break;
							case BooleanOp.OR: results[curgroup] = results[curgroup] || result; break;
							default: throw new Exception("Unknown boolean expression: " + node.Bop);
						}
					}
				}
			}

			// Phase 2
			prevgroup = Id.Identity;
			result = results[curgroup];

			for(int i=0; i<Tree.Count; i++)
			{
				node = GetNode(i);

				if (node != null)
				{
					// grab current group
					curgroup = map[i];

					// combine results
					if (prevgroup != Id.Identity && curgroup != prevgroup)
					{
						switch (node.Bop)
						{
							case BooleanOp.AND: result = results[curgroup] && results[prevgroup] && result; break;
							case BooleanOp.OR: result = results[curgroup] || results[prevgroup] || result; break;
							default: throw new Exception("Unknown boolean expression: " + node.Bop);
						}
					}

					// store previous
					prevgroup = curgroup;
				}
			}

			return result;
		}

		// prepare for eval

		public bool Verify()
		{
			int numverified = 0;

			// Empty is not valid
			// All ScratchNode's must have BooleanOp, ExpressionOp, and Content
			//  EXCEPT the first one should not have a BooleanOp

			if (!Dirty) return !IsError;

			for(Id id = new Id(0); id.Index<Tree.Count; id.Index++)
			{
				ExprNode<T> node = GetNode(id);

				if (node != null)
				{
					numverified++;

					// check for the validity of the node
					if ((node.Bop == BooleanOp.NONE && numverified > 1)
						|| node.Eop == ExpressionOp.NONE
						|| node.Content == null
						|| !node.Content.Verify())
					{
						IsError = true;
						ErrorInfo = id;
						return false;
					}
				}
			}

			// empty sets are not valid
			if (numverified == 0)
			{
				IsError = true;
				ErrorInfo = new Id(0);
				return false;
			}

			// set that this has been verified
			IsError = false;
			ErrorInfo = Id.Identity;
			Dirty = false;

			return !IsError;
		}

		// information
		public bool IsError { get; private set; }
		public Id ErrorInfo { get; private set; }

		// creation
		public void Clear()
		{
			Tree.Clear();
			Dirty = true;
			CurrentGroup = 0;
		}

		public Id InsertRow() // at end
		{
			ExprNode<T> node = new ExprNode<T>();
			int index;

			Tree.Add(node);
			index = Tree.Count-1;

			Dirty = true;

			return new Id(index);
		}

		public void DeleteRow(Id id)
		{
			Validate(id);

			// IMPORTANT: To retain the ordering of the other rows this line must just be null'd out and not removed from the list
			Tree[id.Index] = null;

			Dirty = true;
		}

		public bool Group(Id head, Id tail)
		{
			Id group;

			Validate(head);
			Validate(tail);

			// validate
			if (head.Index >= tail.Index
				|| GetNode(head) == null
				|| GetNode(tail) == null)
			{
				return false;
			}

			// additional validation
			for(Id id=new Id(head.Index); id.Index<=tail.Index; id.Index++)
			{
				if (GetNode(id) != null && GetNode(id).Group != Id.Identity) return false;
			}

			// set group
			group = GetNextGroup();
			for(Id id=new Id(head.Index); id.Index<=tail.Index; id.Index++)
			{
				if (GetNode(id) != null) GetNode(id).Group = group;
			}

			Dirty = true;

			return true;
		}

		public bool Ungroup(Id head, Id tail)
		{
			Id group;

			Validate(head);
			Validate(tail);

			// validate
			if (head.Index >= tail.Index
				|| GetNode(head) == null
				|| GetNode(tail) == null)
			{
				return false;
			}

			// additional validation
			group = GetNode(head).Group;
			for(Id id=new Id(head.Index); id.Index<=tail.Index; id.Index++)
			{
				if (GetNode(id) != null && GetNode(id).Group != group) return false;
			}

			// ungroup
			for(Id id=new Id(head.Index); id.Index<=tail.Index; id.Index++)
			{
				if (GetNode(id) != null) GetNode(id).Group = Id.Identity;
			}

			Dirty = true;

			return true;
		}

		public bool SetBooleanOp(Id id, BooleanOp bo)
		{
			Validate(id);

			if (GetNode(id) == null) throw new Exception("Invalid null node: " + id.Index);

			GetNode(id).Bop = bo;

			Dirty = true;

			return true;
		}

		public bool SetExpressionOp(Id id, ExpressionOp eo)
		{
			Validate(id);

			if (GetNode(id) == null) throw new Exception("Invalid null node: " + id.Index);

			GetNode(id).Eop = eo;

			Dirty = true;

			return true;
		}

		public bool SetContent(Id id, T content)
		{
			Validate(id);

			if (GetNode(id) == null) throw new Exception("Invalid null node: " + id.Index);

			GetNode(id).Content = content;

			Dirty = true;

			return true;
		}

        public T GetContent(Id id)
        {
            Validate(id);

            if (GetNode(id) == null) throw new Exception("Invalid null node: " + id.Index);

            Dirty = true; // cannot tell if this will be manipulated

            return GetNode(id).Content;
        }
	}
}
