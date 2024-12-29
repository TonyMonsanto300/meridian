using System;
using XenWorld.Model.Map;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;

namespace XenWorld.src.Service {
    public static class MapCursor {
        public static int X = 0;
        public static int Y = 0;
    }

    public static class MapCursorService {
        public static PuppetDirection MapCursorDirection { get; private set; } = PuppetDirection.None;
        public static MapCell[,] HighlightedCells;
        public static MapCell TargetCell {
            get { return HighlightedCells[MapCursor.X, MapCursor.Y]; }
        }

        public static void MoveCursor(PuppetDirection direction = PuppetDirection.None) {
            if (direction == PuppetDirection.None) {
                direction = MapCursorDirection;
            }

            if (DirectionService.directionToCoordinates.TryGetValue(direction, out var coordinates)) {
                int newX = MapCursor.X + coordinates.x;
                int newY = MapCursor.Y + coordinates.y;

                if (newX >= 0 && newX < HighlightedCells.GetLength(0) && newY >= 0 && newY < HighlightedCells.GetLength(1) && HighlightedCells[newX, newY] != null) {
                    // Clear the cursor from the current cell
                    if (HighlightedCells[MapCursor.X, MapCursor.Y] != null) {
                        HighlightedCells[MapCursor.X, MapCursor.Y].Cursor = false;
                    }

                    // Move the cursor to the new cell
                    MapCursor.X = newX;
                    MapCursor.Y = newY;
                    MapCursorDirection = direction;
                    HighlightedCells[MapCursor.X, MapCursor.Y].Cursor = true;

                    Console.WriteLine($"Cursor moved to {direction} at position ({MapCursor.X}, {MapCursor.Y}).");
                } else {
                    Console.WriteLine($"Cannot move cursor to {direction}. Target cell is invalid.");
                }
            } else {
                Console.WriteLine($"Invalid direction: {direction}. Resetting cursor.");
                //ResetCursor();
            }
        }

        public static void MoveCursorByDelta(int deltaX, int deltaY) {
            // Calculate the center of the grid
            int centerX = HighlightedCells.GetLength(0) / 2;
            int centerY = HighlightedCells.GetLength(1) / 2;

            // Determine the new position
            int newX = MapCursor.X + deltaX;
            int newY = MapCursor.Y + deltaY;

            // Detect crossing over the center
            if ((MapCursor.X != centerX || MapCursor.Y != centerY) &&
                (newX == centerX && newY == centerY)) {
                // Adjust newX and newY to skip over the center
                newX += deltaX;
                newY += deltaY;
            }

            // Validate the new position
            if (newX >= 0 && newX < HighlightedCells.GetLength(0) &&
                newY >= 0 && newY < HighlightedCells.GetLength(1) &&
                HighlightedCells[newX, newY] != null) {
                // Clear the cursor from the current cell
                HighlightedCells[MapCursor.X, MapCursor.Y].Cursor = false;

                // Move the cursor to the new cell
                MapCursor.X = newX;
                MapCursor.Y = newY;
                HighlightedCells[MapCursor.X, MapCursor.Y].Cursor = true;

                Console.WriteLine($"Cursor moved to position ({MapCursor.X}, {MapCursor.Y}).");
            } else {
                Console.WriteLine($"Cannot move cursor to position ({newX}, {newY}). Target cell is invalid.");
            }
        }

        public static void ResetCursor() {
            if (HighlightedCells != null) {
                for (int i = 0; i < HighlightedCells.GetLength(0); i++) {
                    for (int j = 0; j < HighlightedCells.GetLength(1); j++) {
                        if (HighlightedCells[i, j] != null) {
                            HighlightedCells[i, j].Cursor = false;
                        }
                    }
                }
                // Set cursor position back to the center
                MapCursor.X = HighlightedCells.GetLength(0) / 2;
                MapCursor.Y = HighlightedCells.GetLength(1) / 2;
                MapCursorDirection = PuppetDirection.None;
                Console.WriteLine("Cursor has been reset.");
            }
        }

