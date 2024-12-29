using XenWorld.Model.Map;
using XenWorld.Config;
using System;
using System.Collections.Generic;


namespace XenWorld.Factory.Map {
    namespace XenWorld.Factory.Map {
        public class ZoneBuilder {

            private readonly string _groundTerrain;
            private readonly string _borderTerrain;
            private readonly string _wallTerrain;
            private readonly string _pathTerrain;
            private readonly List<string> _floorTypes;

            private readonly int _chunkX;
            private readonly int _chunkY;

            private readonly Func<ZoneMap, ZoneMap> _generator;

            private readonly Random _random;

            public ZoneBuilder(
                Func<ZoneMap, ZoneMap> generator,
                Random random,
                List<string> floorTypes,
                string groundTerrain = "grass",
                string borderTerrain = "border",
                string wallTerrain = "wall_panel",
                string pathTerrain = "path_white",
                int chunkX = 40,
                int chunkY = 30
            ) {
                _generator = generator ?? throw new ArgumentNullException(nameof(generator));
                _random = random ?? throw new ArgumentNullException(nameof(random));
                _floorTypes = floorTypes ?? throw new ArgumentNullException(nameof(floorTypes));
                _groundTerrain = groundTerrain;
                _borderTerrain = borderTerrain;
                _wallTerrain = wallTerrain;
                _pathTerrain = pathTerrain;
                _chunkX = chunkX;
                _chunkY = chunkY;
            }

            public ZoneMap CreateMap(int x = 0, int y = 0) {
                if (x == 0) {
                    x = RenderConfig.MapViewPortX;
                }
                if (y == 0) {
                    y = RenderConfig.MapViewPortY;
                }
                return new ZoneMap(x, y);
            }

            public ZoneMap Generate() {
                return _generator(CreateMap(_chunkX, _chunkY));
            }
        }
    }

    
}
