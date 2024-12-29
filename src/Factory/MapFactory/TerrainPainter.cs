using System;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;
using XenWorld.src.Model;

namespace XenWorld.src.Factory.MapFactory {
    public static class TerrainPainter {
        public static bool PaintSquare(ZoneMap map, Coordinate center, int size, string terrainKey, bool setIndoor = false) {
            for (int dx = -(size / 2); dx <= size / 2; dx++) {
                for (int dy = -(size / 2); dy <= size / 2; dy++) {
                    int x = center.X + dx;
                    int y = center.Y + dy;

                    if (!map.IsWithinBounds(x, y)) {
                        Console.WriteLine($"Terrain build failed: ({x}, {y}) is out of bounds.");
                        return false;
                    }

                    map.Grid[x, y].Terrain = TerrainDictionary.Context[terrainKey];
                    if (setIndoor) {
                        map.Grid[x, y].Indoor = true;
                    }
                }
            }
            return true;
        }
    }
}
