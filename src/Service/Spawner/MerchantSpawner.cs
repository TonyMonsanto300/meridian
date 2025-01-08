using XenWorld.Controller;
using XenWorld.Model.Map;
using XenWorld.Service;
using XenWorld.src.Factory;
using XenWorld.src.Helper;
using XenWorld.src.Manager;
using XenWorld.src.Model;
using XenWorld.src.Model.Puppet;

public class MerchantSpawner : AbstractSpawner {
    protected override BuildingTypeEnum BuildingType => BuildingTypeEnum.Shop;

    protected override void CreateNPCs(Building building) {
        var anchor = building.AnchorCell;
        PuppetDirection doorDirection = BuildingHelper.GetDoorDirection(building);

        var adjustments = GetPositionAdjustments(doorDirection);

        // Calculate positions
        var merchantPosition = new Coordinate(anchor.Coordinate.X + adjustments.merchantX, anchor.Coordinate.Y + adjustments.merchantY);
        var assistantPosition = new Coordinate(anchor.Coordinate.X + adjustments.assistantX, anchor.Coordinate.Y + adjustments.assistantY);

        // Ensure positions are valid
        if (!IsValidPosition(merchantPosition) || !IsValidPosition(assistantPosition)) { return; }

        // Create NPCs
        var merchantPuppet = PuppetFactory.CreateShopOwner(merchantPosition);
        var assistantPuppet = PuppetFactory.CreateShopKeeper(assistantPosition);

        // Assign to building
        building.Owner = merchantPuppet;
        building.Tenants.Add(assistantPuppet);

        // Add to DummyManager
        DummyManager.DummyControllers.Add(new DummyController(merchantPuppet, MapManager.ActiveMap));
        DummyManager.DummyControllers.Add(new DummyController(assistantPuppet, MapManager.ActiveMap));
    }

    private (int merchantX, int merchantY, int assistantX, int assistantY) GetPositionAdjustments(PuppetDirection direction) {
        return direction switch {
            PuppetDirection.North => (1, 2, -1, 2),
            PuppetDirection.South => (1, -2, -1, -2),
            PuppetDirection.East => (-2, 1, -2, -1),
            PuppetDirection.West => (2, 1, 2, -1),
            _ => (0, 0, 0, 0)
        };
    }
}