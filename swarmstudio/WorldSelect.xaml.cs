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
    /// Interaction logic for WorldSelect.xaml
    /// </summary>
    public partial class WorldSelect : Page
    {
        public WorldSelect()
        {
            this.InitializeComponent();

            Loaded += WhenLoaded;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        private void WhenLoaded(object sender, RoutedEventArgs e)
        {
            // count the stars
            int possibleStars, receivedStars;

            // World 1
            CountStars(out possibleStars, out receivedStars, new LevelID[] { LevelID.World_1_1, LevelID.World_1_2, LevelID.World_1_3, LevelID.World_1_4, LevelID.World_1_5, LevelID.World_1_6, LevelID.World_1_7, LevelID.World_1_8, LevelID.World_1_9, LevelID.World_1_10, LevelID.World_1_11, LevelID.World_1_12, LevelID.World_1_13, LevelID.World_1_14, LevelID.World_1_15, LevelID.World_1_16 });
            World_1_Stars.Text = receivedStars + "/" + possibleStars;

            // World 2
            CountStars(out possibleStars, out receivedStars, new LevelID[] { LevelID.World_2_1, LevelID.World_2_2, LevelID.World_2_3, LevelID.World_2_4, LevelID.World_2_5, LevelID.World_2_6, LevelID.World_2_7, LevelID.World_2_8, LevelID.World_2_9, LevelID.World_2_10, LevelID.World_2_11, LevelID.World_2_12, LevelID.World_2_13, LevelID.World_2_14, LevelID.World_2_15, LevelID.World_2_16 });
            World_2_Stars.Text = receivedStars + "/" + possibleStars;
            
            // World 3
            CountStars(out possibleStars, out receivedStars, new LevelID[] { LevelID.World_3_1, LevelID.World_3_2, LevelID.World_3_3, LevelID.World_3_4, LevelID.World_3_5, LevelID.World_3_6, LevelID.World_3_7, LevelID.World_3_8, LevelID.World_3_9, LevelID.World_3_10, LevelID.World_3_11, LevelID.World_3_12, LevelID.World_3_13, LevelID.World_3_14, LevelID.World_3_15, LevelID.World_3_16 });
            World_3_Stars.Text = receivedStars + "/" + possibleStars;

            // Battle
            CountStars(out possibleStars, out receivedStars, new LevelID[] { LevelID.Battle_Maze, LevelID.Battle_Hill, LevelID.Battle_Open, LevelID.Battle_Quad, LevelID.Bonus_1, LevelID.Bonus_2, LevelID.Bonus_3, LevelID.Bonus_4 });
            Battle_Stars.Text = receivedStars + "/" + possibleStars;
        }

        // 
        // Utility
        //

        private void CountStars(out int possibleStars, out int receivedStars, LevelID[] levels)
        {
            possibleStars = 0;
            receivedStars = 0;

            foreach (LevelID id in levels)
            {
                if (SavedGamesRepsoitory.Loaded(id))
                {
                    receivedStars += SavedGamesRepsoitory.GetStars(id);
                }
                possibleStars += 3;
            }
        }

        //
        // UI Logic
        //

        private void World1_Tapped(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new World1());
        }

        private void World2_Tapped(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new World2());
        }

        private void World3_Tapped(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new World3());
        }

        private void Battle_Tapped(object sender, MouseButtonEventArgs e)
        {
            this.NavigationService.Navigate(new Battle());
        }

        private void Back_Tapped(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Startup());
        }
    }
}
