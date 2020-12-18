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
    /// Interaction logic for Complete.xaml
    /// </summary>
    public partial class Complete : UserControl
    {
        public event Action OnGoMenu;
        public event Action OnReplay;
        public event Action OnGoNext;

        public event Action<int, bool> OnSumbitRating;
        public event Action<string> OnSumbitScript;

        private bool[] Rated;

        public Complete()
        {
            this.InitializeComponent();

            ClearScripts();
        }

        //
        // public interface
        //

        public void Show(bool success, double rating, int stars)
        {
            if (success) NextButton.Visibility = Visibility.Visible;
            else NextButton.Visibility = Visibility.Collapsed;

            // light the right ones
            StarPanel.ShowStars(stars);

            var pct = (double)((int)(rating * 10000.0)) / 100.0;
            RatingText.Text = (pct > 100 ? 100d : pct) + "%" + (pct > 100 ? "*" : "");

            // by default do not disply the script submittion
            ScriptUpload.Visibility = Visibility.Collapsed;
        }

        public int Scripts
        {
            get
            {
                int count = 0;
                if (ScriptPanel_1.Visibility == Visibility.Visible) count++;
                if (ScriptPanel_2.Visibility == Visibility.Visible) count++;
                if (ScriptPanel_3.Visibility == Visibility.Visible) count++;
                return count;
            }
        }

        public void ClearScripts()
        {
            ScriptPanel_1.Visibility = Visibility.Collapsed;
            ScriptPanel_2.Visibility = Visibility.Collapsed;
            ScriptPanel_3.Visibility = Visibility.Collapsed;

            Rated = new bool[3] { true, true, true };
        }

        public void AddScript(string name, int id)
        {
            if (ScriptPanel_1.Visibility == Visibility.Collapsed)
            {
                // set details
                Script_1_Name.Text = name;
                Script_1_Id.Text = id + "";

                // enable
                ScriptPanel_1.Visibility = Visibility.Visible;
                Script_1_Like.Visibility = Script_1_Dislike.Visibility = Visibility.Visible;
                Script_1_Like.IsEnabled = Script_1_Dislike.IsEnabled = true;
                Rated[0] = false;
            }
            else if (ScriptPanel_2.Visibility == Visibility.Collapsed)
            {
                // set details
                Script_2_Name.Text = name;
                Script_2_Id.Text = id + "";

                // enable
                ScriptPanel_2.Visibility = Visibility.Visible;
                Script_2_Like.Visibility = Script_2_Dislike.Visibility = Visibility.Visible;
                Script_2_Like.IsEnabled = Script_2_Dislike.IsEnabled = true;
                Rated[1] = false;
            }
            else if (ScriptPanel_3.Visibility == Visibility.Collapsed)
            {
                // set details
                Script_3_Name.Text = name;
                Script_3_Id.Text = id + "";

                // enable
                ScriptPanel_3.Visibility = Visibility.Visible;
                Script_3_Like.Visibility = Script_3_Dislike.Visibility = Visibility.Visible;
                Script_3_Like.IsEnabled = Script_3_Dislike.IsEnabled = true;
                Rated[2] = false;
            }
            else throw new Exception("Called AddScript too many times");
        }

        public void ShowScriptRating(bool showUpload)
        {
            ScriptUpload.Visibility = Visibility.Visible;
            ScriptSubmitter.Visibility = (showUpload) ? Visibility.Visible : Visibility.Collapsed;
            UploadButton.IsEnabled = true;
        }

        //
        // UI logic
        //

        private void Menu_Tapped(object sender, RoutedEventArgs e)
        {
            if (OnGoMenu != null) OnGoMenu();
        }

        private void Replay_Tapped(object sender, RoutedEventArgs e)
        {
            if (OnReplay != null) OnReplay();
        }

        private void Next_Tapped(object sender, RoutedEventArgs e)
        {
            if (OnGoNext != null) OnGoNext();
        }

        private void Like_1_Tapped(object sender, RoutedEventArgs e)
        {
            if (Rated[0]) return;
            Rated[0] = true;

            // sumbit the like request
            if (OnSumbitRating != null) OnSumbitRating(Convert.ToInt32(Script_1_Id.Text), true /* like */);

            // disable like
            Script_1_Dislike.IsEnabled = false;
        }

        private void Dislike_1_Tapped(object sender, RoutedEventArgs e)
        {
            if (Rated[0]) return;
            Rated[0] = true;

            // sumbit the dislike request
            if (OnSumbitRating != null) OnSumbitRating(Convert.ToInt32(Script_1_Id.Text), false /* like */);

            // disable like
            Script_1_Like.IsEnabled = false;
        }

        private void Like_2_Tapped(object sender, RoutedEventArgs e)
        {
            if (Rated[1]) return;
            Rated[1] = true;

            // sumbit the like request
            if (OnSumbitRating != null) OnSumbitRating(Convert.ToInt32(Script_2_Id.Text), true /* like */);

            // disable like
            Script_2_Dislike.IsEnabled = false;
        }

        private void Dislike_2_Tapped(object sender, RoutedEventArgs e)
        {
            if (Rated[1]) return;
            Rated[1] = true;

            // sumbit the dislike request
            if (OnSumbitRating != null) OnSumbitRating(Convert.ToInt32(Script_2_Id.Text), false /* like */);

            // disable like
            Script_2_Like.IsEnabled = false;
        }

        private void Like_3_Tapped(object sender, RoutedEventArgs e)
        {
            if (Rated[2]) return;
            Rated[2] = true;

            // sumbit the like request
            if (OnSumbitRating != null) OnSumbitRating(Convert.ToInt32(Script_3_Id.Text), true /* like */);

            // disable like
            Script_3_Dislike.IsEnabled = false;
        }

        private void Dislike_3_Tapped(object sender, RoutedEventArgs e)
        {
            if (Rated[2]) return;
            Rated[2] = true;

            // sumbit the dislike request
            if (OnSumbitRating != null) OnSumbitRating(Convert.ToInt32(Script_3_Id.Text), false /* like */);

            // disable like
            Script_3_Like.IsEnabled = false;
        }

        private void Submit_Tapped(object sender, RoutedEventArgs e)
        {
            string scriptName = Name_Part1.Text + " ";
            scriptName += (Name_Part2.Items[Name_Part2.SelectedIndex] as ComboBoxItem).Content + " ";
            scriptName += (Name_Part3.Items[Name_Part3.SelectedIndex] as ComboBoxItem).Content + " ";

            if (OnSumbitScript != null) OnSumbitScript(scriptName);

            UploadButton.IsEnabled = false;
        }
    }
}
