using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// todo place this file somewhere rather than app local

namespace Swarm
{
    public static class Storage
    {
        public static IList<string> Load(string filename)
        {
            // load from a local directory
            try
            {
                if (File.Exists(filename)) return File.ReadAllLines(filename);
                return null;
            }
            catch (Exception)
            {
                // UNAUTHORIZEDACCESSEXCEPTION, FILELOADEXCEPTION, FILENOTFOUNDEXCEPTION
                return null;
            }
        }

        public static bool Store(string filename, IList<string> lines)
        {
            // store to a local directory
            try
            {
                File.WriteAllLines(filename, lines);
                return true;
            }
            catch (Exception)
            {
                // UNAUTHORIZEDACCESSEXCEPTION, FILELOADEXCEPTION, FILENOTFOUNDEXCEPTION
                return false;
            }
        }
    }

    class SavedLevelData
    {
        public string FileName { get; private set; }
        public int Stars { get; set; }
        public bool Unlocked { get; set; }
        public string Script { get; set; }
        public int Attempts { get; set; }
        public bool Loaded { get; set; }

        public SavedLevelData(string filename)
        {
            FileName = filename;
            Stars = 0;
            Unlocked = false;
            Loaded = false;
        }
    }

    public static class SavedGamesRepsoitory
    {
        private static Dictionary<LevelID, SavedLevelData> LevelStorage;

        static SavedGamesRepsoitory()
        {
            IsLoaded = false;

            // init storage
            LevelStorage = new Dictionary<LevelID, SavedLevelData>();
            LevelStorage.Add(LevelID.World_1_1, new SavedLevelData("World_1_1"));
            LevelStorage.Add(LevelID.World_1_2, new SavedLevelData("World_1_2"));
            LevelStorage.Add(LevelID.World_1_3, new SavedLevelData("World_1_3"));
            LevelStorage.Add(LevelID.World_1_4, new SavedLevelData("World_1_4"));
            LevelStorage.Add(LevelID.World_1_5, new SavedLevelData("World_1_5"));
            LevelStorage.Add(LevelID.World_1_6, new SavedLevelData("World_1_6"));
            LevelStorage.Add(LevelID.World_1_7, new SavedLevelData("World_1_7"));
            LevelStorage.Add(LevelID.World_1_8, new SavedLevelData("World_1_8"));
            LevelStorage.Add(LevelID.World_1_9, new SavedLevelData("World_1_9"));
            LevelStorage.Add(LevelID.World_1_10, new SavedLevelData("World_1_10"));
            LevelStorage.Add(LevelID.World_1_11, new SavedLevelData("World_1_11"));
            LevelStorage.Add(LevelID.World_1_12, new SavedLevelData("World_1_12"));
            LevelStorage.Add(LevelID.World_1_13, new SavedLevelData("World_1_13"));
            LevelStorage.Add(LevelID.World_1_14, new SavedLevelData("World_1_14"));
            LevelStorage.Add(LevelID.World_1_15, new SavedLevelData("World_1_15"));
            LevelStorage.Add(LevelID.World_1_16, new SavedLevelData("World_1_16"));

            LevelStorage.Add(LevelID.World_2_1, new SavedLevelData("World_2_1"));
            LevelStorage.Add(LevelID.World_2_2, new SavedLevelData("World_2_2"));
            LevelStorage.Add(LevelID.World_2_3, new SavedLevelData("World_2_3"));
            LevelStorage.Add(LevelID.World_2_4, new SavedLevelData("World_2_4"));
            LevelStorage.Add(LevelID.World_2_5, new SavedLevelData("World_2_5"));
            LevelStorage.Add(LevelID.World_2_6, new SavedLevelData("World_2_6"));
            LevelStorage.Add(LevelID.World_2_7, new SavedLevelData("World_2_7"));
            LevelStorage.Add(LevelID.World_2_8, new SavedLevelData("World_2_8"));
            LevelStorage.Add(LevelID.World_2_9, new SavedLevelData("World_2_9"));
            LevelStorage.Add(LevelID.World_2_10, new SavedLevelData("World_2_10"));
            LevelStorage.Add(LevelID.World_2_11, new SavedLevelData("World_2_11"));
            LevelStorage.Add(LevelID.World_2_12, new SavedLevelData("World_2_12"));
            LevelStorage.Add(LevelID.World_2_13, new SavedLevelData("World_2_13"));
            LevelStorage.Add(LevelID.World_2_14, new SavedLevelData("World_2_14"));
            LevelStorage.Add(LevelID.World_2_15, new SavedLevelData("World_2_15"));
            LevelStorage.Add(LevelID.World_2_16, new SavedLevelData("World_2_16"));

            LevelStorage.Add(LevelID.World_3_1, new SavedLevelData("World_3_1"));
            LevelStorage.Add(LevelID.World_3_2, new SavedLevelData("World_3_2"));
            LevelStorage.Add(LevelID.World_3_3, new SavedLevelData("World_3_3"));
            LevelStorage.Add(LevelID.World_3_4, new SavedLevelData("World_3_4"));
            LevelStorage.Add(LevelID.World_3_5, new SavedLevelData("World_3_5"));
            LevelStorage.Add(LevelID.World_3_6, new SavedLevelData("World_3_6"));
            LevelStorage.Add(LevelID.World_3_7, new SavedLevelData("World_3_7"));
            LevelStorage.Add(LevelID.World_3_8, new SavedLevelData("World_3_8"));
            LevelStorage.Add(LevelID.World_3_9, new SavedLevelData("World_3_9"));
            LevelStorage.Add(LevelID.World_3_10, new SavedLevelData("World_3_10"));
            LevelStorage.Add(LevelID.World_3_11, new SavedLevelData("World_3_11"));
            LevelStorage.Add(LevelID.World_3_12, new SavedLevelData("World_3_12"));
            LevelStorage.Add(LevelID.World_3_13, new SavedLevelData("World_3_13"));
            LevelStorage.Add(LevelID.World_3_14, new SavedLevelData("World_3_14"));
            LevelStorage.Add(LevelID.World_3_15, new SavedLevelData("World_3_15"));
            LevelStorage.Add(LevelID.World_3_16, new SavedLevelData("World_3_16"));

            LevelStorage.Add(LevelID.Battle_Hill, new SavedLevelData("Battle_Hill"));
            LevelStorage.Add(LevelID.Battle_Open, new SavedLevelData("Battle_Open"));
            LevelStorage.Add(LevelID.Battle_Quad, new SavedLevelData("Battle_Quad"));
            LevelStorage.Add(LevelID.Battle_Maze, new SavedLevelData("Battle_Maze"));

            LevelStorage.Add(LevelID.Bonus_1, new SavedLevelData("Bonus_1"));
            LevelStorage.Add(LevelID.Bonus_2, new SavedLevelData("Bonus_2"));
            LevelStorage.Add(LevelID.Bonus_3, new SavedLevelData("Bonus_3"));
            LevelStorage.Add(LevelID.Bonus_4, new SavedLevelData("Bonus_4"));

            // load all the data
            LoadAll();
        }

