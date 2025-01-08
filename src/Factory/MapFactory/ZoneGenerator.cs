using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenWorld.Factory.Map.XenWorld.Factory.Map;
using XenWorld.Model.Map;
using XenWorld.src.Factory.MapFactory.Architect;
using XenWorld.src.Factory.MapFactory.MapFabricator;

namespace XenWorld.src.Factory.MapFactory {
    public class ZoneGenerator {

        static Random random = new Random();
        static List<string> floorTypes = new List<string>() { "floor_panel", "floor_wood" };

        // Instantiate the ZoneGenerator with dependencies
        static ZoneGenerator generatorLogic = new ZoneGenerator(random, floorTypes);

        // Create ZoneBuilder instances with respective generator functions
        static ZoneBuilder mountainTownBuilder = new ZoneBuilder(
            generatorLogic.GenerateMountainTown,
            random,
            floorTypes,
            groundTerrain: "grass",
            borderTerrain: "border",
            pathTerrain: "path_white"
        );

        static ZoneBuilder mountainPassBuilder = new ZoneBuilder(
            generatorLogic.GenerateMountainPass,
            random,
            floorTypes,
            groundTerrain: "grass",
            borderTerrain: "border",
            pathTerrain: "path_white"
        );

        static ZoneBuilder mountainFortressBuilder = new ZoneBuilder(
            generatorLogic.GenerateMountainFortress,
            random,
            floorTypes,
            groundTerrain: "grass",
            borderTerrain: "border",
            pathTerrain: "path_white"
        );

        static ZoneBuilder plainsTownBuilder = new ZoneBuilder(
            generatorLogic.GeneratePlainsTown,
            random,
            floorTypes,
            groundTerrain: "grass",
            borderTerrain: "border",
            pathTerrain: "path_white"
        );

        // Initialize the dictionary with zone names and their builders
        public static Dictionary<string, ZoneBuilder> ZoneBuilders = new Dictionary<string, ZoneBuilder>() {
            { "MountainTown", mountainTownBuilder },
            { "MountainPass", mountainPassBuilder },
            { "MountainFortress", mountainFortressBuilder },
            { "PlainsTown", plainsTownBuilder }
        };

        private readonly Random _random;
        private readonly List<string> _floorTypes;
        private readonly string _groundTerrain;
        private readonly string _borderTerrain;
        private readonly string _pathTerrain;

        public ZoneGenerator(
            Random random,
            List<string> floorTypes,
            string groundTerrain = "grass",
            string borderTerrain = "border",
            string pathTerrain = "path_white"
        ) {
            _random = random;
            _floorTypes = floorTypes;
            _groundTerrain = groundTerrain;
            _borderTerrain = borderTerrain;
            _pathTerrain = pathTerrain;
        }

        public ZoneMap GenerateMountainTown(ZoneMap map) {
            FillFabricator.FillZone(map, _groundTerrain);
            MountainPathArchitect.Carve(map, _borderTerrain, _groundTerrain);
            PathFabricator.CreateMainPath(map, _pathTerrain);

            int debriSize = _random.Next(4, 10);
            int smallRoomNumber = _random.Next(4, 9);
            int mediumRoomNumber = _random.Next(1, 4);
            int largeRoomNumber = _random.Next(0, 1);

            RoomFabricator.GenerateRooms(map, largeRoomNumber, 17, _floorTypes[_random.Next(_floorTypes.Count)], type: BuildingTypeEnum.Temple);
            RoomFabricator.GenerateRooms(map, largeRoomNumber, 9, _floorTypes[_random.Next(_floorTypes.Count)], type: BuildingTypeEnum.Thrall);
            RoomFabricator.GenerateRooms(map, mediumRoomNumber, 5, _floorTypes[_random.Next(_floorTypes.Count)], type: BuildingTypeEnum.Shop);
            RoomFabricator.GenerateRooms(map, smallRoomNumber, 3, _floorTypes[_random.Next(_floorTypes.Count)], type: BuildingTypeEnum.House);

            CleanupFabricator.CleanUpDebris(map, debriSize);
            return map;
        }

        public ZoneMap GenerateMountainPass(ZoneMap map) {
            FillFabricator.FillZone(map, _groundTerrain);
            MountainPathArchitect.Carve(map, _borderTerrain, _groundTerrain);

            int debriSize = _random.Next(4, 10);
            CleanupFabricator.CleanUpDebris(map, debriSize);
            return map;
        }

        public ZoneMap GenerateMountainFortress(ZoneMap map) {
            FillFabricator.FillZone(map, _groundTerrain);
            MountainPathArchitect.Carve(map, _borderTerrain, _groundTerrain);

            RoomFabricator.GenerateRooms(map, 1, 25, _floorTypes[_random.Next(_floorTypes.Count)]);

            int debriSize = _random.Next(4, 10);
            CleanupFabricator.CleanUpDebris(map, debriSize);
            return map;
        }

        public ZoneMap GeneratePlainsTown(ZoneMap map) {
            FillFabricator.FillZone(map, _groundTerrain);

            RoomFabricator.GenerateRooms(map, 1, 9, _floorTypes[_random.Next(_floorTypes.Count)], _groundTerrain, type: BuildingTypeEnum.Thrall);
            RoomFabricator.GenerateRooms(map, 2, 5, _floorTypes[_random.Next(_floorTypes.Count)], _groundTerrain, type: BuildingTypeEnum.Shop);
            RoomFabricator.GenerateRooms(map, 8, 3, _floorTypes[_random.Next(_floorTypes.Count)], _groundTerrain, type: BuildingTypeEnum.House);
            return map;
        }
    }
}
