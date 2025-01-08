using System.Collections.Generic;

namespace XenWorld.Model.Map {
    public enum BuildingTypeEnum {
        World,
        House,
        Shop,
        Thrall,
        Temple
    }

    public class Building {
        public MapCell[] Cells { get; set; }
        public MapCell AnchorCell { get; set; }
        public MapCell? DoorCell { get; set; }
        public MapTerrain FloorTerrain { get; set; }
        public Puppet? Owner { get; set; }
        public List<Puppet> Tenants = new List<Puppet>();
        public BuildingTypeEnum Type { get; set; } = BuildingTypeEnum.World;

        public Building(MapCell[] cells, MapCell anchor, MapTerrain floor, BuildingTypeEnum type, MapCell doorCell = null) {
            Cells = cells;
            AnchorCell = anchor;
            FloorTerrain = floor;
            Owner = null;
            Type = type;
            DoorCell = doorCell;
        }
    }
}