        public static void HighlightAdjacentCells(InteractionMode currentMode, int range = 1) {
            // Calculate the size of the grid based on the range
            int gridSize = range * 2 + 1;
            HighlightedCells = new MapCell[gridSize, gridSize]; // Reset the grid

            int playerX = PlayerManager.Controller.Puppet.Coordinate.X;
            int playerY = PlayerManager.Controller.Puppet.Coordinate.Y;

            int maxX = MapManager.ActiveMap.Width - 1; // The maximum X index (world boundary)
            int maxY = MapManager.ActiveMap.Height - 1; // The maximum Y index (world boundary)

            // Build the new HighlightedCells array
            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    int offsetX = i - range;
                    int offsetY = j - range;

                    // Calculate Manhattan distance
                    int manhattanDistance = Math.Abs(offsetX) + Math.Abs(offsetY);

                    if (manhattanDistance > range) {
                        // Skip cells beyond the specified range
                        continue;
                    }

                    int newX = playerX + offsetX;
                    int newY = playerY + offsetY;

                    if (newX >= 0 && newX <= maxX && newY >= 0 && newY <= maxY) {
                        MapCell cell = MapManager.ActiveMap.Grid[newX, newY];

                        // For mining, we don't exclude obstacles, but we exclude:
                        // 1. Occupied cells
                        // 2. World border cells (X == 0, Y == 0, or at the edge)
                        if (currentMode == InteractionMode.Mine) {
                            if (cell.Occupant != null || newX == 0 || newY == 0 || newX == maxX || newY == maxY) {
                                continue; // Can't mine on occupied cells or world borders
                            }
                        } else {
                            // For other modes, exclude obstacles and the player's current position
                            if (cell.Terrain.Obstacle || (newX == playerX && newY == playerY)) {
                                continue; // Exclude the player's position
                            }
                        }

                        // Highlight valid cells
                        cell.Highlighted = true;
                        HighlightedCells[i, j] = cell;
                    }
                }
            }

            // Check if the cursor's current position is still valid
            if (MapCursor.X >= 0 && MapCursor.X < gridSize &&
                MapCursor.Y >= 0 && MapCursor.Y < gridSize &&
                HighlightedCells[MapCursor.X, MapCursor.Y] != null) {
                // The cursor's current position is still valid
                HighlightedCells[MapCursor.X, MapCursor.Y].Cursor = true;
            } else {
                // The cursor's position is invalid; find the nearest valid cell
                if (FindNearestValidCell(out int newX, out int newY)) {
                    MapCursor.X = newX;
                    MapCursor.Y = newY;
                    HighlightedCells[MapCursor.X, MapCursor.Y].Cursor = true;
                } else {
                    Console.WriteLine("No valid cell found to place the cursor.");
                    // Optionally, handle this case (e.g., exit the interaction mode)
                }
            }

            Console.WriteLine($"Highlighted cells within range {range}.");
        }

        private static bool FindNearestValidCell(out int startX, out int startY) {
            int gridSizeX = HighlightedCells.GetLength(0);
            int gridSizeY = HighlightedCells.GetLength(1);
            int centerX = gridSizeX / 2;
            int centerY = gridSizeY / 2;

            int maxDistance = Math.Max(gridSizeX, gridSizeY);

            for (int distance = 1; distance < maxDistance; distance++) {
                for (int x = Math.Max(0, centerX - distance); x <= Math.Min(gridSizeX - 1, centerX + distance); x++) {
                    for (int y = Math.Max(0, centerY - distance); y <= Math.Min(gridSizeY - 1, centerY + distance); y++) {
                        if (HighlightedCells[x, y] != null) {
                            startX = x;
                            startY = y;
                            return true;
                        }
                    }
                }
            }

            startX = -1;
            startY = -1;
            return false;
        }

        public static void ClearHighlightedCells() {
            if (HighlightedCells != null) {
                // Iterate over the highlighted grid and clear highlights and cursor
                for (int i = 0; i < HighlightedCells.GetLength(0); i++) {
                    for (int j = 0; j < HighlightedCells.GetLength(1); j++) {
                        if (HighlightedCells[i, j] != null) {
                            HighlightedCells[i, j].Highlighted = false;
                            HighlightedCells[i, j].Cursor = false;
                            HighlightedCells[i, j] = null;
                        }
                    }
                }
                HighlightedCells = null;
            }
        }
    }
}