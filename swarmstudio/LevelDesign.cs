using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// TODO! Add a Blue computer to the LevelDesign for Battle stages

namespace Swarm
{
    public enum LevelID { Blank=0, Start=1, 
        World_1_1=2, World_1_2=3, World_1_3=4, World_1_4=5, World_1_5=6, World_1_6=7, World_1_7=8, World_1_8=9, World_1_9=10, World_1_10=11, World_1_11=12, World_1_12=13, World_1_13=14, World_1_14=15, World_1_15=16, World_1_16=17, 
        World_2_1=18, World_2_2=19, World_2_3=20, World_2_4=21, World_2_5=22, World_2_6=23, World_2_7=24, World_2_8=25, World_2_9=26, World_2_10=27, World_2_11=28, World_2_12=29, World_2_13=30, World_2_14=31, World_2_15=32, World_2_16=33,
        World_3_1=34, World_3_2=35, World_3_3=36, World_3_4=37, World_3_5=38, World_3_6=39, World_3_7=40, World_3_8=41, World_3_9=42, World_3_10=43, World_3_11=44, World_3_12=45, World_3_13=46, World_3_14=47, World_3_15=48, World_3_16=49,
        Battle_Maze=50, Battle_Hill=51, Battle_Open=52, Battle_Quad=53,
        Bonus_1=54, Bonus_2=55, Bonus_3=56, Bonus_4=57};

    public class LevelDetails
    {
        // user details
        public PlotColor         Color;
        public string            Script;
        public Tuple<int, int>[] Start;
        public Tuple<int, int>[] End;
        public bool              IsBattle;
        public float             BattleWinPercent;

        // AI
        public SwarmDetails[] AI;

        // Level design
        public PlotState[,] Field;
        public double Scale;
        public Tuple<double, double> Center;

        // used for rating
        public int ShorestSolution;
        public int LeastInterations;

        public LevelDetails()
        {
            Color = PlotColor.Blue;
            Start = new Tuple<int,int>[0];
            End = new Tuple<int, int>[0];
            AI = new SwarmDetails[0];
            Field = null;
            Scale = 1.0;
            Center = new Tuple<double, double>(0, 0);
            ShorestSolution = 0;
            LeastInterations = 0;
            IsBattle = false;
            BattleWinPercent = 0f;
        }
    }

    public class SwarmDetails
    {
        public PlotColor         Color;
        public string            Script;
        public Tuple<int, int>[] Start;
        public Tuple<int, int>[] End;

        public SwarmDetails(PlotColor color, string script, Tuple<int,int>[] start)
        {
            Color = color;
            Script = script;
            Start = start;
            End = new Tuple<int, int>[0];
        }
    }

    public static class LevelDesign
    {
        public static LevelID GetNextLevel(LevelID id)
        {
            switch (id)
            {
                // world 1
                case LevelID.World_1_1: return LevelID.World_1_2;
                case LevelID.World_1_2: return LevelID.World_1_3;
                case LevelID.World_1_3: return LevelID.World_1_4;
                case LevelID.World_1_4: return LevelID.World_1_5;
                case LevelID.World_1_5: return LevelID.World_1_6;
                case LevelID.World_1_6: return LevelID.World_1_7;
                case LevelID.World_1_7: return LevelID.World_1_8;
                case LevelID.World_1_8: return LevelID.World_1_9;
                case LevelID.World_1_9: return LevelID.World_1_10;
                case LevelID.World_1_10: return LevelID.World_1_11;
                case LevelID.World_1_11: return LevelID.World_1_12;
                case LevelID.World_1_12: return LevelID.World_1_13;
                case LevelID.World_1_13: return LevelID.World_1_14;
                case LevelID.World_1_14: return LevelID.World_1_15;
                case LevelID.World_1_15: return LevelID.World_1_16;
                case LevelID.World_1_16: return LevelID.Blank;

                // world 2
                case LevelID.World_2_1: return LevelID.World_2_2;
                case LevelID.World_2_2: return LevelID.World_2_3;
                case LevelID.World_2_3: return LevelID.World_2_4;
                case LevelID.World_2_4: return LevelID.World_2_5;
                case LevelID.World_2_5: return LevelID.World_2_6;
                case LevelID.World_2_6: return LevelID.World_2_7;
                case LevelID.World_2_7: return LevelID.World_2_8;
                case LevelID.World_2_8: return LevelID.World_2_9;
                case LevelID.World_2_9: return LevelID.World_2_10;
                case LevelID.World_2_10: return LevelID.World_2_11;
                case LevelID.World_2_11: return LevelID.World_2_12;
                case LevelID.World_2_12: return LevelID.World_2_13;
                case LevelID.World_2_13: return LevelID.World_2_14;
                case LevelID.World_2_14: return LevelID.World_2_15;
                case LevelID.World_2_15: return LevelID.World_2_16;
                case LevelID.World_2_16: return LevelID.Blank;

                // world 3
                case LevelID.World_3_1: return LevelID.World_3_2;
                case LevelID.World_3_2: return LevelID.World_3_3;
                case LevelID.World_3_3: return LevelID.World_3_4;
                case LevelID.World_3_4: return LevelID.World_3_5;
                case LevelID.World_3_5: return LevelID.World_3_6;
                case LevelID.World_3_6: return LevelID.World_3_7;
                case LevelID.World_3_7: return LevelID.World_3_8;
                case LevelID.World_3_8: return LevelID.World_3_9;
                case LevelID.World_3_9: return LevelID.World_3_10;
                case LevelID.World_3_10: return LevelID.World_3_11;
                case LevelID.World_3_11: return LevelID.World_3_12;
                case LevelID.World_3_12: return LevelID.World_3_13;
                case LevelID.World_3_13: return LevelID.World_3_14;
                case LevelID.World_3_14: return LevelID.World_3_15;
                case LevelID.World_3_15: return LevelID.World_3_16;
                case LevelID.World_3_16: return LevelID.Blank;

                // battle
                case LevelID.Bonus_1: return LevelID.Bonus_2;
                case LevelID.Bonus_2: return LevelID.Bonus_3;
                case LevelID.Bonus_3: return LevelID.Bonus_4;
                case LevelID.Bonus_4: return LevelID.Blank;
                case LevelID.Battle_Open: return LevelID.Blank;
                case LevelID.Battle_Quad: return LevelID.Blank;
                case LevelID.Battle_Hill: return LevelID.Blank;
                case LevelID.Battle_Maze: return LevelID.Blank;
                default: throw new Exception("Unknown level id : " + id);
            }
        }

        public static LevelDetails GetLevelDetails(LevelID id)
        {
            switch (id)
            {
                // top level
                case LevelID.Blank: return DefaultLevel();
                case LevelID.Start: return StartLevel();

                // world 1
                case LevelID.World_1_1: return GoLeftLevel();
                case LevelID.World_1_2: return TurnRightLevel();
                case LevelID.World_1_3: return BiDirectionalLevel();
                case LevelID.World_1_4: return FallingEnemiesLevel();
                case LevelID.World_1_5: return IntroExplodeLevel();
                case LevelID.World_1_6: return DiagnalLevel();
                case LevelID.World_1_7: return FollowTheLineLevel();
                case LevelID.World_1_8: return MultiTierLevel();
                case LevelID.World_1_9: return FourInARowLevel();
                case LevelID.World_1_10: return GoBothWaysLevel();
                case LevelID.World_1_11: return BlowTheWallLevel();
                case LevelID.World_1_12: return FollowThePatternLevel();
                case LevelID.World_1_13: return SkirtTheEdgeLevel();
                case LevelID.World_1_14: return MarchInLineLevel();
                case LevelID.World_1_15: return RingAroundLevel();
                case LevelID.World_1_16: return RaceTheFloodLevel();

                // world 2
                case LevelID.World_2_1: return RaceToTheEndLevel();
                case LevelID.World_2_2: return DownNAroundLevel();
                case LevelID.World_2_3: return SpreadFromTheCenterLevel();
                case LevelID.World_2_4: return BlowYourWayOutLevel(); 
                case LevelID.World_2_5: return FauxDefenseLevel();
                case LevelID.World_2_6: return FullCoverageLevel();
                case LevelID.World_2_7: return MeetInTheMiddleLevel();
                case LevelID.World_2_8: return UpNDownLevel();
                case LevelID.World_2_9: return FanoutLevel();
                case LevelID.World_2_10: return ProximityMinesLevel();
                case LevelID.World_2_11: return TrackerLevel();
                case LevelID.World_2_12: return ClearTheBoardLevel();
                case LevelID.World_2_13: return MazeLevel();  // shares similar script with ScatterLevel
                case LevelID.World_2_14: return StickToTheEdgeLevel(); 
                case LevelID.World_2_15: return ProximityMineTrackLevel();
                case LevelID.World_2_16: return RescueMissionLevel();

                // world 3
                case LevelID.World_3_1: return FromAllSidesLevel(); // could be harder than looks
                case LevelID.World_3_2: return ZigZagLevel();
                case LevelID.World_3_3: return CirclingEnemyLevel();
                case LevelID.World_3_4: return FindTheRightPathLevel();
                case LevelID.World_3_5: return WobbleEnemyLevel();
                case LevelID.World_3_6: return EnemyDuplicatesLevel();
                case LevelID.World_3_7: return SpiralLevel();
                case LevelID.World_3_8: return SwarmLevel();
                case LevelID.World_3_9: return GuardedExitLevel();
                case LevelID.World_3_10: return TrackGridLevel();
                case LevelID.World_3_11: return ScatterLevel(); // shares similar script with MazeLevel
                case LevelID.World_3_12: return MarchOnTheKeepLevel();
                case LevelID.World_3_13: return ReplenishEnemyLevel();   // deceptively hard
                case LevelID.World_3_14: return CornerDefenseLevel();   // hard 
                case LevelID.World_3_15: return AssassinLevel();
                case LevelID.World_3_16: return EnmeyInCirclesLevel();  // hard

                // battle
                case LevelID.Bonus_1: return OnSlaughtOfEnemiesLevel();
                case LevelID.Bonus_2: return OpenMazeLevel();
                case LevelID.Bonus_3: return DefendTheHillLevel();
                case LevelID.Bonus_4: return SwarmingFullCoverageLevel();
                case LevelID.Battle_Hill: return HillBattleLevel();
                case LevelID.Battle_Maze: return MazeBattleLevel();
                case LevelID.Battle_Open: return OpenBattleLevel();
                case LevelID.Battle_Quad: return QuadBattleLevel();

                default: throw new Exception("Unknown level id : " + id);
            }
        }

        //
        // levels
        //

        public static LevelDetails DefaultLevel()
        {
            LevelDetails level = new LevelDetails();
            level.Field = GetField(0, 0, new byte[0,0]);

            return level;
        }

        public static LevelDetails StartLevel()
        {
            LevelDetails level = new LevelDetails();

                    level.Field = GetField(0, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,1,1,0,1,1,1,0,0,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,1,0,0,0,1,1,0,0,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,1,0,0,0,1,1,0,0,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,0,0,1,0,0,0,0,1,0,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,0,0,0,1,0,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0, 0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1, 1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1, 1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0, 0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},

							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0, 0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,1, 1,1,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1, 1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1, 1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,1,1, 1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,0,0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,0,0,1,0,0,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,0,0,0,1,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,1,0,1,1,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,0,0,1,1,1,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,0,0,1,1,1,1,1,0,0,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
							});

                    // Start
                    level.Start = new Tuple<int, int>[4] {new Tuple<int, int>(25, 0), new Tuple<int, int>(27, 25), new Tuple<int, int>(3, 43), new Tuple<int, int>(40, 17)};

