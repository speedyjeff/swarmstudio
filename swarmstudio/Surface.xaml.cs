using Swarm;
using SwarmGameLogic;
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
using System.Windows.Threading;

namespace swarmstudio
{
    public class ScriptOverride
    {
        public PlotColor Color;
        public string Name;
        public bool IsUser;
        public string Script;
        public int Id;
        public bool IsDisabled;
    }

    /// <summary>
    /// Interaction logic for Surface.xaml
    /// </summary>
    public partial class Surface : UserControl
    {
        // constants
        private const float BorderOpacity = 0.75f;
        private const float VisitedOpacity = 0.40f;
        private const float OccupiedOpacity = 1.0f;
        private const float DuplicationOpacity = 0.75f;
        private const float DefendedOpacity = 0.9f;
        private const float UnoccupiedOpacity = 0.4f;
        private const float ExplosionOpacity = 0.4f;
        private const double PlotWidth = 10.0;
        private const double PlotHeight = 10.0;
        private const double PlotBorder = 1.0;  // ASSUMED 1 below!

        private const char SwarmMessageMarker = '+';

        private const int SkipIterationsOnComplete = 2;

        private delegate void DispatcherDeleage(object[] objs);

        private SolidColorBrush TransparentPlotBrush = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
        private SolidColorBrush EnableStrokeBrush = new SolidColorBrush(Colors.Black);
        private SolidColorBrush DisableStrokeBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
        private SolidColorBrush BlackPlotBrush = new SolidColorBrush(Colors.Black);
        private SolidColorBrush EndPlotBrush = new SolidColorBrush(Colors.Blue);
        private SolidColorBrush EndStrokeBrush = new SolidColorBrush(Colors.LightGray);

        // data
        private DispatcherTimer GameLoopTimer;
        private SwarmGame SwarmGame;
        private int NumPlayers;
        private LevelDetails Level;
        private int SkippedIterations;
        private Tuple<int, int> InspectionCoord;

        private WriteableBitmap SurfaceBitmap;
        private uint[] SurfacePixels;

        // statistics
        private int NumIterations;

        // public callbacks
        public delegate string GetScriptDelegate();
        public event GetScriptDelegate OnGetScript;

        public delegate void CompleteDelegate(int scriptLength, int iterations, double rating, bool success);
        public event CompleteDelegate OnComplete;

        public delegate void ErrorDelegate(string keyValuePairs);
        public event ErrorDelegate OnError;

        public event Action OnInspectionEnabled;

        public delegate void InspectionDelegate(PlotColor color, Swarm.Matrix matrix, Swarm.Grid grid, Move previous);
        public event InspectionDelegate OnInspection;

        public Surface()
        {
            this.InitializeComponent();

            // grab the Surface bitmap
            SurfaceBitmap = new WriteableBitmap(pixelWidth: 500, pixelHeight: 500, dpiX: 96, dpiY: 96, PixelFormats.Bgra32, palette: null); // uggh nasty hardcoded values
            SurfacePixels = new uint[SurfaceBitmap.PixelHeight * SurfaceBitmap.PixelWidth];

            // display the image
            SurfaceImage.Source = SurfaceBitmap;

            // startup swarm game
            SwarmGame = new SwarmGame();
            NumPlayers = 0;
            NumIterations = 0;
            SkippedIterations = 0;
            InspectionCoord = new Tuple<int, int>(-1, -1);

            // create the ui timer
            GameLoopTimer = new DispatcherTimer();
            GameLoopTimer.Interval = new TimeSpan(days:0, hours:0, minutes:0, seconds:0, milliseconds:100);
            GameLoopTimer.Tick += GameLoop;
        }

        //
        // public interface
        //

        public void Stop()
        {
            GameLoopTimer.Stop();
            NumPlayers = 0;
            NumIterations = 0;
            SkippedIterations = 0;
            IsStarted = false;
        }

        public bool IsStarted { get; private set; }
        public bool IsInit { get; private set; }

