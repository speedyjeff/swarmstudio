using System;
using System.Collections;
using System.Collections.Generic;

using Common;
using Swarm;
using Interpreter;


// Rules
//  100x100 board
//  5 initial pieces
//  turn based
//  On turn:
//    may duplicate (takes 5 turns, needs 1 adjacent space)
//    may move up to 1 space (takes 1 turn)
//    may defend position
//  Attack:
//    may step on other player
//      if in defend poster, attacking player dies
//      if in duplicate phase, switch ownership of duplication
//      if regular attacking player wins

public enum PlotState
{
	Forbidden   = 0,
	Unoccupied  = 1,
	Occupied    = 2,
	Defended    = 3,
	Duplication = 4,
	Visited     = 5,
	Enemy       = 6,
    Destroyed   = 7,
	Any         = 9
}

public enum GridState
{
    Forbidden = 0,
    Unoccupied = 1,
    Occupied = 2,
    Any = 9
}

public enum PlotColor
{
	Clear   = 0,
    Blue = 1,
	Red     = 2,
	Green   = 3,
	Yellow  = 4		// MUST REMAIN LAST ELEMENT. IS USED AS SIZE (PlotColor.Yellow+1)
}

public enum Move
{
	Nothing   = 0,
	Up        = 1,
	Down      = 2,
	Left      = 3,
	Right     = 4,
	Defend    = 5,
	Duplicate = 6,
    Explode   = 7   // this value is used in the parser as the end
}

internal class Plot
{
	public PlotState State;
	public PlotColor Color;
	public int       WaitDuration;

	public Plot()
	{
		State        = PlotState.Forbidden;
		Color        = PlotColor.Clear;
		WaitDuration = 0;
	}

	public object Clone()
	{
		Plot p         = new Plot();
		p.State        = State;
		p.Color        = Color;
		p.WaitDuration = WaitDuration;

		return p;
	}
}

internal struct Coordinate
{
	public int H;
	public int W;

	public Coordinate(int h, int w)
	{
		H = h;
		W = w;
	}
}

namespace SwarmGameLogic
{

public static class SwarmUtil
{
	public static char Seperator { get { return ':'; } }

	public static string Encode(PlotColor color, int h, int w, PlotState state, int waitTime)
	{
		string str;

		// height
		str = Seperator + h.ToString();

		// color
		switch (color)
		{
		case PlotColor.Clear:  str += "c"; break;
		case PlotColor.Red:    str += "r"; break;
		case PlotColor.Blue:   str += "b"; break;
		case PlotColor.Green:  str += "g"; break;
		case PlotColor.Yellow: str += "y"; break;
        default: throw new Exception("Unknown color : " + color);
		}

		// waittime
		str += waitTime;

		// state
		switch(state)
		{
		case PlotState.Forbidden:   str += "f"; break;
		case PlotState.Unoccupied:  str += "u"; break;
		case PlotState.Occupied:    str += "o"; break;
		case PlotState.Defended:    str += "e"; break;
		case PlotState.Duplication: str += "d"; break;
		case PlotState.Visited:     str += "v"; break;
        case PlotState.Destroyed:   str += "x"; break;
        default: throw new Exception("Unknown state : " + state);
		}

		// width
		str += w;

		return str;
	}

