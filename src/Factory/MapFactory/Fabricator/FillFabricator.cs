using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public static class FillFabricator {
        public static void FillZone(ZoneMap map, string terrainName) {
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    map.Grid[x, y] = new MapCell(TerrainDictionary.Context[terrainName], x, y);
                }
            }
        }
    }
}