        public void Initialize(LevelID id, List<ScriptOverride> overrides)
        {
            // stop the engine
            Stop();

            // grab the level details
            Level = LevelDesign.GetLevelDetails(id);

            if (Level == null) throw new Exception("Empty level : " + id);
            if (overrides == null && Level.IsBattle) throw new Exception("Must pass in additional information for battle boards");

            // update the level details if in Battle mode
            if (overrides != null)
            {
                int userColors = 0;

                // apply any overrides
                foreach (ScriptOverride script in overrides)
                {
                    if (script.IsUser)
                    {
                        Level.Color = script.Color;
                        Level.Start = Level.AI[(int)script.Color].Start;
                        userColors++;
                    }
                    else if (script.IsDisabled)
                    {
                        // clear this competitor
                        Level.AI[(int)script.Color] = null;
                    }
                    else if (!string.IsNullOrWhiteSpace(script.Script))
                    {
                        Level.AI[(int)script.Color].Script = script.Script;
                    }
                }

                if (userColors == 0 || userColors > 1) throw new Exception("Invalid configuration - there must be 1 user defined script: " + userColors);
            }

            // setup the board based on the field config
            Initialize();

            IsInit = true;
        }

        public void Reset()
        {
            if (!IsInit) throw new Exception("Must call Initialize first");

            // stop the engine
            Stop();

            // setup the board based on the field config
            Initialize();
        }

        public bool Start(bool background)
        {
            string transaction;
            string script;

            // configure the user script
            script = "";
            if (OnGetScript != null)
            {
                script = OnGetScript();
            }
            if (script == "" || script == null)
            {
                if (OnError != null) OnError("Script=empty");
                return false;
            }
            if (!SwarmGame.SetSwarmConfig(Level.Color, script, Level.Start, Level.End, out transaction))
            {
                if (OnError != null) OnError(transaction);
                return false;
            }
            ChangeState(transaction, false);
            if (!Level.IsBattle) NumPlayers++;

            // store the script
            Level.Script = script;

            // start the game
            if (!SwarmGame.Start())
            {
                throw new Exception("Failed to start swarm!");
            }

            if (NumPlayers < 0 || NumPlayers > 4) throw new Exception("Invalid number of players: " + NumPlayers);

            // start the game timer
            if (background) GameLoopTimer.Start();

            IsStarted = true;

            return true;
        }

        public void SingleStep()
        {
            if (IsStarted) GameLoop(this, this);
        }

        //
        // Utility
        // 

        private void Initialize()
        {
            string transaction;
            ScaleTransform s;

            // startup swarm game
            SwarmGame = new SwarmGame();

            // clear the field - perf optimization
            for (int i = 0; i < SurfacePixels.Length; i++) SurfacePixels[i] = 0xffffffff;

            // initialize battle mode
            if (Level.IsBattle)
            {
                SwarmGame.IsBattle = true;
                SwarmGame.BattleWinningPercentage = Level.BattleWinPercent;
            }

            // initalize the design
            SwarmGame.SetFieldConfig(Level.Field, Level.Color, out transaction);
            ChangeState(transaction, true);

            // set start for the user - ensures there is a border
            foreach (Tuple<int, int> pnt in Level.Start)
            {
                SetStartPoint(pnt.Item1, pnt.Item2);
            }

            // set end for the user - put before the AI since it may be underneath
            foreach (Tuple<int, int> pnt in Level.End)
            {
                SetEndPoint(pnt.Item1, pnt.Item2);
            }

            // configure AI scripts
            foreach (SwarmDetails ai in Level.AI)
            {
                if (ai != null)
                {
                    if (!SwarmGame.SetSwarmConfig(ai.Color, ai.Script, ai.Start, ai.End, out transaction))
                    {
                        throw new Exception("There was an error with the AI script : " + transaction);
                    }
                    ChangeState(transaction, false);
                    NumPlayers++;
                }
            }

            // set scaling
            s = GridSurface.RenderTransform as ScaleTransform;
            s.ScaleX = Level.Scale;
            s.ScaleY = Level.Scale;

            // set center
            s.CenterX = Level.Center.Item1;
            s.CenterY = Level.Center.Item2;

            // init the user - without script for the start
            if (!SwarmGame.SetSwarmConfig(Level.Color, "RETURN [0]", Level.Start, Level.End, out transaction))
            {
                throw new Exception("There was an error with the initial user script : " + transaction);
            }
            ChangeState(transaction, false);
        }

