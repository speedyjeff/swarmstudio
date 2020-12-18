using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Swarm;
using Interpreter;

public class Parser
{
	public static bool Debug { get; set; }

	static Parser()
	{
		Debug = false;
	}

	private static void InsertNotify(object sender, Id parentId, Id id, bool parentBranch)
	{
		//if (Debug) Console.WriteLine("INSERTED : {0}/{1} P:{2} B:{3}", id.Level, id.Index, parentId.Index, parentBranch);
	}

	private static void RemoveNotify(object sender, Id parentId, Id id, bool parentBranch)
	{
		//if (Debug) Console.WriteLine("REMOVED : {0}/{1} P:{2} B:{3}", id.Level, id.Index, parentId.Index, parentBranch);
	}


	// 
	// Parse user defined values
	//
	private static bool TryParseInt32(string str, out int val)
	{
		// FORMAT: ##
		return Int32.TryParse(str, out val);
	}

	private static bool TryParseMatrix(string str, out Matrix val)
	{
		// FORMAT: {# # # # #}{# # # # #}{# # # # #}{# # # # #}{# # # # #}

		string[] lines;
		int[,] input;

		val = null;

		input = new int[Matrix.Dimension,Matrix.Dimension];

		lines = str.Split(new char[] {'}'});
		if (lines.Length != Matrix.Dimension+1) return false;

		for(int i=0; i<lines.Length-1; i++)
		{
			string[] nums;

			// replace multiple spaces with single
			while(lines[i].Contains("\t") || lines[i].Contains("  "))
			{
				lines[i] = lines[i].Replace("\t", " ");
				lines[i] = lines[i].Replace("  ", " ");
			}
			lines[i] = lines[i].TrimStart(new char[] {' '});

			nums = lines[i].Replace("{","").Split(new char[] {' ', ','});
			if (nums.Length != Matrix.Dimension) return false;

			for(int j=0; j<nums.Length; j++)
			{
				int num;
				if (!Int32.TryParse(nums[j], out num)) return false;
				if (num < (int)PlotState.Forbidden || num > (int)PlotState.Any) return false;
				input[i,j] = num;
			}
		}

		val = new Matrix(input);
		return true;
	}

	private static bool TryParseGrid(string str, out Grid val)
	{
		// FORMAT: {# # #}{# # #}{# # #}

		string[] lines;
		int[,] input;

		val = null;

		input = new int[Grid.Dimension,Grid.Dimension];

		lines = str.Split(new char[] {'}'});
		if (lines.Length != Grid.Dimension+1) return false;

		for(int i=0; i<lines.Length-1; i++)
		{
			string[] nums;

			// replace multiple spaces with single
			while(lines[i].Contains("\t") || lines[i].Contains("  "))
			{
				lines[i] = lines[i].Replace("\t", " ");
				lines[i] = lines[i].Replace("  ", " ");
			}
			lines[i] = lines[i].TrimStart(new char[] {' '});

			nums = lines[i].Replace("{","").Split(new char[] {' ', ','});
			if (nums.Length != Grid.Dimension) return false;

			for(int j=0; j<nums.Length; j++)
			{
				int num;
				if (!Int32.TryParse(nums[j], out num)) return false;
				if (num < (int)GridState.Forbidden || num > (int)GridState.Any) return false;
				input[i,j] = num;
			}
		}

		val = new Grid(input);
		return true;
	}

	private static bool TryParseId(string str, out Id val)
	{
		// FORMAT: ##, ##

		string[] nums;
		int index, level;

		val = Id.Identity;

		nums = str.Split(new char[] {','});

		if (nums.Length != 2) return false;

		if (!Int32.TryParse(nums[0], out level)) return false;
		if (!Int32.TryParse(nums[1], out index)) return false;

		val = new Id(index, level);
		return true;
	}

	//
	// Parse util
	//
	private static int ParseUntil(string text, int start, char[] seperators, out string token)
	{
		bool found = false;
		int end;

		token = "";
		
		for(end=start+1; end<text.Length && !found; end++)
		{
			foreach (char seperator in seperators)
			{
				if (text[end] == seperator) found = true;
			}
		}

		// accmodate the seperator
		end--;

		// parse out token
		token = text.Substring(start, end-start); 

		return end-start;
	}

	//
	// output parameters
	// 
	public static int LineNum { get; set; }
	public static int CharNum { get; set; }
	public static int Code { get; set; }
	public static bool IsError { get; set; }

