using System;
using System.Collections.Generic;
using XenWorld.Controller;
using XenWorld.Model;
using XenWorld.Model.Map;
using XenWorld.src.Factory;
using XenWorld.src.Manager;
using XenWorld.src.Model;
using XenWorld.src.Model.Puppet;

public class PlayerSpawner : AbstractSpawner {
    protected override BuildingTypeEnum BuildingType => BuildingTypeEnum.World; // Not used, as the player isn't tied to a building.

    new public void Spawn() {
        Puppet playerPuppet = null;

        // Try to find a valid location for the player
        for (int selectedX = MapManager.ActiveMap.Width - 1; selectedX >= 0; selectedX--) {
            var availableY = GetAvailableYPositions(selectedX);

            if (availableY.Count > 0) {
                int chosenY = new Random().Next(availableY.Count);
                int selectedY = availableY[chosenY];

                playerPuppet = PuppetFactory.CreatePlayer(new Coordinate(selectedX, selectedY));
                break;
            }
        }

        // Fallback if no suitable location is found
        if (playerPuppet == null) {
            playerPuppet = PlacePlayerInFallback();
        }

        // Inject the player into the map
        MapManager.ActiveMap.Grid[playerPuppet.Location.X, playerPuppet.Location.Y].Occupant = playerPuppet;

        // Add the player controller
        PlayerManager.Controller = new PlayerController(playerPuppet);
    }

    protected override void CreateNPCs(Building building) {
        throw new NotImplementedException("PlayerSpawner does not use building-based NPC creation.");
    }

    private List<int> GetAvailableYPositions(int x) {
        var availableY = new List<int>();

        for (int y = 0; y < MapManager.ActiveMap.Height; y++) {
            if (!MapManager.ActiveMap.Grid[x, y].Terrain.Obstacle && MapManager.ActiveMap.Grid[x, y].Occupant == null) {
                availableY.Add(y);
            }
        }

        return availableY;
    }

    private Puppet PlacePlayerInFallback() {
        int fallbackX = MapManager.ActiveMap.Width - 2;
        int fallbackY = MapManager.ActiveMap.Height / 2;

        if (!MapManager.ActiveMap.Grid[fallbackX, fallbackY].Terrain.Obstacle &&
            MapManager.ActiveMap.Grid[fallbackX, fallbackY].Occupant == null) {
            return PuppetFactory.CreatePlayer(new Coordinate(fallbackX, fallbackY));
        }

        // Find the first available position
        for (int x = 0; x < MapManager.ActiveMap.Width; x++) {
            for (int y = 0; y < MapManager.ActiveMap.Height; y++) {
                if (!MapManager.ActiveMap.Grid[x, y].Terrain.Obstacle && MapManager.ActiveMap.Grid[x, y].Occupant == null) {
                    return PuppetFactory.CreatePlayer(new Coordinate(x, y));
                }
            }
        }

        throw new Exception("No available cell to place the player puppet.");
    }
}