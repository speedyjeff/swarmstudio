using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Swarm
{
    public class Levels
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public string Script { get; set; }
        public double Rating { get; set; }
        public bool Success { get; set; }
        public string UGuid { get; set; }
        public DateTime Time { get; set; }
        public string UID { get; set; }
        public int Iterations { get; set; }
        public int Complexity { get; set; }
    }

    public class Scripts
    {
        public int Id { get; set; }
        public int Level { get; set; }
        public int Color { get; set; }
        public string Script { get; set; }
        public long Likes { get; set; }
        public long Dislikes { get; set; }
        public string UGuid { get; set; }
        public DateTime Time { get; set; }
        public string UID { get; set; }
        public string Name { get; set; }
    }

    public static class Utility
    {
        public static SolidColorBrush RedPlotBrush = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
        public static SolidColorBrush BluePlotBrush = new SolidColorBrush(Color.FromArgb(255, 0, 61, 255));
        public static SolidColorBrush GreenPlotBrush = new SolidColorBrush(Color.FromArgb(255, 45, 255, 0));
        public static SolidColorBrush YellowPlotBrush = new SolidColorBrush(Colors.Goldenrod);

        public static int RatingToStars(double rating)
        {
            if (rating > 0.92) return 3;
            else if (rating > 0.65) return 2;
            else if (rating > 0.0) return 1;
            else return 0;
        }
    }
}