	private static void SetError(int linenum, int charnum, int code)
	{
		IsError = true;
		LineNum = linenum;
		CharNum = charnum;
		Code = code;
	}

	//
	// private state
	//
	private enum Tokens { NONE, 
				IF, 
				THEN, 
				ELSE,
				GOTO, 
				RETURN, 
				RETURN_VAL,
				MATRIX, 
				MATRIX_VAL,
				GRID,
				GRID_VAL,
				RAND,
				RAND_VAL,
				LAST,
				LAST_VAL,
				AND,
				OR,
				EQ,
				NE
				}
	
	private static bool TryParseToken(string str, out Tokens token)
	{
		token = Tokens.NONE;

		switch(str.ToUpper())
		{
		case "IF":         token = Tokens.IF; break;
		case "THEN":       token = Tokens.THEN; break;
		case "GOTO":       token = Tokens.GOTO; break;
		case "RETURN":     token = Tokens.RETURN; break;
		case "ELSE":       token = Tokens.ELSE; break;
		case "EQ":         token = Tokens.EQ; break;
		case "NE":         token = Tokens.NE; break;
		case "MATRIX":     token = Tokens.MATRIX; break;
		case "GRID":       token = Tokens.GRID; break;
		case "RAND":       token = Tokens.RAND; break;
		case "LAST":       token = Tokens.LAST; break;
		case "OR":         token = Tokens.OR; break;
		case "AND":        token = Tokens.AND; break;
		default:           token = Tokens.NONE; break;
		}

		return (token != Tokens.NONE);
	}

	private static Tokens ReturnValue(Tokens token)
	{
		switch(token)
		{
		case Tokens.RETURN: return Tokens.RETURN_VAL;
		case Tokens.MATRIX: return Tokens.MATRIX_VAL;
		case Tokens.GRID:   return Tokens.GRID_VAL;
		case Tokens.RAND:   return Tokens.RAND_VAL;
		case Tokens.LAST:   return Tokens.LAST_VAL;
		default:            return Tokens.NONE;
		}
	}

	private static bool CheckTokenChange(Tokens prevtoken, Tokens token)
	{
		switch(token)
		{
		case Tokens.NONE:       throw new Exception("Cannot have NONE token");
		case Tokens.IF:         return (Tokens.NONE       == prevtoken || Tokens.THEN     == prevtoken || Tokens.ELSE     == prevtoken);
		case Tokens.THEN:       return (Tokens.MATRIX_VAL == prevtoken || Tokens.GRID_VAL == prevtoken || Tokens.RAND_VAL == prevtoken || Tokens.LAST_VAL == prevtoken);
		case Tokens.ELSE:       return (Tokens.RETURN_VAL == prevtoken || Tokens.GOTO     == prevtoken);
		case Tokens.GOTO:       return (Tokens.NONE       == prevtoken || Tokens.THEN     == prevtoken || Tokens.ELSE     == prevtoken);
		case Tokens.RETURN:     return (Tokens.NONE       == prevtoken || Tokens.THEN     == prevtoken || Tokens.ELSE     == prevtoken);
		case Tokens.RETURN_VAL: return (Tokens.RETURN     == prevtoken);
		case Tokens.MATRIX:     return (Tokens.IF         == prevtoken || Tokens.AND      == prevtoken || Tokens.OR       == prevtoken);
		case Tokens.MATRIX_VAL: return (Tokens.MATRIX     == prevtoken);
		case Tokens.GRID:       return (Tokens.IF         == prevtoken || Tokens.AND      == prevtoken || Tokens.OR       == prevtoken);
		case Tokens.GRID_VAL:   return (Tokens.GRID       == prevtoken);
		case Tokens.RAND:       return (Tokens.IF         == prevtoken || Tokens.AND      == prevtoken || Tokens.OR       == prevtoken);
		case Tokens.RAND_VAL:   return (Tokens.RAND       == prevtoken);
		case Tokens.LAST:       return (Tokens.IF         == prevtoken || Tokens.AND      == prevtoken || Tokens.OR       == prevtoken);
		case Tokens.LAST_VAL:   return (Tokens.LAST       == prevtoken);
		case Tokens.AND:        return (Tokens.MATRIX_VAL == prevtoken || Tokens.GRID_VAL == prevtoken || Tokens.RAND_VAL == prevtoken || Tokens.LAST_VAL == prevtoken);
		case Tokens.OR:         return (Tokens.MATRIX_VAL == prevtoken || Tokens.GRID_VAL == prevtoken || Tokens.RAND_VAL == prevtoken || Tokens.LAST_VAL == prevtoken);
		case Tokens.EQ:         return (Tokens.MATRIX     == prevtoken || Tokens.GRID     == prevtoken || Tokens.RAND     == prevtoken || Tokens.LAST     == prevtoken);
		case Tokens.NE:         return (Tokens.MATRIX     == prevtoken || Tokens.GRID     == prevtoken || Tokens.RAND     == prevtoken || Tokens.LAST     == prevtoken);
		default:                throw new Exception("Invalid token: " + token);
		}
	}