	public static bool Decode(string str, out PlotColor color, out int h, out int w, out PlotState state, out int waitTime)
	{
		// the rest should follow this format
		//  HEIGHTcolorWAITDURATIONstateWIDTH
		int    part;
		string numStr;
		char[] chArray;

		part     = 0;
		numStr   = "";
		color    = PlotColor.Clear;
		h = w    = 0;
		state    = PlotState.Forbidden;
		waitTime = 0;
		chArray  = str.ToCharArray();

		// check that this is a valid message
		if (0 == chArray.Length || !Char.IsDigit(chArray[0])) return false;

		foreach (char c in chArray)
		{
			switch(part)
			{
			case 0:
				if (Char.IsDigit(c))
				{
					numStr += c;
				}
				else
				{
					// height
					h      = Convert.ToInt32(numStr);
					numStr = "";
					// color
					switch(c)
					{
					case 'r': color = PlotColor.Red; break;
					case 'b': color = PlotColor.Blue; break;
					case 'g': color = PlotColor.Green; break;
					case 'y': color = PlotColor.Yellow; break;
					case 'c': color = PlotColor.Clear; break;
                    default: throw new Exception("Unknown color : " + c);
					}
					part = 1;
				}
				break;
			case 1:
				if (Char.IsDigit(c))
				{
					numStr += c;
				}
				else
				{
					// wait time
					waitTime = Convert.ToInt32(numStr);
					numStr   = "";
					// state
					switch(c)
					{
					case 'f': state = PlotState.Forbidden; break;
					case 'u': state = PlotState.Unoccupied; break;
					case 'o': state = PlotState.Occupied; break;
					case 'e': state = PlotState.Defended; break;
					case 'd': state = PlotState.Duplication; break;
					case 'v': state = PlotState.Visited; break;
                    case 'x': state = PlotState.Destroyed; break;
                    default: throw new Exception("Unknown state : " + c);
					}
					part = 2;
				}
				break;
			case 2:
				numStr += c;
				break;
			}
		}
		// width
		w = Convert.ToInt32(numStr);

		return true;
	}
} 

internal class Field
{
	// data
	private Plot[,] m_field;
    private Move[,] m_prev;
	private string  m_transaction;
	private int[]   m_count;

	public Field()
	{
		m_field       = null;
		m_transaction = "";
		m_count       = null;
	}

	public Field(int height, int width)
	{
		m_field = new Plot[height , width];
        m_prev = new Move[height, width];
		m_count = new int[ ((int)PlotColor.Yellow+1) ];

		// initialize the counts
		m_count[ (int)PlotColor.Clear ]  = height * width;
		m_count[ (int)PlotColor.Red ]    = 0;
		m_count[ (int)PlotColor.Blue ]   = 0;
		m_count[ (int)PlotColor.Green ]  = 0;
		m_count[ (int)PlotColor.Yellow ] = 0;

		for (int i=0; i<m_field.GetLength(0); i++)
		{
			for (int j=0; j<m_field.GetLength(1); j++)
			{
				m_field[i,j]       = new Plot();
				m_field[i,j].State = PlotState.Unoccupied;
                m_prev[i, j] = Move.Nothing;
			}
		}
	}

	public bool ChangeState(PlotColor color, int h, int w, PlotState state)
	{
		return ChangeState(color, h, w, state, 0 /* default duration */);
	}

	public bool ChangeState(PlotColor color, int h, int w, PlotState state, int waitTime)
	{
		if (h >= 0 && h < m_field.GetLength(0) && w >= 0 && w < m_field.GetLength(1))
		{
			// append transaction
			m_transaction += SwarmUtil.Encode(color, h, w, state, waitTime);

			// upate color counts
			m_count[ (int)m_field[h,w].Color ]--;
			m_count[ (int)color ]++;

			m_field[h,w].State        = state;
			m_field[h,w].Color        = color;
			m_field[h,w].WaitDuration = waitTime;
			return true;
		}

		return false;	
	}

	public bool ResetTransaction()
	{
		m_transaction = "";
		return true;
	}

	public int Count(PlotColor color)
	{
		if (PlotColor.Clear <= color && color <= PlotColor.Yellow)
		{
			return m_count[ (int)color ];
		}
		return 0;
	}

	public int    Height      { get { return (null == m_field)?0:m_field.GetLength(0); } }
	public int    Width       { get { return (null == m_field)?0:m_field.GetLength(1); } }
	public string Transaction { get { return m_transaction; } }

	public Plot this[int h, int w]
	{
		get
		{
			if (null != m_field && h >= 0 && h < m_field.GetLength(0) && w >= 0 && w < m_field.GetLength(1))
			{
                return m_field[h, w];
			}
			else
			{
				return new Plot();
			}
		}
	}

