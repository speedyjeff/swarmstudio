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
    /// Interaction logic for Stars.xaml
    /// </summary>
    public partial class Stars : UserControl
    {
        public Stars()
        {
            this.InitializeComponent();
        }

        //
        // public interfac
        //

        public void ShowStars(int cnt)
        {
            // hide all
            Star_Lit_0.Visibility = Visibility.Collapsed;
            Star_Lit_1.Visibility = Visibility.Collapsed;
            Star_Lit_2.Visibility = Visibility.Collapsed;
            Star_Unlit_0.Visibility = Visibility.Collapsed;
            Star_Unlit_1.Visibility = Visibility.Collapsed;
            Star_Unlit_2.Visibility = Visibility.Collapsed;

            switch (cnt)
            {
                case 3:
                    Star_Lit_0.Visibility = Visibility.Visible;
                    Star_Lit_1.Visibility = Visibility.Visible;
                    Star_Lit_2.Visibility = Visibility.Visible;
                    break;
                case 2:
                    Star_Lit_0.Visibility = Visibility.Visible;
                    Star_Lit_1.Visibility = Visibility.Visible;
                    Star_Unlit_2.Visibility = Visibility.Visible;
                    break;
                case 1:
                    Star_Lit_0.Visibility = Visibility.Visible;
                    Star_Unlit_1.Visibility = Visibility.Visible;
                    Star_Unlit_2.Visibility = Visibility.Visible;
                    break;
                default:
                    Star_Unlit_0.Visibility = Visibility.Visible;
                    Star_Unlit_1.Visibility = Visibility.Visible;
                    Star_Unlit_2.Visibility = Visibility.Visible;
                    break;
            }
        }
    }
}
