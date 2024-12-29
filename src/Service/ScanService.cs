using System;

namespace XenWorld.src.Service {
    public static class ScanService {
        public static void ScanCell(MapCell targetCell) {
            if (targetCell.Occupant != null) {
                Console.WriteLine($"Scanning {targetCell.Occupant.Name} at ({targetCell.Coordinate.X}, {targetCell.Coordinate.Y}).");
                // Add more detailed scanning information as needed
            } else {
                Console.WriteLine("Nothing to scan.");
            }
        }
    }
}