    public Move GetPreviousMove(int h, int w)
    {
        if (null == m_prev || h < 0 || h >= m_prev.GetLength(0) || w < 0 || w >= m_prev.GetLength(1)) throw new Exception(h + ", " + w + " out of range for Previous");

        return m_prev[h,w];
    }

    public void SetPreviousMove(int h, int w, Move p)
    {
        if (null == m_prev || h < 0 || h >= m_prev.GetLength(0) || w < 0 || w >= m_prev.GetLength(1)) throw new Exception(h + ", " + w + " out of range for Previous");

        m_prev[h,w] = p;
    }
}

internal class MyScriptEngine
{
	private ScriptHost<Data> host;

    public MyScriptEngine(string script)
    {
        host = Parser.Parse(script);

        Init();

        if (host == null)
        {
            IsError = true;
            LineNum = Parser.LineNum;
            CharNum = Parser.CharNum;
            Code = Parser.Code;
        }
        else if (!host.Verify())
        {
            IsError = true;
            ErrorInfo = host.ErrorInfo;
        }
	}

    private void Init()
    {
        IsError = false;
        ErrorInfo = Id.Identity;
        LineNum = 0;
        CharNum = 0;
        Code = 0;
    }

    public bool IsError { get; private set; }
    public Id ErrorInfo { get; private set; }
    public int Code { get; private set; }
    public int LineNum { get; private set; }
    public int CharNum { get; private set; }
	public Move Result { get; private set; }

	public void Execute(Data d)
	{
		Result = host.Execute(d).Result;
	}
}

internal class Swarm
{
	// data
	private PlotColor m_color;
    private MyScriptEngine Engine;
    private Tuple<int, int>[] Ends;

	public Swarm(PlotColor color, string script, Tuple<int,int>[] end)
	{
		m_color = color;
        Engine = new MyScriptEngine(script);
        Ends = end;
        
        IsError = Engine.IsError;
	}

    public bool IsError { get; private set; }

    public string GetError()
    {
        string error = "";

        if (!IsError) return "";

        if (Engine.ErrorInfo != Id.Identity)
        {
            error += "ErrorLine=" + Engine.ErrorInfo.Level;
        }
        else
        {
            error += "Code=" + Engine.Code + ";LineNumber=" + Engine.LineNum + ";CharNumber=" + Engine.CharNum;
        }

        return error;
    }

	public Move RequestMove(int previousState, int h, int w, int[,] field, int[,] grid, Move last)
	{
        if (IsError) return Move.Nothing;

		lock(Engine)
		{
			Data d = new Data(DataType.Content);
            d.MyGrid = new Grid(grid);
            d.MyLast = last;
			d.MyMatrix = new Matrix( field );

			Engine.Execute(d);

			return Engine.Result;
		}
	}

	public PlotColor Color  { get { return m_color; } }
    public Tuple<int, int>[] EndPoints { get { return Ends; } }
}

public class SwarmGame
{
	// constants
	private const int   c_width               = 50;  // plots
	private const int   c_height              = 50;  // plots
	private const int   c_fieldView           = 2;  // plots
    private const int   c_gridView            = 1;  // plots
	private const int   c_defendDuration      = 1;    // turns
	private       int   m_maxIndividuals      = 25;
	private       int   m_duplicationDuration = 5;    // turns
    private       int   m_explosionRadius     = 1; // plots

	// data
	private Field   m_field;
	private Swarm[] m_swarm;
	private int     m_currentSwarm;
	private bool[]  m_validSwarm;
    private int     m_fieldSpaces; // used in battle mode to calculate winning percentage

