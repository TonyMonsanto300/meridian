using System;
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
                Console.WriteLine("No movement direction provided.");
                return;
            }

            if (HighlightedCells.GetLength(0) == 3 && HighlightedCells.GetLength(1) == 3) {
                // Special handling for 3x3 grids, which worked perfectly
                MoveCursorInSmallGrid(direction);
            } else {
                // General handling for larger grids, enable center crossing
                MoveCursorInLargeGrid(direction);
            }
        }

        private static void MoveCursorInSmallGrid(PuppetDirection direction) {
            int center = 1;  // Center index for a 3x3 grid
            int newX = center, newY = center; // Start from the center

            switch (direction) {
                case PuppetDirection.North:
                    newY--;
                    break;
                case PuppetDirection.South:
                    newY++;
                    break;
                case PuppetDirection.East:
                    newX++;
                    break;
                case PuppetDirection.West:
                    newX--;
                    break;
                case PuppetDirection.NorthEast:
                    newX++;
                    newY--;
                    break;
                case PuppetDirection.NorthWest:
                    newX--;
                    newY--;
                    break;
                case PuppetDirection.SouthEast:
                    newX++;
                    newY++;
                    break;
                case PuppetDirection.SouthWest:
                    newX--;
                    newY++;
                    break;
            }

            if (IsWithinBounds(newX, newY) && HighlightedCells[newX, newY] != null) {
                ClearAndMoveCursor(newX, newY);
            } else {
                Console.WriteLine("Cannot move cursor. Target cell is invalid.");
            }
        }

        private static void MoveCursorInLargeGrid(PuppetDirection direction) {
            int center = HighlightedCells.GetLength(0) / 2;
            int newX = MapCursor.X;
            int newY = MapCursor.Y;

            // Determine new position based on direction
            switch (direction) {
                case PuppetDirection.North:
                    newY--;
                    break;
                case PuppetDirection.South:
                    newY++;
                    break;
                case PuppetDirection.East:
                    newX++;
                    break;
                case PuppetDirection.West:
                    newX--;
                    break;
                case PuppetDirection.NorthEast:
                    newX++;
                    newY--;
                    break;
                case PuppetDirection.NorthWest:
                    newX--;
                    newY--;
                    break;
                case PuppetDirection.SouthEast:
                    newX++;
                    newY++;
                    break;
                case PuppetDirection.SouthWest:
                    newX--;
                    newY++;
                    break;
            }

            // Check if attempting to move into center and prepare to cross if center is null
            if (HighlightedCells[center, center] == null && IsMovingIntoCenter(newX, newY, center)) {
                int skipStep = 1; // Define how much further to move across the center
                switch (direction) {
                    case PuppetDirection.North:
                    case PuppetDirection.South:
                        newY += (direction == PuppetDirection.North) ? -skipStep : skipStep;
                        break;
                    case PuppetDirection.East:
                    case PuppetDirection.West:
                        newX += (direction == PuppetDirection.East) ? skipStep : -skipStep;
                        break;
                    case PuppetDirection.NorthEast:
                    case PuppetDirection.SouthWest:
                    case PuppetDirection.NorthWest:
                    case PuppetDirection.SouthEast:
                        newY += (direction == PuppetDirection.NorthEast || direction == PuppetDirection.NorthWest) ? -skipStep : skipStep;
                        newX += (direction == PuppetDirection.NorthEast || direction == PuppetDirection.SouthEast) ? skipStep : -skipStep;
                        break;
                }
            }

            if (IsWithinBounds(newX, newY) && (HighlightedCells[newX, newY] != null)) {
                ClearAndMoveCursor(newX, newY);
            } else {
                Console.WriteLine("Cannot move cursor. Target cell is invalid, out of bounds, or occupied.");
            }
        }

        private static bool IsMovingIntoCenter(int newX, int newY, int center) {
            // Check if the new coordinates are exactly the center
            return newX == center && newY == center;
        }

        private static bool IsWithinBounds(int x, int y) {
            return x >= 0 && x < HighlightedCells.GetLength(0) && y >= 0 && y < HighlightedCells.GetLength(1);
        }

        private static void ClearAndMoveCursor(int newX, int newY) {
            if (HighlightedCells[MapCursor.X, MapCursor.Y] != null) {
                HighlightedCells[MapCursor.X, MapCursor.Y].Cursor = false;
            }
            MapCursor.X = newX;
            MapCursor.Y = newY;
            HighlightedCells[MapCursor.X, MapCursor.Y].Cursor = true;
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

            int playerX = PlayerManager.Controller.Puppet.Location.X;
            int playerY = PlayerManager.Controller.Puppet.Location.Y;

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

            // Cull isolated highlighted cells
            for (int i = 0; i < gridSize; i++) {
                for (int j = 0; j < gridSize; j++) {
                    if (HighlightedCells[i, j] != null && HighlightedCells[i, j].Highlighted) {
                        // Check for neighbors
                        bool hasNeighbor = false;
                        foreach (var (dx, dy) in new[] { (0, 1), (1, 0), (0, -1), (-1, 0) }) {
                            int ni = i + dx;
                            int nj = j + dy;
                            if (ni >= 0 && ni < gridSize && nj >= 0 && nj < gridSize && HighlightedCells[ni, nj] != null && HighlightedCells[ni, nj].Highlighted) {
                                hasNeighbor = true;
                                break;
                            }
                        }

                        // If no neighbors and not directly adjacent to the player (manhattan distance > 1), un-highlight
                        if (!hasNeighbor && (Math.Abs(i - range) + Math.Abs(j - range) > 1)) {
                            HighlightedCells[i, j].Highlighted = false;
                        }
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