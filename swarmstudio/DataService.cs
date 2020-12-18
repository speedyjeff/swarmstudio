using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Swarm
{
    public static class DataService
    {
        private static string UserGuid = "";

        public static void Init()
        {
            // assign a guid
            UserGuid = GuidRepsoitory.Load();
            if (UserGuid == "")
            {
                UserGuid = Guid.NewGuid().ToString();
                GuidRepsoitory.Store(UserGuid);
            }
        }
        public static void StoreLevelData(int level, double rating, string script, bool success, int iterations, int complexity)
        {
            Levels data = new Levels();
            data.Level = level;
            data.Rating = rating;
            data.Script = script;
            data.Success = success;
            data.UGuid = UserGuid;
            data.Time = DateTime.UtcNow;
            data.UID = "";
            data.Iterations = iterations;
            data.Complexity = complexity;

            // removed
        }

        public static void StoreScriptData(int level, int color, string name, string script)
        {
            Scripts data = new Scripts();
            data.Level = level;
            data.Name = name;
            data.Color = color;
            data.Script = script;
            data.Likes = 0;
            data.Dislikes = 0;
            data.UGuid = UserGuid;
            data.Time = DateTime.UtcNow;
            data.UID = "";

            // removed
        }

        public static void UpdateScriptData(int id, bool like)
        {
            Scripts data = new Scripts();
            data.Id = id;
            data.Likes = (like ? 1 : 0);
            data.Dislikes = (!like ? 1 : 0);
            data.Name = "";
            data.Script = "";
            data.UGuid = "";
            data.UID = "";

            // removed
        }

        public static IEnumerable<Scripts> GetScriptData(int color, int level, int start, int count)
        {
            // removed

            return new List<Scripts>();
        } 

        public static bool IsAuthenticated
        {
            get
            {
                return false;
            }
        }

        public static bool Authenticate()
        {
            // removed

            return false;
        }
    }
}
