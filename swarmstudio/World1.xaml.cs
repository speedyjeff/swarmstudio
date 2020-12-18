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

// todo storyboards

namespace swarmstudio
{
    /// <summary>
    /// Interaction logic for World1.xaml
    /// </summary>
    public partial class World1 : Page
    {
        private const double DISABLED_OPACITY = 0.5;
        private const double ENABLED_OPACITY = 1.0;

        private LevelID NextWorldID;

        public World1()
        {
            this.InitializeComponent();

            Loaded += WhenLoaded;

            NextWorldID = LevelID.Blank;

            //Storyboard.SetTarget(LoadingStoryboard, SpinnerTransform);
        }

        private void WhenLoaded(object sender, RoutedEventArgs e)
        {
            bool[] unlocked; // use this to ensure there are no failures due to file corruption
            LevelID[] levels;
            int cnt;

            //if (!SavedGamesRepsoitory.IsLoaded) throw new Exception("The data is not loaded yet!"); // TODO! This sucks!

            // NOTE! This algorithm only is resilent to one file being corrupted - if there are multiple you may be SOL

            // initialize everything
            levels = new LevelID[] { LevelID.World_1_1, LevelID.World_1_2, LevelID.World_1_3, LevelID.World_1_4, LevelID.World_1_5, LevelID.World_1_6, LevelID.World_1_7, LevelID.World_1_8, LevelID.World_1_9, LevelID.World_1_10, LevelID.World_1_11, LevelID.World_1_12, LevelID.World_1_13, LevelID.World_1_14, LevelID.World_1_15, LevelID.World_1_16 };
            unlocked = new bool[16];
            cnt = 0;
            foreach (LevelID id in levels)
            {
                unlocked[cnt] = Populate(id, cnt + 1, id == LevelID.World_1_1 /* initial unlocked */);

                if (cnt > 0 && unlocked[cnt] && !unlocked[cnt - 1])
                {
                    // likely corruption - fix it
                    Populate(levels[cnt - 1], cnt, true /* initial unlocked */);
                    SavedGamesRepsoitory.Store(levels[cnt - 1], 1, "", true /* unlocked */, 1);
                }

                // advance
                cnt++;
            }
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
                unlocked = SavedGamesRepsoitory.GetUnlocked(id);
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
            // Make sure this world is open and ready to play
            if (id != LevelID.World_1_1 && (!SavedGamesRepsoitory.Loaded(id) || !SavedGamesRepsoitory.GetUnlocked(id))) return;

            // show loading
            MainGrid.IsHitTestVisible = false;
            //LoadingImage.Visibility = Visibility.Visible;
            //LoadingStoryboard.Begin();
            NextWorldID = id;

            // go
            DesignPane.CachedDesignPane.Initialize(NextWorldID, PlotColor.Blue);
            this.NavigationService.Navigate(DesignPane.CachedDesignPane);
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
                case 1: GotoWorld(LevelID.World_1_1); break;
                case 2: GotoWorld(LevelID.World_1_2); break;
                case 3: GotoWorld(LevelID.World_1_3); break;
                case 4: GotoWorld(LevelID.World_1_4); break;
                case 5: GotoWorld(LevelID.World_1_5); break;
                case 6: GotoWorld(LevelID.World_1_6); break;
                case 7: GotoWorld(LevelID.World_1_7); break;
                case 8: GotoWorld(LevelID.World_1_8); break;
                case 9: GotoWorld(LevelID.World_1_9); break;
                case 10: GotoWorld(LevelID.World_1_10); break;
                case 11: GotoWorld(LevelID.World_1_11); break;
                case 12: GotoWorld(LevelID.World_1_12); break;
                case 13: GotoWorld(LevelID.World_1_13); break;
                case 14: GotoWorld(LevelID.World_1_14); break;
                case 15: GotoWorld(LevelID.World_1_15); break;
                case 16: GotoWorld(LevelID.World_1_16); break;
                default: throw new Exception("Unknown id : " + id);
            }
        }

    }
}
