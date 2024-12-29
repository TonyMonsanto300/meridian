using System.Collections.Generic;
using XenWorld.Factory.Map.XenWorld.Factory.Map;
using XenWorld.Model.Map;
using XenWorld.src.Factory.MapFactory;
using XenWorld.src.Factory.MapFactory.MapFabricator;

namespace XenWorld.Factory.Map {
    public class MapFactory {
        public static ZoneMap GetDefaultMap() {
            return _generateMountainContinent();
        }

        private static ZoneMap _generateMountainContinent() {
            List<ZoneBuilder> zoneBuilders= new List<ZoneBuilder>() {
                ZoneGenerator.ZoneBuilders["MountainTown"],
                ZoneGenerator.ZoneBuilders["MountainPass"],
                ZoneGenerator.ZoneBuilders["PlainsTown"],
                ZoneGenerator.ZoneBuilders["MountainFortress"]
            };
            return ContinentFabricator.GenerateContinent(zoneBuilders, "grass", "panel_wall", 5, 5);
        }
    }
}
