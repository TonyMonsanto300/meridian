using System;
using System.Collections.Generic;
using XenWorld.Model.Map;

namespace XenWorld.src.Factory.MapFactory.Finder {
    public static class CellFinder {
        public  static (int x, int y)? FindNearestOutdoorCell(ZoneMap map, int centerX, int centerY) {
            Queue<(int x, int y)> queue = new Queue<(int, int)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            queue.Enqueue((centerX, centerY));
            visited.Add((centerX, centerY));

            while (queue.Count > 0) {
                var (x, y) = queue.Dequeue();

                foreach (var (nx, ny) in MapHelper.GetNeighbors(x, y)) {
                    if (map.IsWithinBounds(nx, ny) && visited.Add((nx, ny))) {
                        queue.Enqueue((nx, ny));

                        if (!map.Grid[nx, ny].Indoor && !map.Grid[nx, ny].Terrain.Obstacle) {
                            Console.WriteLine($"Found nearest outdoor cell at ({nx}, {ny})");
                            return (nx, ny);
                        }
                    }
                }
            }

            Console.WriteLine("No suitable outdoor cell found.");
            return null;
        }
    }
}