                    // End
                    level.End = new Tuple<int, int>[0];

                    // Scale
                    level.Scale = 1.5;

                    // Center
                    level.Center = new Tuple<double, double>(250, 250);

                    // AI
                    level.AI = new SwarmDetails[3];

                    // RED
                    level.AI[0] = new SwarmDetails(PlotColor.Red,
                        @"IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [8] THEN
	                        RETURN [1]
	                      ELSE
		                      IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [8] THEN
			                      RETURN [2]
		                      ELSE
			                      IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [4] THEN
				                      RETURN [3]
			                      ELSE
				                      IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [4] THEN
					                      RETURN [4]
				                      ELSE
					                      IF rand EQ [4] THEN
						                      RETURN [6]
					                      ELSE
						                      GOTO [0,0]",
                        new Tuple<int, int>[4] { new Tuple<int, int>(5, 5), new Tuple<int, int>(20, 10), new Tuple<int, int>(37, 42), new Tuple<int, int>(15, 25) }
                        );

                    // YELLOW
                    level.AI[1] = new SwarmDetails(PlotColor.Yellow,
                         @"IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [8] THEN
					          RETURN [4]
                           ELSE
		                      IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [8] THEN
			                      RETURN [3]
		                      ELSE
			                      IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [4] THEN
	                                  RETURN [1]
                                  ELSE
                                      IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [4] THEN
			                              RETURN [2]
				                      ELSE
					                      IF rand EQ [4] THEN
						                      RETURN [6]
					                      ELSE
						                      GOTO [0,0]",
                        new Tuple<int, int>[4] { new Tuple<int, int>(35, 10), new Tuple<int, int>(45, 35), new Tuple<int, int>(7, 17), new Tuple<int, int>(22, 42) }
                        );

                    // GREEN
                    level.AI[2] = new SwarmDetails(PlotColor.Green,
                         @"IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [8] THEN
	                        RETURN [3]
	                      ELSE
		                      IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [4] THEN
			                      RETURN [2]
		                      ELSE
			                      IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [4] THEN
				                      RETURN [1]
			                      ELSE
				                      IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [8] THEN
					                      RETURN [4]
				                      ELSE
					                      IF rand EQ [4] THEN
						                      RETURN [6]
					                      ELSE
						                      GOTO [0,0]",
                        new Tuple<int, int>[4] { new Tuple<int, int>(20, 45), new Tuple<int, int>(5, 45), new Tuple<int, int>(20, 26), new Tuple<int, int>(47, 5) }
                        );

                    // ratings
                    level.ShorestSolution = 1;
                    level.LeastInterations = 1;