        public static bool IsLoaded { get; private set; }

        public static void LoadAll()
        {
            int stars;
            string script;
            bool unlocked;
            int attempts;

            foreach(SavedLevelData save in LevelStorage.Values)
            {
                save.Loaded = false;
                var data = Storage.Load(save.FileName);

                if (ReadData(data, out stars, out script, out unlocked, out attempts))
                {
                    save.Stars = stars;
                    save.Script = script;
                    save.Unlocked = unlocked;
                    save.Attempts = attempts;

                    // mark as loaded
                    save.Loaded = true;
                }
                else
                {
                    // mark as not read
                    save.Loaded = false;
                }
            }

            // signal we are loaded
            IsLoaded = true;
        }

        public static bool Loaded(LevelID id) { return LevelStorage[id].Loaded; }
        public static int GetStars(LevelID id) { return LevelStorage[id].Stars; }
        public static string GetScript(LevelID id) { return LevelStorage[id].Script; }
        public static bool GetUnlocked(LevelID id) { return LevelStorage[id].Unlocked; }
        public static int GetAttempts(LevelID id) { return LevelStorage[id].Attempts; }

        public static void Store(LevelID id, int stars, string script, bool unlocked, int attempts)
        {
            List<string> data;
            int retry;
            bool success;

            // update the cache
            if (LevelStorage[id].Stars < stars) LevelStorage[id].Stars = stars;
            LevelStorage[id].Attempts += attempts;
            if (unlocked) LevelStorage[id].Unlocked = unlocked;
            if (script != null && script != "") LevelStorage[id].Script = script;
            LevelStorage[id].Loaded = true;

            // translate to the right format
            data = new List<string>();
            data.Add("JCS;STARS=" + LevelStorage[id].Stars + ";UNLOCKED=" + LevelStorage[id].Unlocked + ";ATTEMPTS=" + LevelStorage[id].Attempts + Environment.NewLine);
            if (LevelStorage[id].Script != null) data.Add(LevelStorage[id].Script);

            // store
            success = false;
            retry = 3;
            do
            {
                if (Storage.Store(LevelStorage[id].FileName, data))
                {
                    success = true;
                }
                else
                {
                    retry--;
                }
            }
            while (retry > 0 && !success);
        }

        private static bool ReadData(IList<string> data, out int stars, out string script, out bool unlocked, out int attempts)
        {
            stars = 0;
            script = "";
            unlocked = false;
            attempts = 0;

            // FORMAT - 
            //  1: JCS;Key=Value....
            //  2: Script...
            //  3: ...

            if (data == null || data.Count <= 0 || !data[0].StartsWith("JCS;")) return false;

            // grab the data
            if (data.Count >= 1)
            {
                string[] pairs = data[0].Split(new char[] { ';' });

                for (int i = 1; i < pairs.Length; i++)
                {
                    if (pairs[i].Contains("="))
                    {
                        string[] parts = pairs[i].Split(new char[] { '=' }, 2);

                        if (parts.Length == 2)
                        {
                            switch (parts[0])
                            {
                                case "STARS":
                                    Int32.TryParse(parts[1], out stars);
                                    break;
                                case "UNLOCKED":
                                    Boolean.TryParse(parts[1], out unlocked);
                                    break;
                                case "ATTEMPTS":
                                    Int32.TryParse(parts[1], out attempts);
                                    break;
                                default:
                                    // nothing
                                    break;
                            }
                        }
                    }
                }
            }

            // grab the script
            if (data.Count >= 2)
            {
                script = "";
                for (int i = 1; i < data.Count; i++) script += data[i];
            }

            return true;
        }

    }

    public static class GuidRepsoitory
    {
        private static string UsageFileName = "guid";

        public static void Store(string guid)
        {
            IList<string> data;
            int retry;
            bool success;

            // convert the guid
            data = new List<string>();
            data.Add(guid);

            // store
            success = false;
            retry = 3;
            do
            {
                if (Storage.Store(UsageFileName, data))
                {
                    success = true;
                }
                else
                {
                    retry--;
                }
            }
            while (retry > 0 && !success);
        }

        public static string Load()
        {
            var data = Storage.Load(UsageFileName);

            if (data == null || data.Count == 0) return "";
            
            // return the first element
            return data[0];
        }
    }
}
