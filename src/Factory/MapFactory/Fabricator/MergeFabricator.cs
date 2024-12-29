using System;
using System.Collections.Generic;
using XenWorld.Model.Map;
using XenWorld.Repository.Map;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public class MergeFabricator {
        public ZoneMap MergeMapsVertically(ZoneMap baseMap, ZoneMap newMap, string groundTerrain, string wallTerrain) {
            if (baseMap == null || newMap == null) {
                throw new ArgumentNullException("MapGrids cannot be null.");
            }

            // Determine the new dimensions
            int newWidth = Math.Max(baseMap.Width, newMap.Width);
            int newHeight = baseMap.Height + newMap.Height;
            ZoneMap mergedMap = new ZoneMap(newWidth, newHeight);

            baseMap.Buildings.ForEach(room => {
                mergedMap.Buildings.Add(room);
            });
            newMap.Buildings.ForEach(room => {
                room.AnchorCell.Coordinate.Y += baseMap.Height;
                room.DoorCell.Coordinate.Y += baseMap.Height;
                mergedMap.Buildings.Add(new Building(room.Cells, room.AnchorCell, room.FloorTerrain, room.Type, room.DoorCell));
            });

            // Initialize all cells in mergedMap to _groundTerrain by default
            for (int x = 0; x < newWidth; x++) {
                for (int y = 0; y < newHeight; y++) {
                    mergedMap.Grid[x, y] = new MapCell(TerrainDictionary.Context[groundTerrain], x, y);
                }
            }

            // Copy baseMap into mergedMap
            for (int x = 0; x < baseMap.Width; x++) {
                for (int y = 0; y < baseMap.Height; y++) {
                    mergedMap.Grid[x, y] = CloneMapCell(baseMap.Grid[x, y]);
                }
            }

            // Copy newMap into mergedMap, offset by baseMap.Height
            for (int x = 0; x < newMap.Width; x++) {
                for (int y = 0; y < newMap.Height; y++) {
                    mergedMap.Grid[x, baseMap.Height + y] = CloneMapCell(newMap.Grid[x, y]);
                }
            }

            // Step 1: Find the bottommost ground cell in baseMap
            (int x, int y) baseGroundCell = FindBottommostGroundCell(baseMap, groundTerrain);
            if (baseGroundCell.x == -1) {
                throw new InvalidOperationException("No ground cell found on the bottom side of the base map.");
            }

            // Step 2: Find the topmost ground cell in newMap
            (int x, int y) newGroundCell = FindTopmostGroundCell(newMap, groundTerrain);
            if (newGroundCell.x == -1) {
                throw new InvalidOperationException("No ground cell found on the top side of the new map.");
            }

            // Step 3: Adjust coordinates for mergedMap
            int mergedBaseX = baseGroundCell.x;
            int mergedBaseY = baseGroundCell.y;

            int mergedNewX = newGroundCell.x;
            int mergedNewY = baseMap.Height + newGroundCell.y;

            // Step 4: Draw a path between (mergedBaseX, mergedBaseY) and (mergedNewX, mergedNewY)
            PathFabricator.DrawGatewayPath(mergedMap, mergedBaseX, mergedBaseY, mergedNewX, mergedNewY, groundTerrain, wallTerrain);

            //mergedMap.Rooms = new List<Room>(baseMap.Rooms);
            //newMap.Rooms.ForEach(room => {
            //    room.Anchor.Coordinate.Y += baseMap.Height;
            //});

            //mergedMap.Rooms = new List<Room>(baseMap.Rooms);

            return mergedMap;
        }

        // Updated merging logic with precise path carving
        public ZoneMap MergeMapsHorizontally(ZoneMap baseMap, ZoneMap newMap, string groundTerrain, string wallTerrain) {
            if (baseMap == null || newMap == null) {
                throw new ArgumentNullException("MapGrids cannot be null.");
            }

            // Determine the new dimensions
            int newWidth = baseMap.Width + newMap.Width;
            int newHeight = Math.Max(baseMap.Height, newMap.Height);
            ZoneMap mergedMap = new ZoneMap(newWidth, newHeight);

            newMap.Buildings.ForEach(room => {
                room.AnchorCell.Coordinate.X += baseMap.Width;
                room.DoorCell.Coordinate.X += baseMap.Width;
            });

            mergedMap.Buildings.AddRange(baseMap.Buildings);
            mergedMap.Buildings.AddRange(newMap.Buildings);

            // Initialize all cells in mergedMap to _groundTerrain by default
            for (int x = 0; x < newWidth; x++) {
                for (int y = 0; y < newHeight; y++) {
                    mergedMap.Grid[x, y] = new MapCell(TerrainDictionary.Context[groundTerrain], x, y);
                }
            }

            // Copy baseMap into mergedMap
            for (int x = 0; x < baseMap.Width; x++) {
                for (int y = 0; y < baseMap.Height; y++) {
                    mergedMap.Grid[x, y] = CloneMapCell(baseMap.Grid[x, y]);
                }
            }

            // Copy newMap into mergedMap, offset by baseMap.Width
            for (int x = 0; x < newMap.Width; x++) {
                for (int y = 0; y < newMap.Height; y++) {
                    mergedMap.Grid[baseMap.Width + x, y] = CloneMapCell(newMap.Grid[x, y]);
                }
            }

            // Step 1: Find the rightmost ground cell in baseMap
            (int x, int y) baseGroundCell = FindRightmostGroundCell(baseMap, groundTerrain);
            if (baseGroundCell.x == -1) {
                throw new InvalidOperationException("No ground cell found on the right side of the base map.");
            }

            // Step 2: Find the leftmost ground cell in newMap
            (int x, int y) newGroundCell = FindLeftmostGroundCell(newMap, groundTerrain);
            if (newGroundCell.x == -1) {
                throw new InvalidOperationException("No ground cell found on the left side of the new map.");
            }

            // Step 3: Adjust coordinates for mergedMap
            int mergedBaseX = baseGroundCell.x;
            int mergedBaseY = baseGroundCell.y;

            int mergedNewX = baseMap.Width + newGroundCell.x;
            int mergedNewY = newGroundCell.y;

            // Step 4: Draw a path between (mergedBaseX, mergedBaseY) and (mergedNewX, mergedNewY)
            PathFabricator.DrawGatewayPath(mergedMap, mergedBaseX, mergedBaseY, mergedNewX, mergedNewY, groundTerrain, wallTerrain);

            return mergedMap;
        }

        // Helper method to find the rightmost ground cell in a map
        private static (int x, int y) FindRightmostGroundCell(ZoneMap map, string groundTerrain) {
            for (int x = map.Width - 1; x >= 0; x--) {
                for (int y = 0; y < map.Height; y++) {
                    if (map.Grid[x, y].Terrain.Name == groundTerrain) {
                        return (x, y);
                    }
                }
            }
            return (-1, -1); // Not found
        }

        // Helper method to find the leftmost ground cell in a map
        private static (int x, int y) FindLeftmostGroundCell(ZoneMap map, string groundTerrain) {
            for (int x = 0; x < map.Width; x++) {
                for (int y = 0; y < map.Height; y++) {
                    if (map.Grid[x, y].Terrain.Name == groundTerrain) {
                        return (x, y);
                    }
                }
            }
            return (-1, -1); // Not found
        }

        // Helper method to find the bottommost ground cell in a map
        private static (int x, int y) FindBottommostGroundCell(ZoneMap map, string groundTerrain) {
            for (int y = map.Height - 1; y >= 0; y--) {
                for (int x = 0; x < map.Width; x++) {
                    if (map.Grid[x, y].Terrain.Name == groundTerrain) {
                        return (x, y);
                    }
                }
            }
            return (-1, -1); // Not found
        }

        // Helper method to find the topmost ground cell in a map
        private static (int x, int y) FindTopmostGroundCell(ZoneMap map, string groundTerrain) {
            for (int y = 0; y < map.Height; y++) {
                for (int x = 0; x < map.Width; x++) {
                    if (map.Grid[x, y].Terrain.Name == groundTerrain) {
                        return (x, y);
                    }
                }
            }
            return (-1, -1); // Not found
        }

        private static MapCell CloneMapCell(MapCell original) {
            MapCell clonedCell = new MapCell(original.Terrain, original.Coordinate.X, original.Coordinate.Y) {
                Occupant = original.Occupant, // Assuming Puppet is a reference type; deep copy if necessary
                Highlighted = original.Highlighted,
                Cursor = original.Cursor,
                Indoor = original.Indoor
            };
            return clonedCell;
        }
    }
}
