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
    public enum LogicType { None, Matrix, Grid, Previous, Random, Return, Delete };

    /// <summary>
    /// Interaction logic for LogicSelector.xaml
    /// </summary>
    public partial class LogicSelector : UserControl
    {
        private GridState[,] MyGrid;
        private PlotState[,] MyMatrix;
        private int MyRandom;
        private LogicType SelectedType;

        private const float VisitedOpacity = 0.40f;
        private const float OccupiedOpacity = 1.0f;
        private const float DuplicationOpacity = 0.75f;
        private const float DefendedOpacity = 0.9f;
        private const float UnoccupiedOpacity = 1.0f;
        private const float ExplosionOpacity = 0.6f;
        private const float ForbiddenOpacity = 0.8f;

        private SolidColorBrush TransparentPlotBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        private SolidColorBrush EnemyBrush = new SolidColorBrush(Colors.Black);
        private SolidColorBrush AnyBrush = new SolidColorBrush(Colors.Wheat);
        private SolidColorBrush WhiteBrush = new SolidColorBrush(Colors.White);
        private SolidColorBrush BlackPlotBrush = new SolidColorBrush(Colors.Black);
        private SolidColorBrush EnableStrokeBrush = new SolidColorBrush(Colors.Black);
        private SolidColorBrush ForbiddenBrush = new SolidColorBrush(Colors.LightGray);
        private SolidColorBrush ForbiddenStrokeBrush = new SolidColorBrush(Colors.White);

        public LogicSelector()
        {
            this.InitializeComponent();

            MyGrid = new GridState[3, 3];
            MyMatrix = new PlotState[5, 5];
            MyRandom = -1;
            H = W = -1;
            MyColor = PlotColor.Blue;

            SelectedType = LogicType.None;
        }

        //
        // public interface
        //

        public delegate void TypeChangedDelegate(LogicSelector logic);
        public event TypeChangedDelegate OnTypeChanged;

        public int H { get; private set; }
        public int W { get; private set; }

        public int DefaultHeight { get { return 125; } }
        public int DefaultWidth { get { return 175; } }

        public bool IsInit { get; private set; }

        public PlotColor MyColor { get; private set; }

        public void Initialize(int h, int w, PlotColor color, bool menu)
        {
            // setup context
            H = h;
            W = w;
            MyColor = color;

            Clear();

            // initialize the radio button groups
            Radio_2.GroupName = Radio_4.GroupName = Radio_8.GroupName = Radio_16.GroupName = Radio_32.GroupName = Radio_64.GroupName = h + "x" + w;

            // disable or enable the menu
            if (menu) Menu.Visibility = Visibility.Visible;
            else Menu.Visibility = Visibility.Collapsed;

            // end init
            IsInit = true;
        }

        public LogicType GetLogicType()
        {
            return SelectedType;
        }
        public void SetLogicType(LogicType type)
        {
            SelectedType = type;

            ClearAll();

            switch (type)
            {
                case LogicType.None: break;
                case LogicType.Return: ShowReturn(); break;
                case LogicType.Matrix: ShowMatrix(); break;
                case LogicType.Grid: ShowGrid(); break;
                case LogicType.Previous: ShowPrevious(); break;
                case LogicType.Random: ShowRandom(); break;
                case LogicType.Delete: break;
                default: throw new Exception("Invalid logic type : " + type);
            }

            if (OnTypeChanged != null) OnTypeChanged(this);
        }

        public Swarm.Matrix GetMatrix()
        {
            return new Swarm.Matrix(MyMatrix);
        }
        public void SetMatrix(Swarm.Matrix matrix)
        {
            for (int h = 0; h < Swarm.Matrix.Dimension; h++)
            {
                for (int w = 0; w < Swarm.Matrix.Dimension; w++)
                {
                    SetMatrixValue(matrix[h, w], h, w);
                }
            }
        }

        public Swarm.Grid GetGrid()
        {
            return new Swarm.Grid(MyGrid);
        }
        public void SetGrid(Swarm.Grid grid)
        {
            for (int h = 0; h < Swarm.Grid.Dimension; h++)
            {
                for (int w = 0; w < Swarm.Grid.Dimension; w++)
                {
                    SetGridValue(grid[h, w], h, w);
                }
            }
        }

        public Move GetReturn() { return ReturnLogic.GetValue(); }
        public void SetReturn(Move move) { ReturnLogic.SetValue(move); }

        public Move GetPrevious() { return PreviousLogic.GetValue(); }
        public void SetPrevious(Move move) { PreviousLogic.SetValue(move); }

        public int GetRandom()
        {
            if (MyRandom < 0) MyRandom = RawGetRandomValue();
            return MyRandom;
        }
        public void SetRandom(int val)
        {
            MyRandom = val;

            if (val == 2) Radio_2.IsChecked = true;
            else if (val == 4) Radio_4.IsChecked = true;
            else if (val == 8) Radio_8.IsChecked = true;
            else if (val == 16) Radio_16.IsChecked = true;
            else if (val == 32) Radio_32.IsChecked = true;
            else if (val == 64) Radio_64.IsChecked = true;
            else Radio_2.IsChecked = true;
        }

        public void Clear()
        {
            SetLogicType(LogicType.None);

            // clear values
            Radio_2.IsChecked = true;
            ReturnLogic.SetValue(Move.Nothing);
            PreviousLogic.SetValue(Move.Nothing);
            for (int h = 0; h < MyGrid.GetLength(0); h++)
                for (int w = 0; w < MyGrid.GetLength(1); w++)
                    SetGridValue(GridState.Any, h, w);
            SetGridValue(GridState.Occupied, 1, 1);
            for (int h = 0; h < MyMatrix.GetLength(0); h++)
                for (int w = 0; w < MyMatrix.GetLength(1); w++)
                    SetMatrixValue(PlotState.Any, h, w);
            SetMatrixValue(PlotState.Occupied, 2, 2);
        }

        public void ShowChooser(bool lower)
        {
            SetLogicType(LogicType.None);
            if (lower)
            {
                ChooserTop.Visibility = Visibility.Collapsed;
                ChooserBottom.Visibility = Visibility.Visible;
            }
            else
            {
                ChooserTop.Visibility = Visibility.Visible;
                ChooserBottom.Visibility = Visibility.Collapsed;
            }
        }

        public void ShowChooser()
        {
            ShowChooser(false);
        }

        public void HideChooser()
        {
            ChooserTop.Visibility = Visibility.Collapsed;
            ChooserBottom.Visibility = Visibility.Collapsed;
        }

        //
        // Utility
        //

        private int RawGetRandomValue()
        {
            if (Radio_2.IsChecked.Value) return 2;
            if (Radio_4.IsChecked.Value) return 4;
            if (Radio_8.IsChecked.Value) return 8;
            if (Radio_16.IsChecked.Value) return 16;
            if (Radio_32.IsChecked.Value) return 32;
            if (Radio_64.IsChecked.Value) return 64;
            else return 2;
        }

        private void ShowLowerChooser()
        {
            // clear the combobox
            ContextValue.SelectedIndex = -1;

            ChooserTop.Visibility = Visibility.Collapsed;
            ChooserBottom.Visibility = Visibility.Visible;
        }

        private void ParseName(string name, out int h, out int w)
        {
            string[] parts = name.Split(new char[] { '_' })[1].Split(new char[] { 'x' });

            h = Convert.ToInt32(parts[0]);
            w = Convert.ToInt32(parts[1]);
        }

        private Rectangle GetRectangle(string name, int h, int w)
        {
            return (this.FindName(name + "_" + h + "x" + w) as Rectangle);
        }

        private void ToggleMatrixValue(Rectangle rect, int h, int w)
        {
            SetMatrixValue(rect, NextMatrixValue(MyMatrix[h, w]), h, w);
        }

        private PlotState NextMatrixValue(PlotState state)
        {
            switch (state)
            {
                case PlotState.Forbidden: return PlotState.Unoccupied;
                case PlotState.Unoccupied: return PlotState.Occupied;
                case PlotState.Occupied: return PlotState.Visited;
                case PlotState.Visited: return PlotState.Enemy;
                case PlotState.Enemy: return PlotState.Any;
                case PlotState.Any: return PlotState.Forbidden;
                default: throw new Exception("Unknown state : " + state);
            }
        }

        private void SetMatrixValue(PlotState state, int h, int w)
        {
            SetMatrixValue(GetRectangle("Matrix", h, w), state, h, w);
        }

        private void SetMatrixValue(Rectangle rect, PlotState state, int h, int w)
        {
            SolidColorBrush brush = GetPlotColor(MyColor);
            SolidColorBrush stroke = EnableStrokeBrush;
            double opacity = 0.0;

            // store the state
            MyMatrix[h, w] = state;

            switch (MyMatrix[h, w])
            {
                case PlotState.Visited: opacity = VisitedOpacity; break;
                case PlotState.Occupied: opacity = OccupiedOpacity; break;
                case PlotState.Unoccupied: opacity = UnoccupiedOpacity; brush = WhiteBrush; break;
                case PlotState.Forbidden: opacity = ForbiddenOpacity; brush = ForbiddenBrush; stroke = ForbiddenStrokeBrush; break;
                case PlotState.Enemy: opacity = OccupiedOpacity; brush = EnemyBrush; break;
                case PlotState.Any: opacity = 1.0; brush = AnyBrush; break;
                case PlotState.Duplication: opacity = DuplicationOpacity; break;
                case PlotState.Defended: opacity = DefendedOpacity; break;
                default: throw new Exception("Unknown state : " + MyMatrix[h, w]);
            }

            // set the values
            rect.Fill = brush;
            rect.Opacity = opacity;
            rect.Stroke = stroke;
        }

        private void ToggleGridValue(Rectangle rect, int h, int w)
        {
            SetGridValue(rect, NextGridValue(MyGrid[h, w]), h, w);
        }

        private GridState NextGridValue(GridState state)
        {
            switch (state)
            {
                case GridState.Forbidden: return GridState.Unoccupied;
                case GridState.Unoccupied: return GridState.Occupied;
                case GridState.Occupied: return GridState.Any;
                case GridState.Any: return GridState.Forbidden;
                default: throw new Exception("Unknown state : " + state);
            }
        }

        private void SetGridValue(GridState state, int h, int w)
        {
            SetGridValue(GetRectangle("Grid", h, w), state, h, w);
        }

        private void SetGridValue(Rectangle rect, GridState state, int h, int w)
        {
            SolidColorBrush brush = GetPlotColor(MyColor);
            SolidColorBrush stroke = EnableStrokeBrush;
            double opacity = 0.0;

            // store the state
            MyGrid[h, w] = state;

            switch (MyGrid[h, w])
            {
                case GridState.Occupied: opacity = OccupiedOpacity; break;
                case GridState.Unoccupied: opacity = UnoccupiedOpacity; brush = WhiteBrush; break;
                case GridState.Forbidden: opacity = ForbiddenOpacity; brush = ForbiddenBrush; stroke = ForbiddenStrokeBrush; break;
                case GridState.Any: opacity = 1.0; brush = AnyBrush; break;
                default: throw new Exception("Unknown state : " + MyGrid[h, w]);
            }

            // set the values
            rect.Fill = brush;
            rect.Opacity = opacity;
            rect.Stroke = stroke;
        }

        private void ClearAll()
        {
            HideChooser();

            ReturnLogic.Visibility = Visibility.Collapsed;
            MatrixLogic.Visibility = Visibility.Collapsed;
            GridLogic.Visibility = Visibility.Collapsed;
            PreviousLogic.Visibility = Visibility.Collapsed;
            RandomLogic.Visibility = Visibility.Collapsed;
        }

        private void ShowReturn()
        {
            HideChooser();
            ReturnLogic.Visibility = Visibility.Visible;
        }

        private void ShowMatrix()
        {
            HideChooser();
            MatrixLogic.Visibility = Visibility.Visible;
        }

        private void ShowGrid()
        {
            HideChooser();
            GridLogic.Visibility = Visibility.Visible;
        }

        private void ShowPrevious()
        {
            HideChooser();
            PreviousLogic.Visibility = Visibility.Visible;
        }

        private void ShowRandom()
        {
            HideChooser();
            RandomLogic.Visibility = Visibility.Visible;
        }

        private SolidColorBrush GetPlotColor(PlotColor color)
        {
            switch (color)
            {
                case PlotColor.Red: return Utility.RedPlotBrush;
                case PlotColor.Blue: return Utility.BluePlotBrush;
                case PlotColor.Green: return Utility.GreenPlotBrush;
                case PlotColor.Yellow: return Utility.YellowPlotBrush;
                case PlotColor.Clear: return Utility.BluePlotBrush;
                default: throw new Exception("Unknown color : " + color);
            }
        }

        //
        // UI logic
        // 

        private void MatrixValue_Tapped(object sender, MouseButtonEventArgs e)
        {
            int h, w;
            Rectangle rect = (sender as Rectangle);

            ParseName(rect.Name, out h, out w);

            if (h == 2 && w == 2) { }
            else ToggleMatrixValue(rect, h, w);

            e.Handled = true;
        }

        private void GridValue_Tapped(object sender, MouseButtonEventArgs e)
        {
            int h, w;
            Rectangle rect = (sender as Rectangle);

            ParseName(rect.Name, out h, out w);

            if (h == 1 && w == 1) { }
            else ToggleGridValue(rect, h, w);

            e.Handled = true;
        }

        private void Return_Tapped(object sender, RoutedEventArgs e)
        {
            SetLogicType(LogicType.Return);
            e.Handled = true;
        }

        private void Previous_Tapped(object sender, RoutedEventArgs e)
        {
            SetLogicType(LogicType.Previous);
            e.Handled = true;
        }

        private void Random_Tapped(object sender, MouseButtonEventArgs e)
        {
            SetLogicType(LogicType.Random);
            e.Handled = true;
        }

        private void Matrix_Tapped(object sender, MouseButtonEventArgs e)
        {
            SetLogicType(LogicType.Matrix);
            e.Handled = true;
        }

        private void Grid_Tapped(object sender, MouseButtonEventArgs e)
        {
            SetLogicType(LogicType.Grid);
            e.Handled = true;
        }

        private void Delete_Tapped(object sender, RoutedEventArgs e)
        {
            SetLogicType(LogicType.Delete);
            e.Handled = true;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (ContextValue.SelectedIndex)
            {
                case -1: break; // special value to reset
                case 0: SetLogicType(LogicType.Grid); break;
                case 1: SetLogicType(LogicType.Matrix); break;
                case 2: SetLogicType(LogicType.Previous); break;
                case 3: SetLogicType(LogicType.Random); break;
                default: throw new Exception("Unknown selection index : " + ContextValue.SelectedIndex);
            }
        }

        private void Context_Tapped(object sender, RoutedEventArgs e)
        {
            ShowLowerChooser();
            e.Handled = true;
        }

        private void MainGrid_Tapped(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        private void Reset_Tapped(object sender, RoutedEventArgs e)
        {
            ShowChooser();
            e.Handled = true;
        }

        private void Random_Click(object sender, RoutedEventArgs e)
        {
            MyRandom = RawGetRandomValue();
        }
    }
}
