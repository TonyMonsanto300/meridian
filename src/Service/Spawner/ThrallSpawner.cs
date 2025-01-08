using System.Collections.Generic;
using XenWorld.Controller;
using XenWorld.Model.Map;
using XenWorld.src.Factory;
using XenWorld.src.Helper;
using XenWorld.src.Manager;
using XenWorld.src.Model;
using XenWorld.src.Model.Puppet;

public class ThrallSpawner : AbstractSpawner {
    protected override BuildingTypeEnum BuildingType => BuildingTypeEnum.Thrall;

    protected override void CreateNPCs(Building building) {
        var anchor = building.AnchorCell;
        PuppetDirection doorDirection = BuildingHelper.GetDoorDirection(building);

        var taskmasterPosition = new Coordinate(anchor.Coordinate.X, anchor.Coordinate.Y);
        var thrallPositions = GetThrallPositions(anchor.Coordinate.X, anchor.Coordinate.Y, doorDirection);

        // Ensure taskmaster position is valid
        if (!IsValidPosition(taskmasterPosition)) return;

        // Create Taskmaster
        var taskmasterPuppet = PuppetFactory.CreateTaskMaster(taskmasterPosition);
        building.Owner = taskmasterPuppet;
        DummyManager.DummyControllers.Add(new DummyController(taskmasterPuppet, MapManager.ActiveMap));

        // Create Thralls
        foreach (var thrallPosition in thrallPositions) {
            if (!IsValidPosition(thrallPosition)) continue;

            var thrallPuppet = PuppetFactory.CreateThrall(thrallPosition);
            building.Tenants.Add(thrallPuppet);
            DummyManager.DummyControllers.Add(new DummyController(thrallPuppet, MapManager.ActiveMap));
        }
    }

    private List<Coordinate> GetThrallPositions(int x, int y, PuppetDirection doorDirection) {
        var adjustments = doorDirection switch {
            PuppetDirection.North => new List<(int, int)> {
                (1, -3), (-1, -3), (3, -3), (-3, -3)
            },
            PuppetDirection.South => new List<(int, int)> {
                (1, 3), (-1, 3), (3, 3), (-3, 3)
            },
            PuppetDirection.East => new List<(int, int)> {
                (-3, 1), (-3, -1), (-3, 3), (-3, -3)
            },
            PuppetDirection.West => new List<(int, int)> {
                (3, 1), (3, -1), (3, 3), (3, -3)
            },
            _ => new List<(int, int)>()
        };

        var positions = new List<Coordinate>();
        foreach (var (dx, dy) in adjustments) {
            positions.Add(new Coordinate(x + dx, y + dy));
        }

        return positions;
    }
}