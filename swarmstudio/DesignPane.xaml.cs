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
    /// Interaction logic for DesignPane.xaml
    /// </summary>
    public partial class DesignPane : Page
    {
        private LevelID MyLevelID;
        private string MyScript;
        private PlotColor MyColor;
        private List<ScriptOverride> MyScripts;
        private bool DebugPaneIsPressed;
        private Point DebugPaneLocation;
        private bool IsSingleStep;
        private bool IsBattle;

        private float DesignerScaleFactor = 1.0f;
        private float SurfaceScaleFactor = 1.0f;
        private const float ScaleIncrement = 0.1f;

        private bool VerResizeUp;

        public static DesignPane CachedDesignPane;

        public DesignPane()
        {
            this.InitializeComponent();

            Loaded += WhenLoaded;

            Execution.OnGetScript += Execution_GetScript;
            Execution.OnComplete += Execution_OnComplete;
            Execution.OnError += Execution_OnError;
            Execution.OnInspectionEnabled += Execution_OnInspectionEnabled;
            Execution.OnInspection += Execution_OnInspection;

            DoneScreen.OnGoMenu += DoneScreen_OnGoMenu;
            DoneScreen.OnGoNext += DoneScreen_OnGoNext;
            DoneScreen.OnReplay += DoneScreen_OnReplay;
            DoneScreen.OnSumbitRating += DoneScreen_OnSumbitRating;
            DoneScreen.OnSumbitScript += DoneScreen_OnSumbitScript;

            TutorialPane.OnComplete += TutorialPane_OnComplete;
            TutorialPane.OnDisplayMove += TutorialPane_OnDisplayMove;
            TutorialPane.OnDisplayContext += TutorialPane_OnDisplayContext;
            TutorialPane.OnDisplayDebugInfo += TutorialPane_OnDisplayDebugInfo;
        }

        public void Initialize(LevelID id, PlotColor mycolor, bool isbattle = false, List<ScriptOverride> scripts = null)
        {
            MyLevelID = id;
            MyColor = mycolor;
            IsBattle = isbattle;
            MyScripts = scripts;

            VerResizeUp = false;
            DebugPaneIsPressed = false;
            IsSingleStep = false;

            // populate the submit and rate screen
            DoneScreen.ClearScripts();
            if (MyScripts != null)
            {
                foreach (var so in MyScripts)
                {
                    if (!so.IsUser) DoneScreen.AddScript(so.Name, so.Id);
                }
            }
        }

        private void WhenLoaded(object sender, RoutedEventArgs e)
        {
            // initialize
            Init(MyLevelID, MyScripts);

            // show help?
            if (MyLevelID == LevelID.World_1_1 || MyLevelID == LevelID.World_1_2) ShowHelp();
            else HideHelp();

            // show tutorial?
            if (MyLevelID == LevelID.World_1_1) ShowTutorial();
        }

        //
        // Utility
        //

        private void Init(LevelID id, List<ScriptOverride> overrides)
        {
            // update state
            MyLevelID = id;
            MyScript = "";

            // grab the script if one exists
            if (SavedGamesRepsoitory.Loaded(id))
            {
                MyScript = SavedGamesRepsoitory.GetScript(id);
            }

            // initialize
            HideAll();
            Execution.Initialize(MyLevelID, overrides);
            // initialize color for visual designer
            VisualDesigner.Initialize(MyColor);

            if (!VisualDesigner.SetScript(MyScript))
            {
                MyScript = "RETURN [0]";
                if (!VisualDesigner.SetScript(MyScript)) throw new Exception("Failed to set script with RETURN [0]");
            }

            // set title
            Title.Text = id.ToString().Replace("_", " ");

            // move the scroll bars to the right place
            DesignerPane.ScrollToVerticalOffset(0);
            DesignerPane.ScrollToHorizontalOffset(0);

            // reset execution pane
            ExecutionPane.ScrollToHorizontalOffset(0);
            ExecutionPane.ScrollToVerticalOffset(0);
        }

        private void InternalGetScript()
        {
            if (VisualDesigner.Visibility == Visibility.Visible) MyScript = VisualDesigner.GetScript();
            //if (TextDesigner.Visibility == Visibility.Visible) MyScript = TextDesigner.GetScript();

            System.Diagnostics.Debug.WriteLine(MyScript);

            // save
            SavedGamesRepsoitory.Store(MyLevelID, 0, MyScript, true /* unlocked */, 0 /* attempts */);
        }

        private void HideAll()
        {
            // hide all the ephmerial panes
            //TextDesigner.Visibility = Visibility.Collapsed;
            DonePane.Visibility = Visibility.Collapsed;
            ErrorPane.Visibility = Visibility.Collapsed;
            DebugPane.Visibility = Visibility.Collapsed;
        }

        private void ShowTextEditor()
        {
            // pass along the script
            InternalGetScript();
            //TextDesigner.SetScript(MyScript);

            // make the right one visible
            //TextDesigner.Visibility = Visibility.Visible;
        }

        private void ShowError(string hint)
        {
            ErrorHint.Text = hint;
            ErrorPane.Visibility = Visibility.Visible;
        }

        private void GoBack()
        {
            // make sure to save
            SavedGamesRepsoitory.Store(MyLevelID, 0, MyScript, true /* unlocked */, 0 /* attempts */);

            if (MyLevelID == LevelID.World_1_1 || MyLevelID == LevelID.World_1_2 || MyLevelID == LevelID.World_1_3 || MyLevelID == LevelID.World_1_4 || MyLevelID == LevelID.World_1_5 || MyLevelID == LevelID.World_1_6 || MyLevelID == LevelID.World_1_7 || MyLevelID == LevelID.World_1_8 || MyLevelID == LevelID.World_1_9 || MyLevelID == LevelID.World_1_10 || MyLevelID == LevelID.World_1_11 || MyLevelID == LevelID.World_1_12 || MyLevelID == LevelID.World_1_13 || MyLevelID == LevelID.World_1_14 || MyLevelID == LevelID.World_1_15 || MyLevelID == LevelID.World_1_16)
            {
                this.NavigationService.Navigate(new World1());
            }
            else if (MyLevelID == LevelID.World_2_1 || MyLevelID == LevelID.World_2_2 || MyLevelID == LevelID.World_2_3 || MyLevelID == LevelID.World_2_4 || MyLevelID == LevelID.World_2_5 || MyLevelID == LevelID.World_2_6 || MyLevelID == LevelID.World_2_7 || MyLevelID == LevelID.World_2_8 || MyLevelID == LevelID.World_2_9 || MyLevelID == LevelID.World_2_10 || MyLevelID == LevelID.World_2_11 || MyLevelID == LevelID.World_2_12 || MyLevelID == LevelID.World_2_13 || MyLevelID == LevelID.World_2_14 || MyLevelID == LevelID.World_2_15 || MyLevelID == LevelID.World_2_16)
            {
                this.NavigationService.Navigate(new World2()); 
            }
            else if (MyLevelID == LevelID.World_3_1 || MyLevelID == LevelID.World_3_2 || MyLevelID == LevelID.World_3_3 || MyLevelID == LevelID.World_3_4 || MyLevelID == LevelID.World_3_5 || MyLevelID == LevelID.World_3_6 || MyLevelID == LevelID.World_3_7 || MyLevelID == LevelID.World_3_8 || MyLevelID == LevelID.World_3_9 || MyLevelID == LevelID.World_3_10 || MyLevelID == LevelID.World_3_11 || MyLevelID == LevelID.World_3_12 || MyLevelID == LevelID.World_3_13 || MyLevelID == LevelID.World_3_14 || MyLevelID == LevelID.World_3_15 || MyLevelID == LevelID.World_3_16)
            {
                this.NavigationService.Navigate(new World3());
            }
            else if (MyLevelID == LevelID.Bonus_1 || MyLevelID == LevelID.Bonus_2 || MyLevelID == LevelID.Bonus_3 || MyLevelID == LevelID.Bonus_4 || MyLevelID == LevelID.Battle_Hill || MyLevelID == LevelID.Battle_Maze || MyLevelID == LevelID.Battle_Open || MyLevelID == LevelID.Battle_Quad)
            {
                this.NavigationService.Navigate(new Battle());
            }
            else
                throw new Exception("Unknown level id : " + MyLevelID);
        }

        private void GoNext()
        {
            LevelID next = LevelDesign.GetNextLevel(MyLevelID);

            if (next == LevelID.Blank) GoBack();
            else Init(next, null);
        }

        private void Run()
        {
            IsSingleStep = false;
            if (Execution.IsStarted) Execution.Reset();
            Execution.Start(true);
        }

        private void RunOneStep()
        {
            if (!Execution.IsInit)
            {
                throw new Exception("Execution is not iniatialized");
            }

            // check if the script has changed
            if (IsSingleStep)
            {
                string tmpScript = MyScript;
                InternalGetScript();

                // if not equal then reset single stepping
                if (!MyScript.Equals(tmpScript)) IsSingleStep = false;
            }

            if (!Execution.IsStarted || !IsSingleStep)
            {
                Execution.Reset();
                Execution.Start(false /* backgroun */);
                IsSingleStep = true;
            }
            Execution.SingleStep();
        }

        private void Reset()
        {
            // hide the top most screens
            HideAll();

            // reset
            Execution.Reset();
        }

        private void HideHelp()
        {
            HelpPane.Visibility = Visibility.Collapsed;
            VerResizeBar.Width = 1341;  // 1366 - 25 (hor bar)
            ExecutionPane.Width = 1341;
            DesignerPane.Width = 1341;
            DonePane.Width = 1341;
            ErrorPane.Width = 1341;
        }

        private void ShowHelp()
        {
            HelpPane.Visibility = Visibility.Visible;
            VerResizeBar.Width = 1041;  // 1366 - 300 (help width) - 25 (hor bar)
            ExecutionPane.Width = 1041;
            DesignerPane.Width = 1041;
            DonePane.Width = 1041;
            ErrorPane.Width = 1041;
        }

        private void ShowTutorial()
        {
            TutorialPane.Visibility = Visibility.Visible;
        }

        private void HideTutorial()
        {
            TutorialPane.Visibility = Visibility.Collapsed;
        }

        private void ShowDebugPane()
        {
            DebugPane.Visibility = Visibility.Visible;
        }

        private void HideDebugPane()
        {
            DebugPane.Visibility = Visibility.Collapsed;
        }

        //
        // Callbacks
        //
        void Execution_OnComplete(int scriptLength, int optimalScriptLength, int iterations, int optimalIterations, double rating, bool success)
        {
            int stars = Utility.RatingToStars(rating);

            System.Diagnostics.Debug.WriteLine("Winner? : " + success + " script length : " + scriptLength + " Iterations : " + iterations + " Rating : " + rating + " Stars : " + stars);

            // show the complete screen
            DoneScreen.Show(success, rating, stars, scriptLength, optimalScriptLength, iterations, optimalIterations);
            DonePane.Visibility = Visibility.Visible;

            // record this level details
            SavedGamesRepsoitory.Store(MyLevelID, stars, MyScript, true /* unlocked */, 1);

            // unlock the next one
            LevelID nxtLevel = LevelDesign.GetNextLevel(MyLevelID);
            if (nxtLevel != LevelID.Blank) SavedGamesRepsoitory.Store(nxtLevel, 0, "", success /* unlocked */, 0);

            // store the level usage
            DataService.StoreLevelData((int)MyLevelID, rating, MyScript, success, iterations, scriptLength);

            // decide if we should show the submit and rate screen
            if (IsBattle && DataService.IsAuthenticated)
            {
                if (success || DoneScreen.Scripts > 0)
                {
                    // show upload script only if the script was successful
                    DoneScreen.ShowScriptRating(success /*show upload */);
                }
            }
        }

        private string Execution_GetScript()
        {
            InternalGetScript();
            return MyScript;
        }

        void Execution_OnError(string keyValuePairs)
        {
            Dictionary<string, string> values = new Dictionary<string, string>();
            string[] pairs;

            // parse the keyValuePairs by ';' and then '='
            if (keyValuePairs.Contains(";")) pairs = keyValuePairs.Split(new char[] { ';' });
            else pairs = new string[1] { keyValuePairs };

            foreach (string val in pairs)
            {
                if (val.Contains("="))
                {
                    string[] parts = val.Split(new char[] { '=' });
                    if (parts.Length == 2) values.Add(parts[0], parts[1]);
                }
            }

            // show the hint
            if (values.ContainsKey("LineNumber")) ShowError("Error is around line " + values["LineNumber"]);
            else if (values.ContainsKey("Script")) ShowError(values["Script"]);
            else if (values.ContainsKey("ErrorLine"))
            {
                int line = 0;
                Int32.TryParse(values["ErrorLine"], out line);
                line++;
                ShowError("Error is around line " + line);
            }
            else ShowError("Check elements at the boundaries");
        }

        void Execution_OnInspectionEnabled()
        {
            // show the debug pane
            ShowDebugPane();
        }

        void Execution_OnInspection(PlotColor color, Swarm.Matrix matrix, Swarm.Grid grid, Move previous)
        {
            DebugMatrix.Initialize(0, 0, color, false /* menu */);
            DebugMatrix.SetLogicType(LogicType.Matrix);
            DebugMatrix.SetMatrix(matrix);

            DebugGrid.Initialize(0, 1, color, false /* menu */);
            DebugGrid.SetLogicType(LogicType.Grid);
            DebugGrid.SetGrid(grid);

            DebugPrevious.Text = previous + "";
        }

        private void DoneScreen_OnReplay()
        {
            Reset();
        }

        private void DoneScreen_OnGoNext()
        {
            GoNext();
        }

        private void DoneScreen_OnGoMenu()
        {
            GoBack();
        }

        void DoneScreen_OnSumbitScript(string scriptName)
        {
            DataService.StoreScriptData((int)MyLevelID, (int)MyColor, scriptName, MyScript);
        }

        void DoneScreen_OnSumbitRating(int id, bool like)
        {
            DataService.UpdateScriptData(id, like);
        }

        void TutorialPane_OnComplete()
        {
            HideTutorial();
        }

        void TutorialPane_OnDisplayContext()
        {
            VisualDesigner.DemoShowContext();
        }

        void TutorialPane_OnDisplayMove()
        {
            VisualDesigner.DemoShowMove();
        }

        void TutorialPane_OnDisplayDebugInfo()
        {
            // TODO! NYI
        }


        //
        // UI logic
        //

        private void Text_Tapped(object sender, MouseButtonEventArgs e)
        {
            ShowTextEditor();
        }

        private void Hor_Resize_Tapped(object sender, MouseButtonEventArgs e)
        {
            if (HelpPane.Visibility == Visibility.Collapsed)
            {
                ShowHelp();
            }
            else
            {
                HideHelp();
            }
        }

        private void Run_Tapped(object sender, RoutedEventArgs e)
        {
            Run();
        }

        private void Back_Tapped(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void Reset_Tapped(object sender, RoutedEventArgs e)
        {
            Reset();
        }

        private void Single_Tapped(object sender, RoutedEventArgs e)
        {
            RunOneStep();
        }

        private void Ver_Resize_Tapped(object sender, MouseButtonEventArgs e)
        {
            // middle
            if (DesignerPane.Visibility == Visibility.Collapsed || ExecutionPane.Visibility == Visibility.Collapsed)
            {
                DesignerPane.Height = 372;    // 0.5 * 768 - 25 (ver bar)
                ExecutionPane.Height = 372;         // 0.5 * 768 - 25 (ver bar)

                ExecutionPane.Visibility = Visibility.Visible;
                DesignerPane.Visibility = Visibility.Visible;
                Menu.Visibility = Visibility.Visible;
                Title.Visibility = Visibility.Visible;
            }
            // going up
            else if (VerResizeUp)
            {
                DesignerPane.Height = 743; // 768 - 25 (ver bar)
                ExecutionPane.Visibility = Visibility.Collapsed;
                Menu.Visibility = Visibility.Collapsed;
                VerResizeUp = false;
                Title.Visibility = Visibility.Collapsed;
            }
            // going down
            else
            {
                ExecutionPane.Height = 743; // 768 - 25 (ver bar)
                DesignerPane.Visibility = Visibility.Collapsed;
                Menu.Visibility = Visibility.Visible;
                VerResizeUp = true;
                Title.Visibility = Visibility.Visible;
            }
        }

        private void DesignerPane_PointerMoved(object sender, MouseEventArgs e)
        {
            e.Handled = false;
        }

        private void DesignerPane_PointerPressed(object sender, MouseButtonEventArgs e)
        {
            e.Handled = false;
        }

        private void Grid_KeyDown(object sender, KeyboardEventArgs e)
        {
        }

        private void Close_Tapped(object sender, RoutedEventArgs e)
        {
            HideAll();
        }

        private void DebugPane_Moved(object sender, MouseEventArgs e)
        {
            if (DebugPaneIsPressed)
            {
                TranslateTransform t;
                Point current;

                t = (DebugPane.RenderTransform as TranslateTransform);

                current = e.GetPosition(MainGrid);

                t.X += current.X - DebugPaneLocation.X;
                t.Y += current.Y - DebugPaneLocation.Y;

                DebugPaneLocation = current;
            }
        }

        private void DebugPane_Pressed(object sender, MouseEventArgs e)
        {
            DebugPaneIsPressed = true;
            DebugPaneLocation = e.GetPosition(MainGrid);
            e.Handled = false;
        }

        private void DebugPane_Released(object sender, MouseButtonEventArgs e)
        {
            DebugPaneIsPressed = false;
        }

        private void VisualDesigner_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                // zoom in/out
                if (e.Delta < 0) DesignerScaleFactor -= ScaleIncrement;
                else DesignerScaleFactor += ScaleIncrement;

                // ensure it does not exceed normal range
                if (DesignerScaleFactor > 1f) DesignerScaleFactor = 1f;
                else if (DesignerScaleFactor < ScaleIncrement) DesignerScaleFactor = ScaleIncrement;

                // zoom the control in and out
                var scale = VisualDesigner.RenderTransform as ScaleTransform;
                scale.ScaleX = DesignerScaleFactor;
                scale.ScaleY = DesignerScaleFactor;
                e.Handled = true;
            }
        }

        private void Execution_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.RightCtrl) || Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                // zoom in/out
                if (e.Delta < 0) SurfaceScaleFactor -= ScaleIncrement;
                else SurfaceScaleFactor += ScaleIncrement;

                // ensure it does not exceed normal range
                if (SurfaceScaleFactor > 1f) SurfaceScaleFactor = 1f;
                else if (SurfaceScaleFactor < ScaleIncrement) SurfaceScaleFactor = ScaleIncrement;

                // zoom the control in and out
                var scale = Execution.RenderTransform as ScaleTransform;
                scale.ScaleX = SurfaceScaleFactor;
                scale.ScaleY = SurfaceScaleFactor;
                e.Handled = true;
            }
        }
    }
}