                    return level;
        }

        public static LevelDetails GoLeftLevel()
        {
            LevelDetails level = new LevelDetails();

                    // field
                    level.Field = GetField(25, 0, new byte[,] {{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}});

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(25, 0);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[1];
                    level.End[0] = new Tuple<int, int>(25, 24);     // hardcoded values from table above

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-75, 300);

                    // ratings
                    level.ShorestSolution = 12;
                    level.LeastInterations = 24;

                    return level;
        }

        public static LevelDetails FallingEnemiesLevel()
        {
            LevelDetails level = new LevelDetails();

                    // field
                    level.Field = GetField(17, 0, new byte[,] {
                            {0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0},
							{0,0,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0,1,0,0}
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(25, 0);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[1];
                    level.End[0] = new Tuple<int, int>(25, 24);     // hardcoded values from table above

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 250);

                    // AI
                    level.AI = new SwarmDetails[3];

                    // RED
                    level.AI[0] = new SwarmDetails(PlotColor.Red,
                        "RETURN [2]",
                        new Tuple<int,int>[2] { new Tuple<int,int>(17, 4), new Tuple<int,int>(17, 7)}
                        );

                    // YELLOW
                    level.AI[1] = new SwarmDetails(PlotColor.Yellow,
                        @"IF grid EQ [{9 0 9}{9 9 9}{9 9 9}] THEN 
                            RETURN [6]
                          ELSE
                            RETURN [2]",
                        new Tuple<int, int>[2] { new Tuple<int, int>(17, 10), new Tuple<int, int>(17, 13) }
                        );

                    // GREEN
                    level.AI[2] = new SwarmDetails(PlotColor.Green,
                        @"IF grid EQ [{9 0 9}{9 9 9}{9 9 9}] THEN 
                            RETURN [6]
                          ELSE
                            RETURN [2]",
                        new Tuple<int, int>[3] { new Tuple<int, int>(17, 16), new Tuple<int, int>(17, 19), new Tuple<int, int>(17, 22) }
                        );

                    // ratings
                    level.ShorestSolution = 86;
                    level.LeastInterations = 26;

                    return level;
        }

        public static LevelDetails SkirtTheEdgeLevel()
        {
            LevelDetails level = new LevelDetails();
                
                    // field
                    level.Field = GetField(20, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(25, 0);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[1];
                    level.End[0] = new Tuple<int, int>(25, 24);     // hardcoded values from table above

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 250);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // Yellow
                    level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                        @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 6 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{6 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}]THEN
                        	RETURN [5]
                          ELSE
                            IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{0 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 0 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                            	RETURN [4]
                            ELSE
	                            IF matrix EQ [{9 9 0 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 0 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
		                            RETURN [2]
	                            ELSE
		                            IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 0 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 0}{9 9 9 9 9}{9 9 9 9 9}] THEN
			                            RETURN [3]
		                            ELSE
			                            GOTO
                        IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [4] THEN
	                        RETURN [1]
                        ELSE
	                        IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [4] THEN
		                        RETURN [2]
	                        ELSE
		                        IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [4] THEN
			                        RETURN [3]
		                        ELSE
			                        IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [4] THEN
				                        RETURN [4]
			                        ELSE
				                        IF rand EQ [4] THEN
					                        RETURN [6]
				                        ELSE
					                        GOTO [1,0]",
                        new Tuple<int, int>[5] { new Tuple<int, int>(25, 2), new Tuple<int, int>(24, 2), new Tuple<int, int>(25, 21), new Tuple<int, int>(22, 23), new Tuple<int, int>(20, 10) }
                        );

                    // ratings
                    level.ShorestSolution = 160;
                    level.LeastInterations = 34;

                    return level;
        }

        public static LevelDetails FollowThePatternLevel()
        {
            LevelDetails level = new LevelDetails();

                    // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1}, // 15
                            {1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 20
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(21, 14);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[1];
                    level.End[0] = new Tuple<int, int>(23, 2);     // hardcoded values from table above

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 210);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 470;
                    level.LeastInterations = 18;

                    return level;
        }

        public static LevelDetails GoBothWaysLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(11, 0, new byte[,] {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 11
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 12
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0}, // 13
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0}, // 14
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0}, // 15
							{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0,0}, // 16
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 17
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0}, // 18
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 19
							{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0,0,0}, // 20
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0}, // 21
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0}, // 22
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0,0,0}, // 23
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 24
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[3] { new Tuple<int, int>(18, 0), new Tuple<int, int>(20, 3), new Tuple<int, int>(16, 3) };

                    // End
                    level.End = new Tuple<int, int>[3] { new Tuple<int, int>(18, 18), new Tuple<int, int>(23, 15), new Tuple<int, int>(13, 14) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 180);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 190;
                    level.LeastInterations = 18;

                    return level;
        }

        public static LevelDetails FollowTheLineLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(20, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,5,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(20, 0);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[1];
                    level.End[0] = new Tuple<int, int>(23, 21);     // hardcoded values from table above

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 250);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 137;
                    level.LeastInterations = 24;

                    return level;
        }

        public static LevelDetails MarchInLineLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,5,5,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,5,1,1,1,1}, // 19
                            {1,1,1,1,5,5,5,5,5,1,1,1,1,1,1,1,1,1,1,1,5,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,1,5,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,5,1,1,1,1,1,1,1,1,1,1,1,5,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,5,5,5,5,5,5,5,5,5,5,5,5,5,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(17, 2);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[1];
                    level.End[0] = new Tuple<int, int>(18, 20);     // hardcoded values from table above

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 200);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 286;
                    level.LeastInterations = 29;

                    return level;
        }

        public static LevelDetails RingAroundLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(20, 10);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[4] { new Tuple<int, int>(17, 0), new Tuple<int, int>(23, 0), new Tuple<int, int>(18, 24), new Tuple<int, int>(22, 24) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 210);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // GREEN
                    level.AI[0] = new SwarmDetails(PlotColor.Green,
                        @"IF grid EQ [{0 0 0}{9 9 0}{9 9 0}] THEN
                            RETURN [6]
                          ELSE
                             IF grid EQ [{9 0 9}{9 9 9}{9 9 9}] THEN
                               RETURN [4]
                              ELSE
                                 IF grid EQ [{9 9 9}{0 9 9}{9 9 9}] THEN
                                   RETURN [1]
                                ELSE
                                    IF grid EQ [{9 9 9}{9 9 9}{9 0 9}] THEN
                                        RETURN [3]
                                      ELSE
                                         IF grid EQ [{9 9 9}{9 9 0}{9 9 9}] THEN
                                           RETURN [2]
                                         ELSE
                                            GOTO
                           RETURN [5]",
                        new Tuple<int, int>[1] { new Tuple<int, int>(15, 7) }
                        );

                    // ratings
                    level.ShorestSolution = 308;
                    level.LeastInterations = 61;
                    
            return level;
        }

        public static LevelDetails DiagnalLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(23, 2) };

                    // End
                    level.End = new Tuple<int, int>[1] { new Tuple<int, int>(17, 9)};

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 66;
                    level.LeastInterations = 13;
                    
            return level;
        }

        public static LevelDetails FanoutLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(20, 5) };

                    // End
                    level.End = new Tuple<int, int>[4] { new Tuple<int, int>(20, 2), new Tuple<int, int>(20, 8), new Tuple<int, int>(17, 5), new Tuple<int, int>(23, 5) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 342;
                    level.LeastInterations = 89;
                    
            return level;
        }

        public static LevelDetails FourInARowLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(23, 2) };

                    // End
                    level.End = new Tuple<int, int>[4] { new Tuple<int, int>(23, 9), new Tuple<int, int>(21, 9), new Tuple<int, int>(19, 9), new Tuple<int, int>(17, 9) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 191;
                    level.LeastInterations = 15;
                    
            return level;
        }

        public static LevelDetails GuardedExitLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(11, 0, new byte[,] {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 11
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 12
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 13
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 14
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 15
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 16
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 17
							{0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0}, // 18
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 19
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 20
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 21
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 22
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 23
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 24
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(18, 2) };

                    // End
                    level.End = new Tuple<int, int>[1] { new Tuple<int, int>(23, 22)};

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 180);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // GREEN
                    level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                        @"IF grid EQ [{0 0 0}{0 9 0}{0 9 0}] THEN
                            RETURN [6]
                          ELSE
                            RETURN [2]",
                        new Tuple<int, int>[1] { new Tuple<int, int>(13, 22) }
                        );

                    // ratings
                    level.ShorestSolution = 160;
                    level.LeastInterations = 26;

                    return level;
        }

        public static LevelDetails BlowTheWallLevel()
        {
            LevelDetails level = new LevelDetails();
            Tuple<int,int>[] starting;
            int cnt;

                    // field
                    level.Field = GetField(21, 0, new byte[,] {
							{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(23, 0);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[1] { new Tuple<int, int>(23, 24)};

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 250);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // GREEN
                    starting = new Tuple<int,int>[15];
                    cnt = 0;
                    for(int h=21; h<=25; h++)
                        for(int w=20; w<=22; w++)
                            starting[cnt++] = new Tuple<int, int>(h, w);
                    level.AI[0] = new SwarmDetails(PlotColor.Red,
                        @"RETURN [5]",
                        starting
                        );

                    // ratings
                    level.ShorestSolution = 160;
                    level.LeastInterations = 44;
                    
            return level;
        }

        public static LevelDetails ProximityMinesLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(19, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(22, 0);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[1] { new Tuple<int, int>(22, 24) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 250);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // Yellow
                    level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                        @"IF grid EQ [{9 9 9}{2 9 9}{9 9 9}] or grid EQ [{9 9 9}{9 9 2}{9 9 9}] or grid EQ [{9 2 9}{9 9 9}{9 9 9}] or grid EQ [{9 9 9}{9 9 9}{9 2 9}]  THEN
                            RETURN [7]
                          ELSE
                            RETURN [5]",
                        new Tuple<int, int>[3] { new Tuple<int, int>(20, 5), new Tuple<int, int>(24, 5), new Tuple<int, int>(22, 22)}
                        );

                    // ratings
                    level.ShorestSolution = 86;
                    level.LeastInterations = 44;
                    
            return level;
        }

        public static LevelDetails BlowYourWayOutLevel()
        {
            LevelDetails level = new LevelDetails();
            Tuple<int, int>[] starting;
            int cnt;

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(15, 0) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(25, 11) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // RED
            starting = new Tuple<int, int>[128];
            cnt = 0;
            for (int h = 15; h <= 25; h++)
            {
                for (int w = 0; w <= 11; w++)
                {
                    if ((h == 15 && (w == 0 || w == 1 || w == 2))
                        ||
                        (h == 25 && w == 11)) { } // skip
                    else starting[cnt++] = new Tuple<int, int>(h, w);
                }
            }
            level.AI[0] = new SwarmDetails(PlotColor.Red,
                @"RETURN [5]",
                starting
                );

            // ratings
            level.ShorestSolution = 269;
            level.LeastInterations = 111;

            return level;
        }

        public static LevelDetails RaceTheFloodLevel()
        {
            LevelDetails level = new LevelDetails();
            int cnt;

            // field
                    level.Field = GetField(21, 0, new byte[,] {
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(21, 20);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[10];
                    cnt = 0;
                    for (int h = 21; h <= 25; h++)
                        for (int w = 0; w <= 1; w++)
                        {
                            level.End[cnt++] = new Tuple<int, int>(h, w);
                        }

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 250);

                    // AI
                    level.AI = new SwarmDetails[3];

                    // RED
                    level.AI[0] = new SwarmDetails(PlotColor.Red,
                        @"IF grid EQ [{9 9 0}{9 9 0}{9 9 0}] THEN
	                        RETURN [6]
	                      ELSE
			                RETURN [3]",
                        new Tuple<int, int>[2] { new Tuple<int, int>(21, 24), new Tuple<int, int>(22, 24) }
                        );

                    // YELLOW
                    level.AI[1] = new SwarmDetails(PlotColor.Yellow,
                         level.AI[0].Script,
                        new Tuple<int, int>[1] { new Tuple<int, int>(23, 24) }
                        );

                    // GREEN
                    level.AI[2] = new SwarmDetails(PlotColor.Green,
                         level.AI[0].Script,
                        new Tuple<int, int>[2] { new Tuple<int, int>(24, 24), new Tuple<int, int>(25, 24) }
                        );

                    // ratings
                    level.ShorestSolution = 359;
                    level.LeastInterations = 40;
                    
            return level;
        }

        public static LevelDetails RaceToTheEndLevel()
        {
            LevelDetails level = new LevelDetails();
            int cnt;
            
                    // field
                    level.Field = GetField(20, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1];
                    level.Start[0] = new Tuple<int, int>(25, 6);     // hardcoded values from table above

                    // End
                    level.End = new Tuple<int, int>[5];
                    cnt = 0;
                    for (int h = 20; h <= 24; h++)
                        level.End[cnt++] = new Tuple<int, int>(h, 24);

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 250);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // GREEN
                    level.AI[0] = new SwarmDetails(PlotColor.Green,
                        @"IF grid EQ [{9 9 0}{9 9 0}{9 9 0}] THEN
	                        RETURN [5]
	                      ELSE
			                RETURN [4]",
                        new Tuple<int, int>[5] { new Tuple<int, int>(20, 0), new Tuple<int, int>(21, 0), new Tuple<int, int>(22, 0), new Tuple<int, int>(23, 0), new Tuple<int, int>(24, 0) }
                        );

                    // ratings
                    level.ShorestSolution = 86;
                    level.LeastInterations = 23;
                    
            return level;
        }

        public static LevelDetails ZigZagLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(20, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(22, 0) };

                    // End
                    level.End = new Tuple<int, int>[1] { new Tuple<int, int>(22, 24) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[3];

                    // RED
                    level.AI[0] = new SwarmDetails(PlotColor.Red,
                        @"IF grid EQ [{9 0 9}{9 9 9}{9 9 9}] THEN
	                        RETURN [2]
	                      ELSE
			                IF grid EQ [{9 9 9}{9 9 9}{9 0 9}] THEN
                                RETURN [1]
                            ELSE
                                IF grid EQ [{9 9 9}{2 9 9}{9 9 9}] or grid EQ [{9 9 9}{9 9 2}{9 9 9}] or grid EQ [{9 2 9}{9 9 9}{9 9 9}] or grid EQ [{9 9 9}{9 9 9}{9 2 9}]  THEN
                                    RETURN [5]
                                ELSE
                                    GOTO
                        IF last EQ [1] THEN
                            RETURN [1]
                        ELSE
                            RETURN [2]",
                        new Tuple<int, int>[4] { new Tuple<int, int>(20,5), new Tuple<int, int>(23, 11), new Tuple<int, int>(23, 15), new Tuple<int, int>(20, 21) }
                        );

                    // Green
                    level.AI[1] = new SwarmDetails(PlotColor.Green,
                        level.AI[0].Script,
                        new Tuple<int, int>[3] { new Tuple<int, int>(21, 7), new Tuple<int, int>(24, 13), new Tuple<int, int>(21, 19) }
                        );

                    // YELLOW
                    level.AI[2] = new SwarmDetails(PlotColor.Yellow,
                        level.AI[0].Script,
                        new Tuple<int, int>[2] { new Tuple<int, int>(22, 9), new Tuple<int, int>(22, 17) }
                        );

                    // ratings
                    level.ShorestSolution = 160;
                    level.LeastInterations = 28;
                    
            return level;
        }

        public static LevelDetails WobbleEnemyLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,0,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //           6
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(24, 1) };

                    // End
                    level.End = new Tuple<int, int>[4] { new Tuple<int, int>(19, 6), new Tuple<int, int>(20, 7), new Tuple<int, int>(21, 6), new Tuple<int, int>(20, 5) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[2];

                    // RED
                    level.AI[0] = new SwarmDetails(PlotColor.Red,
                        @"IF last EQ [3] THEN
                            RETURN [4]
                          ELSE
                            RETURN [3]",
                        new Tuple<int, int>[2]{ new Tuple<int, int>(18, 6), new Tuple<int, int>(22, 7) }
                        );

                    // GREE
                    level.AI[1] = new SwarmDetails(PlotColor.Green,
                        @"IF last EQ [1] THEN
                            RETURN [2]
                          ELSE
                            RETURN [1]",
                        new Tuple<int, int>[2] { new Tuple<int, int>(20, 4), new Tuple<int, int>(21, 8) }
                        );

                    // ratings
                    level.ShorestSolution = 319;
                    level.LeastInterations = 14;
                    
            return level;
        }

        public static LevelDetails ClearTheBoardLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,0,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,0,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,0,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,0,1,1,1,1,1}, // 22
							{1,1,1,0,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //           6
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(15, 11) };

                    // End
                    level.End = new Tuple<int, int>[5] { new Tuple<int, int>(18, 6), new Tuple<int, int>(23, 10), new Tuple<int, int>(19, 3), new Tuple<int, int>(16, 2), new Tuple<int, int>(25, 4) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 526;
                    level.LeastInterations = 115;
                    
            return level;
        }

        public static LevelDetails FauxDefenseLevel()
        {
            LevelDetails level = new LevelDetails();
            Tuple<int,int>[] starting;
            int cnt;

            // field
                    level.Field = GetField(20, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
                            //                                               24
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(22, 24) };

                    // End
                    level.End = new Tuple<int, int>[1] { new Tuple<int, int>(22, 0) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // YELLOW
                    starting = new Tuple<int, int>[10];
                    cnt = 0;
                    for (int w = 12; w <= 13; w++)
                        for (int h = 20; h <= 24; h++)
                            starting[cnt++] = new Tuple<int, int>(h, w);
                    level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                        @"IF grid EQ [{9 2 9}{9 9 9}{9 2 9}] THEN
                            RETURN [5]
                          ELSE
                            GOTO
                          IF grid EQ [{9 9 9}{9 9 2}{9 9 9}] THEN
	                        RETURN [0]
	                      ELSE
			                RETURN [5]",
                        starting
                        );

                    // ratings
                    level.ShorestSolution = 160;
                    level.LeastInterations = 49;
                    
            return level;
        }

        public static LevelDetails StickToTheEdgeLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //           6         11
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(25, 11) };

                    // End
                    level.End = new Tuple<int, int>[4] { new Tuple<int, int>(24, 6), new Tuple<int, int>(16, 6), new Tuple<int, int>(20, 1), new Tuple<int, int>(20, 10) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // Yellow
                    level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                        @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 0 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 0 9 9}] THEN
                        	RETURN [1]
                          ELSE
                            IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{0 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 0 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                            	RETURN [4]
                            ELSE
	                            IF matrix EQ [{9 9 0 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 0 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
		                            RETURN [2]
	                            ELSE
		                            IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 0 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 0}{9 9 9 9 9}{9 9 9 9 9}] THEN
			                            RETURN [3]
		                            ELSE
			                            GOTO
                        IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [4] THEN
	                        RETURN [1]
                        ELSE
	                        IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [4] THEN
		                        RETURN [2]
	                        ELSE
		                        IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [4] THEN
			                        RETURN [3]
		                        ELSE
			                        IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [4] THEN
				                        RETURN [4]
			                        ELSE
				                        IF rand EQ [64] THEN
					                        RETURN [6]
				                        ELSE
					                        GOTO [1,0]",
                        new Tuple<int, int>[4] { new Tuple<int, int>(19, 6), new Tuple<int, int>(20, 7), new Tuple<int, int>(21, 6), new Tuple<int, int>(20, 5) }
                        );

                    // ratings
                    level.ShorestSolution = 377;
                    level.LeastInterations = 32;
                    
            return level;
        }

        public static LevelDetails MeetInTheMiddleLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(16, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //           6         11
                    });

                    // Start
                    level.Start = new Tuple<int, int>[4] { new Tuple<int, int>(16, 0), new Tuple<int, int>(25, 0), new Tuple<int, int>(16, 11), new Tuple<int, int>(25, 11) };

                    // End
                    level.End = new Tuple<int, int>[4] { new Tuple<int, int>(20, 6), new Tuple<int, int>(20, 7), new Tuple<int, int>(21, 5), new Tuple<int, int>(21, 6) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 176;
                    level.LeastInterations = 12;
                    
            return level;
        }

        public static LevelDetails SpreadFromTheCenterLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,0,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //         5 6       10
                    });

                    // Start
                    level.Start = new Tuple<int, int>[4] { new Tuple<int, int>(19, 5), new Tuple<int, int>(21, 5), new Tuple<int, int>(20, 4), new Tuple<int, int>(20, 6) };

                    // End
                    level.End = new Tuple<int, int>[4] { new Tuple<int, int>(15, 5), new Tuple<int, int>(25, 5), new Tuple<int, int>(20, 0), new Tuple<int, int>(20, 10) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[0];

                    // ratings
                    level.ShorestSolution = 279;
                    level.LeastInterations = 4;
                    
            return level;
        }

        public static LevelDetails MarchOnTheKeepLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
                    level.Field = GetField(15, 5, new byte[,] {
                            {1,1,1,1,1}, // 15
                            {1,1,1,1,1}, // 16
                            {1,1,1,1,1}, // 17
                            {1,1,1,1,1}, // 18
                            {1,1,1,1,1}, // 19
                            {1,1,1,1,1}, // 20
							{1,1,1,1,1}, // 21
							{1,1,1,1,1}, // 22
							{1,1,1,1,1}, // 23
							{1,1,1,1,1}, // 24
							{1,1,1,1,1}  // 25
                          // 5       9 
                    });

                    // Start
                    level.Start = new Tuple<int, int>[5] { new Tuple<int, int>(25, 5), new Tuple<int, int>(25, 6), new Tuple<int, int>(25, 7), new Tuple<int, int>(25, 8), new Tuple<int, int>(25, 9)};

                    // End
                    level.End = new Tuple<int, int>[1] { new Tuple<int, int>(15, 7) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-140, 210);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // Yellow
                    level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                        @"RETURN [5]",
                        new Tuple<int, int>[3] { new Tuple<int, int>(15, 6), new Tuple<int, int>(15, 8), new Tuple<int, int>(16, 7) }
                        );

                    // ratings
                    level.ShorestSolution = 160;
                    level.LeastInterations = 20;
         
            return level;
        }
                
        public static LevelDetails OnSlaughtOfEnemiesLevel()
        {
            LevelDetails level = new LevelDetails();

                    // field
                    level.Field = GetField(11, 0, new byte[,] {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 11
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 12
                            {0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0}, // 13
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 14
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 15
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 16
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 17
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 18
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 19
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 20
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 21
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 22
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0}, // 23
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 24
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 25
                    });

                    // Start
                    level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(13, 2) };

                    // End
                    level.End = new Tuple<int, int>[1] { new Tuple<int, int>(23, 22) };

                    // Scale
                    level.Scale = 3.0;

                    // Center
                    level.Center = new Tuple<double, double>(-70, 180);

                    // AI
                    level.AI = new SwarmDetails[1];

                    // GREEN
                    level.AI[0] = new SwarmDetails(PlotColor.Green,
                        @"IF grid EQ [{0 9 0}{0 9 0}{0 0 0}] THEN
                            RETURN [6]
                          ELSE
                            IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{6 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                                RETURN [7]
                            ELSE
                                IF grid EQ [{9 0 9}{9 9 9}{9 9 9}] THEN
                                    RETURN [3]
                                ELSE
                                    RETURN [1]",
                        new Tuple<int, int>[1] { new Tuple<int, int>(23, 22) }
                        );

                    // ratings
                    level.ShorestSolution = 451;
                    level.LeastInterations = 43;

                    return level;
        }

        public static LevelDetails SpiralLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,0,0,0,0,0,0,0,0,0,1}, // 16
                            {1,0,1,1,1,1,1,1,1,0,1}, // 17
                            {1,0,1,0,0,0,0,0,1,0,1}, // 18
                            {1,0,1,0,1,1,1,0,1,0,1}, // 19
                            {1,0,1,0,1,0,1,0,1,0,1}, // 20
							{1,0,1,0,1,0,0,0,1,0,1}, // 21
							{1,0,1,0,1,1,1,1,1,0,1}, // 22
							{1,0,1,0,0,0,0,0,0,0,1}, // 23
							{1,0,1,1,1,1,1,1,1,1,1}, // 24
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(20, 6) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(24, 0) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 306;
            level.LeastInterations = 64;

            return level;
        }

        public static LevelDetails TrackerLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{0,0,0,0,0,0,0,1,0,0,0,0}, // 21
							{0,0,0,0,0,0,0,1,0,0,0,0}, // 22
							{0,0,0,0,0,0,0,1,0,0,0,0}, // 23
							{0,0,0,0,0,0,0,1,0,0,0,0}, // 24
							{0,0,0,0,0,0,0,1,0,0,0,0}  // 25
                        //                 7       11
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(20, 0) };

            // End
            level.End = new Tuple<int, int>[2] { new Tuple<int, int>(25, 7), new Tuple<int, int>(15, 11) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // GREEN
            level.AI[0] = new SwarmDetails(PlotColor.Green,
                @"IF grid EQ [{9 9 9}{9 9 9}{9 9 2}] THEN
                    RETURN [4]
                  ELSE
                    GOTO
                IF grid EQ [{9 9 9}{9 9 9}{2 9 9}] THEN
                    RETURN [3]
                  ELSE
                    GOTO
                IF grid EQ [{2 9 9}{9 9 9}{9 9 9}] THEN
                    RETURN [3]
                  ELSE
                    GOTO
                IF grid EQ [{9 9 2}{9 9 9}{9 9 9}] THEN
                    RETURN [4]
                  ELSE
                    GOTO
                RETURN [0]",
                new Tuple<int, int>[1] { new Tuple<int, int>(19, 0) }
                );

            // ratings
            level.ShorestSolution = 249;
            level.LeastInterations = 26;

            return level;
        }

        public static LevelDetails TrackGridLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,0,1,1,1,1,1}, // 17
                            {1,1,0,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,0,1,1,1,1}, // 20
							{1,1,1,1,1,0,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,0,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(15, 11) };

            // End
            level.End = new Tuple<int, int>[4] { new Tuple<int, int>(19, 4), new Tuple<int, int>(15, 6), new Tuple<int, int>(21, 8), new Tuple<int, int>(24, 7) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // RED
            level.AI[0] = new SwarmDetails(PlotColor.Red,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 6 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}] THEN
                    RETURN [4]
                  ELSE
                    GOTO
                IF matrix EQ [{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 6 9 9 9}] THEN
                    RETURN [3]
                  ELSE
                    GOTO
                IF matrix EQ [{9 6 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 6 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 6 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 6 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                    RETURN [1]
                  ELSE
                    GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 6 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 6 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 6 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 6 9}] THEN
                    RETURN [2]
                  ELSE
                    GOTO
                RETURN [0]",
                new Tuple<int, int>[3] { new Tuple<int, int>(23, 3), new Tuple<int, int>(24, 9), new Tuple<int, int>(19, 5) }
                );

            // ratings
            level.ShorestSolution = 342;
            level.LeastInterations = 109;

            return level;
        }

        public static LevelDetails CornerDefenseLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(15, 0) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(25, 11)};

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // YELLOW
            level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                @"IF grid EQ [{9 9 0}{9 9 0}{0 0 0}] THEN
                    RETURN [6]
                  ELSE
                    GOTO

                IF matrix EQ [{9 9 9 0 0}{9 9 9 0 0}{9 9 9 0 0}{9 9 2 0 0}{0 0 0 0 0}] AND grid EQ [{9 9 9}{1 9 9}{9 9 9}]  THEN
                    RETURN [3]
                ELSE
                    GOTO

                IF matrix EQ [{9 9 9 9 0}{9 9 9 9 0}{9 9 9 2 0}{0 0 0 0 0}{0 0 0 0 0}] AND grid EQ [{9 9 9}{1 9 9}{9 9 9}] THEN
                    RETURN [3]
                  ELSE
                    GOTO
                IF matrix EQ [{9 9 9 0 0}{9 9 9 0 0}{9 9 9 0 0}{9 9 2 0 0}{0 0 0 0 0}] AND grid EQ [{9 1 9}{9 9 9}{9 9 9}]  THEN
                    RETURN [1]
                ELSE
                    GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 2}{0 0 0 0 0}{0 0 0 0 0}] OR matrix EQ [{9 9 9 0 0}{9 9 9 0 0}{9 9 9 0 0}{9 9 9 0 0}{9 9 9 0 0}] OR matrix EQ [{9 9 9 9 0}{9 9 9 9 0}{9 9 9 9 0}{9 9 9 2 0}{0 0 0 0 0}] THEN
                    RETURN [5]
                  ELSE
                    GOTO
                RETURN [0]",
                new Tuple<int, int>[1] { new Tuple<int, int>(25, 11)}
                );

            // ratings
            level.ShorestSolution = 305;
            level.LeastInterations = 61;

            return level;
        }

        public static LevelDetails IntroExplodeLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(25, 0, new byte[,] { { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 } });

            // Start
            level.Start = new Tuple<int, int>[2] { new Tuple<int, int>(25, 24), new Tuple<int, int>(25, 19) };     // hardcoded values from table above

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(25, 0) };     // hardcoded values from table above

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-75, 300);

            // AI
            level.AI = new SwarmDetails[1];

            // RED
            level.AI[0] = new SwarmDetails(PlotColor.Red,
                @"RETURN [5]",
                new Tuple<int, int>[1] { new Tuple<int, int>(25, 5) }
                );

            // ratings
            level.ShorestSolution = 86;
            level.LeastInterations = 24;

            return level;
        }

        public static LevelDetails ProximityMineTrackLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,0,1,1}, // 17
                            {1,1,1,1,0,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,0,1,1,1,1}, // 20
							{1,1,1,1,1,0,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,0,1}, // 23
							{1,1,0,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(14, 2) };     // hardcoded values from table above

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(19, 6) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-100, 200);

            // AI
            level.AI = new SwarmDetails[1];

            // Red
            level.AI[0] = new SwarmDetails(PlotColor.Red,
                @"IF grid EQ [{9 9 9}{2 9 9}{9 9 9}] or grid EQ [{9 9 9}{9 9 2}{9 9 9}] or grid EQ [{9 2 9}{9 9 9}{9 9 9}] or grid EQ [{9 9 9}{9 9 9}{9 2 9}]  THEN
                            RETURN [7]
                          ELSE
                            RETURN [5]",
                new Tuple<int, int>[4] { new Tuple<int, int>(21, 3), new Tuple<int, int>(22, 6), new Tuple<int, int>(20, 10), new Tuple<int, int>(17, 6) }
                );

            // ratings
            level.ShorestSolution = 137;
            level.LeastInterations = 9;

            return level;
        }

        public static LevelDetails EnemyDuplicatesLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(20, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
                          //                                                 24
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(22, 0) };

            // End
            level.End = new Tuple<int, int>[5] { new Tuple<int, int>(20, 24), new Tuple<int, int>(21, 24), new Tuple<int, int>(22, 24), new Tuple<int, int>(23, 24), new Tuple<int, int>(24, 24) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // RED
            level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                @"IF grid EQ [{0 0 0}{9 9 9}{9 9 9}] OR grid EQ [{9 9 9}{9 9 9}{0 0 0}] THEN
                    RETURN [5]
                  ELSE
                    RETURN [6]",
                new Tuple<int, int>[1] { new Tuple<int, int>(22, 22) }
                );

            // ratings
            level.ShorestSolution = 86;
            level.LeastInterations = 41;

            return level;
        }

        public static LevelDetails OpenBattleLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(0, 0, new byte[0,0]);

            for (int h = 0; h < level.Field.GetLength(0); h++)
                for (int w = 0; w < level.Field.GetLength(1); w++)
                    level.Field[h, w] = PlotState.Unoccupied;

            // Start
            level.Start = null;

            // End
            level.End = new Tuple<int, int>[0];

            // Battle
            level.IsBattle = true;
            level.BattleWinPercent = 0.5f;

            // Scale
            level.Scale = 2.0;

            // Center
            level.Center = new Tuple<double, double>(-160, -100);

            // AI
            level.AI = new SwarmDetails[(int)PlotColor.Yellow+1];

            // BLUE
            level.AI[(int)PlotColor.Blue] = new SwarmDetails(PlotColor.Blue,
                @"IF grid EQ [{0 0 0}{0 2 9}{0 9 9}] THEN RETURN [6] ELSE GOTO [1, 0] IF matrix EQ [{0 0 0 0 0}{0 0 0 0 0}{0 2 2 9 9}{0 9 9 9 9}{0 9 9 9 9}] THEN RETURN [5] ELSE GOTO [2, 0] IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN RETURN [4] ELSE GOTO [3, 0] IF grid EQ [{9 9 9}{9 2 9}{9 1 9}] THEN RETURN [2] ELSE GOTO [4, 0] IF rand EQ [64] THEN RETURN [6] ELSE GOTO [5, 0] IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 6 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 6 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 6 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 6 9}{9 9 9 9 9}{9 9 9 9 9}] THEN RETURN [7] ELSE GOTO [6, 0] RETURN [1]",
                new Tuple<int, int>[4] { new Tuple<int, int>(0, 0), new Tuple<int, int>(1, 0), new Tuple<int, int>(2, 0), new Tuple<int, int>(3, 0) });

            // YELLOW
            level.AI[(int)PlotColor.Yellow] = new SwarmDetails(PlotColor.Yellow,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO
                IF grid EQ [{9 9 9}{9 2 9}{9 1 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO
                RETURN [6]",
                new Tuple<int, int>[4] { new Tuple<int, int>(0, 49), new Tuple<int, int>(1, 49), new Tuple<int, int>(2, 49), new Tuple<int, int>(3, 49) }
                );

            // RED
            level.AI[(int)PlotColor.Red] = new SwarmDetails(PlotColor.Red,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO
                IF grid EQ [{9 1 9}{9 2 9}{9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO
                RETURN [6]",
                new Tuple<int, int>[4] { new Tuple<int, int>(46, 0), new Tuple<int, int>(47, 0), new Tuple<int, int>(48, 0), new Tuple<int, int>(49, 0) }
                );

            // GREEN
            level.AI[(int)PlotColor.Green] = new SwarmDetails(PlotColor.Green,
                @"IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [8] THEN
	                        RETURN [1]
	                      ELSE
		                      IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [8] THEN
			                      RETURN [2]
		                      ELSE
			                      IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [4] THEN
				                      RETURN [3]
			                      ELSE
				                      IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [4] THEN
					                      RETURN [4]
				                      ELSE
					                      IF rand EQ [4] THEN
						                      RETURN [6]
					                      ELSE
						                      GOTO [0,0]",
                new Tuple<int, int>[4] { new Tuple<int, int>(46, 49), new Tuple<int, int>(47, 49), new Tuple<int, int>(48, 49), new Tuple<int, int>(49, 49) }
                );
            
            // ratings
            level.ShorestSolution = 522;
            level.LeastInterations = 113;

            return level;
        }

        public static LevelDetails QuadBattleLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(0, 0, new byte[0, 0]);

            for (int h = 0; h < level.Field.GetLength(0); h++)
                for (int w = 0; w < level.Field.GetLength(1); w++)
                {
                    if (h == 24 && ((w >= 0 && w < 15) || (w >= 35 && w < 50))) level.Field[h,w] = PlotState.Forbidden;
                    else if (w == 24 && ((h >= 0 && h < 15) || (h >= 35 && h < 50))) level.Field[h,w] = PlotState.Forbidden;
                    else level.Field[h, w] = PlotState.Unoccupied;
                }

            // Start
            level.Start = null;

            // End
            level.End = new Tuple<int, int>[0];

            // Battle
            level.IsBattle = true;
            level.BattleWinPercent = 0.5f;

            // Scale
            level.Scale = 2.0;

            // Center
            level.Center = new Tuple<double, double>(-160, -100);

            // AI
            level.AI = new SwarmDetails[(int)PlotColor.Yellow + 1];

            // BLUE
            level.AI[(int)PlotColor.Blue] = new SwarmDetails(PlotColor.Blue,
                @"IF grid EQ [{0 0 0}{0 2 9}{0 9 9}] THEN RETURN [6] ELSE GOTO [1, 0] IF matrix EQ [{9 9 9 9 9}{9 9 1 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN RETURN [1] ELSE GOTO [2, 0] IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 1 9 9}{9 9 9 9 9}] THEN RETURN [2] ELSE GOTO [3, 0] IF grid EQ [{9 9 9}{9 2 1}{9 9 9}] THEN RETURN [4] ELSE GOTO [4, 0] IF grid EQ [{9 9 9}{9 2 9}{9 1 9}] THEN RETURN [2] ELSE GOTO [5, 0] RETURN [4]",
                new Tuple<int, int>[4] { new Tuple<int, int>(0, 0), new Tuple<int, int>(1, 0), new Tuple<int, int>(2, 0), new Tuple<int, int>(3, 0) });

            // YELLOW
            level.AI[(int)PlotColor.Yellow] = new SwarmDetails(PlotColor.Yellow,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF grid EQ [{9 9 9}{9 2 9}{9 1 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 6 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 6 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 6 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 6 9}{9 9 9 9 9}] THEN
	                RETURN [7]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 5 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 5 9 9}{9 9 9 9 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 5 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO
                RETURN [6]",
                new Tuple<int, int>[4] { new Tuple<int, int>(0, 49), new Tuple<int, int>(1, 49), new Tuple<int, int>(2, 49), new Tuple<int, int>(3, 49) }
                );

            // RED
            level.AI[(int)PlotColor.Red] = new SwarmDetails(PlotColor.Red,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO
                IF grid EQ [{9 1 9}{9 2 9}{9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 6 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 6 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 6 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 6 9}{9 9 9 9 9}] THEN
	                RETURN [7]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 5 9 9}{9 9 9 9 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 5 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO
                RETURN [6]",
                new Tuple<int, int>[4] { new Tuple<int, int>(46, 0), new Tuple<int, int>(47, 0), new Tuple<int, int>(48, 0), new Tuple<int, int>(49, 0) }
                );

            // GREEN
            level.AI[(int)PlotColor.Green] = new SwarmDetails(PlotColor.Green,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF grid EQ [{9 1 9}{9 2 9}{9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 6 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 6 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 6 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 6 9}{9 9 9 9 9}] THEN
	                RETURN [7]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 5 9 9}{9 9 9 9 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 5 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO
                RETURN [6]",
                new Tuple<int, int>[4] { new Tuple<int, int>(46, 49), new Tuple<int, int>(47, 49), new Tuple<int, int>(48, 49), new Tuple<int, int>(49, 49) }
                );

            // ratings
            level.ShorestSolution = 400;
            level.LeastInterations = 200;

            return level;
        }

        public static LevelDetails MazeBattleLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(0, 0, new byte[,] {					  
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0, 0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0, 0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,0,0,0,0,1,1,0,1,1,0,0,0,0,1,1},
							{1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,0,0,0,0,1,1,0,0,1,1,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,1,1,0,0,1,1,0,0,0,0,1,1,0,1,1},
							{1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,0, 0,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,0,0,0,0,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,0, 0,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,0,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,0,0,0,0,0,0,0,1,1,0, 0,1,1,0,0,0,0,0,0,0,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1},
							{1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1},
							{1,1,1,1,1,1,1,1,1,1,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,0,1,1,0,0,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,0,0,1,1,0,1,1,1,1,1,1,1,1,1,1},
							{0,0,0,0,0,0,1,1,0,0,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,1,1,0,0,0,0,0,0},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,0, 0,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,1,1,0,0,0, 0,0,0,1,1,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,1,1},
							{1,1,0,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,0, 0,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,0, 0,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,1,0,1,1},
							{1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1},
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1, 1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},

							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1, 1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
							{1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1, 1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1},
							{1,1,0,1,1,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,1,1,1,1,1,0,0,0,0,0,0,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,0, 0,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,0, 0,1,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,1,1,1,1,0,1,1},
							{1,1,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0,1,1,0,1,1,0,0,0, 0,0,0,1,1,0,1,1,0,0,0,0,1,1,0,0,0,0,0,0,0,0,0,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,1,1,0, 0,1,1,0,0,0,0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{0,0,0,0,0,0,1,1,0,0,0,1,1,0,1,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,1,1,0,0,0,0,0,0},
							{1,1,1,1,1,1,1,1,1,1,0,1,1,0,0,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,0,0,1,1,0,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0, 0,0,0,0,0,0,0,0,0,0,0,1,1,1,0,1,1,1,1,1,1,1,1,1,1},
							{1,1,0,0,0,0,0,0,0,0,0,0,1,1,0,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,0,0,0,0,0,0,0,1,1},
							{1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,0, 0,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,1,1,1,0,1,1,0,0,0,0,0,0,0,0,1,1,0, 0,1,1,0,0,0,0,0,0,0,0,1,1,0,1,1,1,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,0,0,0,0,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,0, 0,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,0,0,0,0,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,0, 0,1,1,0,1,1,1,1,1,1,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,0,1,1,0,1,1,0,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0,1,1},
							{1,1,0,1,1,0,0,0,0,1,1,0,0,1,1,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,1,1,0,0,1,1,0,0,0,0,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1},
							{1,1,0,1,1,1,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,1,1,1,0,1,1,0,1,1,1,1,1,0,1,1},
							{1,1,0,0,0,0,1,1,0,1,1,0,0,0,0,0,1,1,0,1,1,0,1,1,0, 0,1,1,0,1,1,0,1,1,0,0,0,0,0,1,1,0,1,1,0,0,0,0,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0, 0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1},
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,1,1,1,1,1,0, 0,1,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}
							}
                );

            // Start
            level.Start = null;

            // End
            level.End = new Tuple<int, int>[0];

            // Battle
            level.IsBattle = true;
            level.BattleWinPercent = 0.5f;

            // Scale
            level.Scale = 2.0;

            // Center
            level.Center = new Tuple<double, double>(-160, -100);

            // AI
            level.AI = new SwarmDetails[(int)PlotColor.Yellow + 1];

            // BLUE
            level.AI[(int)PlotColor.Blue] = new SwarmDetails(PlotColor.Blue,
                @"IF grid EQ [{9 1 9}{9 2 9}{9 9 9}] AND rand EQ [4] THEN
                	RETURN [1]
                ELSE
                	GOTO [1, 0]
                IF grid EQ [{9 9 9}{9 2 1}{9 9 9}] AND rand EQ [4] THEN
                	RETURN [4]
                ELSE
                	GOTO [2, 0]
                IF grid EQ [{9 9 9}{9 2 9}{9 1 9}] AND rand EQ [8] THEN
                	RETURN [2]
                ELSE
                	GOTO [3, 0]
                IF grid EQ [{9 9 9}{1 2 9}{9 9 9}] AND rand EQ [4] THEN
            	    RETURN [3]
                ELSE
                	GOTO [4, 0]
                IF rand EQ [64] AND rand EQ [2] THEN
                	RETURN [6]
                ELSE
                	GOTO [5, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 1 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 5 9 9}{9 9 9 9 9}] THEN
                	RETURN [2]
                ELSE
                	GOTO [6, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                	RETURN [4]
                ELSE
                	GOTO [7, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 6 9 9}{9 6 2 6 9}{9 9 6 9 9}{9 9 9 9 9}] THEN
                	RETURN [7]
                ELSE
                	GOTO [8, 0]
                RETURN [5]",
                new Tuple<int, int>[4] { new Tuple<int, int>(0, 0), new Tuple<int, int>(1, 0), new Tuple<int, int>(2, 0), new Tuple<int, int>(3, 0) });

            // YELLOW
            level.AI[(int)PlotColor.Yellow] = new SwarmDetails(PlotColor.Yellow,
                @"IF grid EQ [{0 0 0}{9 2 0}{9 9 0}] OR grid EQ [{9 9 0}{9 2 0}{0 0 0}] THEN
	                RETURN [6]
                ELSE
	                GOTO [1, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 1 9 9}{9 9 9 9 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO [2, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO [3, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 1 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO [4, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO [5, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 5 9 9}{9 9 9 9 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO [6, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 5 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO [7, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 5 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO [8, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO [9, 0]
                RETURN [5]",
                new Tuple<int, int>[4] { new Tuple<int, int>(0, 49), new Tuple<int, int>(1, 49), new Tuple<int, int>(2, 49), new Tuple<int, int>(3, 49) }
                );

            // RED
            level.AI[(int)PlotColor.Red] = new SwarmDetails(PlotColor.Red,
                @"IF grid EQ [{0 9 9}{0 2 9}{0 0 0}] OR grid EQ [{0 0 0}{0 2 9}{0 9 9}] THEN
	                RETURN [6]
                ELSE
	                GOTO [1, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 1 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO [2, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO [3, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 1 9 9}{9 9 9 9 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO [4, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO [5, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO [6, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 5 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO [7, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 5 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO [8, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 5 9 9}{9 9 9 9 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO [9, 0]
                RETURN [5]",
                new Tuple<int, int>[4] { new Tuple<int, int>(46, 0), new Tuple<int, int>(47, 0), new Tuple<int, int>(48, 0), new Tuple<int, int>(49, 0) }
                );

            // GREEN
            level.AI[(int)PlotColor.Green] = new SwarmDetails(PlotColor.Green,
                @"IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [8] THEN
	                        RETURN [1]
	                      ELSE
		                      IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [8] THEN
			                      RETURN [2]
		                      ELSE
			                      IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [4] THEN
				                      RETURN [3]
			                      ELSE
				                      IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [4] THEN
					                      RETURN [4]
				                      ELSE
					                      IF rand EQ [4] THEN
						                      RETURN [6]
					                      ELSE
						                      GOTO [0,0]",
                 new Tuple<int, int>[4] { new Tuple<int, int>(46, 49), new Tuple<int, int>(47, 49), new Tuple<int, int>(48, 49), new Tuple<int, int>(49, 49) }
                );

            // ratings
            level.ShorestSolution = 400;
            level.LeastInterations = 200;

            return level;
        }

        public static LevelDetails HillBattleLevel()
        {
            int[][] wbounds = new int[][] {
						// WIDTH_VALUE LOW_HEIGHT HIGH_HEIGHT EXCLUDE_H EXCLUDE_H
						new int[]{5 , 5, 44, 0, 0}, // left
						new int[]{44, 5, 44, 0, 0}, // right

						new int[]{10, 10, 39, 25, 24}, // left n=2
						new int[]{39, 10, 39, 25, 24}, // right n=2

						new int[]{15, 15, 34, 0, 0}, // left n=3
						new int[]{34, 15, 34, 0, 0}, // right n=3

						new int[]{20, 20, 29, 25, 24}, // left n=4
						new int[]{29, 20, 29, 25, 24} // right n=4
						};
            int[][] hbounds = new int[][] {
						// HEIGHT_VALUE          LOW_WIDTH           HIGH_WIDTH               EXCLUE_W       EXCLUDE_W
						new int[]{5,  5, 44, 25, 24}, // top
						new int[]{44, 5, 44, 25, 24}, // bottom

						new int[]{10, 10, 39, 0, 0}, // top n=2
						new int[]{39, 10, 39, 0, 0}, // bottom n=2

						new int[]{15, 15, 34, 25, 24}, // top n=3
						new int[]{34, 15, 34, 25, 24}, // bottom n=3

						new int[]{20, 20, 29, 0, 0}, // top n=4
						new int[]{29, 20, 29, 0, 0} // bottom n=4
						};
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(0, 0, new byte[0, 0]);

            for (int w = 0; w < 50; w++)
            {
                for (int h = 0; h < 50; h++)
                {
                    level.Field[h, w] = PlotState.Unoccupied;

                    foreach (int[] bound in wbounds)
                    {
                        if (bound[0] == w && bound[1] <= h && h <= bound[2] && (h != bound[3] && h != bound[4]))
                        {
                            level.Field[h, w ] = PlotState.Forbidden;
                        }
                    }

                    foreach (int[] bound in hbounds)
                    {
                        if (bound[0] == h && bound[1] <= w && w <= bound[2] && (w != bound[3] && w != bound[4]))
                        {
                            level.Field[h, w] = PlotState.Forbidden;
                        }
                    }
                }
            }

            // Start
            level.Start = null;

            // End
            level.End = new Tuple<int, int>[0];

            // Battle
            level.IsBattle = true;
            level.BattleWinPercent = 0.5f;

            // Scale
            level.Scale = 2.0;

            // Center
            level.Center = new Tuple<double, double>(-160, -100);

            // AI
            level.AI = new SwarmDetails[(int)PlotColor.Yellow+1];

            // BLUE
            level.AI[(int)PlotColor.Blue] = new SwarmDetails(PlotColor.Blue,
                @"IF grid EQ [{0 0 0}{0 2 9}{0 9 9}] THEN RETURN [6] ELSE GOTO [1, 0] IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN RETURN [3] ELSE GOTO [2, 0] IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN RETURN [4] ELSE GOTO [3, 0] IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 1 9 9}{9 9 9 9 9}] THEN RETURN [2] ELSE GOTO [4, 0] IF matrix EQ [{9 9 9 9 9}{9 9 1 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN RETURN [1] ELSE GOTO [5, 0] IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN RETURN [4] ELSE GOTO [6, 0] IF grid EQ [{9 9 9}{9 2 1}{9 9 9}] THEN RETURN [4] ELSE GOTO [7, 0] RETURN [2]",
                new Tuple<int, int>[4] { new Tuple<int, int>(0, 0), new Tuple<int, int>(1, 0), new Tuple<int, int>(2, 0), new Tuple<int, int>(3, 0) });

            // YELLOW
            level.AI[(int)PlotColor.Yellow] = new SwarmDetails(PlotColor.Yellow,
                @"IF grid EQ [{0 0 0}{9 2 0}{9 9 0}] OR grid EQ [{9 9 0}{9 2 0}{0 0 0}] OR grid EQ [{0 9 9}{0 2 9}{0 0 0}] OR grid EQ [{0 0 0}{0 2 9}{0 9 9}] THEN
                	RETURN [6]
                ELSE
	                GOTO [1, 0]
                IF grid EQ [{9 9 9}{9 2 1}{9 0 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 2}] OR matrix EQ [{9 9 9 0 9}{9 9 9 0 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 0 9}] OR matrix EQ [{9 9 9 9 0}{9 9 9 9 0}{9 9 2 9 1}{9 9 9 9 9}{9 9 9 9 0}] THEN
	                RETURN [4]
                ELSE
	                GOTO [2, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 2 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 2 9 9 9}] OR grid EQ [{9 9 9}{9 2 1}{9 1 0}] OR grid EQ [{9 9 1}{9 2 0}{9 1 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO [3, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{0 0 1 9 0}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{0 0 2 9 0}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{0 0 5 9 0}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{0 0 6 9 0}] THEN
	                RETURN [2]
                ELSE
	                GOTO [4, 0]
                IF matrix EQ [{9 9 0 9 9}{9 9 0 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 0 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO [5, 0]
                IF grid EQ [{9 9 9}{0 2 9}{0 1 9}] OR matrix EQ [{9 9 9 9 9}{0 9 9 9 9}{0 9 2 9 9}{0 9 9 9 9}{0 9 9 9 9}] OR grid EQ [{9 9 9}{9 2 9}{0 1 9}] OR grid EQ [{0 9 9}{9 2 9}{9 1 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO [6, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 5 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO [7, 0]
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO [8, 0]
                IF grid EQ [{9 1 9}{9 2 9}{9 9 9}] AND rand EQ [4] THEN
	                RETURN [1]
                ELSE
	                GOTO [9, 0]
                RETURN [2]",
                new Tuple<int, int>[4] { new Tuple<int, int>(0, 49), new Tuple<int, int>(1, 49), new Tuple<int, int>(2, 49), new Tuple<int, int>(3, 49) }
                );

            // RED
            level.AI[(int)PlotColor.Red] = new SwarmDetails(PlotColor.Red,
                @"IF grid EQ [{9 1 9}{9 2 9}{9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 1 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 1 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [3]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 6 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 6 9 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 6 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 9 6 9}{9 9 9 9 9}] THEN
	                RETURN [7]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [4]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 2 9 9}{9 9 5 9 9}{9 9 9 9 9}] THEN
	                RETURN [2]
                ELSE
	                GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 5 9 9}{9 9 2 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
	                RETURN [1]
                ELSE
	                GOTO
                RETURN [6]",
                new Tuple<int, int>[4] { new Tuple<int, int>(46, 0), new Tuple<int, int>(47, 0), new Tuple<int, int>(48, 0), new Tuple<int, int>(49, 0) }
                );

            // GREEN
            level.AI[(int)PlotColor.Green] = new SwarmDetails(PlotColor.Green,
                @"IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] AND rand EQ [8] THEN
	                        RETURN [1]
	                      ELSE
		                      IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] AND rand EQ [8] THEN
			                      RETURN [2]
		                      ELSE
			                      IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] AND rand EQ [4] THEN
				                      RETURN [3]
			                      ELSE
				                      IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] AND rand EQ [4] THEN
					                      RETURN [4]
				                      ELSE
					                      IF rand EQ [4] THEN
						                      RETURN [6]
					                      ELSE
						                      GOTO [0,0]",
                new Tuple<int, int>[4] { new Tuple<int, int>(46, 49), new Tuple<int, int>(47, 49), new Tuple<int, int>(48, 49), new Tuple<int, int>(49, 49) }
                );


            // ratings
            level.ShorestSolution = 500;
            level.LeastInterations = 250;

            return level;
        }

        public static LevelDetails BiDirectionalLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,0,1}, // 15
                            {1,0,1}, // 16
                            {1,0,1}, // 17
                            {1,0,1}, // 18
                            {1,0,1}, // 19
                            {1,0,1}, // 20
							{1,0,1}, // 21
							{1,0,1}, // 22
							{1,0,1}, // 23
							{1,0,1}, // 24
							{1,0,1}  // 25
                    });

            // Start
            level.Start = new Tuple<int, int>[2] { new Tuple<int, int>(15, 0), new Tuple<int, int>(25, 2), };

            // End
            level.End = new Tuple<int, int>[2] { new Tuple<int, int>(25, 0), new Tuple<int, int>(15, 2)};

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 101;
            level.LeastInterations = 10;

            return level;
        }

        public static LevelDetails DownNAroundLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(20, 0, new byte[,] {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1}, // 20
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1}, // 21
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1}, // 24
                            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,0,0,0,0}, // 25
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0}, // 26
                            //                                               24
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(23, 0) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(21, 20) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 240);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 86;
            level.LeastInterations = 22;

            return level;
        }

        public static LevelDetails FindTheRightPathLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(17, 0, new byte[,] { 
                                {1,0,1,1,1,1,1,0,1,1,1,0,1}, // 17
                                {1,0,1,0,0,0,1,0,1,0,1,0,1}, // 18
                                {1,0,1,1,1,0,1,0,1,0,1,0,1}, // 19
                                {1,0,0,0,1,0,1,0,1,0,1,0,1}, // 20
                                {1,1,1,0,1,0,1,0,1,0,1,0,1}, // 21
                                {1,0,1,0,1,1,1,0,1,0,1,1,1}, // 22
                                {0,0,1,0,0,1,0,0,1,0,0,0,0}, // 23
                                {0,0,1,0,0,1,0,0,1,0,0,0,0}, // 24
                                {1,1,1,1,1,1,1,1,1,1,1,1,1}  // 25
                            //                           12
                        });

            // Start
            level.Start = new Tuple<int, int>[1];
            level.Start[0] = new Tuple<int, int>(25, 0);     // hardcoded values from table above

            // End
            level.End = new Tuple<int, int>[1];
            level.End[0] = new Tuple<int, int>(17, 12);     // hardcoded values from table above

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-75, 210);

            // ratings
            level.ShorestSolution = 431;
            level.LeastInterations = 38;

            return level;
        }

        public static LevelDetails AssassinLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(18, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(24, 0) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(18, 11) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // GREEN
            level.AI[0] = new SwarmDetails(PlotColor.Green,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 6 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}] OR grid EQ [{9 9 9}{9 9 2}{9 9 9}] THEN
                    RETURN [4]
                  ELSE
                    GOTO
                IF matrix EQ [{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 6 9 9 9}] OR grid EQ [{9 9 9}{2 9 9}{9 9 9}] THEN
                    RETURN [3]
                  ELSE
                    GOTO
                IF matrix EQ [{9 6 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 6 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 6 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 6 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR grid EQ [{9 2 9}{9 9 9}{9 9 9}] THEN
                    RETURN [1]
                  ELSE
                    GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 6 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 6 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 6 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 6 9}] OR grid EQ [{9 9 9}{9 9 9}{9 2 9}] THEN
                    RETURN [2]
                  ELSE
                    GOTO
                RETURN [0]",
                new Tuple<int, int>[5] { new Tuple<int, int>(23, 1), new Tuple<int, int>(19, 1), new Tuple<int, int>(23, 10), new Tuple<int, int>(19, 10), new Tuple<int, int>(21, 6) }
                );

            // ratings
            level.ShorestSolution = 234;
            level.LeastInterations = 53;

            return level;
        }

        public static LevelDetails MultiTierLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(17, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,0,0,0,0}, // 17
                            {1,1,1,1,1,1,1,1,0,0,0,0}, // 18
                            {1,1,1,1,1,1,1,1,0,0,0,0}, // 19
                            {1,1,1,1,1,1,1,1,1,1,0,0}, // 20
							{1,1,1,1,1,1,1,1,1,1,0,0}, // 21
							{1,1,1,1,1,1,1,1,1,1,0,0}, // 22
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 25
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[3] { new Tuple<int, int>(19, 0), new Tuple<int, int>(22, 0), new Tuple<int, int>(25, 0) };

            // End
            level.End = new Tuple<int, int>[3] { new Tuple<int, int>(17, 7), new Tuple<int, int>(20, 9), new Tuple<int, int>(23, 11) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 86;
            level.LeastInterations = 13;

            return level;
        }

        public static LevelDetails CirclingEnemyLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(14, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1,1}, // 14
                            {1,0,0,0,0,0,0,0,0,0,0,0,1}, // 15
                            {1,0,1,1,1,1,1,1,1,1,1,0,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,0,1,1,1,1,1,1,1,1,1,0,1}, // 18
                            {1,0,1,1,1,1,1,1,1,1,1,0,1}, // 19
                            {1,0,1,1,1,1,0,1,1,1,1,0,1}, // 20
							{1,0,1,1,1,1,1,1,1,1,1,0,1}, // 21
							{1,0,1,1,1,1,1,1,1,1,1,0,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,0,1,1,1,1,1,1,1,1,1,0,1}, // 24
							{1,0,0,0,0,0,0,0,0,0,0,0,1}, // 25
                            {1,1,1,1,1,1,1,1,1,1,1,1,1}  // 26
                            //           6           12
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(26, 6) };

            // End
            level.End = new Tuple<int, int>[4] { new Tuple<int, int>(19, 7), new Tuple<int, int>(19, 5), new Tuple<int, int>(21, 5), new Tuple<int, int>(21, 7) };

            // Scale
            level.Scale = 2.85;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // YELLOW
            level.AI[0] = new SwarmDetails(PlotColor.Yellow,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 0}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 0 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 0 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 0 9 9 9}] THEN
                    RETURN [4]
                  ELSE
                    IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{0 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{0 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{0 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{0 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                        RETURN [2]
                    ELSE
                        IF matrix EQ [{0 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 0 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 0 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 0 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                            RETURN [3]
                        ELSE
                            RETURN [1]",
                new Tuple<int, int>[4] { new Tuple<int, int>(22, 8), new Tuple<int, int>(22, 4), new Tuple<int, int>(18, 8), new Tuple<int, int>(18, 4) }
                );

            // ratings
            level.ShorestSolution = 666;
            level.LeastInterations = 24;

            return level;
        }

        public static LevelDetails UpNDownLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(20, 0, new byte[,] {
                            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1}, // 20
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0,0,0,0,0}, // 22
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,0,0,0,0,0,0,0}, // 23
							{0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,1,1,1,1,1}, // 24
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(22, 0) };

            // End
            level.End = new Tuple<int, int>[2] { new Tuple<int, int>(20, 24), new Tuple<int, int>(24, 24)};

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 86;
            level.LeastInterations = 41;

            return level;
        }

        public static LevelDetails ReplenishEnemyLevel()
        {
            LevelDetails level = new LevelDetails();
            Tuple<int, int>[] starting;
            int cnt;

            // field
            level.Field = GetField(19, 0, new byte[,] {
                            {0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0}, // 19
							{0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 21
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 22
                            {1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 23
							{0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0}, // 24
							{0,0,0,0,0,1,1,1,1,1,1,1,1,0,0,0,0}, // 25
                           //                      11        16
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(22, 17) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(22, 0) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // RED
            starting = new Tuple<int, int>[30];
            cnt = 0;
            for (int w = 6; w <= 11; w++)
                for (int h = 20; h <= 24; h++)
                    starting[cnt++] = new Tuple<int, int>(h, w);
            level.AI[0] = new SwarmDetails(PlotColor.Green,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 0 9}{9 9 9 9 9}{0 0 0 0 0}] OR grid EQ [{9 1 9}{9 9 9}{9 9 0}] THEN
                    RETURN [1]
                ELSE
                    GOTO
                IF matrix EQ [{0 0 0 0 0}{9 9 9 9 9}{9 9 9 0 9}{9 9 9 9 9}{9 9 9 9 9}] OR grid EQ [{9 9 0}{9 9 9}{9 1 9}] THEN
                    RETURN [2]
                ELSE
                    GOTO
                IF grid EQ [{0 0 0}{9 9 9}{9 9 9}] OR grid EQ [{9 2 9}{9 9 9}{0 0 0}] THEN
                    RETURN [4]
                ELSE
                    GOTO
                IF matrix EQ [{0 0 0 0 0}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                    IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] THEN
                        RETURN [6]
                    ELSE
                        GOTO
                ELSE
                    GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{0 0 0 0 0}] THEN
                    IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] THEN
                        RETURN [6]
                    ELSE
                        GOTO
                ELSE
                    GOTO
                RETURN [5]",
                starting
                );

            // ratings
            level.ShorestSolution = 195;
            level.LeastInterations = 23;

            return level;
        }

        public static LevelDetails FullCoverageLevel()
        {
            LevelDetails level = new LevelDetails();
            int cnt;
            int w, wd;

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1},  // 25
                            {1,1,1,1,1,1,1,1,1,1,1}  // 26
                            //           6       10
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(15, 0) };

            // End
            level.End = new Tuple<int, int>[22];
            cnt = 0;
            w = 0;
            wd = 10;
            for (int h = 16; h <= 26; h++)
            {
                level.End[cnt++] = new Tuple<int, int>(h, w);
                level.End[cnt++] = new Tuple<int, int>(h, wd);
                w++;
                wd--;
            }

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 220);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 449;
            level.LeastInterations = 51;

            return level;
        }

        public static LevelDetails MazeLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(17, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {0,0,0,0,0,0,1,0,0,0,0,0}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
							{1,0,0,0,0,0,0,0,0,0,0,0}, // 20
							{1,0,1,1,1,0,1,1,1,1,1,1}, // 21
							{1,0,1,0,1,0,0,0,0,0,0,1}, // 22
							{1,1,1,0,1,1,1,1,1,1,0,1}, // 23
                            {0,0,0,0,1,0,0,0,0,1,0,1}, // 24
                            {1,1,1,1,1,0,1,1,1,1,1,1}, // 25
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(25, 0) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(17, 0) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 342;
            level.LeastInterations = 32;

            return level;
        }

        public static LevelDetails ScatterLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(17, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 19
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1,1}, // 23
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 24
                            {1,1,1,1,1,1,1,1,1,1,1,1}, // 25
                            //           6         10
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(21, 6) };

            // End
            level.End = new Tuple<int, int>[6] { new Tuple<int, int>(18, 3), new Tuple<int, int>(20, 9), new Tuple<int, int>(22, 1), new Tuple<int, int>(24, 6), new Tuple<int, int>(25, 10), new Tuple<int, int>(19, 5) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 342;
            level.LeastInterations = 91;

            return level;
        }

        public static LevelDetails EnmeyInCirclesLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(17, 0, new byte[,] {
                            {0,0,0,0,0,1,0,0,0,0,0}, // 17
                            {0,1,1,1,1,1,1,1,1,1,0}, // 18
                            {0,1,1,1,1,1,1,1,1,1,0}, // 19
							{0,1,1,1,1,1,1,1,1,1,0}, // 20
							{1,1,1,1,1,1,1,1,1,1,1}, // 21
							{0,1,1,1,1,1,1,1,1,1,0}, // 22
							{0,1,1,1,1,1,1,1,1,1,0}, // 23
                            {0,1,1,1,1,1,1,1,1,1,0}, // 24
                            {0,0,0,0,0,1,0,0,0,0,0}, // 25
                            //         5         10
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(24, 1) };

            // End
            level.End = new Tuple<int, int>[4] { new Tuple<int, int>(17, 5), new Tuple<int, int>(25, 5), new Tuple<int, int>(21, 0), new Tuple<int, int>(21, 10)};

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // RED
            level.AI[0] = new SwarmDetails(PlotColor.Red,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 5 5 9}{9 9 9 5 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                    RETURN [2]
                ELSE
                    GOTO
                IF matrix EQ [{9 9 9 9 9}{9 5 5 9 9}{9 5 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                    RETURN [4]
                ELSE
                    GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 5 9 9 9}{9 5 5 9 9}{9 9 9 9 9}] THEN
                    RETURN [1]
                ELSE
                    GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 5 9}{9 9 5 5 9}{9 9 9 9 9}] THEN
                    RETURN [3]
                ELSE
                    GOTO
                IF last EQ [1] THEN
                    RETURN [3]
                ELSE
                    GOTO
                IF last EQ [3] THEN
                    RETURN [2]
                ELSE
                    GOTO
                IF last EQ [2] THEN
                    RETURN [4]
                ELSE
                    GOTO
                RETURN [1]",
                new Tuple<int,int>[] { new Tuple<int,int>(21,5) }
                );

            // ratings
            level.ShorestSolution = 595;
            level.LeastInterations = 66;

            return level;
        }

        public static LevelDetails FromAllSidesLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(20, 0, new byte[,] {
                            {1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1}, // 20
							{0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0}, // 21
							{0,0,0,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,0,0,0}, // 22
							{0,1,1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1,1,0}, // 23
							{1,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,1,1}, // 24
                            //                                               24
                    });

            // Start
            level.Start = new Tuple<int, int>[4] { new Tuple<int, int>(20, 0), new Tuple<int, int>(24, 0), new Tuple<int, int>(20, 24), new Tuple<int, int>(24, 24) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(22, 12) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 86;
            level.LeastInterations = 14;

            return level;
        }

        public static LevelDetails SwarmLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(20, 0, new byte[,] {
                            {0,0,1,1,1,1,1,1,0,0,0,1,0,1,1,1,1,0,1,1,1,1,1,1,1,1,1,1,0}, // 20
							{0,0,1,0,0,0,0,1,0,0,0,1,0,1,0,0,1,0,1,0,0,1,0,1,0,1,0,1,0}, // 21
							{0,0,1,1,1,1,0,1,0,1,0,1,0,1,1,1,1,0,1,1,1,1,0,1,0,1,0,1,0}, // 22
							{0,0,0,0,0,1,0,1,0,1,0,1,0,1,0,0,1,0,1,1,1,0,0,1,0,0,0,1,0}, // 23
							{0,1,1,1,1,1,0,1,1,1,1,1,1,1,0,0,1,1,1,0,1,1,0,1,0,0,0,1,1}, // 24
                            //                                               24
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(24, 1) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(24, 28) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 452;
            level.LeastInterations = 57;

            return level;
        }

        public static LevelDetails OpenMazeLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(17, 0, new byte[,] {
                            {1,1,1,1,1,0,1,1,1,1,1,1}, // 17
                            {0,0,0,0,1,0,1,0,0,0,0,0}, // 18
                            {1,1,1,1,1,0,1,1,1,1,1,1}, // 19
							{1,0,1,0,1,1,1,1,0,0,0,1}, // 20
							{1,0,0,0,1,0,1,1,1,1,0,1}, // 21
							{1,1,1,1,1,0,1,1,0,1,0,1}, // 22
							{1,0,0,0,1,0,1,1,0,1,1,1}, // 23
                            {1,0,1,1,1,0,1,1,0,0,0,0}, // 24
                            {1,0,1,1,1,0,1,1,1,1,1,1}, // 25
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[4] { new Tuple<int, int>(17, 0), new Tuple<int, int>(17, 11), new Tuple<int, int>(25, 0), new Tuple<int, int>(25, 11) };

            // End
            level.End = new Tuple<int, int>[6] { new Tuple<int, int>(17, 4), new Tuple<int, int>(19, 11), new Tuple<int, int>(21, 9), new Tuple<int, int>(23, 9), new Tuple<int, int>(24, 2), new Tuple<int, int>(20, 2) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 342;
            level.LeastInterations = 96;

            return level;
        }

        public static LevelDetails DefendTheHillLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {0,1,1,1,1,1,1,1,1,1,1,1,0}, // 15
                            {0,1,0,0,0,0,1,0,0,0,0,1,0}, // 16
							{0,1,0,1,1,1,1,1,1,1,0,1,0}, // 17
                            {0,1,0,1,1,0,0,0,1,1,0,1,0}, // 18
                            {0,1,0,1,1,1,1,1,1,1,0,1,0}, // 19
                            {0,1,0,1,1,1,1,1,1,1,0,1,0}, // 20
                            {0,1,0,1,1,1,1,1,1,1,0,1,0}, // 21
							{0,1,0,1,1,0,0,0,1,1,0,1,0}, // 22
							{0,1,0,1,1,1,1,1,1,1,0,1,0}, // 23
							{0,1,0,0,0,0,1,0,0,0,0,1,0}, // 24
                            {0,1,1,1,1,1,1,1,1,1,1,1,0}, // 25
                            //           6           12
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(20, 6) };

            // End
            level.End = new Tuple<int, int>[6] { new Tuple<int, int>(19, 6), new Tuple<int, int>(21, 6), new Tuple<int, int>(17, 3), new Tuple<int, int>(17, 9), new Tuple<int, int>(23, 3), new Tuple<int, int>(23, 9) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[2];

            // RED
            level.AI[0] = new SwarmDetails(PlotColor.Red,
                @"IF grid EQ [{0 0 0}{0 9 9}{0 9 0}] THEN
                    RETURN [6]
                ELSE
                    GOTO

                IF grid EQ [{0 1 0}{1 9 1}{0 0 0}] THEN
                    RETURN [1]
                ELSE
                    GOTO

                IF grid EQ [{0 9 9}{0 9 0}{0 1 9}] THEN
                    RETURN [2]
                ELSE
                    GOTO

                IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] THEN
                    RETURN [4]
                ELSE
                   IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] THEN
                       RETURN [1]
                    ELSE
                        IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] THEN
                            RETURN [2]
                        ELSE
                            IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] THEN
                                RETURN [3]
                            ELSE
                                GOTO

                RETURN [5]",
                new Tuple<int, int>[2] { new Tuple<int, int>(15, 1), new Tuple<int, int>(25, 1)}
                );

            // YELLOW
            level.AI[1] = new SwarmDetails(PlotColor.Yellow,
                @"IF grid EQ [{0 0 0}{9 9 0}{0 9 0}] THEN
                    RETURN [6]
                ELSE
                    GOTO

                IF grid EQ [{0 1 0}{1 9 1}{0 0 0}] THEN
                    RETURN [1]
                ELSE
                    GOTO

                IF grid EQ [{9 9 0}{0 9 0}{9 1 0}] THEN
                    RETURN [2]
                ELSE
                    GOTO

                IF grid EQ [{9 9 9}{1 9 9}{9 9 9}] THEN
                    RETURN [3]
                ELSE
                    IF grid EQ [{9 1 9}{9 9 9}{9 9 9}] THEN
                        RETURN [1]
                    ELSE
                        IF grid EQ [{9 9 9}{9 9 9}{9 1 9}] THEN
                            RETURN [2]
                        ELSE
                            IF grid EQ [{9 9 9}{9 9 1}{9 9 9}] THEN
                                RETURN [4]
                            ELSE
                                GOTO

                RETURN [5]",
                new Tuple<int, int>[2] { new Tuple<int, int>(15, 11), new Tuple<int, int>(25, 11) }
                );

            // ratings
            level.ShorestSolution = 652;
            level.LeastInterations = 65;

            return level;
        }

        public static LevelDetails TurnRightLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(20, 0, new byte[,] {
                            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 20
							{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 21
							{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 22
							{1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0}, // 23
							{1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1,1}, // 24
                          //                                                 24
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(20, 0) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(24, 24) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[0];

            // ratings
            level.ShorestSolution = 86;
            level.LeastInterations = 28;

            return level;
        }

        public static LevelDetails RescueMissionLevel()
        {
            LevelDetails level = new LevelDetails();

            // field
            level.Field = GetField(20, 0, new byte[,] {
                            {1,1,1,1,0,0,0,0,0,0,0,0}, // 20
                            {1,1,1,1,0,1,0,0,0,0,0,0}, // 21
							{1,1,1,1,0,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,0,0,0,0,0,0,0}, // 23
							{1,1,1,1,0,0,0,0,0,0,0,0}, // 24
                            //           6         11
                    });

            // Start
            level.Start = new Tuple<int, int>[2] { new Tuple<int, int>(20, 0), new Tuple<int, int>(21, 5) };

            // End
            level.End = new Tuple<int, int>[1] { new Tuple<int, int>(22, 11) };

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // GREEN
            level.AI[0] = new SwarmDetails(PlotColor.Green,
                @"RETURN [5]",
                new Tuple<int, int>[1] { new Tuple<int, int>(22, 5) }
                );

            // ratings
            level.ShorestSolution = 234;
            level.LeastInterations = 15;

            return level;
        }

        public static LevelDetails SwarmingFullCoverageLevel()
        {
            LevelDetails level = new LevelDetails();
            int cnt;
            int w, wd;

            // field
            level.Field = GetField(15, 0, new byte[,] {
                            {1,1,1,1,1,1,1,1,1,1,1}, // 15
                            {1,1,1,1,1,1,1,1,1,1,1}, // 16
                            {1,1,1,1,1,1,1,1,1,1,1}, // 17
                            {1,1,1,1,1,1,1,1,1,1,1}, // 18
                            {1,1,1,1,1,1,1,1,1,1,1}, // 19
                            {1,1,1,1,1,1,1,1,1,1,1}, // 20
							{1,1,1,1,1,1,1,1,1,1,1}, // 21
							{1,1,1,1,1,1,1,1,1,1,1}, // 22
							{1,1,1,1,1,1,1,1,1,1,1}, // 23
							{1,1,1,1,1,1,1,1,1,1,1}, // 24
							{1,1,1,1,1,1,1,1,1,1,1},  // 25
                            {1,1,1,1,1,1,1,1,1,1,1}  // 26
                            //           6       10
                    });

            // Start
            level.Start = new Tuple<int, int>[1] { new Tuple<int, int>(26, 10) };

            // End
            level.End = new Tuple<int, int>[22];
            cnt = 0;
            w = 0;
            wd = 10;
            for (int h = 16; h <= 26; h++)
            {
                level.End[cnt++] = new Tuple<int, int>(h, w);
                level.End[cnt++] = new Tuple<int, int>(h, wd);
                w++;
                wd--;
            }

            // Scale
            level.Scale = 3.0;

            // Center
            level.Center = new Tuple<double, double>(-140, 210);

            // AI
            level.AI = new SwarmDetails[1];

            // GREEN
            level.AI[0] = new SwarmDetails(PlotColor.Green,
                @"IF matrix EQ [{9 9 9 9 9}{9 9 9 6 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 6}] THEN
                    RETURN [4]
                  ELSE
                    GOTO
                IF matrix EQ [{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 6 9 9 9}] THEN
                    RETURN [3]
                  ELSE
                    GOTO
                IF matrix EQ [{9 6 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 6 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 6 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 6}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 6 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}] THEN
                    RETURN [1]
                  ELSE
                    GOTO
                IF matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 6 9}{9 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{6 9 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 6 9 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 6 9 9}] OR matrix EQ [{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 9 9}{9 9 9 6 9}] THEN
                    RETURN [2]
                  ELSE
                    GOTO
                IF grid EQ [{9 2 9}{9 9 9}{9 9 9}] OR grid EQ [{9 9 9}{2 9 9}{9 9 9}] OR grid EQ [{9 9 9}{9 9 2}{9 9 9}] OR grid EQ [{9 9 9}{9 9 2}{9 9 9}] THEN
                    RETURN [5]
                ELSE
                    RETURN [6]",
                new Tuple<int, int>[4] { new Tuple<int, int>(18, 5), new Tuple<int, int>(21, 2), new Tuple<int, int>(21, 8) , new Tuple<int, int>(24, 5) }
                );

            // ratings
            level.ShorestSolution = 232;
            level.LeastInterations = 131;

            return level;
        }

        //
        // utility
        //
        private static PlotState[,] GetField(int startH, int startW, byte[,] state)
        {
            PlotState[,] field;

            field = new PlotState[50, 50]; // Ugh! Hardcoded values
            for (int h = 0; h < field.GetLength(0); h++)
            {
                for (int w = 0; w < field.GetLength(1); w++)
                {
                    int dh, dw;

                    dh = h - startH;
                    dw = w - startW;

                    if (dh >= 0 && dh < state.GetLength(0) && dw >= 0 && dw < state.GetLength(1)) field[h, w] = (PlotState)state[dh, dw];
                    else field[h, w] = PlotState.Forbidden;
                }
            }

            return field;
        }
    }
}