	private static bool CheckTokenValue(Tokens token)
	{
		switch(token)
		{
		case Tokens.GOTO:       return true;
		case Tokens.RETURN_VAL: return true;
		case Tokens.MATRIX_VAL: return true;
		case Tokens.GRID_VAL:   return true;
		case Tokens.RAND_VAL:   return true;
		case Tokens.LAST_VAL:   return true;
		default:                return false;
		}
	}

	//
	// Parse text
	//
	public static ScriptHost<Data> Parse(string text)
	{
		ScriptHost<Data> host;
		Stack<Id> parents;
		int linenum;
		int charnum;
		string strtoken;
		string value;
		bool validid;
		Id id;
		bool clearstatement;
		BooleanOp bop;
		ExpressionOp eop;
		Tokens prevtoken;
		bool first;
		Tokens token;
		bool clearprv;

        // exit early if there is nothing to do
        if (text == null || text.Equals("")) return null;

		// init
		parents = new Stack<Id>();
		linenum = 1;
		charnum = 1;
		value = "";
		strtoken = "";
		prevtoken = Tokens.NONE;
		token = Tokens.NONE;
		host = new ScriptHost<Data>();
		id = host.Head;
		bop = BooleanOp.NONE;
		eop = ExpressionOp.NONE;
		first = true;
		validid = true;  // id == Head

		host.OnInsert += new ScriptHostHandler(InsertNotify);
		host.OnRemove += new ScriptHostHandler(RemoveNotify);

		// parse
		for(int index=0; index<text.Length; index++)
		{
			int increment = 1;

			// trim off whitespace
			while(index < text.Length
				&& (text[index] == '\t' 
				|| text[index] == ' ' 
				|| text[index] == '\r'
				|| text[index] == '\n'))
			{
				index++; 
				increment++;

				if (index >= text.Length-1) return host; // exit early?!?!

				if (text[index] == '\n' || text[index] == '\r')
				{
					linenum++;
					charnum = 1;
				}
			}

			// parse content
			switch(text[index])
			{
				case '[': 
					// user defined type
					increment = ParseUntil(text, ++index, new char[] {']'}, out value);
					index += increment;
					increment++;

					if (!CheckTokenValue(token))
					{
						SetError(linenum, charnum, 1200);
						return null;
					}
					break;
				default:
					// language defined type
					increment = ParseUntil(text, index, new char[] {' ', '\t', '\n', '\r'}, out strtoken);
					if (text[index + increment] == '\r' || text[index + increment] == '\n')
					{
						linenum++;
						charnum = 1;
					}
					index += increment;
					increment++;

					// parse out the token
					if (!TryParseToken(strtoken, out token))
					{
						SetError(linenum, charnum, 1000);
						return null;
					}

					if (!CheckTokenChange(prevtoken, token))
					{
						SetError(linenum, charnum, 1100);
						return null;
					}

					break;
			}


			//if (Debug) Console.WriteLine("{0}:{1} V:({2}) T:[{3}] ID:{4}", linenum, charnum, value, token, id);

			// build tree
			clearstatement = false;
			clearprv = false;
			switch(token)
			{
			// flow
			case Tokens.IF:
				if (validid) validid = false; // use id
				else if (parents.Count == 0 && !first) id = host.GetNextHead(id);
				parents.Push(id);
				first = false;
				host.InsertBranch(id);
				clearstatement = true;
				break;
			case Tokens.THEN:
				id = host.GetTrueBranch(id);
				validid = true;
				clearstatement = true;
				break;
			case Tokens.GOTO:
				if (value != "")
				{
					Id myid;

					if (!TryParseId(value, out myid) || myid.Equals(Id.Identity)) 
					{
						SetError(linenum, charnum, 90);
						return null;
					}

					host.Remove(id);
					host.InsertGoto(id, myid);

					validid = false;
				}
				else
				{
					if (!validid)
					{
						// add a new head
						id = host.GetNextHead(id);
					}
					
					host.InsertGoto(id, host.GetNextHead(id));
					validid = false;
				}
				clearprv = true;
				break;
			case Tokens.RETURN:
				prevtoken = token;
				token = ReturnValue(prevtoken);
				break;
			case Tokens.RETURN_VAL:
				int val = -1;
				Data data;

				if (!validid)
				{
					// add a new head
					id = host.GetNextHead(id);
				}

				if (value == "" || !TryParseInt32(value, out val) || (val < (int)Move.Nothing || val > (int)Move.Explode))
				{
					SetError(linenum, charnum, 100);
					return null;
				}

				host.InsertResult(id);
				data = new Data(DataType.Result);
				data.Result = (Move)val;
				host.SetContent(id, data);

				validid = false;
				clearprv = true;
				break;
			case Tokens.ELSE:
				id = parents.Pop();
				id = host.GetFalseBranch(id);
				validid = true;
				break;

			// statements
			case Tokens.EQ:
				if (eop != ExpressionOp.NONE)
				{
					SetError(linenum, charnum, 200);
					return null;
				}
				eop = ExpressionOp.EQ;
				token = ReturnValue(prevtoken);
				break;
			case Tokens.NE:
				if (eop != ExpressionOp.NONE)
				{
					SetError(linenum, charnum, 300);
					return null;
				}
				eop = ExpressionOp.NE;
				token = ReturnValue(prevtoken);
				break;
			case Tokens.MATRIX:
				break;
			case Tokens.MATRIX_VAL:
				Matrix matrix;

				if (value == "" 
					|| !TryParseMatrix(value, out matrix)
					|| eop == ExpressionOp.NONE 
					|| (bop == BooleanOp.NONE && null != host.GetContent(id)) 
					|| (bop != BooleanOp.NONE && null == host.GetContent(id)) )
				{
					SetError(linenum, charnum, 400);
					return null;
				}
				
				if (bop == BooleanOp.NONE)
				{
					host.SetContent(id, new Data(DataType.Branch) );
				}
				host.GetContent(id).MatrixBranch(matrix, bop, eop);
				clearstatement = true;
				break;
			case Tokens.GRID:
				break;
			case Tokens.GRID_VAL:
				Grid grid;

				if (value == "" 
					|| !TryParseGrid(value, out grid)
					|| eop == ExpressionOp.NONE 
					|| (bop == BooleanOp.NONE && null != host.GetContent(id)) 
					|| (bop != BooleanOp.NONE && null == host.GetContent(id)) )
				{
					SetError(linenum, charnum, 450);
					return null;
				}
				
				if (bop == BooleanOp.NONE)
				{
					host.SetContent(id, new Data(DataType.Branch) );
				}
				host.GetContent(id).GridBranch(grid, bop, eop);			
				clearstatement = true;
				break;
			case Tokens.RAND:
				break;
			case Tokens.RAND_VAL:
				int rand;

				if (value == "" 
					|| !TryParseInt32(value, out rand)
					|| rand <= 0
					|| eop == ExpressionOp.NONE 
					|| (bop == BooleanOp.NONE && null != host.GetContent(id)) 
					|| (bop != BooleanOp.NONE && null == host.GetContent(id)) )
				{
					SetError(linenum, charnum, 500);
					return null;
				}
				
				if (bop == BooleanOp.NONE)
				{
					host.SetContent(id, new Data(DataType.Branch) );
				}
				host.GetContent(id).RandomBranch(rand, bop, eop);
				clearstatement = true;
				break;
			case Tokens.LAST:
				break;
			case Tokens.LAST_VAL:
				int move;

				if (value == "" 
					|| !TryParseInt32(value, out move)
					|| eop == ExpressionOp.NONE 
					|| (bop == BooleanOp.NONE && null != host.GetContent(id)) 
					|| (bop != BooleanOp.NONE && null == host.GetContent(id)) 
					|| (move < (int)Move.Nothing || move > (int)Move.Explode))
				{
					SetError(linenum, charnum, 550);
					return null;
				}
				
				if (bop == BooleanOp.NONE)
				{
					host.SetContent(id, new Data(DataType.Branch) );
				}
				host.GetContent(id).LastBranch((Move)move, bop, eop);
				clearstatement = true;
				break;

			// boolean
			case Tokens.OR:
				if (value != "" || eop != ExpressionOp.NONE)
				{
					SetError(linenum, charnum, 600);
					return null;
				}
				bop = BooleanOp.OR;
				break;
			case Tokens.AND:
				if (value != "" || eop != ExpressionOp.NONE)
				{
					SetError(linenum, charnum, 700);
					return null;
				}
				bop = BooleanOp.AND;
				break;

			default:
				SetError(linenum, charnum, 800);
				return null;
			}

			prevtoken = token;

			if (clearprv && parents.Count == 0) prevtoken = Tokens.NONE;

			if (clearstatement)
			{
				bop = BooleanOp.NONE;
				eop = ExpressionOp.NONE;
			}
			value = "";

			charnum += increment;
		}

		return host;
	}

