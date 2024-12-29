using System;
using System.Collections.Generic;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public static class PathFabricator {
        static Random random = new Random();

        public static void CreateMainPath(ZoneMap map, string pathTerrain) {
            int startX = 1;
            int endX = map.Width - 2;
            int currentX = startX;
            int currentY = random.Next(2, map.Height - 2);
            map.Grid[currentX, currentY] = new MapCell(TerrainDictionary.Context[pathTerrain], currentX, currentY);

            while (currentX < endX) {
                bool shouldTurn = random.NextDouble() < 0.3;
                if (shouldTurn) {
                    bool goUp = random.NextDouble() < 0.5;
                    int verticalSteps = random.Next(2, 5);
                    if (goUp) {
                        verticalSteps = Math.Min(verticalSteps, currentY - 1);
                        for (int i = 0; i < verticalSteps; i++) {
                            currentY -= 1;
                            if (map.IsWithinBounds(currentX, currentY)) {
                                map.Grid[currentX, currentY] = new MapCell(TerrainDictionary.Context[pathTerrain], currentX, currentY);
                            } else {
                                currentY += 1;
                                break;
                            }
                        }
                    } else {
                        verticalSteps = Math.Min(verticalSteps, map.Height - 2 - currentY);
                        for (int i = 0; i < verticalSteps; i++) {
                            currentY += 1;
                            if (map.IsWithinBounds(currentX, currentY)) {
                                map.Grid[currentX, currentY] = new MapCell(TerrainDictionary.Context[pathTerrain], currentX, currentY);
                            } else {
                                currentY -= 1;
                                break;
                            }
                        }
                    }
                }

                currentX += 1;
                if (map.IsWithinBounds(currentX, currentY)) {
                    map.Grid[currentX, currentY] = new MapCell(TerrainDictionary.Context[pathTerrain], currentX, currentY);
                } else {
                    break;
                }
            }
        }

        // Draws a line of "floor_wood" from (startX, startY) to (endX, endY) using Bresenham's Line Algorithm
        public static void DrawPath(ZoneMap map, int startX, int startY, int endX, int endY, string pathName = "grass", string floorName="floor_wood", string wallName="wall_panel", bool indoor = false) {
            int dx = Math.Abs(endX - startX);
            int dy = Math.Abs(endY - startY);
            int sx = startX < endX ? 1 : -1;
            int sy = startY < endY ? 1 : -1;
            int err = dx - dy;

            int x = startX, y = startY;

            while (true) {
                // Check if the next step reaches the end cell
                if (x == endX && y == endY) {
                    break; // Reached the end; stop drawing
                }

                // Set the current path cell to "floor_wood" if it's not the end cell
                if (!(x == endX && y == endY) && map.Grid[x, y].Terrain.Name != pathName) {
                    MapCell currentcell = map.Grid[x, y];
                    if(currentcell.Terrain.Name == wallName) {
                        currentcell.Terrain = TerrainDictionary.Context[floorName];
                        currentcell.Indoor = true;
                    }
                    else if (currentcell.Indoor == false) {
                        currentcell.Terrain = TerrainDictionary.Context[pathName];
                        currentcell.Indoor = indoor;
                    }
                }

                int e2 = 2 * err;

                if (e2 > -dy && e2 < dx) {
                    // Diagonal movement: add an elbow to form an L-shape
                    // Prioritize horizontal elbow first
                    int elbowX = x + sx;
                    int elbowY = y;
                    if (map.IsWithinBounds(elbowX, elbowY) && map.Grid[elbowX, elbowY].Terrain.Name != pathName) {
                        MapCell elbowCell = map.Grid[elbowX, elbowY];
                        if (elbowCell.Indoor == false) {
                            elbowCell.Terrain = TerrainDictionary.Context[pathName];
                            elbowCell.Indoor = indoor;
                        }
                    }

                    // Proceed diagonally
                    err -= dy;
                    x += sx;
                    y += sy;
                    err += dx;
                } else if (e2 > -dy) {
                    // Move horizontally
                    err -= dy;
                    x += sx;
                } else {
                    // Move vertically
                    err += dx;
                    y += sy;
                }
            }
        }

        public static Building DrawBuildingPath(
            ZoneMap map,
            Building newBuilding,
            int endX,
            int endY,
            string pathName = "grass",
            string floorName = "floor_wood",
            string wallTerrain = "wall_panel"
        ) {
            int dx = Math.Abs(endX - newBuilding.AnchorCell.Coordinate.X);
            int dy = Math.Abs(endY - newBuilding.AnchorCell.Coordinate.Y);
            int sx = newBuilding.AnchorCell.Coordinate.X < endX ? 1 : -1;
            int sy = newBuilding.AnchorCell.Coordinate.Y < endY ? 1 : -1;

            bool isHorizontalDominant = dx >= dy;

            int currentX = newBuilding.AnchorCell.Coordinate.X;
            int currentY = newBuilding.AnchorCell.Coordinate.Y;

            List<(int x, int y)> pathCells = new List<(int x, int y)>();

            while (currentX != endX || currentY != endY) {
                // Check if current cell is part of an existing room
                Building hitRoom = GetRoomAtCell(map, currentX, currentY);
                if (hitRoom != null) {
                    // Replace wall with floor to create a door
                    MapCell hitCell = map.Grid[currentX, currentY];
                    if (hitCell.Terrain.Wall) {
                        hitCell.Terrain = TerrainDictionary.Context[pathName];
                        hitCell.Indoor = true;
                        newBuilding.DoorCell = hitCell;
                    }

                    // Replace path cells with hitRoom's floor
                    foreach (var (px, py) in pathCells) {
                        map.Grid[px, py].Terrain = hitRoom.FloorTerrain;
                        map.Grid[px, py].Indoor = true;

                        // Add walls around the new path
                        foreach (var (nx, ny) in GetFourNeighbors(px, py)) {
                            if (map.IsWithinBounds(nx, ny)) {
                                MapCell neighborCell = map.Grid[nx, ny];
                                if (neighborCell.Terrain.Obstacle && neighborCell.Terrain.Name != wallTerrain) {
                                    neighborCell.Terrain = TerrainDictionary.Context[wallTerrain];
                                }
                            }
                        }
                    }

                    return hitRoom;
                }

                // Modify the current cell
                if (!(currentX == endX && currentY == endY) && map.Grid[currentX, currentY].Terrain.Name != pathName) {
                    MapCell currentCell = map.Grid[currentX, currentY];
                    if (currentCell.Terrain.Wall && newBuilding.DoorCell == null) {
                        currentCell.Terrain = TerrainDictionary.Context[floorName];
                        currentCell.Indoor = true;
                        newBuilding.DoorCell = currentCell;
                    } else if (!currentCell.Indoor) {
                        currentCell.Terrain = TerrainDictionary.Context[pathName];
                        currentCell.Indoor = isHorizontalDominant ? false : true;
                    }
                }

                // Add current cell to pathCells
                pathCells.Add((currentX, currentY));

                // Determine movement based on dominant direction
                if (isHorizontalDominant) {
                    // Primary movement: horizontal
                    if (currentX != endX) {
                        currentX += sx;
                    } else if (currentY != endY) {
                        currentY += sy;
                    }
                } else {
                    // Primary movement: vertical
                    if (currentY != endY) {
                        currentY += sy;
                    } else if (currentX != endX) {
                        currentX += sx;
                    }
                }
            }

            return null; // No room was hit
        }

        private static IEnumerable<(int x, int y)> GetFourNeighbors(int x, int y) {
            yield return (x, y - 1); // Up
            yield return (x, y + 1); // Down
            yield return (x - 1, y); // Left
            yield return (x + 1, y); // Right
        }

        private static Building GetRoomAtCell(ZoneMap map, int x, int y) {
            foreach (var room in map.Buildings) {
                foreach (var cell in room.Cells) {
                    if (cell.Coordinate.X == x && cell.Coordinate.Y == y) {
                        if (cell.Indoor && !cell.Terrain.Obstacle) {
                            return room;
                        }
                    }
                }
            }
            return null;
        }

        public static void DrawGatewayPath(ZoneMap map, int startX, int startY, int endX, int endY, string groundTerrain, string wallTerrain) {
            int currentX = startX;
            int currentY = startY;

            // Move horizontally towards endX
            while (currentX != endX) {
                currentX += (endX > currentX) ? 1 : -1;

                // Check bounds
                if (!map.IsWithinBounds(currentX, currentY)) {
                    break;
                }

                // If the current cell is the end cell, stop
                if (currentX == endX && currentY == endY) {
                    break;
                }

                // Replace only obstacle cells
                if (map.Grid[currentX, currentY].Terrain.Obstacle && !(map.Grid[currentX, currentY].Terrain.Name == wallTerrain)) {
                    map.Grid[currentX, currentY].Terrain = TerrainDictionary.Context[groundTerrain];
                } else {
                    // Stop path carving if not an obstacle
                    break;
                }
            }

            // Move vertically towards endY
            while (currentY != endY) {
                currentY += (endY > currentY) ? 1 : -1;

                // Check bounds
                if (!map.IsWithinBounds(currentX, currentY)) {
                    break;
                }

                // If the current cell is the end cell, stop
                if (currentX == endX && currentY == endY) {
                    break;
                }

                // Replace only obstacle cells
                if (map.Grid[currentX, currentY].Terrain.Obstacle && !(map.Grid[currentX, currentY].Terrain.Name == "wall_panel")) {
                    map.Grid[currentX, currentY].Terrain = TerrainDictionary.Context[groundTerrain];
                } else {
                    // Stop path carving if not an obstacle
                    break;
                }
            }
        }


    }
}