	public SwarmGame()
	{
		m_field        = new Field(c_height, c_width);
		m_swarm        = new Swarm[ ((int)PlotColor.Yellow+1) ];
		m_validSwarm   = new bool[ ((int)PlotColor.Yellow+1) ];
		m_currentSwarm = (int)PlotColor.Clear;

        IsBattle = false;
        BattleWinningPercentage = 0.7f;
        m_fieldSpaces = 0;

        for (int i = 0; i < m_swarm.Length; i++) m_swarm[i] = null;
		for(int i=0; i<m_validSwarm.Length; i++) m_validSwarm[i] = false;
	}

    public bool IsBattle { get; set; }
    public float BattleWinningPercentage { get; set; }

    public bool SetSwarmConfig(PlotColor color, string script, Tuple<int, int>[] start, Tuple<int, int>[] end, out string transaction)
	{
        // setup worker
        m_swarm[(int)color] = new Swarm(color, script, end);
        if (m_swarm[(int)color].IsError)
        {
            transaction = m_swarm[(int)color].GetError();
            return false;
        }
        m_validSwarm[(int)color] = true;

		lock(m_field)
		{
            transaction = "";

			// reset transaction
			m_field.ResetTransaction();

			// initial swarms
			foreach(Tuple<int, int> pnt in start)
			{
				m_field.ChangeState( color, pnt.Item1, pnt.Item2, PlotState.Occupied);
			}

            // set the transaction
			transaction = SwarmUtil.Seperator + m_field.Transaction;
		}

		return true;
	}

    public void SetFieldConfig(PlotState[,] field, PlotColor visitedSwarm, out string transaction)
    {
		transaction = "";

        lock (m_field)
        {
            // reset transaction
            m_field.ResetTransaction();

            // clear the board
            for (int w = 0; w < m_field.Width; w++)
            {
                for (int h = 0; h < m_field.Height; h++)
                {
                    if (field[h, w] == PlotState.Visited) m_field.ChangeState(visitedSwarm, h, w, field[h, w]);
                    else m_field.ChangeState(PlotColor.Clear, h, w, field[h,w]);

                    if (field[h, w] != PlotState.Forbidden) m_fieldSpaces++;
                }
            }

            transaction = SwarmUtil.Seperator + m_field.Transaction;
        }

        if (m_fieldSpaces == 0) m_fieldSpaces = 1;
    }

	public bool Start()
	{
		if ((int)PlotColor.Clear == m_currentSwarm)
		{
            int cntValid = 0;

            // ensure there is at least 1 valid swarm
            for (int i = 0; i < m_validSwarm.Length; i++) if (m_validSwarm[i]) cntValid++;
            if (cntValid == 0) return false;

            // advance to next swarm
			NextTurnInternal();

			return true;
		}
		else
		{
			return false;
		}
	}

    public Matrix GetMatrix(int h, int w)
    {
        if (h < 0 || h > c_height || w < 0 || w > c_width) throw new Exception("Indices out of bounds : " + h + ", " + w);
        return new Matrix(GetFieldView(m_field[h, w].Color, h, w));
    }

    public Grid GetGrid(int h, int w)
    {
        if (h < 0 || h > c_height || w < 0 || w > c_width) throw new Exception("Indices out of bounds : " + h + ", " + w);
        return new Grid(GetGridView(m_field[h, w].Color, h, w));
    }

    public Move GetPreviousMove(int h, int w)
    {
        if (h < 0 || h > c_height || w < 0 || w > c_width) throw new Exception("Indices out of bounds : " + h + ", " + w);
        return m_field.GetPreviousMove(h, w);
    }

    public PlotColor GetColor(int h, int w)
    {
        if (h < 0 || h > c_height || w < 0 || w > c_width) throw new Exception("Indices out of bounds : " + h + ", " + w);
        return m_field[h, w].Color;
    }