	//
	// Parse ScriptHost
	//
	private static string ScriptText;
	private static Stack<string> PrevBranches;

	public static string Parse(ScriptHost<Data> host)
	{
		ScriptText = "";

		PrevBranches = new Stack<string>();
		host.OnSearch += DisplayNode;
		host.DepthSearch();
		host.OnSearch -= DisplayNode;

		return ScriptText;
	}

	private static void DisplayNode(object sender, int level, Id id)
	{
		ScriptText += ParseNode((sender as ScriptHost<Data>), level, id);
	}

	private static string ParseNode(ScriptHost<Data> host, int level, Id id)
	{
		string node = "";
		string tabs = "";

		for(int t=0; t<level; t++) tabs += "\t";
		node += tabs;

		switch (host.GetNodeType(id))
		{
		case NodeType.Goto:
			Id next = host.GetGotoDestination(id);
			node += "GOTO";
			if (next != Id.Identity) node += " [" + next.Level + ", " + next.Index + "]";
			break;
		case NodeType.Branch:
			node += "IF ";
			node += ParseBranch(host.GetContent(id));
			node += " THEN";
			PrevBranches.Push(tabs + "ELSE" + Environment.NewLine);
			break;
		case NodeType.Result:
			node += "RETURN [" + (int)host.GetContent(id).Result + "]";
			break;
		default:
			throw new Exception("Invalid NodeType: " + host.GetNodeType(id));
		}

		node += Environment.NewLine;

		if (host.GetNodeType(id) != NodeType.Branch && PrevBranches.Count > 0) node += PrevBranches.Pop();

		return node;
	}

