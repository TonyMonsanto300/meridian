using System;
using System.Collections.Generic;
using XenWorld.Controller;
using XenWorld.Model.Map;
using XenWorld.src.Factory;
using XenWorld.src.Manager;
using XenWorld.src.Model;

public class BanditSpawner : AbstractSpawner {
    private readonly int _numberOfEnemies;
    private readonly Random _random = new Random();

    public BanditSpawner(int numberOfEnemies) {
        _numberOfEnemies = numberOfEnemies > 0 ? numberOfEnemies : throw new ArgumentException("Number of enemies must be greater than zero.");
    }

    protected override BuildingTypeEnum BuildingType => BuildingTypeEnum.World; // Bandits are not tied to buildings

    protected override void CreateNPCs(Building building) {
        List<MapCell> availableCells = GetAvailableCells();
        List<DummyController> banditControllers = new List<DummyController>();

        for (int i = 0; i < _numberOfEnemies; i++) {
            if (availableCells.Count > 0) {
                var cell = availableCells[_random.Next(availableCells.Count)];
                var banditPuppet = PuppetFactory.CreateBandit(new Coordinate(cell.Coordinate.X, cell.Coordinate.Y));
                var banditController = new DummyController(banditPuppet, MapManager.ActiveMap, NPCType.Enemy);
                banditControllers.Add(banditController);
                availableCells.Remove(cell); // Prevent reuse of the cell
            } else {
                break; // No more available cells
            }
        }
        // Add generated bandits to the global NPC controllers
        DummyManager.DummyControllers.AddRange(banditControllers);
    }

    private List<MapCell> GetAvailableCells() {
        var availableCells = new List<MapCell>();

        for (int x = 0; x < MapManager.ActiveMap.Width; x++) {
            for (int y = 0; y < MapManager.ActiveMap.Height; y++) {
                var cell = MapManager.ActiveMap.Grid[x, y];
                if (!cell.Terrain.Obstacle && cell.Occupant == null) {
                    availableCells.Add(cell);
                }
            }
        }

        return availableCells;
    }
}