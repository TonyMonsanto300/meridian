using XenWorld.src.Manager;
using XenWorld.src.Model;

namespace XenWorld.src.Service {
    public static class MoveService {
        public static bool MovePlayer(Coordinate newCoordinate) {
            if (!IsWithinBounds(newCoordinate)) return false;
            if (!CanEnterCell(newCoordinate)) return false;
            if (!IsDiagonalMoveAllowed(newCoordinate)) return false;

            UpdatePlayerPosition(newCoordinate);
            return true;
        }

        private static bool IsWithinBounds(Coordinate coordinate) {
            return coordinate.X >= 0 && coordinate.X < MapManager.ActiveMap.Width
                && coordinate.Y >= 0 && coordinate.Y < MapManager.ActiveMap.Height;
        }

        private static bool CanEnterCell(Coordinate coordinate) {
            MapCell targetCell = MapManager.ActiveMap.Grid[coordinate.X, coordinate.Y];
            return targetCell.Occupant == null && !targetCell.Terrain.Obstacle;
        }

        private static bool IsDiagonalMoveAllowed(Coordinate newCoordinate) {
            int deltaX = newCoordinate.X - PlayerManager.Controller.Puppet.Location.X;
            int deltaY = newCoordinate.Y - PlayerManager.Controller.Puppet.Location.Y;

            if (deltaX == 0 || deltaY == 0) return true; // Not a diagonal move

            Coordinate currentCoordinate = PlayerManager.Controller.Puppet.Location;
            MapCell horizontalAdjacent = MapManager.ActiveMap.Grid[currentCoordinate.X + deltaX, currentCoordinate.Y];
            MapCell verticalAdjacent = MapManager.ActiveMap.Grid[currentCoordinate.X, currentCoordinate.Y + deltaY];

            return !(horizontalAdjacent?.Terrain.Obstacle == true && verticalAdjacent?.Terrain.Obstacle == true);
        }

        private static void UpdatePlayerPosition(Coordinate newCoordinate) {
            Coordinate currentCoordinate = PlayerManager.Controller.Puppet.Location;

            // Remove player from the current cell
            MapManager.ActiveMap.Grid[currentCoordinate.X, currentCoordinate.Y].Occupant = null;

            // Update player's coordinates
            PlayerManager.Controller.Puppet.Location = newCoordinate;

            // Place player in the new cell
            MapManager.ActiveMap.Grid[newCoordinate.X, newCoordinate.Y].Occupant = PlayerManager.Controller.Puppet;
        }
    }
}
