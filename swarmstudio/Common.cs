using System;
using System.Collections.Generic;

namespace Common
{
	public interface IContent
	{
		bool Eval(IContent c);
		bool Verify();
	}

	internal abstract class INode<T> where T : IContent
	{
		public INode()
		{
			Parent = Id.Identity;
			ParentBranch = false;
			RightBranch = Id.Identity;
			LeftBranch = Id.Identity;
			Content = default(T);
		}

		public T Content { get; set; }
		public Id Parent { get; set; }
		public bool ParentBranch { get; set; }
		public Id RightBranch { get; set; }
		public Id LeftBranch { get; set; }
		public virtual bool Eval(T content) { return false; }
	}

	public class Id
	{
		public Id(int i) { Index = i; Level = 0; }

		public Id(int i, int l) { Index = i; Level = l; }

		public int Index { get; set; }
		public int Level { get; set; }
		public long Hash
		{
			get
			{
				return ((long)Level << 32) + (long)Index;
			}
		}

		public static Id Identity = new Id(-1, -1);

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		// this must be overridden to get the right semantics for Stack.Contains (which uses .Equals for comparison)
		public override bool Equals(object id)
		{
			return ((Index == (id as Id).Index) && (Level == (id as Id).Level));
		}

		public override string ToString()
		{
			return "Level: " + Level + " Index: " + Index;
		}
	}

	internal class ContentTree<T> where T : IContent
	{
		private INode<T>[] Data;
		private Stack<Id> Freelist;
		private int CurrentIndex;	// next free spot
		private int Capacity;		// default size of array (and increment for growth)
		private int Level;

		public event EventHandler OnGrow;
		public event EventHandler OnReset;
		
		public ContentTree(int capacity, int level)
		{
			Data      = new INode<T>[capacity];
			Freelist  = new Stack<Id>();
			CurrentIndex     = 1;	// Head remains allocated
			Capacity = capacity;
			Level = level;
			Head = new Id(0, level);
		}

		public Id Head { get; private set; }

		public INode<T> this[Id id]
		{
			get
			{
				int index = id.Index;
				if (index < 0 || index >= Data.Length) throw new Exception("Invalid index: " + index);
				return Data[index];
			}
			set
			{
				int index = id.Index;
				if (index < 0 || index >= Data.Length) throw new Exception("Invalid index: " + index);
				Data[index] = value;
			}	
		}

		public Id GetNext()
		{
			// check the free list first
			if (Freelist.Count > 0)
			{
				return Freelist.Pop();
			}

			// return the current Index and add 1
			return new Id(CurrentIndex++, Level);
		}

		public bool Insert(Id id, INode<T> content)
		{
			// grow array
			if (id.Index >= Data.Length)
			{
				// create new
				INode<T>[] newData = new INode<T>[ Data.Length + Capacity ];
				// Copy content
				Array.Copy(Data, newData, Data.Length);
				// swap
				Data = newData;
				
				// fire event
				if (OnGrow != null) OnGrow(this, null);
			}

			// sanity checks
			if (!IsValid(id)) throw new Exception("Invalid indexer: " + id.Index);

			// additional sanity checks
			if (this[id] != default(INode<T>)
				&& 
				(!this[id].RightBranch.Equals(Id.Identity)
				||
				!this[id].LeftBranch.Equals(Id.Identity))
			)
			{
				throw new Exception("Added a new item on top of an existing item: " + id.Index);
			}

			// insert this element
			this[id] = content;

			return true;
		}

		public bool Delete(Id id)
		{
			// sanity checks
			if (!IsValid(id)) throw new Exception("Invalid indexer: " + id.Index);

			// remove this item
			this[id] = default(INode<T>);
			if (!Freelist.Contains(id)) Freelist.Push(id);

			// check for empty
			if (Freelist.Count == CurrentIndex)
			{
				// sanity check
				for(int i=0; i<Data.Length; i++)
				{
					if (Data[i] != default(INode<T>)) throw new Exception("List is not really empty: [" + i + "] is not null");
				}

				CurrentIndex = 1;  // Head remains allocated
				Freelist.Clear();

				// fire event
				if (OnReset != null) OnReset(this, null);
			}

			return true;
		}

		public static bool IsValid(Id id)
		{
			// check if it if valid
			return (!id.Equals(Id.Identity) 
				&& id.Index >= 0);
		}

		public Id FindParent(Id id)
		{
			// exploit the fact that ON AVERAGE a tree is expanded from the bottom
			for(int i=Math.Min(CurrentIndex-1, Data.Length-1); i>=0; i--)
			{
				if (Data[i] != null && (Data[i].RightBranch.Equals(id) || Data[i].LeftBranch.Equals(id))) return new Id(i, Level);
			}

			return Id.Identity;
		}

		public void DEBUG_PrintTree()
		{
			PrintTree(Head, 0);
		}

		private void PrintTree(Id id, int level)
		{
			if (id.Equals(Id.Identity))
			{
				//Console.WriteLine("{0}: {1}", level, id.Index);
			}
			else if (this[id] == default(INode<T>))
			{
				//Console.WriteLine("{0}: {1}/{2} null", level, id.Level, id.Index);
			}
			else
			{
				/*Console.WriteLine("{0}: {1}/{2} {3} [F:{4}/{5} T:{6}/{7}] {8} P:{9}/{10} B:{11}", 
							level, //0
							id.Level, //1
							id.Index, //2
							this[id].GetType(), //3
							this[id].LeftBranch.Level, //4
							this[id].LeftBranch.Index, //5
							this[id].RightBranch.Level, //6
							this[id].RightBranch.Index, //7
							this[id].Content == null ? "null" : "LIVE", //8
							this[id].Parent.Level, //9
							this[id].Parent.Index, //10
							this[id].ParentBranch //11
						);
                 */
				PrintTree(this[id].RightBranch, level+1);
				PrintTree(this[id].LeftBranch, level+1);
			}
		}
	}
}