	private int[,] GetFieldView(PlotColor color, int h, int w)
	{
		int[,] field;

		field = new int[ 2*c_fieldView + 1, 2*c_fieldView + 1];

		for (int i=-1*c_fieldView; i<=c_fieldView; i++)
		{
			for (int j=-1*c_fieldView; j<=c_fieldView; j++)
			{
                PlotState state = PlotState.Any;

                switch (m_field[i + h, j + w].State)
                {
                    case PlotState.Defended:
                        if (m_field[i + h, j + w].Color == color) state = PlotState.Occupied;
                        else state = PlotState.Enemy;
                        break;
                    case PlotState.Destroyed:
                        state = PlotState.Unoccupied;
                        break;
                    case PlotState.Duplication: 
                        if (m_field[i + h, j + w].Color == color) state = PlotState.Occupied;
                        else state = PlotState.Enemy;
                        break;
                    case PlotState.Forbidden: 
                        state = PlotState.Forbidden;
                        break;
                    case PlotState.Visited:
                        if (m_field[i + h, j + w].Color == color) state = PlotState.Visited;
                        else state = PlotState.Unoccupied;
                        break;
                    case PlotState.Occupied:
                        if (m_field[i + h, j + w].Color == color) state = PlotState.Occupied;
                        else state = PlotState.Enemy;
                        break;
                    case PlotState.Unoccupied:
                        state = PlotState.Unoccupied;
                        break;

                    // case PlotState.Enemy: break;
                    // case PlotState.Any: break;
                    default: throw new Exception("Unknown state : " + m_field[i + h, j + w].State);
                }

                if (state == PlotState.Any) throw new Exception("Invalid field state!");

                field[i + c_fieldView, j + c_fieldView] = (int)state;
			}
		}

		return field;
	}

    private int[,] GetGridView(PlotColor color, int h, int w)
    {
        int[,] field;

        field = new int[2 * c_gridView + 1, 2 * c_gridView + 1];

        for (int i = -1 * c_gridView; i <= c_gridView; i++)
        {
            for (int j = -1 * c_gridView; j <= c_gridView; j++)
            {
                switch (m_field[i + h, j + w].State)
                {
                case PlotState.Forbidden: 
                    field[i + c_gridView, j + c_gridView] = (int)GridState.Forbidden;
                    break;
                case PlotState.Unoccupied:
                    field[i + c_gridView, j + c_gridView] = (int)GridState.Unoccupied;
                    break;
                case PlotState.Occupied:
                    field[i + c_gridView, j + c_gridView] = (int)GridState.Occupied;
                    break;
                case PlotState.Visited:
                    field[i + c_gridView, j + c_gridView] = (int)GridState.Unoccupied;
                    break;
                case PlotState.Enemy:
                    field[i + c_gridView, j + c_gridView] = (int)GridState.Occupied;
                    break;
                case PlotState.Duplication:
                    field[i + c_gridView, j + c_gridView] = (int)GridState.Occupied;
                    break;
                case PlotState.Defended:
                    field[i + c_gridView, j + c_gridView] = (int)GridState.Occupied;
                    break;
                case PlotState.Destroyed:
                    field[i + c_gridView, j + c_gridView] = (int)GridState.Unoccupied;
                    break;
                default: throw new Exception("Unknown state : " + m_field[i + h, j + w].State);
                }
            }
        }

        return field;
    }

	private List<Coordinate> GetSwarm(PlotColor color)
	{
		// review the field and return a list of coordinates for the given color
		List<Coordinate> coords;

		coords = new List<Coordinate>();

		for (int i=0; i<m_field.Height; i++)
		{
			for (int j=0; j<m_field.Width; j++)
			{
				if (color == m_field[i,j].Color && (PlotState.Occupied == m_field[i,j].State || PlotState.Defended == m_field[i,j].State || PlotState.Duplication == m_field[i,j].State))
				{
					coords.Add( new Coordinate(i,j) );
				}
			}
		}

		return coords;
	}

    public bool NextTurn(out string transaction)
    {
        return NextTurn(m_swarm[m_currentSwarm].Color, out transaction);
    }

