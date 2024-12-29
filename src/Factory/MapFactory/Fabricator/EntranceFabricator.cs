using System;using System.Collections.Generic;
using System.Linq;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public static class EntranceFabricator {
        public static void CreateEntrances(ZoneMap map) {
            var largestCluster = GetLargestGrassCluster(map);
            if (largestCluster.Count == 0) return;

            // Get the farthest left and right x-coordinates in the cluster
            int minX = largestCluster.Min(cell => cell.x);
            int maxX = largestCluster.Max(cell => cell.x);

            // Get all y-coordinates for cells at minX and maxX within the cluster
            var leftColumnYs = largestCluster.Where(cell => cell.x == minX).Select(cell => cell.y).ToList();
            var rightColumnYs = largestCluster.Where(cell => cell.x == maxX).Select(cell => cell.y).ToList();

            // Forcefully extend grass tiles to the left border
            foreach (int y in leftColumnYs) {
                for (int x = minX; x >= 0; x--) {
                    var cell = map.Grid[x, y];
                    // Only change to grass if the cell is not adjacent to an indoor cell and is not indoor itself
                    if (!cell.Indoor && !IsAdjacentToIndoor(map, x, y)) {
                        map.Grid[x, y].Terrain = TerrainDictionary.Context["grass"];
                    } else {
                        // Optionally, you can break early if you encounter an indoor-adjacent cell
                        break;
                    }
                }
            }

            // Forcefully extend grass tiles to the right border
            foreach (int y in rightColumnYs) {
                for (int x = maxX; x < map.Width; x++) {
                    var cell = map.Grid[x, y];
                    // Only change to grass if the cell is not adjacent to an indoor cell and is not indoor itself
                    if (!cell.Indoor && !IsAdjacentToIndoor(map, x, y)) {
                        map.Grid[x, y].Terrain = TerrainDictionary.Context["grass"];
                    } else {
                        // Optionally, you can break early if you encounter an indoor-adjacent cell
                        break;
                    }
                }
            }
        }

        private static List<(int x, int y)> GetLargestGrassCluster(ZoneMap map) {
            bool[,] visited = new bool[map.Width, map.Height];
            List<(int x, int y)> largestCluster = new List<(int, int)>();

            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    if (map.Grid[x, y].Terrain.Name != "grass" || visited[x, y]) continue;

                    // Perform BFS or DFS to collect all connected grass cells
                    List<(int x, int y)> cluster = new List<(int, int)>();
                    Queue<(int x, int y)> queue = new Queue<(int, int)>();
                    queue.Enqueue((x, y));
                    visited[x, y] = true;

                    while (queue.Count > 0) {
                        var (cx, cy) = queue.Dequeue();
                        cluster.Add((cx, cy));

                        foreach (var (nx, ny) in MapHelper.GetNeighbors(cx, cy)) {
                            if (map.IsWithinBounds(nx, ny) && map.Grid[nx, ny].Terrain.Name == "grass" && !visited[nx, ny]) {
                                queue.Enqueue((nx, ny));
                                visited[nx, ny] = true;
                            }
                        }
                    }

                    // Check if this cluster is the largest found so far
                    if (cluster.Count > largestCluster.Count) {
                        largestCluster = cluster;
                    }
                }
            }

            return largestCluster;
        }

        private static bool IsAdjacentToIndoor(ZoneMap map, int x, int y) {
            // Define the four cardinal directions
            var directions = new (int dx, int dy)[] { (0, -1), (0, 1), (-1, 0), (1, 0) };

            foreach (var (dx, dy) in directions) {
                int neighborX = x + dx;
                int neighborY = y + dy;

                // Check if the neighbor is within bounds
                if (map.IsWithinBounds(neighborX, neighborY)) {
                    if (map.Grid[neighborX, neighborY].Indoor) {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
