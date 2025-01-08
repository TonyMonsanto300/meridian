using System;
using System.Collections.Generic;
using System.Linq;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;
using XenWorld.src.Factory.MapFactory.Finder;
using XenWorld.src.Model;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public static class RoomFabricator {
        private static List<Coordinate> FindValidRoomAnchors(ZoneMap map, int size, string borderName) {
            int requiredSize = size + 7; // Increased by 2 to raise inset by 1
            int halfSize = requiredSize / 2;
            List<Coordinate> candidates = new List<Coordinate>();

            for (int x = halfSize; x < map.Width - halfSize; x++) {
                for (int y = halfSize; y < map.Height - halfSize; y++) {
                    if (map.Grid[x, y].Terrain.Name != borderName) continue;

                    if (IsValidAnchor(map, x, y, halfSize, borderName)) {
                        candidates.Add(new Coordinate(x, y));
                    }
                }
            }

            return candidates;
        }

        private static Coordinate SelectRandomAnchor(List<Coordinate> candidates, Random random) {
            return candidates[random.Next(candidates.Count)];
        }

        private static bool IsValidAnchor(ZoneMap map, int x, int y, int halfSize, string borderName) {
            for (int dx = -halfSize; dx <= halfSize; dx++) {
                for (int dy = -halfSize; dy <= halfSize; dy++) {
                    int checkX = x + dx;
                    int checkY = y + dy;

                    if (!map.IsWithinBounds(checkX, checkY) ||
                        map.Grid[checkX, checkY].Indoor ||
                        map.Grid[checkX, checkY].Terrain.Name != borderName) {
                        return false;
                    }
                }
            }
            return true;
        }

        private static List<MapCell> GetRoomCells(ZoneMap map, int centerX, int centerY, int size) {
            List<MapCell> roomCells = new List<MapCell>();
            for (int dx = -(size / 2); dx <= size / 2; dx++) {
                for (int dy = -(size / 2); dy <= size / 2; dy++) {
                    int x = centerX + dx;
                    int y = centerY + dy;

                    if (map.IsWithinBounds(x, y)) {
                        roomCells.Add(map.Grid[x, y]);
                    }
                }
            }
            return roomCells;
        }

        private static bool BuildWall(ZoneMap map, Coordinate center, int wallSize) {
            return TerrainPainter.PaintSquare(map, center, wallSize, "wall_panel");
        }

        private static bool BuildFloor(ZoneMap map, Coordinate center, int size, string floorName) {
            return TerrainPainter.PaintSquare(map, center, size, floorName, setIndoor: true);
        }

        private static bool BuildRoom(ZoneMap map, Coordinate center, int wallSize, int size, string floorName) {
            return BuildWall(map, center, wallSize) &&
                   BuildFloor(map, center, size, floorName);
        }

        public static void GenerateRooms(ZoneMap map, int roomCount = 1, int size = 3, string floorName = "floor_wood", string borderName = "border", BuildingTypeEnum type = BuildingTypeEnum.World) {
            int wallSize = size + 1;
            Random random = new Random();
            int roomsAdded = 0;

            for (int i = 0; i < roomCount; i++) {
                var candidates = FindValidRoomAnchors(map, size, borderName);
                if (candidates.Count == 0) {
                    break;
                }

                Coordinate center = SelectRandomAnchor(candidates, random);
                if (!BuildRoom(map, center, wallSize, size, floorName)) continue;

                roomsAdded += ConstructRoom(map, center, size, floorName, type);
            }

            Console.WriteLine($"{roomsAdded} room(s) successfully added to the map.");
        }

        private static int ConstructRoom(ZoneMap map, Coordinate center, int size, string floorName, BuildingTypeEnum type) {
            List<MapCell> roomCells = GetRoomCells(map, center.X, center.Y, size);
            MapCell anchorCell = map.Grid[center.X, center.Y];
            MapTerrain floorTerrain = TerrainDictionary.Context[floorName];
            Building newRoom = new Building(roomCells.ToArray(), anchorCell, floorTerrain, type);

            Direction preferredDirection = GetPreferredDirection(map, center.X, center.Y);
            var nearestOutdoor = FindNearestOutdoorCellInPreferredDirection(map, center.X, center.Y, preferredDirection);

            if (nearestOutdoor != null) {
                return ConnectRoomToPath(map, newRoom, nearestOutdoor.Value, "path_white", floorName, "wall_panel");
            } else {
                map.Buildings.Add(newRoom);
                return 1;
            }
        }

        private static Direction GetPreferredDirection(ZoneMap map, int x, int y) {
            double centerX = map.Width / 2.0;
            double centerY = map.Height / 2.0;

            bool isTop = y > centerY;
            bool isBottom = y < centerY;
            bool isLeft = x > centerX;
            bool isRight = x < centerX;

            // Determine primary quadrant based on position relative to center
            if (isTop && isLeft) {
                return Direction.BottomRight;
            } else if (isTop && isRight) {
                return Direction.BottomLeft;
            } else if (isBottom && isLeft) {
                return Direction.TopRight;
            } else { // isBottom && isRight
                return Direction.TopLeft;
            }

            // Alternatively, if only primary directions are needed:
            /*
            if (isTop) return Direction.Bottom;
            if (isBottom) return Direction.Top;
            if (isLeft) return Direction.Right;
            if (isRight) return Direction.Left;
            return Direction.Bottom; // Default
            */
        }

        private static (int x, int y)? FindNearestOutdoorCellInPreferredDirection(ZoneMap map, int centerX, int centerY, Direction preferredDirection) {
            // Implement a BFS that prioritizes the preferred direction
            Queue<(int x, int y)> queue = new Queue<(int, int)>();
            HashSet<(int, int)> visited = new HashSet<(int, int)>();
            queue.Enqueue((centerX, centerY));
            visited.Add((centerX, centerY));

            while (queue.Count > 0) {
                int levelCount = queue.Count;
                List<(int x, int y)> currentLevel = new List<(int x, int y)>();

                for (int i = 0; i < levelCount; i++) {
                    var (x, y) = queue.Dequeue();
                    currentLevel.Add((x, y));
                }

                // Sort current level based on preferred direction
                currentLevel.Sort((a, b) => CompareDirection(a, b, preferredDirection));

                foreach (var (x, y) in currentLevel) {
                    foreach (var (nx, ny) in GetNeighborsOrdered(x, y, preferredDirection)) {
                        if (map.IsWithinBounds(nx, ny) && visited.Add((nx, ny))) {
                            queue.Enqueue((nx, ny));

                            if (!map.Grid[nx, ny].Indoor && !map.Grid[nx, ny].Terrain.Obstacle) {
                                Console.WriteLine($"Found nearest outdoor cell at ({nx}, {ny}) in preferred direction.");
                                return (nx, ny);
                            }
                        }
                    }
                }
            }

            Console.WriteLine("No suitable outdoor cell found.");
            return null;
        }

        private static int CompareDirection((int x, int y) a, (int x, int y) b, Direction preferredDirection) {
            // Prioritize cells closer to the preferred direction
            // This can be adjusted based on how Direction is defined
            // For simplicity, we'll assign a priority score
            int scoreA = GetDirectionScore(a, preferredDirection);
            int scoreB = GetDirectionScore(b, preferredDirection);
            return scoreB.CompareTo(scoreA); // Higher score first
        }

        private static int GetDirectionScore((int x, int y) cell, Direction preferredDirection) {
            // Assign scores based on how aligned the cell is with the preferred direction
            // Higher score means more aligned
            // Example for Direction.Top: cells with higher y should have higher scores
            switch (preferredDirection) {
                case Direction.Top:
                    return cell.y;
                case Direction.Bottom:
                    return -cell.y;
                case Direction.Left:
                    return cell.x;
                case Direction.Right:
                    return -cell.x;
                case Direction.BottomLeft:
                    return cell.x + cell.y;
                case Direction.BottomRight:
                    return -cell.x + cell.y;
                case Direction.TopLeft:
                    return cell.x - cell.y;
                case Direction.TopRight:
                    return -cell.x - cell.y;
                default:
                    return 0;
            }
        }

        private enum Direction {
            Top,
            Bottom,
            Left,
            Right,
            BottomLeft,
            BottomRight,
            TopLeft,
            TopRight
        }

        private static IEnumerable<(int x, int y)> GetNeighborsOrdered(int x, int y, Direction preferredDirection) {
            // Return neighbors ordered based on preferred direction
            List<(int x, int y)> neighbors = MapHelper.GetNeighbors(x, y).ToList();

            switch (preferredDirection) {
                case Direction.Top:
                    neighbors.Sort((a, b) => a.y.CompareTo(b.y));
                    break;
                case Direction.Bottom:
                    neighbors.Sort((a, b) => b.y.CompareTo(a.y));
                    break;
                case Direction.Left:
                    neighbors.Sort((a, b) => a.x.CompareTo(b.x));
                    break;
                case Direction.Right:
                    neighbors.Sort((a, b) => b.x.CompareTo(a.x));
                    break;
                case Direction.BottomLeft:
                    neighbors.Sort((a, b) => (a.x + a.y).CompareTo(b.x + b.y));
                    break;
                case Direction.BottomRight:
                    neighbors.Sort((a, b) => (-a.x + a.y).CompareTo(-b.x + b.y));
                    break;
                case Direction.TopLeft:
                    neighbors.Sort((a, b) => (a.x - a.y).CompareTo(b.x - b.y));
                    break;
                case Direction.TopRight:
                    neighbors.Sort((a, b) => (-a.x - a.y).CompareTo(-b.x - b.y));
                    break;
            }

            return neighbors;
        }

        private static int ConnectRoomToPath(ZoneMap map, Building newRoom, (int x, int y) nearestOutdoor, string pathName, string floorName, string wallName) {
            Building hitRoom = PathFabricator.DrawBuildingPath(
                map,
                newRoom,
                nearestOutdoor.x,
                nearestOutdoor.y,
                pathName,
                floorName,
                wallName
            );

            if (hitRoom != null) {
                MergeRooms(newRoom, hitRoom);
                return 1;
            } else {
                map.Buildings.Add(newRoom);
                return 1;
            }
        }

        private static void MergeRooms(Building newRoom, Building hitRoom) {
            foreach (var cell in newRoom.Cells) {
                hitRoom.Cells = AppendToMapCellArray(hitRoom.Cells, cell);
                cell.Terrain = hitRoom.FloorTerrain;
                cell.Indoor = true;
            }
            Console.WriteLine("Rooms merged successfully.");
        }

        private static MapCell[] AppendToMapCellArray(MapCell[] array, MapCell cell) {
            var list = new List<MapCell>(array) { cell };
            return list.ToArray();
        }
    }
}
