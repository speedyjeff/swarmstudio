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
    /// Interaction logic for Startup.xaml
    /// </summary>
    public partial class Startup : Page
    {
        public Startup()
        {
            this.InitializeComponent();

            Loaded += WhenLoaded;

            BackgroundSurface.OnGetScript += BackgroundSurface_GetScript;
            BackgroundSurface.Initialize(LevelID.Start, null);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        private void WhenLoaded(object sender, RoutedEventArgs e)
        {
            // start the background
            BackgroundSurface.Start(true);
        }

        //
        // Utility
        //
        private string BackgroundSurface_GetScript()
        {
            return @"IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [8] THEN
	                        RETURN [1]
	                      ELSE
                            IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [4] THEN
					           RETURN [4]		                      
                            ELSE
                                IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [8] THEN
			                        RETURN [2]
                                ELSE
                                    IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [4] THEN
                                        RETURN [3]
				                      ELSE
					                      IF rand EQ [4] THEN
						                      RETURN [6]
					                      ELSE
						                      GOTO [0,0]";
        }

        //
        // UI Logic
        //

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            // ensure to the stop the surface
            BackgroundSurface.Stop();

            // navigate away
            this.NavigationService.Navigate(new WorldSelect());
        }
    }
}
