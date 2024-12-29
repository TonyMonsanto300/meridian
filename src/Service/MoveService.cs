using XenWorld.src.Manager;
using XenWorld.src.Model;

namespace XenWorld.src.Service {
    public static class MoveService {
        public static bool MovePlayer(Coordinate newCoordinate) {
            // Check bounds
            if (newCoordinate.X < 0 || newCoordinate.X >= MapManager.ActiveMap.Width || newCoordinate.Y < 0 || newCoordinate.Y >= MapManager.ActiveMap.Height) {
                return false; // Out of bounds
            }

            MapCell targetCell = MapManager.ActiveMap.Grid[newCoordinate.X, newCoordinate.Y];

            // Check if the target cell is occupied or if the terrain is an obstacle
            if (targetCell.Occupant != null || targetCell.Terrain.Obstacle) {
                return false; // Cannot move into this cell
            }

            // Additional checks for diagonal movement to prevent movement through obstacles on adjacent sides
            int deltaX = newCoordinate.X - PlayerManager.Controller.Puppet.Coordinate.X;
            int deltaY = newCoordinate.Y - PlayerManager.Controller.Puppet.Coordinate.Y;

            if (deltaX != 0 && deltaY != 0) { // Only apply these checks for diagonal moves
                MapCell adjacentCell1 = MapManager.ActiveMap.Grid[PlayerManager.Controller.Puppet.Coordinate.X + deltaX, PlayerManager.Controller.Puppet.Coordinate.Y]; // Horizontal adjacent cell
                MapCell adjacentCell2 = MapManager.ActiveMap.Grid[PlayerManager.Controller.Puppet.Coordinate.X, PlayerManager.Controller.Puppet.Coordinate.Y + deltaY]; // Vertical adjacent cell

                if ((adjacentCell1 != null && adjacentCell1.Terrain.Obstacle) &&
                    (adjacentCell2 != null && adjacentCell2.Terrain.Obstacle)) {
                    return false; // Prevent diagonal movement if both adjacent cells are obstacles
                }
            }

            // Update the map grid to remove the player from the current cell
            MapManager.ActiveMap.Grid[PlayerManager.Controller.Puppet.Coordinate.X, PlayerManager.Controller.Puppet.Coordinate.Y].Occupant = null;

            // Update player's position
            PlayerManager.Controller.Puppet.Coordinate.X = newCoordinate.X;
            PlayerManager.Controller.Puppet.Coordinate.Y = newCoordinate.Y;

            // Place the player in the new cell
            MapManager.ActiveMap.Grid[PlayerManager.Controller.Puppet.Coordinate.X, PlayerManager.Controller.Puppet.Coordinate.Y].Occupant = PlayerManager.Controller.Puppet;

            return true; // Move successful
        }

    }
}
