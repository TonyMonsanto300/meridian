using System;
using XenWorld.Repository.Map;

namespace XenWorld.src.Service {
    public static class MiningService {
        public static void MineCell(MapCell targetCell) {
            if (targetCell.Terrain.Obstacle && !targetCell.Terrain.Wall) {
                targetCell.Terrain = TerrainDictionary.Context["grass"];
                Console.WriteLine($"Mined at ({targetCell.Coordinate.X}, {targetCell.Coordinate.Y}). Terrain changed to grass.");
            } else {
                Console.WriteLine("Cannot mine here.");
            }
        }
    }
}
