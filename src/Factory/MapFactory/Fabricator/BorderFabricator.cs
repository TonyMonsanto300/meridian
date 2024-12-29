using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public static class BorderFabricator {
        public static void BuildBorder(ZoneMap map, string borderTerrain) {
            for (int x = 0; x < map.Width; x++) {
                map.Grid[x, 0] = new MapCell(TerrainDictionary.Context[borderTerrain], x, 0);
                map.Grid[x, map.Height - 1] = new MapCell(TerrainDictionary.Context[borderTerrain], x, map.Height-1);
            }

            for (int y = 0; y < map.Height; y++) {
                map.Grid[0, y] = new MapCell(TerrainDictionary.Context[borderTerrain], 0, y);
                map.Grid[map.Width - 1, y] = new MapCell(TerrainDictionary.Context[borderTerrain], map.Width-1, y);
            }
        }
    }
}
