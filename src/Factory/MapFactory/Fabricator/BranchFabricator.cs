using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public static class BranchFabricator {
        private static Random random= new Random();
        private static void CreateBranch(ZoneMap map, int startX, int startY, int currentDepth, int maxDepth, string borderTerrain, string groundTerrain) {
            if (currentDepth > maxDepth) return;

            int branchLength = random.Next(5, 10);
            int currentX = startX;
            int currentY = startY;
            bool goUp = random.NextDouble() < 0.5;
            bool isHorizontal = random.NextDouble() < 0.5;

            for (int i = 0; i < branchLength; i++) {
                if (random.NextDouble() < 0.2) {
                    goUp = !goUp;
                }

                if (isHorizontal) {
                    bool goRight = random.NextDouble() < 0.5;
                    currentX += goRight ? 1 : -1;
                } else {
                    if (goUp) {
                        currentY -= 1;
                    } else {
                        currentY += 1;
                    }
                }

                if (map.IsWithinBounds(currentX, currentY)) {
                    if (map.Grid[currentX, currentY].Terrain.Name == borderTerrain) {
                        map.Grid[currentX, currentY] = new MapCell(TerrainDictionary.Context[groundTerrain], currentX, currentY);
                        if (random.NextDouble() < 0.3) {
                            CreateBranch(map, currentX, currentY, currentDepth + 1, maxDepth, borderTerrain, groundTerrain);
                        }
                    }
                } else {
                    break;
                }

                if (random.NextDouble() < 0.1) {
                    isHorizontal = !isHorizontal;
                }
            }
        }

        public static void CreateBranches(ZoneMap map, int maxDepth, string borderTerrain, string groundTerrain) {
            int numberOfBranches = random.Next(3, 6);
            List<(int x, int y)> mainPathCells = GetMainPathCells(map, borderTerrain, groundTerrain);

            for (int i = 0; i < numberOfBranches; i++) {
                if (mainPathCells.Count == 0) break;
                var branchStart = mainPathCells[random.Next(mainPathCells.Count)];
                CreateBranch(map, branchStart.x, branchStart.y, 1, maxDepth, borderTerrain, groundTerrain);
            }
        }

        private static List<(int x, int y)> GetMainPathCells(ZoneMap map, string borderTerrain, string groundTerrain) {
            List<(int x, int y)> pathCells = new List<(int x, int y)>();
            for (int x = 1; x < map.Width - 1; x++) {
                for (int y = 1; y < map.Height - 1; y++) {
                    if (map.Grid[x, y].Terrain.Name == groundTerrain) {
                        pathCells.Add((x, y));
                    }
                }
            }
            return pathCells;
        }
    }
}
