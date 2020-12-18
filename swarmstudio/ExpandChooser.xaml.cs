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
    /// Interaction logic for ExpandChooser.xaml
    /// </summary>
    public partial class ExpandChooser : UserControl
    {
        private bool IsDefaultUser { get; set; }

        public ExpandChooser()
        {
            this.InitializeComponent();

            Script = "";
            Id = -1;
            IsDefaultUser = false;
        }

        //
        // Public interface
        //

        public delegate void SelectionDelegate(bool populate);
        public event SelectionDelegate OnSelection;

        public void Init(string name, SolidColorBrush color, bool defaultUser)
        {
            TitleText.Text = name;
            TitleText.Foreground = color;
            ChoiceText.Foreground = color;
            BorderRectangle.Stroke = color;

            IsDefaultUser = defaultUser;

            Radio_Create.GroupName = Radio_Choose.GroupName = Radio_Disabled.GroupName = Radio_Computer.GroupName = $"{color}_battle_choice";

            Reset();
        }

        public void Reset()
        {
            // reset to default
            Radio_Choose.IsChecked = false;
            Radio_Create.IsChecked = IsDefaultUser;
            Radio_Computer.IsChecked = !IsDefaultUser;
            Radio_Disabled.IsChecked = false;

            Script = "";
            ScriptName = "";
            SetDefaultText();
        }

        public string Script { get; set; }
        public int Id { get; set; }
        public string ScriptName { get; set; }
        public bool IsDisabled { get { return Radio_Disabled.IsChecked.Value; } }

        public bool IsUser
        {
            get
            {
                return Radio_Create.IsChecked.Value;
            }
        }

        public void ToggleCreate()
        {
            if (Radio_Create.IsChecked.Value)
            {
                Radio_Create.IsChecked = false;
                Radio_Computer.IsChecked = true;
                SetDefaultText();
            }
        }

        //
        // utility
        //

        private void SetDefaultText()
        {
            // hide all
            ChoiceText.Visibility = Visibility.Collapsed;
            ComputerText.Visibility = Visibility.Collapsed;
            DefaultText.Visibility = Visibility.Collapsed;
            DisabledText.Visibility = Visibility.Collapsed;

            // display the right one
            if (Radio_Choose.IsChecked.Value)
            {
                ChoiceText.Visibility = Visibility.Visible;
                ChoiceText.Text = ScriptName;
            }
            else if (Radio_Computer.IsChecked.Value) ComputerText.Visibility = Visibility.Visible;
            else if (Radio_Create.IsChecked.Value) DefaultText.Visibility = Visibility.Visible;
            else if (Radio_Disabled.IsChecked.Value) DisabledText.Visibility = Visibility.Visible;
        }  

        //
        // UI Logic
        //
        private void Radio_Checked(object sender, RoutedEventArgs e)
        {
            SetDefaultText();
        }

        private void Radio_Tapped(object sender, RoutedEventArgs e)
        {
            if (Radio_Choose.IsChecked.Value)
            {
                // ensure the user is authenticated
                if (!DataService.IsAuthenticated) DataService.Authenticate();

                if (DataService.IsAuthenticated && OnSelection != null) OnSelection(true);

                // todo scriptname
            }
            else
            {
                if (OnSelection != null) OnSelection(false);
            }

            // set text
            SetDefaultText();
        }
    }
}
