using Common;
using Interpreter;
using Swarm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace swarmstudio
{
    /// <summary>
    /// Interaction logic for VisualDesigner.xaml
    /// </summary>
    public partial class VisualDesigner : UserControl
    {
        private bool[,] IsSelectable;
        private bool HACK_TemporarilyDisableTypeChangedCallback;

        private PlotColor MyColor;

        private int ParseH; // used by SetScript
        private int ParseW; // used by SetScript

        private SolidColorBrush GreenBrush = new SolidColorBrush(Color.FromArgb(100, 28, 124, 0));
        private SolidColorBrush WhiteBrush = new SolidColorBrush(Colors.White);
        private SolidColorBrush GrayBrush = new SolidColorBrush(Colors.LightGray);

        private const double MARKED_OPACITY = 1.0;
        private const double UNMARKED_OPACITY = 0.5;

        public VisualDesigner()
        {
            this.InitializeComponent();

            IsSelectable = new bool[10, 5]; // hardcoded based on XAML!
            MyColor = PlotColor.Blue;
            HACK_TemporarilyDisableTypeChangedCallback = false; // nasty hack to enable swapping
        }

        //
        // public interface
        //

        public string GetScript()
        {
            ScriptHost<Data> host;
            string script;
            Data data;
            Id head;

            // build a host based on the tree
            host = new ScriptHost<Data>();
            head = host.Head;
            data = null;
            for (int h = 0; h < IsSelectable.GetLength(0); h++)
            {
                for (int w = 0; w < IsSelectable.GetLength(1); w++)
                {
                    if (IsSelectable[h, w])
                    {
                        LogicSelector logic = GetSelector(h, w);
                        LogicType type = logic.GetLogicType();

                        // create the branch
                        if (w == 0 &&
                            (type == LogicType.Matrix
                            || type == LogicType.Grid
                            || type == LogicType.Previous
                            || type == LogicType.Random
                            || (type == LogicType.None && IsSelectable[h, w + 1]))
                            )
                        {
                            host.InsertBranch(head);
                            data = new Data(DataType.Branch);
                            host.SetContent(head, data);
                        }

                        switch (type)
                        {
                            case LogicType.Return:
                                if (w > 0)
                                {
                                    head = host.GetTrueBranch(head);
                                }

                                host.InsertResult(head);
                                data = new Data(DataType.Result);
                                data.Result = logic.GetReturn();
                                host.SetContent(head, data);
                                break;
                            case LogicType.Grid:
                                data.GridBranch(logic.GetGrid(), (w == 0) ? BooleanOp.NONE : BooleanOp.OR, ExpressionOp.EQ);
                                break;
                            case LogicType.Matrix:
                                data.MatrixBranch(logic.GetMatrix(), (w == 0) ? BooleanOp.NONE : BooleanOp.OR, ExpressionOp.EQ);
                                break;
                            case LogicType.Previous:
                                data.LastBranch(logic.GetPrevious(), (w == 0) ? BooleanOp.NONE : BooleanOp.OR, ExpressionOp.EQ);
                                break;
                            case LogicType.Random:
                                data.RandomBranch(logic.GetRandom(), (w == 0) ? BooleanOp.NONE : BooleanOp.AND, ExpressionOp.EQ);
                                break;
                            case LogicType.None:
                                if (w == 0 && !IsSelectable[h, w + 1])
                                {
                                    host.InsertResult(head);
                                    data = new Data(DataType.Result);
                                    data.Result = Move.Nothing;
                                    host.SetContent(head, data);
                                }
                                else
                                {
                                    data.RandomBranch(123, (w == 0) ? BooleanOp.NONE : BooleanOp.OR, ExpressionOp.EQ);
                                }
                                break;
                            default: throw new Exception("Invalid type : " + type);
                        }
                    }
                }
                // get the next head
                head = host.GetNextHead(head);
            }

            script = Parser.Parse(host);

            System.Diagnostics.Debug.WriteLine(script);

            return script;
        }

        public bool SetScript(string text)
        {
            ScriptHost<Data> host;

            // clear out the board
            ClearBoard();

            // exit early if empty
            if (string.IsNullOrWhiteSpace(text)) return false;

            // parse the script
            host = Parser.Parse(text);

            if (host == null) return false;

            ParseH = 0;
            ParseW = 0;

            host.OnSearch += ParseNode;
            host.DepthSearch();
            host.OnSearch -= ParseNode;

            return true;
        }

        public void Initialize(PlotColor color)
        {
            MyColor = color;
            switch (MyColor)
            {
                case PlotColor.Blue: IdentityRectangle.Fill = Utility.BluePlotBrush; break;
                case PlotColor.Green: IdentityRectangle.Fill = Utility.GreenPlotBrush; break;
                case PlotColor.Red: IdentityRectangle.Fill = Utility.RedPlotBrush; break;
                case PlotColor.Yellow: IdentityRectangle.Fill = Utility.YellowPlotBrush; break;
                default: throw new Exception("Invalid color: " + MyColor);
            }
        }

        public void DemoShowMove()
        {
            LogicSelector logic = GetSelector(0, 0);
            logic.Clear();
            logic.SetLogicType(LogicType.Return);
        }

        public void DemoShowContext()
        {
            LogicSelector logic = GetSelector(0, 0);
            logic.Clear();
            logic.ShowChooser(true /* lower */);
        }

        // 
        // utility
        //

        private void ClearBoard()
        {
            // clear out everything
            for (int h = 0; h < IsSelectable.GetLength(0); h++)
            {
                for (int w = 0; w < IsSelectable.GetLength(1); w++)
                {
                    UnmarkSelectable(h, w);
                }
            }

            // the first item is always selectable
            MarkSelectable(0, 0);

            // just in case something gets wedged
            HACK_TemporarilyDisableTypeChangedCallback = false; // nasty hack to enable swapping
        }

        private void ParseNode(object sender, int level, Id id)
        {
            BooleanOp bop;
            ExpressionOp eop;
            BranchType type;
            Swarm.Matrix matrix;
            Swarm.Grid grid;
            int rand;
            Move last;
            ScriptHost<Data> host;
            LogicSelector logic;

            host = (sender as ScriptHost<Data>);

            switch (host.GetNodeType(id))
            {
                case NodeType.Goto: break;
                case NodeType.Branch:
                    // advance
                    if (ParseW != 0) { ParseH++; ParseW = 0; }

                    Data data = host.GetContent(id);
                    data.Reset();

                    while (data.Enumerate(out bop, out eop, out type, out matrix, out grid, out rand, out last))
                    {
                        // mark this item as selectable
                        MarkSelectable(ParseH, ParseW);
                        logic = GetSelector(ParseH, ParseW);

                        // value
                        switch (type)
                        {
                            case BranchType.Matrix:
                                logic.SetLogicType(LogicType.Matrix);
                                logic.SetMatrix(matrix);
                                break;
                            case BranchType.Grid:
                                logic.SetLogicType(LogicType.Grid);
                                logic.SetGrid(grid);
                                break;
                            case BranchType.Random:
                                if (rand == 123)
                                {
                                    // this is the indicator that this is an empty
                                }
                                else
                                {
                                    logic.SetLogicType(LogicType.Random);
                                    logic.SetRandom(rand);
                                }
                                break;
                            case BranchType.Last:
                                logic.SetLogicType(LogicType.Previous);
                                logic.SetPrevious(last);
                                break;
                            default: throw new Exception("Unknown BranchType: " + type);
                        }

                        // advance
                        ParseW++;
                    }
                    break;
                case NodeType.Result:
                    Move result = host.GetContent(id).Result;

                    // mark this item as selectable
                    MarkSelectable(ParseH, ParseW);
                    logic = GetSelector(ParseH, ParseW);

                    if (result == Move.Nothing)
                    {
                        // this is an indication of a nothing
                    }
                    else
                    {
                        logic.SetLogicType(LogicType.Return);
                        logic.SetReturn(result);
                    }

                    // advance
                    ParseH++; ParseW = 0;
                    break;
                default: throw new Exception("Invalid NodeType: " + host.GetNodeType(id));
            }
        }

        private void ParseName(string name, out int h, out int w)
        {
            string[] parts = name.Split(new char[] { '_' })[1].Split(new char[] { 'x' });

            h = Convert.ToInt32(parts[0]);
            w = Convert.ToInt32(parts[1]);
        }

        private LogicSelector GetSelector(int h, int w)
        {
            if (h < 0 || h >= IsSelectable.GetLength(0) || w < 0 || w >= IsSelectable.GetLength(1)) throw new Exception("Out of bounds for IsSelectable : " + h + ", " + w);

            var logic = (this.FindName("LS_" + h + "x" + w) as LogicSelector);

            return logic;
        }

        private System.Windows.Controls.Grid GetGrid(int h, int w)
        {
            if (h < 0 || h >= IsSelectable.GetLength(0) || w < 0 || w >= IsSelectable.GetLength(1)) throw new Exception("Out of bounds for IsSelectable : " + h + ", " + w);

            var grid = (this.FindName("Grid_" + h + "x" + w) as System.Windows.Controls.Grid);
            if (grid == null) throw new Exception("Failed to find the right control! : " + h + ", " + w);

            return grid;
        }

        private Rectangle GetRectangle(int h, int w)
        {
            if (h < 0 || h >= IsSelectable.GetLength(0) || w < 0 || w >= IsSelectable.GetLength(1)) throw new Exception("Out of bounds for IsSelectable : " + h + ", " + w);

            var rect = (this.FindName("Rect_" + h + "x" + w) as Rectangle);
            if (rect == null) throw new Exception("Failed to find the right control! : " + h + ", " + w);

            return rect;
        }

        private LogicSelector EnableChooser(int h, int w)
        {
            // exit early if this squre is not able to be selected
            if (!IsSelectable[h, w]) return null;

            // get the grid
            var grid = GetGrid(h, w);

            // get the logic 
            var logic = GetSelector(h, w);
            if (!logic.IsInit)
            {
                logic.Initialize(h, w, MyColor, true /* menu */);
                logic.OnTypeChanged += LogicSelector_OnTypeChanged;
            }

            // show chooser
            if (logic.GetLogicType() == LogicType.None) logic.ShowChooser();

            // display
            logic.Visibility = Visibility.Visible;

            // modify the opacity
            grid.Opacity = MARKED_OPACITY;

            if (logic.GetLogicType() == LogicType.Return) MarkReturn(h, w);
            else UnmarkReturn(h, w);

            return logic;
        }

        private bool DisableChooser(int h, int w)
        {
            System.Windows.Controls.Grid grid;
            LogicSelector logic;
            Rectangle rect;

            // exit early if this squre is able to be selected
            if (IsSelectable[h, w]) return false;

            // get the logic 
            logic = GetSelector(h, w);
            logic.Clear();
            logic.Visibility = Visibility.Collapsed;

            // modify the opacity
            grid = GetGrid(h, w);
            grid.Opacity = UNMARKED_OPACITY;

            rect = GetRectangle(h, w);
            rect.Fill = WhiteBrush;

            return true;
        }

        private void MarkReturn(int h, int w)
        {
            if (h < 0 || h >= IsSelectable.GetLength(0) || w < 0 || w >= IsSelectable.GetLength(1)) throw new Exception("Out of bounds for IsSelectable : " + h + ", " + w);

            var rect = GetRectangle(h, w);
            rect.Fill = GreenBrush;
        }

        private void UnmarkReturn(int h, int w)
        {
            if (h < 0 || h >= IsSelectable.GetLength(0) || w < 0 || w >= IsSelectable.GetLength(1)) throw new Exception("Out of bounds for IsSelectable : " + h + ", " + w);

            var rect = GetRectangle(h, w);
            rect.Fill = GrayBrush;
        }

        private LogicSelector MarkSelectable(int h, int w)
        {
            if (h < 0 || h >= IsSelectable.GetLength(0) || w < 0 || w >= IsSelectable.GetLength(1)) throw new Exception("Out of bounds for IsSelectable : " + h + ", " + w);

            // mark as unselectable
            IsSelectable[h, w] = true;

            // clear the cell
            return EnableChooser(h, w);
        }

        private void UnmarkSelectable(int h, int w)
        {
            if (h < 0 || h >= IsSelectable.GetLength(0) || w < 0 || w >= IsSelectable.GetLength(1)) throw new Exception("Out of bounds for IsSelectable : " + h + ", " + w);

            // mark as selectable
            IsSelectable[h, w] = false;

            // clear the cell
            DisableChooser(h, w);
        }

        private void SwapLogic(int from, int to)
        {
            LogicSelector tempFrom = new LogicSelector();
            LogicSelector tempTo = new LogicSelector();
            bool fromSelectable, toSelectable;

            // move all the logic selectors from 'from' to 'to'

            // check if this is a legal swap
            LogicSelector flogic = GetSelector(from, 0);
            LogicSelector tlogic = GetSelector(to, 0);

            if (flogic.GetLogicType() == LogicType.None && tlogic.GetLogicType() == LogicType.None) return;

            // NASTY HACK! temporarily disable TypeChanged callback - or else all these changes will dramatically and negatively impact the swaps
            HACK_TemporarilyDisableTypeChangedCallback = true;

            for (int w = 0; w < IsSelectable.GetLength(1); w++)
            {
                flogic = GetSelector(from, w);
                tlogic = GetSelector(to, w);

                // make temp copy of content
                TransferDetails(flogic, tempFrom);
                TransferDetails(tlogic, tempTo);

                // store
                fromSelectable = IsSelectable[from, w];
                toSelectable = IsSelectable[to, w];

                // Unmark both
                UnmarkSelectable(from, w);
                UnmarkSelectable(to, w);

                // reenable if they were selectable
                if (fromSelectable)
                {
                    // if 'from' was selectable then the new 'to' is as well
                    TransferDetails(tempFrom, tlogic);
                    MarkSelectable(to, w);

                    if (tlogic.GetLogicType() == LogicType.Return) MarkReturn(tlogic.H, tlogic.W);
                }
                if (toSelectable)
                {
                    // if 'to' was selectable then the new 'from' is as well
                    TransferDetails(tempTo, flogic);
                    MarkSelectable(from, w);

                    if (flogic.GetLogicType() == LogicType.Return) MarkReturn(flogic.H, flogic.W);
                }
            }

            // always make sure that 0x0 is Selectable
            if (!IsSelectable[0, 0]) MarkSelectable(0, 0);

            // check that there is a selector at the end of the swap
            for (int h = 1; h < IsSelectable.GetLength(0); h++)
            {
                if (!IsSelectable[h, 0])
                {
                    LogicSelector logic = GetSelector(h - 1, 0);
                    if (logic.GetLogicType() != LogicType.None && logic.GetLogicType() != LogicType.Return) MarkSelectable(h, 0);
                    break;
                }
            }

            HACK_TemporarilyDisableTypeChangedCallback = false;
        }

        private void TransferDetails(LogicSelector from, LogicSelector to)
        {
            // NOTE H, W, and Color are not being transfered! 
            to.SetLogicType(from.GetLogicType());
            switch (from.GetLogicType())
            {
                case LogicType.Grid: to.SetGrid(from.GetGrid()); break;
                case LogicType.Matrix: to.SetMatrix(from.GetMatrix()); break;
                case LogicType.None: to.Clear(); break;
                case LogicType.Previous: to.SetPrevious(from.GetPrevious()); break;
                case LogicType.Random: to.SetRandom(from.GetRandom()); break;
                case LogicType.Return: to.SetReturn(from.GetReturn()); break;
                default: throw new Exception("Unkonwn logic type : " + from.GetLogicType());
            }
        }

        //
        // Callback events
        //

        void LogicSelector_OnTypeChanged(LogicSelector logic)
        {
            // HACK this is necessary to ensure that the swap can occur without the logic below kicking in -- ie we are breaking some of these rules
            if (HACK_TemporarilyDisableTypeChangedCallback) return;

            switch (logic.GetLogicType())
            {
                case LogicType.Return:
                    MarkReturn(logic.H, logic.W);

                    // mark all the squres on the right as unselectable
                    for (int w = logic.W + 1; w < IsSelectable.GetLength(1); w++)
                    {
                        UnmarkSelectable(logic.H, w);
                    }

                    // special case - if this return is the first on the list then mark all other rows as unselectable
                    if (logic.W == 0)
                    {
                        for (int h = logic.H + 1; h < IsSelectable.GetLength(0); h++)
                        {
                            for (int w = 0; w < IsSelectable.GetLength(1); w++)
                            {
                                UnmarkSelectable(h, w);
                            }
                        }
                    }
                    break;
                case LogicType.Delete:
                    LogicSelector lneighbor;

                    // delete this item and all the ones to the right
                    for (int w = logic.W; w < IsSelectable.GetLength(1); w++)
                    {
                        UnmarkSelectable(logic.H, w);
                    }

                    // a few special cases
                    if (logic.W == 0)
                    {
                        // specail case for the first selector
                        if (logic.H == 0) MarkSelectable(logic.H, logic.W);
                        else
                        {
                            // do not delete if there is real logic above this one
                            lneighbor = GetSelector(logic.H - 1, 0);
                            if (lneighbor.GetLogicType() != LogicType.None && lneighbor.GetLogicType() != LogicType.Return) MarkSelectable(logic.H, logic.W);

                            // do not delete if there is any logic below
                            if (logic.H + 1 < IsSelectable.GetLength(0))
                            {
                                if (IsSelectable[logic.H + 1, 0]) MarkSelectable(logic.H, logic.W);
                            }
                        }
                    }
                    else
                    {
                        // do not delete if the neighbor on the left is not blank
                        lneighbor = GetSelector(logic.H, logic.W - 1);
                        if (lneighbor.GetLogicType() != LogicType.None) MarkSelectable(logic.H, logic.W);
                    }

                    break;
                case LogicType.None:
                    UnmarkReturn(logic.H, logic.W);
                    break;
                default:
                    UnmarkReturn(logic.H, logic.W);

                    // mark the squre on the right as selectable
                    if (logic.W + 1 < IsSelectable.GetLength(1))
                    {
                        MarkSelectable(logic.H, logic.W + 1);
                    }

                    // ensure the first item of the next row is selectable
                    if (logic.H + 1 < IsSelectable.GetLength(0))
                    {
                        MarkSelectable(logic.H + 1, 0);
                    }
                    break;
            }
        }

        //
        // UI logic
        //

        private void Grid_Tapped(object sender, MouseButtonEventArgs e)
        {
        }

        private void Up_Tapped(object sender, MouseButtonEventArgs e)
        {
            // the transfer 'from' and 'to' are encoded in the image name
            ParseName((sender as Image).Name, out int from, out int to);

            if (from - 1 != to) throw new Exception("Incorect transition : " + from + " to " + to);

            SwapLogic(from, to);
        }

        private void Down_Tapped(object sender, MouseButtonEventArgs e)
        {
            // the transfer 'from' and 'to' are encoded in the image name
            ParseName((sender as Image).Name, out int from, out int to);

            if (from + 1 != to) throw new Exception("Incorect transition : " + from + " to " + to);

            SwapLogic(from, to);
        }

        private void DeleteAll_Tapped(object sender, MouseButtonEventArgs e)
        {
            ClearBoard();
        }
    }
}
