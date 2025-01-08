using System.Collections.Generic;
using System.Linq;
using XenWorld.Model.Map;
using XenWorld.src.Manager;
using XenWorld.src.Model;

public abstract class AbstractSpawner {
    protected AbstractSpawner() { }
    protected abstract BuildingTypeEnum BuildingType { get; }

    public void Spawn() {
        if (BuildingType == BuildingTypeEnum.World) {
            CreateNPCs();
        } else {
            MapManager.ActiveMap.Buildings.Where(building => building.Type == BuildingType).ToList().ForEach(building => {
                CreateNPCs(building);
            });
        }
    }

    protected abstract void CreateNPCs(Building building = null);

    protected bool IsValidPosition(Coordinate position) {
        return MapManager.ActiveMap.IsWithinBounds(position.X, position.Y) && !MapManager.ActiveMap.Grid[position.X, position.Y].Occupied;
    }
}