        private void SendCompleteNotification(bool success)
        {
            if (OnComplete != null)
            {
                int len = ((Level.Script == null) ? 0 : Level.Script.Length);
                double rating = (((double)Level.ShorestSolution / (double)len) * 0.5) + (((double)Level.LeastInterations / (double)NumIterations) * 0.5);
                OnComplete(len, NumIterations, success ? rating : 0.0, success);
            }
        }

        // main game loop
        private void GameLoop(object o, object e)
        {
            string transaction;

            // check for a winner
            if (PlotColor.Clear != SwarmGame.Winner || SwarmGame.Lost(Level.Color))
            {
                // pause and then display the complete screen
                if (e != null) { } // exit now  HACK!
                else if (SkippedIterations++ < SkipIterationsOnComplete) return;

                GameLoopTimer.Stop();
                SendCompleteNotification(Level.Color == SwarmGame.Winner /* success */);
                return;
            }

            // advance the game
            for (int i = 0; i < NumPlayers; i++)
            {
                if (SwarmGame.NextTurn(out transaction))
                {
                    ChangeState(transaction, false);
                }
            }

            // send inspection
            SendInspection();

            // advance the counter
            NumIterations++;
        }

        private void SendInspection()
        {
            int h = InspectionCoord.Item1;
            int w = InspectionCoord.Item2;
            if (OnInspection != null && h >= 0 && w >= 0) OnInspection(SwarmGame.GetColor(h, w), SwarmGame.GetMatrix(h, w), SwarmGame.GetGrid(h, w), SwarmGame.GetPreviousMove(h, w));
        }

        // must be run on UI thread
        private void SetStartPoint(int h, int w)
        {
            FillBorder(h, w, BorderOpacity /* opacity */ , EnableStrokeBrush /* stroke */ );
        }

        private void SetEndPoint(int h, int w)
        {
            FillRectangle(h, w, VisitedOpacity /* opacity */ , EndPlotBrush /* fill */);
            FillBorder(h, w, 1.0f /* opacity */ , EndStrokeBrush /* stroke */ );
        }

        private void ChangeState(string transaction, bool init)
        {
            ChangeState(transaction.Split(SwarmUtil.Seperator), init);
        }

        private void ChangeState(string[] states, bool init)
        {
            PlotColor color;
            PlotState state;
            int w, h;
            int wait;

            foreach (string str in states)
            {
                if (SwarmUtil.Decode(str, out color, out h, out w, out state, out wait))
                {
                    float opacity = VisitedOpacity;
                    SolidColorBrush brush = TransparentPlotBrush;
                    bool drawRect = true;
                    bool drawBorder = init;

                    switch (color)
                    {
                        case PlotColor.Red: brush = Utility.RedPlotBrush; break;
                        case PlotColor.Blue: brush = Utility.BluePlotBrush; break;
                        case PlotColor.Green: brush = Utility.GreenPlotBrush; break;
                        case PlotColor.Yellow: brush = Utility.YellowPlotBrush; break;
                        case PlotColor.Clear: drawRect = false; break; // brush = TransparentPlotBrush; break; // stroke = DisableStrokeBrush; break;
                        default: throw new Exception("" + color);
                    }

                    switch (state)
                    {
                        case PlotState.Visited: opacity = VisitedOpacity; break;
                        case PlotState.Occupied: opacity = OccupiedOpacity; break;
                        case PlotState.Defended: opacity = DefendedOpacity; break;
                        case PlotState.Duplication: opacity = DuplicationOpacity; break;
                        case PlotState.Unoccupied: opacity = UnoccupiedOpacity; break;
                        case PlotState.Forbidden: drawRect = false; drawBorder = false; break; //opacity = 0f; break;
                        case PlotState.Destroyed: opacity = ExplosionOpacity; brush = BlackPlotBrush; break;
                        default: throw new Exception("Unknown state : " + state);
                    }

                    // only fill what you have too - this is a perf optimization
                    if (drawRect) FillRectangle(h, w, opacity, brush);
                    if (drawBorder) FillBorder(h, w, BorderOpacity, EnableStrokeBrush);
                }
            }

            UpdateSurface();
        }

