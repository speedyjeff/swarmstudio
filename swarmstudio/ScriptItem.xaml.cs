using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for ScriptItem.xaml
    /// </summary>
    public partial class ScriptItem : UserControl
    {
        private SolidColorBrush HighlightBrush = new SolidColorBrush(Colors.LightGoldenrodYellow);
        private SolidColorBrush UnhighlightBrush = new SolidColorBrush(Colors.White);

        public ScriptItem()
        {
            this.InitializeComponent();
        }

        //
        // Public interface
        //

        public void Initalize(int id, string name, string uid, long like, long dislike)
        {
            // create the image
            var bitmap = new WriteableBitmap(300, 10, dpiX: 96, dpiY: 96, PixelFormats.Bgra32, palette: null);
            var pixels = new uint[bitmap.PixelWidth * bitmap.PixelHeight];

            var likeWidth = (int)((double)bitmap.PixelWidth * ((double)like / (double)(dislike + like)));

            // create the rating strip

            for (int x = 0; x < bitmap.PixelWidth; x++)
            {
                for (int y = 0; y < bitmap.PixelHeight; y++)
                {
                    if (y == 0 || y == (bitmap.PixelHeight - 1) || x == 0 || x == (bitmap.PixelWidth - 1)) SetPixel(pixels, bitmap.PixelWidth, y, x, 255, 0, 0, 0);
                    else if (x <= likeWidth) SetPixel(pixels, bitmap.PixelWidth, y, x, 255, 20, 180, 10);
                    else SetPixel(pixels, bitmap.PixelWidth, y, x, 255, 185, 185, 185);
                }
            }

            // display image
            bitmap.WritePixels(new Int32Rect(0, 0, bitmap.PixelWidth, bitmap.PixelHeight), pixels, bitmap.PixelWidth * 4, 0);
            RatingImage.Source = bitmap;

            // set text
            NameText.Text = String.Format("{0}. {1}", id, name);
            AuthorText.Text = uid;
            LikeText.Text = String.Format("{0:N0}", like);
            DislikeText.Text = String.Format("{0:N0}", dislike);

            // reset
            Unhighlight();
        }

        public void Highlight()
        {
            MainGrid.Background = HighlightBrush;
        }

        public void Unhighlight()
        {
            MainGrid.Background = UnhighlightBrush;
        }

        //
        // Utility
        //

        // https://github.com/dotnet/runtime/blob/d21fe17023e2a03f9b8eec4a5dcf086187b84ca2/src/libraries/System.Drawing.Primitives/src/System/Drawing/Color.cs#L319
        private void SetPixel(uint[] pixels, int width, int y, int x, byte alpha, byte red, byte green, byte blue)
        {
            pixels[(width * y) + x] = (uint)((alpha << 24) + (red << 16) + (green << 8) + blue);
        }

        //
        // UI logic
        //

        private void StackPanel_Tapped(object sender, MouseButtonEventArgs e)
        {

        }
    }
}
