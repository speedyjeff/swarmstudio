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
    /// Interaction logic for MoveSelector.xaml
    /// </summary>
    public partial class MoveSelector : UserControl
    {
        private Dictionary<Image, ImageSource> Selected;
        private Dictionary<Image, ImageSource> Unselected;

        private Move SelectedMove;

        public MoveSelector()
        {
            this.InitializeComponent();

            SelectedMove = Move.Nothing;

            Unselected = new Dictionary<Image, ImageSource>();
            Selected = new Dictionary<Image, ImageSource>();

            // store the unselected version
            Unselected.Add(UP_Img, UP_Img.Source);
            Unselected.Add(DOWN_Img, DOWN_Img.Source);
            Unselected.Add(LEFT_Img, LEFT_Img.Source);
            Unselected.Add(RIGHT_Img, RIGHT_Img.Source);
            Unselected.Add(DUPLICATE_Img, DUPLICATE_Img.Source);
            Unselected.Add(DEFEND_Img, DEFEND_Img.Source);
            Unselected.Add(EXPLODE_Img, EXPLODE_Img.Source);

            // create new selected versions
            Selected.Add(UP_Img, UP_Selected_Img.Source);
            Selected.Add(DOWN_Img, DOWN_Selected_Img.Source);
            Selected.Add(LEFT_Img, LEFT_Selected_Img.Source);
            Selected.Add(RIGHT_Img, RIGHT_Selected_Img.Source);
            Selected.Add(DEFEND_Img, DEFEND_Selected_Img.Source);
            Selected.Add(DUPLICATE_Img, DUPLICATE_Selected_Img.Source);
            Selected.Add(EXPLODE_Img, EXPLODE_Selected_Img.Source);
        }

        //
        // public interface
        //

        public void Reset()
        {
            SelectedMove = Move.Nothing;
            ClearAll();
        }

        public void SetValue(Move move)
        {
            ClearAll();

            SelectedMove = move;

            Highlight(move);
        }

        public Move GetValue()
        {
            return SelectedMove;
        }

        //
        // utility
        //

        private void ClearAll()
        {
            foreach (Image img in Unselected.Keys)
            {
                img.Source = Unselected[img];
            }
        }

        private void Highlight(Move move)
        {
            Image img = null;

            switch (move)
            {
                case Move.Nothing: break;
                case Move.Up: img = UP_Img; break;
                case Move.Down: img = DOWN_Img; break;
                case Move.Left: img = LEFT_Img; break;
                case Move.Right: img = RIGHT_Img; break;
                case Move.Defend: img = DEFEND_Img; break;
                case Move.Duplicate: img = DUPLICATE_Img; break;
                case Move.Explode: img = EXPLODE_Img; break;
                default: throw new Exception("Unknown move : " + move);
            }

            if (img != null) img.Source = Selected[img];
        }

        //
        // UI logic
        //

        private void Image_Tapped(object sender, MouseButtonEventArgs e)
        {
            switch ((sender as Image).Name)
            {
                case "UP_Img": SetValue(Move.Up); break;
                case "DOWN_Img": SetValue(Move.Down); break;
                case "LEFT_Img": SetValue(Move.Left); break;
                case "RIGHT_Img": SetValue(Move.Right); break;
                case "DEFEND_Img": SetValue(Move.Defend); break;
                case "DUPLICATE_Img": SetValue(Move.Duplicate); break;
                case "EXPLODE_Img": SetValue(Move.Explode); break;
                default: throw new Exception("Unknown image name : " + (sender as Image).Name);
            }

            e.Handled = true;
        }

        private void Grid_Tapped(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }
    }
}