        private void FillRectangle(int h, int w, float opacity, SolidColorBrush brush)
        {
            double y, x;

            HWtoYX(h, w, out y, out x);

            for (double dy = PlotBorder; dy < PlotHeight - PlotBorder; dy++)
            {
                for (double dx = PlotBorder; dx < PlotWidth - PlotBorder; dx++)
                {
                    SetPixel((int)(y + dy), (int)(x + dx), (byte)(255 * opacity), brush.Color.R, brush.Color.G, brush.Color.B);
                }
            }
        }

        private void FillBorder(int h, int w, float opacity, SolidColorBrush stroke)
        {
            double y, x;
            double dy, dx;

            HWtoYX(h, w, out y, out x);

            // top
            dy = 0;
            for (dx = 0; dx < PlotWidth; dx++)
            {
                SetPixel((int)(y + dy), (int)(x + dx), (byte)(255 * opacity), stroke.Color.R, stroke.Color.G, stroke.Color.B);
            }

            // bottom
            dy = PlotHeight - PlotBorder;
            for (dx = 0; dx < PlotWidth; dx++)
            {
                SetPixel((int)(y + dy), (int)(x + dx), (byte)(255 * opacity), stroke.Color.R, stroke.Color.G, stroke.Color.B);
            }

            // left
            dx = 0;
            for (dy = 0; dy < PlotHeight; dy++)
            {
                SetPixel((int)(y + dy), (int)(x + dx), (byte)(255 * opacity), stroke.Color.R, stroke.Color.G, stroke.Color.B);
            }

            // right
            dx = PlotWidth - PlotBorder;
            for (dy = 0; dy < PlotHeight; dy++)
            {
                SetPixel((int)(y + dy), (int)(x + dx), (byte)(255 * opacity), stroke.Color.R, stroke.Color.G, stroke.Color.B);
            }
        }

        // https://github.com/dotnet/runtime/blob/d21fe17023e2a03f9b8eec4a5dcf086187b84ca2/src/libraries/System.Drawing.Primitives/src/System/Drawing/Color.cs#L319
        private void SetPixel(int y, int x, byte alpha, byte red, byte green, byte blue)
        {
            SurfacePixels[((int)SurfaceBitmap.Width * y) + x] = (uint)((alpha << 24) + (red << 16) + (green << 8) + blue);
        }

        private void UpdateSurface()
        {
            SurfaceBitmap.WritePixels(new Int32Rect(0, 0, SurfaceBitmap.PixelWidth, SurfaceBitmap.PixelHeight), SurfacePixels, SurfaceBitmap.PixelWidth * 4, 0);
        }

        private void HWtoYX(int h, int w, out double y, out double x)
        {
            y = h * PlotHeight;
            x = w * PlotWidth;
        }

        private void YXtoHW(double y, double x, out int h, out int w)
        {
            double ay, ax;

            // round down to the closest Plot
            ay = y - (y % PlotHeight);
            ax = x - (x % PlotWidth);

            h = (int)(ay / PlotHeight);
            w = (int)(ax / PlotWidth);
        }

        private void Plot_Tapped(object sender, MouseButtonEventArgs e)
        {
            int h, w;
            Point pnt = e.GetPosition(SurfaceImage);

            YXtoHW(pnt.Y, pnt.X, out h, out w);

            if (OnInspectionEnabled != null) OnInspectionEnabled();
            InspectionCoord = new Tuple<int, int>(h, w);
            SendInspection();
        }
    }
}
