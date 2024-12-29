using System;
using System.Collections.Generic;
using XenWorld.Config;
using XenWorld.Controller;
using XenWorld.Model;
using XenWorld.Model.Map;
using XenWorld.src.Factory.PuppetFactory;
using XenWorld.src.Loader.Puppet;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model.Puppet.Stats;
using XenWorld.src.Repository.Map;
using XenWorld.src.Repository.Puppet;

namespace XenWorld.Service {
    public static class BuildingHelper {
        public static PuppetDirection GetDoorDirection(Building building) {
            if (building == null) {
                throw new ArgumentNullException(nameof(building));
            }

            if (building.AnchorCell == null) {
                throw new ArgumentException("Building must have an AnchorCell.", nameof(building));
            }

            if (building.DoorCell == null) {
                throw new ArgumentException("Building must have a DoorCell.", nameof(building));
            }

            int deltaX = building.DoorCell.Coordinate.X - building.AnchorCell.Coordinate.X;
            int deltaY = building.DoorCell.Coordinate.Y - building.AnchorCell.Coordinate.Y;

            // Determine the direction based on deltaX and deltaY
            if (deltaX == 0 && deltaY < 0) {
                return PuppetDirection.North;
            } else if (deltaX > 0 && deltaY == 0) {
                return PuppetDirection.East;
            } else if (deltaX == 0 && deltaY > 0) {
                return PuppetDirection.South;
            } else if (deltaX < 0 && deltaY == 0) {
                return PuppetDirection.West;
            } else {
                // Door is not directly North, East, South, or West of the anchor
                return PuppetDirection.None;
            }
        }
    }
    public static class PuppetService {
        private static Random _random = new Random();

        // Helper function to generate random sprite names
        private static string GetMerchantSprite() {
            string[] merchantSprites = { "merchant" }; // Add more sprites if needed
            return merchantSprites[_random.Next(merchantSprites.Length)];
        }

        private static string GetAssistantSprite() {
            string[] assistantSprites = { "assistant" }; // Add more sprites if needed
            return assistantSprites[_random.Next(assistantSprites.Length)];
        }

        // Function to generate and place the player puppet
        public static PlayerController PlacePlayerPuppet() {
            bool playerPlaced = false;
            Puppet playerPuppet = GeneratePlayerPuppet();

            for (int selectedX = MapManager.ActiveMap.Width - 1; selectedX >= 0; selectedX--) {
                List<int> availableY = new List<int>();

                for (int y = 0; y < MapManager.ActiveMap.Height; y++) {
                    if (!MapManager.ActiveMap.Grid[selectedX, y].Terrain.Obstacle && MapManager.ActiveMap.Grid[selectedX, y].Occupant == null) {
                        availableY.Add(y);
                    }
                }

                if (availableY.Count > 0) {
                    int chosenY = _random.Next(availableY.Count);
                    int selectedY = availableY[chosenY];

                    playerPuppet.Coordinate.X = selectedX;
                    playerPuppet.Coordinate.Y = selectedY;

                    playerPlaced = true;
                    break;
                }
            }

            if (!playerPlaced) {
                int fallbackX = MapManager.ActiveMap.Width - 2;
                int fallbackY = MapManager.ActiveMap.Height / 2;

                if (!MapManager.ActiveMap.Grid[fallbackX, fallbackY].Terrain.Obstacle && MapManager.ActiveMap.Grid[fallbackX, fallbackY].Occupant == null) {
                    playerPuppet = new Puppet("Muwatili", SpriteDictionary.Context["player"], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], fallbackX, fallbackY);
                } else {
                    bool found = false;
                    for (int x = 0; x < MapManager.ActiveMap.Width && !found; x++) {
                        for (int y = 0; y < MapManager.ActiveMap.Height && !found; y++) {
                            if (!MapManager.ActiveMap.Grid[x, y].Terrain.Obstacle && MapManager.ActiveMap.Grid[x, y].Occupant == null) {
                                playerPuppet = new Puppet("Muwatili", SpriteDictionary.Context["player"], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], x, y);
                                found = true;
                            }
                        }
                    }

                    if (!found) {
                        throw new Exception("No available cell to place the player puppet.");
                    }
                }
            }

            MapManager.ActiveMap.Grid[playerPuppet.Coordinate.X, playerPuppet.Coordinate.Y].Occupant = playerPuppet;
            PlayerController playerController = new PlayerController(playerPuppet);

