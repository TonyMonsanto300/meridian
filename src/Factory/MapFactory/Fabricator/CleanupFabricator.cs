using System.Collections.Generic;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public static class CleanupFabricator {
        public static void CleanUpDebris(ZoneMap map, int size) {
            bool[,] visited = new bool[map.Width, map.Height];

            // Phase 1: Remove small obstacle clusters
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    // Skip if already visited or not an obstacle
                    if (visited[x, y] || !map.Grid[x, y].Terrain.Obstacle || map.Grid[x, y].Terrain.Wall) {
                        continue;
                    }

                    // Perform BFS to find all connected obstacle tiles
                    List<(int x, int y)> cluster = GetObstacleCluster(map, x, y, visited);

                    // If the cluster size is less than the specified size, remove debris
                    if (cluster.Count < size) {
                        foreach (var (cx, cy) in cluster) {
                            // Change the terrain to a non-obstacle, e.g., "floor_wood" or "grass"
                            map.Grid[cx, cy].Terrain = TerrainDictionary.Context["grass"];
                        }
                    }
                }
            }

            // Phase 2: Iteratively remove obstacle cells with fewer than 2 obstacle neighbors
            bool removedAny;
            do {
                removedAny = false;
                List<(int x, int y)> cellsToRemove = new List<(int x, int y)>();

                for (int x = 1; x < map.Width - 1; x++) { // Exclude borders if they are always obstacles
                    for (int y = 1; y < map.Height - 1; y++) {
                        if (map.Grid[x, y].Terrain.Obstacle && !map.Grid[x, y].Terrain.Wall) {
                            int obstacleNeighbors = 0;
                            foreach (var (nx, ny) in MapHelper.GetNeighbors(x, y)) {
                                if (map.Grid[nx, ny].Terrain.Obstacle && !map.Grid[nx, ny].Terrain.Wall) {
                                    obstacleNeighbors++;
                                }
                            }

                            if (obstacleNeighbors < 2) {
                                cellsToRemove.Add((x, y));
                            }
                        }
                    }
                }

                // Remove the identified cells
                foreach (var (x, y) in cellsToRemove) {
                    map.Grid[x, y].Terrain = TerrainDictionary.Context["grass"]; // Or another suitable non-obstacle terrain
                    removedAny = true;
                }

            } while (removedAny); // Continue until no more cells are removed

            //// Phase 3: Iteratively convert grass cells with fewer than 2 grass neighbors into border
            //bool grassRemovedAny;
            //do {
            //    grassRemovedAny = false;
            //    List<(int x, int y)> grassToConvert = new List<(int x, int y)>();

            //    for (int x = 1; x < map.Width - 1; x++) { // Exclude borders if they are always borders
            //        for (int y = 1; y < map.Height - 1; y++) {
            //            if (map.Grid[x, y].Terrain.Name == "grass") {
            //                int grassNeighbors = 0;
            //                foreach (var (nx, ny) in MapHelper.GetNeighbors(x, y)) {
            //                    if (IsGroundTile(map.Grid[nx, ny].Terrain.Name)) {
            //                        grassNeighbors++;
            //                    }
            //                }

            //                if (grassNeighbors < 2) {
            //                    grassToConvert.Add((x, y));
            //                }
            //            }
            //        }
            //    }

            //    // Convert the identified grass cells to border
            //    foreach (var (x, y) in grassToConvert) {
            //        map.Grid[x, y].Terrain = TerrainDictionary.Context["border"];
            //        grassRemovedAny = true;
            //    }

            //} while (grassRemovedAny); // Continue until no more grass cells are converted

            // Phase 4: Remove Weak Borders
            List<(int x, int y)> weakBorders = FindWeakBorders(map);
            foreach (var (x, y) in weakBorders) {
                // Replace weak border with ground (e.g., "grass")
                map.Grid[x, y].Terrain = TerrainDictionary.Context["grass"];
            }
        }

        private static List<(int x, int y)> GetObstacleCluster(ZoneMap map, int startX, int startY, bool[,] visited) {
            List<(int x, int y)> cluster = new List<(int, int)>();
            Queue<(int x, int y)> queue = new Queue<(int, int)>();
            queue.Enqueue((startX, startY));
            visited[startX, startY] = true;

            while (queue.Count > 0) {
                var (x, y) = queue.Dequeue();
                cluster.Add((x, y));

                foreach (var (nx, ny) in MapHelper.GetNeighbors(x, y)) {
                    if (map.IsWithinBounds(nx, ny) && map.Grid[nx, ny].Terrain.Obstacle && !visited[nx, ny]) {
                        queue.Enqueue((nx, ny));
                        visited[nx, ny] = true;
                    }
                }
            }

            return cluster;
        }

        /// <summary>
        /// Finds all weak border cells in the map.
        /// </summary>
        /// <param name="map">The map grid to search.</param>
        /// <returns>A list of coordinates representing weak border cells.</returns>
        private static List<(int x, int y)> FindWeakBorders(ZoneMap map) {
            List<(int x, int y)> weakBorders = new List<(int x, int y)>();

            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    if (map.Grid[x, y].Terrain.Name == "border") {
                        if (IsWeakBorder(map, x, y)) {
                            weakBorders.Add((x, y));
                        }
                    }
                }
            }

            return weakBorders;
        }

        /// <summary>
        /// Determines if a given border cell is a weak border.
        /// A weak border has ground on two opposite sides (left and right or up/down) 
        /// or is adjacent to out-of-bounds on either side.
        /// </summary>
        /// <param name="map">The map grid.</param>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <returns>True if the border is weak; otherwise, false.</returns>
        private static bool IsWeakBorder(ZoneMap map, int x, int y) {
            // Define directions: Left, Right, Up, Down
            var directions = new (int dx, int dy)[] { (-1, 0), (1, 0), (0, -1), (0, 1) };
            bool hasLeftGround = false, hasRightGround = false;
            bool hasUpGround = false, hasDownGround = false;

            // Check Left
            if (map.IsWithinBounds(x - 1, y)) {
                hasLeftGround = IsGroundTile(map.Grid[x - 1, y].Terrain.Name);
            } else {
                // Out of bounds is considered as a condition for weak border
                hasLeftGround = false;
            }

            // Check Right
            if (map.IsWithinBounds(x + 1, y)) {
                hasRightGround = IsGroundTile(map.Grid[x + 1, y].Terrain.Name);
            } else {
                hasRightGround = false;
            }

            // Check Up
            if (map.IsWithinBounds(x, y - 1)) {
                hasUpGround = IsGroundTile(map.Grid[x, y - 1].Terrain.Name);
            } else {
                hasUpGround = false;
            }

            // Check Down
            if (map.IsWithinBounds(x, y + 1)) {
                hasDownGround = IsGroundTile(map.Grid[x, y + 1].Terrain.Name);
            } else {
                hasDownGround = false;
            }

            // Check for opposite sides (Left & Right) or (Up & Down)
            bool oppositeHorizontal = hasLeftGround && hasRightGround;
            bool oppositeVertical = hasUpGround && hasDownGround;

            // Additionally, check if one side is out of bounds
            bool leftOutOfBounds = x - 1 < 0;
            bool rightOutOfBounds = x + 1 >= map.Width;
            bool upOutOfBounds = y - 1 < 0;
            bool downOutOfBounds = y + 1 >= map.Height;

            bool horizontalWithOOB = (leftOutOfBounds || rightOutOfBounds) && (hasRightGround || hasLeftGround);
            bool verticalWithOOB = (upOutOfBounds || downOutOfBounds) && (hasUpGround || hasDownGround);

            return (oppositeHorizontal || oppositeVertical) || (horizontalWithOOB || verticalWithOOB);
        }

        /// <summary>
        /// Determines if a given terrain name is considered ground.
        /// Ground includes "grass" and "path_white".
        /// </summary>
        /// <param name="terrainName">The name of the terrain.</param>
        /// <returns>True if the terrain is ground; otherwise, false.</returns>
        private static bool IsGroundTile(string terrainName) {
            return terrainName == "grass" || terrainName == "path_white";
        }
    }
}