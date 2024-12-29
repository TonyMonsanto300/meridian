using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Model.Map;

namespace XenWorld.Repository.Map {
    public static class TerrainDictionary {
        public static Dictionary<string, MapTerrain> Context { get; private set; } = new Dictionary<string, MapTerrain>();

        public static void LoadTerrain(string name, Texture2D texture, bool obstacle = false, bool wall = false) {
            Context.Add(name, new MapTerrain(name, texture, obstacle, wall));
        }
    }
}