            return playerController;
        }

        // Function to generate the player puppet
        public static Puppet GeneratePlayerPuppet() {
            var playerPuppet = new Puppet("Muwatili", SpriteDictionary.Context["player"], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard]);
            playerPuppet.Health = new Health(30);
            return playerPuppet;
        }

        // Function to generate enemy puppets
        public static void GenerateEnemyPuppets(ZoneMap activeMap, PlayerController playerController, int numberOfEnemies = 1) {
            List<NPCController> generatedNPCs = new List<NPCController>();

            List<MapCell> availableNPCCells = new List<MapCell>();

            for (int x = 0; x < RenderConfig.MapViewPortX; x++) {
                for (int y = 0; y < RenderConfig.MapViewPortY; y++) {
                    if (!activeMap.Grid[x, y].Terrain.Obstacle && activeMap.Grid[x, y].Occupant == null) {
                        availableNPCCells.Add(activeMap.Grid[x, y]);
                    }
                }
            }

            for (int i = 0; i < numberOfEnemies; i++) {

                if (availableNPCCells.Count > 0) {
                    var cell = availableNPCCells[_random.Next(availableNPCCells.Count)];
                    var npcPuppet = PuppetFactory.CreateBandit(cell.Coordinate.X, cell.Coordinate.Y);
                    var npcController = new NPCController(npcPuppet, playerController, activeMap, NPCType.Enemy);
                    generatedNPCs.Add(npcController);
                    availableNPCCells.Remove(cell);
                } else {
                    break;
                }
            }

            NPCManager.NPCControllers.AddRange(generatedNPCs);
        }

        // Refactored GenerateMerchants
        public static void GenerateMerchants(ZoneMap activeMap, PlayerController playerController) {
            List<NPCController> merchantControllers = new List<NPCController>();

            foreach (var building in activeMap.Buildings) {
                var anchor = building.AnchorCell;
                int x = anchor.Coordinate.X;
                int y = anchor.Coordinate.Y;

                PuppetDirection doorDirection = BuildingHelper.GetDoorDirection(building);

                int merchantXAdjustment = 0;
                int merchantYAdjustment = 0;
                int assistantXAdjustment = 0;
                int assistantYAdjustment = 0;

                // Adjustments based on door direction
                if (doorDirection == PuppetDirection.North) {
                    merchantXAdjustment = +1;
                    merchantYAdjustment = +2;

                    assistantXAdjustment = -1; // Inverse of merchantXAdjustment
                    assistantYAdjustment = merchantYAdjustment; // Same as merchantYAdjustment
                } else if (doorDirection == PuppetDirection.South) {
                    merchantXAdjustment = +1;
                    merchantYAdjustment = -2;

                    assistantXAdjustment = -1; // Inverse of merchantXAdjustment
                    assistantYAdjustment = merchantYAdjustment; // Same as merchantYAdjustment
                } else if (doorDirection == PuppetDirection.East) {
                    merchantXAdjustment = -2;
                    merchantYAdjustment = +1;

                    assistantXAdjustment = merchantXAdjustment; // Same as merchantXAdjustment
                    assistantYAdjustment = -merchantYAdjustment; // Inverse of merchantYAdjustment
                } else if (doorDirection == PuppetDirection.West) {
                    merchantXAdjustment = +2;
                    merchantYAdjustment = +1;

                    assistantXAdjustment = merchantXAdjustment; // Same as merchantXAdjustment
                    assistantYAdjustment = -merchantYAdjustment; // Inverse of merchantYAdjustment
                }

                // Check if the positions are valid
                int merchantX = x + merchantXAdjustment;
                int merchantY = y + merchantYAdjustment;
                int assistantX = x + assistantXAdjustment;
                int assistantY = y + assistantYAdjustment;

                // Ensure positions are within map bounds
                if (IsValidPosition(activeMap, merchantX, merchantY) && IsValidPosition(activeMap, assistantX, assistantY)) {
                    if (building.Type == BuildingType.Shop) {
                        // Create Merchant Puppet
                        var merchantPuppet = new Puppet("Shop Keeper", SpriteDictionary.Context[GetMerchantSprite()], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], merchantX, merchantY);
                        building.Owner = merchantPuppet;
                        var merchantController = new NPCController(merchantPuppet, playerController, activeMap);

                        // Create Assistant Puppet
                        var assistantPuppet = new Puppet("Shop Assistant", SpriteDictionary.Context[GetAssistantSprite()], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], assistantX, assistantY);
                        var assistantController = new NPCController(assistantPuppet, playerController, activeMap);
                        building.Tenants.Add(assistantPuppet);

                        // Add to controllers list
                        merchantControllers.Add(merchantController);
                        merchantControllers.Add(assistantController);
                    }
                } else {
                    // Handle invalid positions if necessary
                    Console.WriteLine($"Cannot place merchant or assistant at the calculated positions for building at ({x}, {y}).");
                }
            }

            NPCManager.NPCControllers.AddRange(merchantControllers);
        }

        public static void GenerateThralls(ZoneMap activeMap, PlayerController playerController) {
            List<NPCController> thrallControllers = new List<NPCController>();

            foreach (var building in activeMap.Buildings) {
                var anchor = building.AnchorCell;
                int x = anchor.Coordinate.X;
                int y = anchor.Coordinate.Y;

                // Check if the building is of type Thrall
                if (building.Type != BuildingType.Thrall) {
                    continue; // Skip if this is not a Thrall building
                }

                PuppetDirection doorDirection = BuildingHelper.GetDoorDirection(building);

                // Offsets for taskmaster and thralls based on direction
                int taskmasterXAdjustment = 0;
                int taskmasterYAdjustment = 0;

                int thrallX1Adjustment = 0;
                int thrallY1Adjustment = 0;

                int thrallX2Adjustment = 0;
                int thrallY2Adjustment = 0;

                int thrallX3Adjustment = 0;
                int thrallY3Adjustment = 0;

                int thrallX4Adjustment = 0;
                int thrallY4Adjustment = 0;

                // Adjustments based on door direction
                if (doorDirection == PuppetDirection.North) {
                    taskmasterXAdjustment = 0;
                    taskmasterYAdjustment = 0; // Taskmaster stays at the anchor

                    // North door thralls adjustments
                    thrallX1Adjustment = 1; thrallY1Adjustment = -3;
                    thrallX2Adjustment = -1; thrallY2Adjustment = -3;
                    thrallX3Adjustment = 3; thrallY3Adjustment = -3;
                    thrallX4Adjustment = -3; thrallY4Adjustment = -3;

                } else if (doorDirection == PuppetDirection.South) {
                    taskmasterXAdjustment = 0;
                    taskmasterYAdjustment = 0;

                    // South door thralls adjustments
                    thrallX1Adjustment = 1; thrallY1Adjustment = 3;
                    thrallX2Adjustment = -1; thrallY2Adjustment = 3;
                    thrallX3Adjustment = 3; thrallY3Adjustment = 3;
                    thrallX4Adjustment = -3; thrallY4Adjustment = 3;

                } else if (doorDirection == PuppetDirection.East) {
                    taskmasterXAdjustment = 0;
                    taskmasterYAdjustment = 0;

                    // East door thralls adjustments
                    thrallX1Adjustment = -3; thrallY1Adjustment = 1;
                    thrallX2Adjustment = -3; thrallY2Adjustment = -1;
                    thrallX3Adjustment = -3; thrallY3Adjustment = 3;
                    thrallX4Adjustment = -3; thrallY4Adjustment = -3;

                } else if (doorDirection == PuppetDirection.West) {
                    taskmasterXAdjustment = 0;
                    taskmasterYAdjustment = 0;

                    // West door thralls adjustments
                    thrallX1Adjustment = 3; thrallY1Adjustment = 1;
                    thrallX2Adjustment = 3; thrallY2Adjustment = -1;
                    thrallX3Adjustment = 3; thrallY3Adjustment = 3;
                    thrallX4Adjustment = 3; thrallY4Adjustment = -3;
                }

                // Taskmaster at anchor position
                int taskmasterX = x + taskmasterXAdjustment;
                int taskmasterY = y + taskmasterYAdjustment;

                // Ensure taskmaster is in a valid position
                if (IsValidPosition(activeMap, taskmasterX, taskmasterY)) {
                    var taskmasterPuppet = new Puppet("Taskmaster", SpriteDictionary.Context[GetTaskmasterSprite()], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], taskmasterX, taskmasterY);
                    building.Owner = taskmasterPuppet;
                    var taskmasterController = new NPCController(taskmasterPuppet, playerController, activeMap);

                    // Now place the thralls
                    // Thrall 1
                    int adjustedThrallX1 = x + thrallX1Adjustment;
                    int adjustedThrallY1 = y + thrallY1Adjustment;

                    // Thrall 2
                    int adjustedThrallX2 = x + thrallX2Adjustment;
                    int adjustedThrallY2 = y + thrallY2Adjustment;

                    // Thrall 3
                    int adjustedThrallX3 = x + thrallX3Adjustment;
                    int adjustedThrallY3 = y + thrallY3Adjustment;

                    // Thrall 4
                    int adjustedThrallX4 = x + thrallX4Adjustment;
                    int adjustedThrallY4 = y + thrallY4Adjustment;

                    // Ensure thralls are in valid positions
                    if (IsValidPosition(activeMap, adjustedThrallX1, adjustedThrallY1)) {
                        var thrallPuppet1 = new Puppet("Thrall", SpriteDictionary.Context[GetThrallSprite()], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], adjustedThrallX1, adjustedThrallY1);
                        building.Tenants.Add(thrallPuppet1);
                        var thrallController1 = new NPCController(thrallPuppet1, playerController, activeMap);
                        thrallControllers.Add(thrallController1);
                    }

                    if (IsValidPosition(activeMap, adjustedThrallX2, adjustedThrallY2)) {
                        var thrallPuppet2 = new Puppet("Thrall", SpriteDictionary.Context[GetThrallSprite()], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], adjustedThrallX2, adjustedThrallY2);
                        building.Tenants.Add(thrallPuppet2);
                        var thrallController2 = new NPCController(thrallPuppet2, playerController, activeMap);
                        thrallControllers.Add(thrallController2);
                    }

                    if (IsValidPosition(activeMap, adjustedThrallX3, adjustedThrallY3)) {
                        var thrallPuppet3 = new Puppet("Thrall", SpriteDictionary.Context[GetThrallSprite()], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], adjustedThrallX3, adjustedThrallY3);
                        building.Tenants.Add(thrallPuppet3);
                        var thrallController3 = new NPCController(thrallPuppet3, playerController, activeMap);
                        thrallControllers.Add(thrallController3);
                    }

                    if (IsValidPosition(activeMap, adjustedThrallX4, adjustedThrallY4)) {
                        var thrallPuppet4 = new Puppet("Thrall", SpriteDictionary.Context[GetThrallSprite()], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], adjustedThrallX4, adjustedThrallY4);
                        building.Tenants.Add(thrallPuppet4);
                        var thrallController4 = new NPCController(thrallPuppet4, playerController, activeMap);
                        thrallControllers.Add(thrallController4);
                    }

                    // Add taskmaster to the controller list
                    thrallControllers.Add(taskmasterController);
                }
            }

            // Add generated thralls to the global NPC controllers
            NPCManager.NPCControllers.AddRange(thrallControllers);
        }

        public static void GenerateCivilians(ZoneMap activeMap, PlayerController playerController) {
            List<NPCController> civilianControllers = new List<NPCController>();

            foreach (var building in activeMap.Buildings) {
                var anchor = building.AnchorCell;
                int x = anchor.Coordinate.X;
                int y = anchor.Coordinate.Y;

                // Check if the building is of type House
                if (building.Type != BuildingType.House) {
                    continue; // Skip if this is not a House building
                }

                // We can place the civilian near the anchor position
                // You can adjust these offsets as needed
                int civilianXAdjustment = 0;
                int civilianYAdjustment = 0;

                // Let's say we place the civilian at the anchor for simplicity
                int civilianX = x + civilianXAdjustment;
                int civilianY = y + civilianYAdjustment;

                // Ensure the position is valid
                if (IsValidPosition(activeMap, civilianX, civilianY)) {
                    var civilianPuppet = new Puppet("Civilian", SpriteDictionary.Context[GetCivilianSprite()], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], civilianX, civilianY);
                    building.Tenants.Add(civilianPuppet);
                    var civilianController = new NPCController(civilianPuppet, playerController, activeMap);
                    civilianControllers.Add(civilianController);
                }
            }

            // Add generated civilians to the global NPC controllers
            NPCManager.NPCControllers.AddRange(civilianControllers);
        }

        private static string GetCivilianSprite() {
            string[] taskmasterSprites = { "peasant" }; // Add more sprites if needed
            return taskmasterSprites[_random.Next(taskmasterSprites.Length)];
        }

        private static string GetTaskmasterSprite() {
            string[] taskmasterSprites = { "taskmaster" }; // Add more sprites if needed
            return taskmasterSprites[_random.Next(taskmasterSprites.Length)];
        }

        private static string GetThrallSprite() {
            string[] thrallSprites = { "thrall" }; // Add more sprites if needed
            return thrallSprites[_random.Next(thrallSprites.Length)];
        }

        // Helper method to check if a position is valid within the map
        private static bool IsValidPosition(ZoneMap map, int x, int y) {
            return x >= 0 && x < map.Width && y >= 0 && y < map.Height &&
                   !map.Grid[x, y].Terrain.Obstacle && map.Grid[x, y].Occupant == null;
        }

        // Function to deploy NPCs onto the map
        public static void DeployNPCs(ZoneMap activeMap) {
            foreach (var npcController in NPCManager.NPCControllers) {
                var puppet = npcController.Puppet;
                int x = puppet.Coordinate.X;
                int y = puppet.Coordinate.Y;

                if (!activeMap.Grid[x, y].Terrain.Obstacle && activeMap.Grid[x, y].Occupant == null) {
                    activeMap.Grid[x, y].Occupant = puppet;
                }
            }
        }

        // Function to initialize all NPCs
        public static void InitializeNPCs() {
            GenerateEnemyPuppets(MapManager.ActiveMap, PlayerManager.Controller, 1000);
            GenerateMerchants(MapManager.ActiveMap, PlayerManager.Controller);
            GenerateThralls(MapManager.ActiveMap, PlayerManager.Controller);
            GenerateCivilians(MapManager.ActiveMap, PlayerManager.Controller);
            DeployNPCs(MapManager.ActiveMap);
        }
    }
}