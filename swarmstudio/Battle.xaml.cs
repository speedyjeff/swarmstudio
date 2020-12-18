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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


// //////////////////////////////////////////////////////////
//
// WARNING WARNING WARNING WARNING WARNING WARNING WARNING //
//
// Any changes made to this file must be made to World1,   //
// World2, and Battle.  The code is almost a direct COPY!  //
//
// //////////////////////////////////////////////////////////

// todo animation

namespace swarmstudio
{
    /// <summary>
    /// Interaction logic for Battle.xaml
    /// </summary>
    public partial class Battle : Page
    {
        private const double DISABLED_OPACITY = 0.5;
        private const double ENABLED_OPACITY = 1.0;

        private LevelID NextWorldID;

        public Battle()
        {
            this.InitializeComponent();

            NextWorldID = LevelID.Blank;
            Loaded += WhenLoaded;

            Selector.OnStartLevel += Selector_OnStartLevel;

            //Storyboard.SetTarget(LoadingStoryboard, SpinnerTransform);
        }

        private void WhenLoaded(object sender, RoutedEventArgs e)
        {
            LevelID[] levels;
            int cnt;

            //if (!SavedGamesRepsoitory.IsLoaded) throw new Exception("The data is not loaded yet!"); // TODO! This sucks!

            // initialize everything
            levels = new LevelID[] { LevelID.Battle_Open, LevelID.Battle_Quad, LevelID.Battle_Hill, LevelID.Battle_Maze, LevelID.Bonus_1, LevelID.Bonus_2, LevelID.Bonus_3, LevelID.Bonus_4 };
            cnt = 1;
            foreach (LevelID id in levels)
            {
                Populate(id, cnt++, true /* initial unlocked */);
            }
        }

        //
        // callbacks
        //
        void Selector_OnStartLevel(LevelID level, PlotColor color, List<ScriptOverride> overrides)
        {
            GotoWorld(level, color, isbattle: overrides != null && overrides.Count > 0, overrides);
        }

        //
        // Utility
        //
        private bool Populate(LevelID id, int elem, bool initialUnlocked)
        {
            int stars = 0;
            bool unlocked = initialUnlocked;

            // if loaded get the details
            if (SavedGamesRepsoitory.Loaded(id))
            {
                stars = SavedGamesRepsoitory.GetStars(id);
                // dont care in Battle - unlocked = SavedGamesRepsoitory.GetUnlocked(id);
            }

            // update the opacity
            if (unlocked) GetGrid(elem).Opacity = ENABLED_OPACITY;
            else GetGrid(elem).Opacity = DISABLED_OPACITY;

            // set the stars
            GetStars(elem).ShowStars(stars);

            return unlocked;
        }

        private void GotoWorld(LevelID id)
        {
            // Contract in DesignPane.xaml.cs
            GotoWorld(id, PlotColor.Blue);
        }

        private void GotoWorld(LevelID id, PlotColor color, bool isbattle = false, List<ScriptOverride> overrides = null)
        {
            // all levels are playable

            // show loading
            Selector.Hide();
            MainGrid.IsHitTestVisible = false;
            //LoadingImage.Visibility = Visibility.Visible;
            //LoadingStoryboard.Begin();
            NextWorldID = id;
            // go
            this.NavigationService.Navigate(new DesignPane(id, color, isbattle, overrides));
        }

        private void ShowBattleSelctor(LevelID level)
        {
            Selector.Show(level);
        }

        private void ParseName(string name, out int id)
        {
            id = Convert.ToInt32(name.Split(new char[] { '_' })[1]);
        }

        private System.Windows.Controls.Grid GetGrid(int elem)
        {
            return (this.FindName("Level_" + elem) as System.Windows.Controls.Grid);
        }

        private Stars GetStars(int elem)
        {
            return (this.FindName("Stars_" + elem) as Stars);
        }

        //
        // UI Logic
        //

        private void Back_Tapped(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new WorldSelect());
        }

        private void Level_Tapped(object sender, MouseButtonEventArgs e)
        {
            int id;

            ParseName((sender as System.Windows.Controls.Grid).Name, out id);

            switch (id)
            {
                case 1: ShowBattleSelctor(LevelID.Battle_Open); break;
                case 2: ShowBattleSelctor(LevelID.Battle_Quad); break;
                case 3: ShowBattleSelctor(LevelID.Battle_Hill); break;
                case 4: ShowBattleSelctor(LevelID.Battle_Maze); break;
                case 5: GotoWorld(LevelID.Bonus_1); break;
                case 6: GotoWorld(LevelID.Bonus_2); break;
                case 7: GotoWorld(LevelID.Bonus_3); break;
                case 8: GotoWorld(LevelID.Bonus_4); break;
                default: throw new Exception("Unknown id : " + id);
            }
        }
    }
}