	public bool NextTurn(PlotColor color, out string transaction)
	{
		int[,]           field;
        int[,]           grid;
		Move             move;
		int              newH, newW;
		List<Coordinate> swarm;

		transaction = "";

		lock(m_field)
		{
			// reset transaction
			m_field.ResetTransaction();

			// check that the color matches
			if (PlotColor.Clear == color || color != m_swarm[m_currentSwarm].Color) return false;

			// iterate through all the individuals that ask them to move
			swarm = GetSwarm(m_swarm[m_currentSwarm].Color);
			foreach (Coordinate coord in swarm)
			{
				// check if the individual is in a wait
                if (PlotState.Destroyed == m_field[coord.H, coord.W].State)
                {
                    // in the event one of the friendly swarms was destroyed it should not move on this turn
                    // TODO! What if the swarm was killed !!! 
                }
                else if (1 < m_field[coord.H, coord.W].WaitDuration)
                {
                    m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W, m_field[coord.H, coord.W].State, m_field[coord.H, coord.W].WaitDuration - 1);
                }
                else
                {
                    // check for a duplication
                    if (PlotState.Duplication == m_field[coord.H, coord.W].State && m_maxIndividuals > swarm.Count)
                    {
                        // add an individual near this point
                        if (PlotState.Unoccupied == m_field[coord.H - 1, coord.W].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H - 1, coord.W, PlotState.Occupied);
                        else if (PlotState.Unoccupied == m_field[coord.H + 1, coord.W].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H + 1, coord.W, PlotState.Occupied);
                        else if (PlotState.Unoccupied == m_field[coord.H, coord.W - 1].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W - 1, PlotState.Occupied);
                        else if (PlotState.Unoccupied == m_field[coord.H, coord.W + 1].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W + 1, PlotState.Occupied);
                        else if (PlotState.Visited == m_field[coord.H - 1, coord.W].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H - 1, coord.W, PlotState.Occupied);
                        else if (PlotState.Visited == m_field[coord.H + 1, coord.W].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H + 1, coord.W, PlotState.Occupied);
                        else if (PlotState.Visited == m_field[coord.H, coord.W - 1].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W - 1, PlotState.Occupied);
                        else if (PlotState.Visited == m_field[coord.H, coord.W + 1].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W + 1, PlotState.Occupied);
                        else if (PlotState.Destroyed == m_field[coord.H - 1, coord.W].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H - 1, coord.W, PlotState.Occupied);
                        else if (PlotState.Destroyed == m_field[coord.H + 1, coord.W].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H + 1, coord.W, PlotState.Occupied);
                        else if (PlotState.Destroyed == m_field[coord.H, coord.W - 1].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W - 1, PlotState.Occupied);
                        else if (PlotState.Destroyed == m_field[coord.H, coord.W + 1].State) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W + 1, PlotState.Occupied);
                    }

                    // grab the view of the field
                    field = GetFieldView(m_swarm[m_currentSwarm].Color, coord.H, coord.W);
                    grid = GetGridView(m_swarm[m_currentSwarm].Color, coord.H, coord.W);

                    // ask for the move
                    move = m_swarm[m_currentSwarm].RequestMove((int)m_field[coord.H, coord.W].State, field.GetLength(0) / 2, field.GetLength(1) / 2, field, grid, m_field.GetPreviousMove(coord.H, coord.W));

                    // apply the move
                    if (Move.Up == move || Move.Down == move || Move.Left == move || Move.Right == move)
                    {
                        newH = coord.H;
                        newW = coord.W;

                        switch (move)
                        {
                            case Move.Up: newH--; break;
                            case Move.Down: newH++; break;
                            case Move.Left: newW--; break;
                            case Move.Right: newW++; break;
                            default: throw new Exception("Unknown move : " + move);
                        }

                        if (PlotState.Defended == m_field[newH, newW].State || PlotState.Forbidden == m_field[newH, newW].State)
                        {
                            // dead
                            m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W, PlotState.Visited);

                            // do not set the previous move
                        }
                        else
                        {
                            // change the color of this plot
                            m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W, PlotState.Visited);
                            m_field.ChangeState(m_swarm[m_currentSwarm].Color, newH, newW, PlotState.Occupied);

                            // set the prvious move
                            m_field.SetPreviousMove(newH, newW, move);
                        }
                    }
                    else if (Move.Defend == move)
                    {
                        m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W, PlotState.Defended, c_defendDuration);

