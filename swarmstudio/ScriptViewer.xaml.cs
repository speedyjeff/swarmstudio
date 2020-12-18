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
    /// Interaction logic for ScriptViewer.xaml
    /// </summary>
    public partial class ScriptViewer : UserControl
    {
        private ScriptItem[] Items;
        private int PreviousIndex;

        public ScriptViewer()
        {
            this.InitializeComponent();

            PreviousIndex = -1;

            Items = new ScriptItem[10]; // defined in XAML
            Items[0] = Item0;
            Items[1] = Item1;
            Items[2] = Item2;
            Items[3] = Item3;
            Items[4] = Item4;
            Items[5] = Item5;
            Items[6] = Item6;
            Items[7] = Item7;
            Items[8] = Item8;
            Items[9] = Item9;
        }

        //
        // public interface
        //

        public event Action OnForward;
        public event Action OnBack;
        public event Action<int> OnSelect;

        public void Reset()
        {
            Initalize(new Scripts[0], 0);
        }

        public void Initalize(Scripts[] scripts, int start)
        {
            int index;

            if (scripts == null || scripts.Length > Items.Length) throw new Exception("bad script array");

            // fill in the used ones
            for (index = 0; index < scripts.Length; index++)
            {
                Items[index].Visibility = Visibility.Visible;
                Items[index].Initalize(start + index, scripts[index].Name, scripts[index].UID, scripts[index].Likes, scripts[index].Dislikes);
            }

            // turn off the unused
            for (; index < Items.Length; index++)
            {
                Items[index].Visibility = Visibility.Collapsed;
            }

            // reset the scrollviwer
            ItemScroller.ScrollToVerticalOffset(0);
            ItemScroller.ScrollToHorizontalOffset(0);
        }

        // 
        // Utility
        //

        private void SelectItem(int index)
        {
            // unhighlight previous
            if (PreviousIndex == 0) Item0.Unhighlight();
            if (PreviousIndex == 1) Item1.Unhighlight();
            if (PreviousIndex == 2) Item2.Unhighlight();
            if (PreviousIndex == 3) Item3.Unhighlight();
            if (PreviousIndex == 4) Item4.Unhighlight();
            if (PreviousIndex == 5) Item5.Unhighlight();
            if (PreviousIndex == 6) Item6.Unhighlight();
            if (PreviousIndex == 7) Item7.Unhighlight();
            if (PreviousIndex == 8) Item8.Unhighlight();
            if (PreviousIndex == 9) Item9.Unhighlight();
            PreviousIndex = index;

            // highlight the right one
            if (index == 0) Item0.Highlight();
            if (index == 1) Item1.Highlight();
            if (index == 2) Item2.Highlight();
            if (index == 3) Item3.Highlight();
            if (index == 4) Item4.Highlight();
            if (index == 5) Item5.Highlight();
            if (index == 6) Item6.Highlight();
            if (index == 7) Item7.Highlight();
            if (index == 8) Item8.Highlight();
            if (index == 9) Item9.Highlight();

            // send callback
            if (OnSelect != null) OnSelect(index);
        }

        //
        // UI logic
        //

        private void Forward_Tapped(object sender, RoutedEventArgs e)
        {
            if (OnForward != null) OnForward();
        }

        private void Back_Tapped(object sender, RoutedEventArgs e)
        {
            if (OnBack != null) OnBack();
        }

        private void Item0_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(0); }
        private void Item1_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(1); }
        private void Item2_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(2); }
        private void Item3_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(3); }
        private void Item4_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(4); }
        private void Item5_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(5); }
        private void Item6_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(6); }
        private void Item7_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(7); }
        private void Item8_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(8); }
        private void Item9_Tapped(object sender, MouseButtonEventArgs e) { SelectItem(9); }

    }
}