	private static string ParseBranch(Data data)
	{
		BooleanOp bop;
		ExpressionOp eop;
		BranchType type;
		Matrix matrix;
		Grid grid;
		int rand;
		Move last;
		string expression = "";

		data.Reset();

		while(data.Enumerate(out bop, out eop, out type, out matrix, out grid, out rand, out last))
		{
			// expression
			switch(bop)
			{
			case BooleanOp.AND:
				expression += " AND ";
				break;
			case BooleanOp.OR:
				expression += " OR ";
				break;
			case BooleanOp.NONE:
				break;
			default:
				throw new Exception("Unknown BooleanOp: " + bop);
			}

			// type
			switch(type)
			{
			case BranchType.Matrix:
				expression += "matrix ";
				break;
			case BranchType.Grid:
				expression += "grid ";
				break;
			case BranchType.Random:
				expression += "rand ";
				break;
			case BranchType.Last:
				expression += "last ";
				break;
			default:
				throw new Exception("Unknown BranchType: " + type);
			}

			// expression
			switch(eop)
			{
			case ExpressionOp.EQ:
				expression += "EQ ";
				break;
			case ExpressionOp.NE:
				expression += "NE ";
				break;
			default:
				throw new Exception("Unknown ExpressionOp: " + eop);
			}

			// value
			switch(type)
			{
			case BranchType.Matrix:
				expression += "[";
				for(int i=0; i<Matrix.Dimension; i++)
				{
					expression += "{";
					for(int j=0; j<Matrix.Dimension; j++)
					{
						expression += (int)matrix[i,j];
						if (j != Matrix.Dimension-1) expression += " ";
					}
					expression += "}";
				}
				expression += "]";
				break;
			case BranchType.Grid:
				expression += "[";
				for(int i=0; i<Grid.Dimension; i++)
				{
					expression += "{";
					for(int j=0; j<Grid.Dimension; j++)
					{
						expression += (int)grid[i,j];
						if (j != Grid.Dimension-1) expression += " ";
					}
					expression += "}";
				}
				expression += "]";
				break;
			case BranchType.Random:
				expression += "[" + rand + "]";
				break;
			case BranchType.Last:
				expression += "[" + (int)last + "]";
				break;
			default:
				throw new Exception("Unknown BranchType: " + type);
			}
		}

		return expression;
	}
}