                        // set the prvious move
                        m_field.SetPreviousMove(coord.H, coord.W, move);
                    }
                    else if (Move.Duplicate == move)
                    {
                        m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W, PlotState.Duplication, m_duplicationDuration);

                        // set the prvious move
                        m_field.SetPreviousMove(coord.H, coord.W, move);
                    }
                    else if (Move.Nothing == move)
                    {
                        m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H, coord.W, PlotState.Occupied);

                        // set the prvious move
                        m_field.SetPreviousMove(coord.H, coord.W, move);
                    }
                    else if (Move.Explode == move)
                    {
                        // destroy everything around this color including itself
                        for (int i = -1 * m_explosionRadius; i <= m_explosionRadius; i++)
                        {
                            for (int j = -1 * m_explosionRadius; j <= m_explosionRadius; j++)
                            {
                                if (m_field[coord.H + i, coord.W + j].State != PlotState.Forbidden) m_field.ChangeState(m_swarm[m_currentSwarm].Color, coord.H+i, coord.W+j, PlotState.Destroyed);
                            }
                        }

                        // set the prvious move
                        m_field.SetPreviousMove(coord.H, coord.W, move);
                    }
                    else
                    {
                        throw new Exception("Unknown move : " + move);
                    }
                }
			}

			// advance the user
			NextTurnInternal();

			transaction = SwarmUtil.Seperator + m_field.Transaction;
		} // lock

		return true;
	}

	private void NextTurnInternal()
	{
		// advance the current player [Red..Yellow]
		do
		{
			m_currentSwarm = (m_currentSwarm + 1) % ((int)PlotColor.Yellow+1);
		}
		while (!m_validSwarm[m_currentSwarm]);
	}

    public bool Lost(PlotColor color)
    {
        return GetSwarm(color).Count == 0;
    }

	public PlotColor Winner
	{
		get
		{
			lock(m_field)
			{
                if (IsBattle)
                {
                    // battle mode
                    // winning situations
                    //  any color has more than BattleWinningPercentage of the board
                    if ((float)m_field.Count(PlotColor.Blue) / (float)(m_fieldSpaces) >= BattleWinningPercentage) return PlotColor.Blue;
                    if ((float)m_field.Count(PlotColor.Red) / (float)(m_fieldSpaces) >= BattleWinningPercentage) return PlotColor.Red;
                    if ((float)m_field.Count(PlotColor.Green) / (float)(m_fieldSpaces) >= BattleWinningPercentage) return PlotColor.Green;
                    if ((float)m_field.Count(PlotColor.Yellow) / (float)(m_fieldSpaces) >= BattleWinningPercentage) return PlotColor.Yellow;

                    return PlotColor.Clear;
                }
                else
                {
                    // Normal - "game" mode
                    // winning situations
                    //  for a given color all the end spaces must have been visited or occupied by that color

                    for (int i = 0; i < m_swarm.Length; i++)
                    {
                        if (m_swarm[i] != null && m_swarm[i].EndPoints != null && m_swarm[i].EndPoints.Length > 0)
                        {
                            int ecnt = 0;
                            for (int j = 0; j < m_swarm[i].EndPoints.Length; j++)
                            {
                                if (m_field[m_swarm[i].EndPoints[j].Item1, m_swarm[i].EndPoints[j].Item2].Color == (PlotColor)i)
                                {
                                    ecnt++;
                                }
                            }

                            if (ecnt > 0 && ecnt == m_swarm[i].EndPoints.Length) return (PlotColor)i;
                        }
                    }

                    return PlotColor.Clear;
                }
			}
		}
	}
}

}