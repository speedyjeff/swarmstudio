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
    /// Interaction logic for Help.xaml
    /// </summary>
    public partial class Help : UserControl
    {
        public Help()
        {
            this.InitializeComponent();

            Loaded += Help_Loaded;
        }

        private void Help_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        //
        // UI Logic
        //

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Help_Chooser == null || GamePlay_Panel == null || 
                Scoring_Panel == null || Battle_Panel == null || Hint_Panel == null) return;

            switch (Help_Chooser.SelectedIndex)
            {
                case 0:
                    GamePlay_Panel.Visibility = Visibility.Visible;
                    Scoring_Panel.Visibility = Visibility.Collapsed;
                    Battle_Panel.Visibility = Visibility.Collapsed;
                    Hint_Panel.Visibility = Visibility.Collapsed;
                    break;
                case 1:
                    GamePlay_Panel.Visibility = Visibility.Collapsed;
                    Scoring_Panel.Visibility = Visibility.Visible;
                    Battle_Panel.Visibility = Visibility.Collapsed;
                    Hint_Panel.Visibility = Visibility.Collapsed;
                    break;
                case 2:
                    GamePlay_Panel.Visibility = Visibility.Collapsed;
                    Scoring_Panel.Visibility = Visibility.Collapsed;
                    Battle_Panel.Visibility = Visibility.Visible;
                    Hint_Panel.Visibility = Visibility.Collapsed;
                    break;
                case 3:
                    GamePlay_Panel.Visibility = Visibility.Collapsed;
                    Scoring_Panel.Visibility = Visibility.Collapsed;
                    Battle_Panel.Visibility = Visibility.Collapsed;
                    Hint_Panel.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
