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

namespace swarmstudio
{
    public enum TutorialState { Start, Step_1, Step_2, Step_3, Step_4, Step_5, Step_6, Step_7, Step_8, Done };

    /// <summary>
    /// Interaction logic for Tutorial.xaml
    /// </summary>
    public partial class Tutorial : UserControl
    {
        TutorialState CurrentState;

        DoubleAnimation XAnimation, YAnimation;
        Storyboard GlideStoryboard;


        public Tutorial()
        {
            Duration duration;

            this.InitializeComponent();

            // create storyboard
            duration = new Duration(new TimeSpan(0, 0, 2));

            // Create two DoubleAnimations and set their properties.
            XAnimation = new DoubleAnimation();
            YAnimation = new DoubleAnimation();

            XAnimation.Duration = duration;
            YAnimation.Duration = duration;

            GlideStoryboard = new Storyboard();
            GlideStoryboard.Duration = duration;

            GlideStoryboard.Children.Add(XAnimation);
            GlideStoryboard.Children.Add(YAnimation);

            Storyboard.SetTarget(XAnimation, FingerImage);
            Storyboard.SetTarget(YAnimation, FingerImage);

            // Set the X and Y properties of the Transform to be the target properties
            // of the two respective DoubleAnimations.
            Storyboard.SetTargetProperty(XAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.X)"));
            Storyboard.SetTargetProperty(YAnimation, new PropertyPath("(UIElement.RenderTransform).(TranslateTransform.Y)"));

            XAnimation.To = 200;
            YAnimation.To = 200;

            Init();
        }

        //
        // public interface
        //
        public event Action OnComplete;
        public event Action OnDisplayMove;
        public event Action OnDisplayContext;
        public event Action OnDisplayDebugInfo;

        //
        // Utility
        //

        private void MoveFinger(double y, double x)
        {
            XAnimation.To = x;
            YAnimation.To = y;

            GlideStoryboard.Begin();
        }

        private void Init()
        {
            CurrentState = TutorialState.Start;
            SetTutorialState();
        }

        private TutorialState NextTutorialState(TutorialState state)
        {
            switch (state)
            {
                case TutorialState.Start: return TutorialState.Step_1;
                case TutorialState.Step_1: return TutorialState.Step_2;
                case TutorialState.Step_2: return TutorialState.Step_3;
                case TutorialState.Step_3: return TutorialState.Step_4;
                case TutorialState.Step_4: return TutorialState.Step_5;
                case TutorialState.Step_5: return TutorialState.Step_6;
                case TutorialState.Step_6: return TutorialState.Step_7;
                case TutorialState.Step_7: return TutorialState.Step_8;
                case TutorialState.Step_8: return TutorialState.Done;
                default:
                    throw new Exception("Unknown tutorial state : " + state);
            }
        }

        private void SetTutorialState()
        {
            switch (CurrentState)
            {
                case TutorialState.Start:
                    NextButton.IsEnabled = true;
                    FingerImage.Visibility = Visibility.Collapsed;
                    TutorialText.Text = "Welcome to SWARM.  This tutorial will walk you through how to play the game and how to navigate the interface." + Environment.NewLine + "Click next to begin (mouse and touch screen enabled)";
                    MoveFinger(500, 270);
                    break;
                case TutorialState.Step_1:
                    NextButton.IsEnabled = true;
                    FingerImage.Visibility = Visibility.Collapsed;
                    TutorialText.Text = "The objective of SWARM is to create an algorithm (below) to maneuver your BLUE dots (your SWARM) to SAFETY (the purble dot).  (some puzzles will have a SWARM of team members)";
                    break;
                case TutorialState.Step_2:
                    FingerImage.Visibility = Visibility.Visible;
                    TutorialText.Text = "Create a plan (or algorithm) to maneuver your BLUE SWARM." + Environment.NewLine + "Think of each member of your SWARM as a BEE or ANT in a COLONY - each individual member is not very smart, but collectively they can accomplish great tasks.";
                    MoveFinger(500, 270);
                    break;
                case TutorialState.Step_3:
                    TutorialText.Text = "Each member of your SWARM uses the same algorithm to move - just like ANTs and BEEs.  Each member of your swarm can move up, down, left, right, defend against an enemy, explode like a bomb, or duplicate.";
                    MoveFinger(500, 270);
                    if (OnDisplayMove != null) OnDisplayMove();
                    break;
                case TutorialState.Step_4:
                    if (OnDisplayContext != null) OnDisplayContext();
                    TutorialText.Text = "Some puzzles require the SWARM members to move conditionally based on their environment.  Each SWARM member can move conditionaly based on a narrow or wide view of their surrounds, previous move, or randomly.";
                    break;
                case TutorialState.Step_5:
                    FingerImage.Visibility = Visibility.Visible;
                    MoveFinger(80, 60);
                    TutorialText.Text = "Once you are ready to try out your brand new algorithm you can use the controls in the upper left corner.  From top to bottom: back, reset, go, and single step.";
                    break;
                case TutorialState.Step_6:
                    TutorialText.Text = "The puzzle itself is clickable and displays detailed information about what each member of the SWARM sees.  This is especially helpful when using Single-Step mode.";
                    MoveFinger(70, 200);
                    if (OnDisplayDebugInfo != null) OnDisplayDebugInfo();
                    break;
                case TutorialState.Step_7:
                    TutorialText.Text = "At any time you can refer to the additional help information on the right.";
                    MoveFinger(300, 1060);
                    break;
                case TutorialState.Step_8:
                    TutorialText.Text = "The first few levels have solutions that can be used as a hint to get started.";
                    MoveFinger(0, 1160);
                    break;
                case TutorialState.Done:
                    NextButton.IsEnabled = false;
                    TutorialText.Text = "Good luck and have fun." + Environment.NewLine + "HINT: For this puzzle your SWARM needs to move right ;)";
                    FingerImage.Visibility = Visibility.Collapsed;
                    if (OnDisplayMove != null) OnDisplayMove();
                    break;
                default:
                    throw new Exception("Unknown tutorial state : " + CurrentState);
            }
        }

        //
        // UI logic
        //

        private void Next_Tapped(object sender, RoutedEventArgs e)
        {
            CurrentState = NextTutorialState(CurrentState);

            SetTutorialState();
        }

        private void Done_Tapped(object sender, RoutedEventArgs e)
        {
            CurrentState = TutorialState.Done;

            SetTutorialState();

            if (OnComplete != null) OnComplete();
        }
    }
}
