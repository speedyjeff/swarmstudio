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
    /// Interaction logic for BattleSelector.xaml
    /// </summary>
    public partial class BattleSelector : UserControl
    {
        private LevelID MyLevel;
        private PlotColor SelectedColor;
        private Scripts[] Scripts;

        // choose logic
        private int Start;
        private int Count = 10; // defined in ScriptViewer.xaml
        private bool CanAdvance;

        public BattleSelector()
        {
            this.InitializeComponent();

            MyLevel = LevelID.Battle_Hill;
            SelectedColor = PlotColor.Clear;
            Scripts = null;
            Start = 0;
            CanAdvance = false;

            BlueExpander.Init("BLUE", Utility.BluePlotBrush, defaultUser: true);
            BlueExpander.OnSelection += BlueExpander_OnSelection;

            RedExpander.Init("RED", Utility.RedPlotBrush, defaultUser: false);
            RedExpander.OnSelection += RedExpander_OnSelection;

            YellowExpander.Init("YELLOW", Utility.YellowPlotBrush, defaultUser: false);
            YellowExpander.OnSelection += YellowExpander_OnSelection;

            GreenExpander.Init("GREEN", Utility.GreenPlotBrush, defaultUser: false);
            GreenExpander.OnSelection += GreenExpander_OnSelection;

            ChoiceViewer.OnBack += ChoiceViewer_OnBack;
            ChoiceViewer.OnForward += ChoiceViewer_OnForward;
            //ChoiceViewer.OnSelect += ChoiceViewer_OnSelect;
        }

        //
        // callbacks
        //

        void ChoiceViewer_OnSelect(int index)
        {
            SetScript(SelectedColor, index);
        }

        void ChoiceViewer_OnForward()
        {
            if (CanAdvance)
            {
                Start += Count;
                PopulateScriptPanel(SelectedColor, Start);
            }
        }

        void ChoiceViewer_OnBack()
        {
            if (Start > 0)
            {
                Start -= Count;
                PopulateScriptPanel(SelectedColor, Start);
            }
        }

        void GreenExpander_OnSelection(bool populate)
        {
            SelectedColor = PlotColor.Green;
            SelectionChange(SelectedColor, populate);
        }

        void YellowExpander_OnSelection(bool populate)
        {
            SelectedColor = PlotColor.Yellow;
            SelectionChange(SelectedColor, populate);
        }

        void RedExpander_OnSelection(bool populate)
        {
            SelectedColor = PlotColor.Red;
            SelectionChange(SelectedColor, populate);
        }

        void BlueExpander_OnSelection(bool populate)
        {
            SelectedColor = PlotColor.Blue;
            SelectionChange(SelectedColor, populate);
        }

        // 
        // public interface
        //

        public delegate void StartLevelDelegate(LevelID level, PlotColor color, List<ScriptOverride> overrides);
        public event StartLevelDelegate OnStartLevel;

        public LevelID Level
        {
            get
            {
                return MyLevel;
            }
            set
            {
                MyLevel = value;
            }
        }

        public void Show(LevelID level)
        {
            Level = level;

            // reset
            BlueExpander.Reset();
            RedExpander.Reset();
            YellowExpander.Reset();
            GreenExpander.Reset();

            // setup choose selector
            ClearChoiceViewer();

            // show
            this.Visibility = Visibility.Visible;
        }

        public void Hide()
        {
            this.Visibility = Visibility.Collapsed;
        }

        //
        // Utility
        //
        private ExpandChooser GetChooser(PlotColor color)
        {
            switch (color)
            {
                case PlotColor.Blue: return BlueExpander;
                case PlotColor.Green: return GreenExpander;
                case PlotColor.Red: return RedExpander;
                case PlotColor.Yellow: return YellowExpander;
                default: throw new Exception("Unknown color: " + color);
            }
        }

        private void SelectionChange(PlotColor color, bool populate)
        {
            // ensure only 1 can be a user
            ExpandChooser chooser = GetChooser(color);
            if (chooser.IsUser)
            {
                switch (color)
                {
                    case PlotColor.Blue:
                        GreenExpander.ToggleCreate();
                        RedExpander.ToggleCreate();
                        YellowExpander.ToggleCreate();
                        break;
                    case PlotColor.Green:
                        BlueExpander.ToggleCreate();
                        RedExpander.ToggleCreate();
                        YellowExpander.ToggleCreate();
                        break;
                    case PlotColor.Red:
                        GreenExpander.ToggleCreate();
                        BlueExpander.ToggleCreate();
                        YellowExpander.ToggleCreate();
                        break;
                    case PlotColor.Yellow:
                        GreenExpander.ToggleCreate();
                        RedExpander.ToggleCreate();
                        BlueExpander.ToggleCreate();
                        break;
                    default: throw new Exception("Unknown color: " + color);
                }
            }

            // reset
            ClearChoiceViewer();

            if (populate)
            {
                chooser.ScriptName = "";
                PopulateScriptPanel(color, Start);
            }
        }

        private void ClearChoiceViewer()
        {
            ChoiceViewer.Reset();
            Start = 0;
            CanAdvance = false;

            // currently not supported
            Scripts = new Scripts[]
            {
                new Scripts() { Name = "currently disabled", Color = (int)PlotColor.Yellow }
            };
            ChoiceViewer.Initalize(Scripts, 1);
        }

        private void PopulateScriptPanel(PlotColor color, int start)
        {
            return;
            /* disabled
            IEnumerable<Scripts> e = DataService.GetScriptData((int)color, (int)MyLevel, start, Count);
            Scripts = e.ToArray<Scripts>();

            ChoiceViewer.Initalize(Scripts, start + 1);

            // mark if you can advance
            CanAdvance = (Scripts.Length == Count);
            */
        }

        private void SetScript(PlotColor color, int index)
        {
            if (Scripts == null || index >= Scripts.Length) throw new Exception("Invalid Scripts array");

            ExpandChooser chooser = GetChooser(color);

            // store pertinent details
            chooser.Script = Scripts[index].Script;
            chooser.Id = Scripts[index].Id;
            chooser.ScriptName = Scripts[index].Name;
        }

        private void CollectAndStart()
        {
            PlotColor mycolor = PlotColor.Clear;
            List<ScriptOverride> overrides = new List<ScriptOverride>();

            // grab the default color
            if (BlueExpander.IsUser) mycolor = PlotColor.Blue;
            if (RedExpander.IsUser) mycolor = PlotColor.Red;
            if (GreenExpander.IsUser) mycolor = PlotColor.Green;
            if (YellowExpander.IsUser) mycolor = PlotColor.Yellow;

            if (mycolor == PlotColor.Clear)
            {
                MessageBox.Show("Please select one script as 'Create Script'");
                return;
            }

            // add user
            overrides.Add(new ScriptOverride() { Color = mycolor, IsUser = true });

            // grab the blue script
            if (!string.IsNullOrWhiteSpace(BlueExpander.Script)) overrides.Add(new ScriptOverride() { Color = PlotColor.Blue, Script = BlueExpander.Script, IsUser = false, Id = BlueExpander.Id, Name = BlueExpander.ScriptName });
            else if (BlueExpander.IsDisabled) overrides.Add(new ScriptOverride() { Color = PlotColor.Blue, IsUser = false, Id = BlueExpander.Id, IsDisabled = true });

            // grab the red script
            if (!string.IsNullOrWhiteSpace(RedExpander.Script)) overrides.Add(new ScriptOverride() { Color = PlotColor.Red, Script = RedExpander.Script, IsUser = false, Id = RedExpander.Id, Name = RedExpander.ScriptName });
            else if (RedExpander.IsDisabled) overrides.Add(new ScriptOverride() { Color = PlotColor.Red, IsUser = false, Id = RedExpander.Id, IsDisabled = true });

            // grab the green script
            if (!string.IsNullOrWhiteSpace(GreenExpander.Script)) overrides.Add(new ScriptOverride() { Color = PlotColor.Green, Script = GreenExpander.Script, IsUser = false, Id = GreenExpander.Id, Name = GreenExpander.ScriptName });
            else if (GreenExpander.IsDisabled) overrides.Add(new ScriptOverride() { Color = PlotColor.Green, IsUser = false, Id = GreenExpander.Id, IsDisabled = true });

            // grab the yellow script
            if (!string.IsNullOrWhiteSpace(YellowExpander.Script)) overrides.Add(new ScriptOverride() { Color = PlotColor.Yellow, Script = YellowExpander.Script, IsUser = false, Id = YellowExpander.Id, Name = YellowExpander.ScriptName });
            else if (YellowExpander.IsDisabled) overrides.Add(new ScriptOverride() { Color = PlotColor.Yellow, IsUser = false, Id = YellowExpander.Id, IsDisabled = true });

            // start
            if (OnStartLevel != null) OnStartLevel(MyLevel, mycolor, overrides);
        }

        //
        // UI Interation
        //

        private void Back_Tapped(object sender, RoutedEventArgs e)
        {
            Hide();
        }

        private void Authenticate_Tapped(object sender, RoutedEventArgs e)
        {
            DataService.Authenticate();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            CollectAndStart();
        }
    }
}
