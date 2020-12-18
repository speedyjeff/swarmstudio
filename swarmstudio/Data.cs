using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using Common;
using Interpreter;

namespace Swarm
{
	//
	// game data
	//
	public class Matrix	
	{
		private PlotState[,] Content;

		public Matrix()
		{
			Content = new PlotState[Dimension,Dimension];

			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					if (i == 2 && j == 2) Content[i,j] = PlotState.Occupied;
					else Content[i,j] = PlotState.Any;
				}
			}
		}

        public Matrix(PlotState[,] field)
        {
            Content = new PlotState[Dimension, Dimension];

            for (int i = 0; i < Content.GetLength(0); i++)
            {
                for (int j = 0; j < Content.GetLength(1); j++)
                {
                    Content[i, j] = field[i, j];
                }
            }
        }

		public Matrix(int[,] field)
		{
			Content = new PlotState[Dimension,Dimension];

			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					Content[i,j] = (PlotState)field[i,j];
				}
			}
		}

		public static int Dimension = 5;

		public PlotState this[int i, int j]
		{ 
			get
			{
				return Content[i,j];
			}
			set
			{
				Content[i,j] = value;
			}
		}

		public object Clone()
		{
			Matrix m = new Matrix();
			
			// deep copy
			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					m[i,j] = Content[i,j];
				}
			}

			return m;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			Matrix m = (obj as Matrix);

			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					if (m[i,j] == PlotState.Any || Content[i,j] == PlotState.Any) {} // Any matches anything
					else if (i == 2 && j == 2) {} // Self is at center (and is not compared)
					else if (m[i,j] != Content[i,j])
					{
						return false;
					}
				}
			}

			return true;
		}

		public override string ToString()
		{
			string str = "";

			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					str += Content[i,j] + " ";
				}
				str += "\n";
			}

			return str;
		}
	}

	public class Grid
	{
		private GridState[,] Content;

		public Grid()
		{
			Content = new GridState[Dimension,Dimension];

			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					if (i == 1 && j == 1) Content[i,j] = GridState.Occupied;
					else Content[i,j] = GridState.Any;
				}
			}
		}

        public Grid(GridState[,] field)
        {
            Content = new GridState[Dimension, Dimension];

            for (int i = 0; i < Content.GetLength(0); i++)
            {
                for (int j = 0; j < Content.GetLength(1); j++)
                {
                    Content[i, j] = field[i, j];
                }
            }
        }

		public Grid(int[,] field)
		{
			Content = new GridState[Dimension,Dimension];

			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					Content[i,j] = (GridState)field[i,j];
				}
			}
		}

		public static int Dimension = 3;

		public GridState this[int i, int j]
		{ 
			get
			{
				return Content[i,j];
			}
			set
			{
				Content[i,j] = value;
			}
		}

		public object Clone()
		{
			Grid m = new Grid();
			
			// deep copy
			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					m[i,j] = Content[i,j];
				}
			}

			return m;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			Grid g = (obj as Grid);

			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					if (g[i,j] == GridState.Any || Content[i,j] == GridState.Any) {} // Any matches anything
					else if (i == 1 && j == 1) {} // Self is at center (and is not compared)
					else if (g[i,j] != Content[i,j])
					{
						return false;
					}
				}
			}

			return true;
		}

		public override string ToString()
		{
			string str = "";

			for(int i=0; i<Content.GetLength(0); i++)
			{
				for(int j=0; j<Content.GetLength(1); j++)
				{
					str += Content[i,j] + " ";
				}
				str += "\n";
			}

			return str;
		}
	}

	// 
	// Statement types
	//
	internal abstract class IStatement : IContent
	{	
		public Matrix MyMatrix { get; set; }
		public Grid MyGrid { get; set; }
		public Move MyLast { get; set; }
		public int MyRandom { get; set; }
		public BranchType MyType { get; set; }

		public virtual bool Eval(IContent c) { return true; }
		public virtual bool Verify() { return true; }
	}

	internal class ContentStatement : IStatement
	{
		public ContentStatement(Matrix matrix, Grid grid, Move move)
		{
			MyMatrix = matrix;
			MyLast = move;
			MyGrid = grid;
		}
	}

	internal class MatrixStatement : IStatement
	{
		public MatrixStatement(Matrix matrix)
		{
			MyMatrix = matrix;
			MyType = BranchType.Matrix;
		}

		public override bool Eval(IContent c)
		{
			return (MyMatrix.Equals((c as IStatement).MyMatrix));
		}	
	}

	internal class GridStatement : IStatement
	{
		public GridStatement(Grid grid)
		{
			MyGrid = grid;
			MyType = BranchType.Grid;
		}

		public override bool Eval(IContent c)
		{
			return (MyGrid.Equals((c as IStatement).MyGrid));
		}	
	}

	internal class LastStatement : IStatement
	{
		public LastStatement(Move last)
		{
			MyLast = last;
			MyType = BranchType.Last;
		}

		public override bool Eval(IContent c)
		{
			return (MyLast.Equals((c as IStatement).MyLast));
		}
	}

	internal class RandomStatement : IStatement
	{
		private static RandomNumberGenerator Rand;

		static RandomStatement()
        {
			Rand = RandomNumberGenerator.Create();
		}

		public RandomStatement(int r)
		{
			MyRandom = r;
			MyType = BranchType.Random;
		}

		public override bool Eval(IContent c)
		{
			var bytes = new byte[4];
			Rand.GetBytes(bytes);
			bytes[0] &= 0x7f;
			var r = BitConverter.ToInt32(bytes, 0);
			return (r % MyRandom) == 0;
		}

		public override bool Verify()
		{
			return (MyRandom >= 0);
		}
	}

	//
	// Tree node
	//
	public enum DataType {Branch, Result, Content};
	public enum BranchType {NONE, Matrix, Grid, Random, Last};

	public class Data : IContent
	{
		private ExpressionTree<IStatement> Expression;

		public Data(DataType type)
		{
			Expression = new ExpressionTree<IStatement>();
			Type = type;
		}

        public DataType Type { get; set; }
		public Move Result { get; set; }

        // used only to pass paremeters
		public Matrix MyMatrix { get; set; }
		public Grid MyGrid { get; set; }
		public Move MyLast { get; set; }

		//
		// Eval/Verify
		//
		public bool Eval(IContent d)
		{
			switch(Type)
			{
				case DataType.Branch: return Expression.Eval(new ContentStatement((d as Data).MyMatrix, (d as Data).MyGrid, (d as Data).MyLast));
				case DataType.Result: return true;
				case DataType.Content: return true;
				default: throw new Exception("Unknown type: " + Type);
			}
		}

		public bool Verify()
		{
			switch(Type)
			{
				case DataType.Branch: return Expression.Verify();
				case DataType.Result: return true;
				case DataType.Content: return true;
				default: throw new Exception("Unknown type: " + Type);
			}
		}

		//
		// Branch setup
		//
		public Id MatrixBranch(Matrix matrix, BooleanOp bop, ExpressionOp eop)
		{	
			Id id;

			id = Expression.InsertRow();
			Expression.SetContent(id, new MatrixStatement(matrix));
			Expression.SetBooleanOp(id, bop);
			Expression.SetExpressionOp(id, eop);

			return id;
		}

		public Id GridBranch(Grid grid, BooleanOp bop, ExpressionOp eop)
		{	
			Id id;

			id = Expression.InsertRow();
			Expression.SetContent(id, new GridStatement(grid));
			Expression.SetBooleanOp(id, bop);
			Expression.SetExpressionOp(id, eop);

			return id;
		}

		public Id RandomBranch(int rand, BooleanOp bop, ExpressionOp eop)
		{
			Id id;

			id = Expression.InsertRow();
			Expression.SetContent(id, new RandomStatement(rand));
			Expression.SetBooleanOp(id, bop);
			Expression.SetExpressionOp(id, eop);

			return id;
		}

		public Id LastBranch(Move last, BooleanOp bop, ExpressionOp eop)
		{
			Id id;

			id = Expression.InsertRow();
			Expression.SetContent(id, new LastStatement(last));
			Expression.SetBooleanOp(id, bop);
			Expression.SetExpressionOp(id, eop);

			return id;
		}

		public bool Group(Id head, Id tail)
		{
			return Expression.Group(head, tail);
		}

		public bool Ungroup(Id head, Id tail)
		{
			return Expression.Ungroup(head,tail);
		}

		public void Delete(Id id)
		{
			Expression.DeleteRow(id);
		}

		public void Reset()
		{
			Expression.Reset();
		}

		public bool Enumerate(out BooleanOp bop, out ExpressionOp eop, out BranchType type, out Matrix matrix, out Grid grid, out int rand, out Move last)
		{
			IStatement content;

			type = BranchType.NONE;
			matrix = null;
			grid = null;
			rand = -1;
			last = Move.Nothing;

			if (Expression.Enumerate(out bop, out eop, out content))
			{
				type = content.MyType;
				matrix = content.MyMatrix;
				grid = content.MyGrid;
				rand = content.MyRandom;
				last = content.MyLast;
				return true;
			}
			else
			{
				return false;
			}
		}
	}
}
