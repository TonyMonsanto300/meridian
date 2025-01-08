using XenWorld.Controller;
using XenWorld.Model.Map;
using XenWorld.src.Factory;
using XenWorld.src.Manager;
using XenWorld.src.Model;

public class VillagerSpawner : AbstractSpawner {
    protected override BuildingTypeEnum BuildingType => BuildingTypeEnum.House;

    protected override void CreateNPCs(Building building) {
        var anchor = building.AnchorCell;
        int x = anchor.Coordinate.X;
        int y = anchor.Coordinate.Y;

        // Adjustments for civilian placement (can be customized as needed)
        int civilianXAdjustment = 0;
        int civilianYAdjustment = 0;

        // Calculate position
        int civilianX = x + civilianXAdjustment;
        int civilianY = y + civilianYAdjustment;

        // Validate the position
        var civilianPosition = new Coordinate(civilianX, civilianY);
        if (!IsValidPosition(civilianPosition)) return;

        // Create Villager Puppet
        var villagerPuppet = PuppetFactory.CreateVillager(civilianPosition);

        // Assign to the building
        building.Tenants.Add(villagerPuppet);

        // Register with DummyManager
        DummyManager.DummyControllers.Add(new DummyController(villagerPuppet, MapManager.ActiveMap));
    }
}