using System;
using System.Collections.Generic;
using XenWorld.Factory.Map.XenWorld.Factory.Map;
using XenWorld.Model.Map;

namespace XenWorld.src.Factory.MapFactory.MapFabricator {
    public static class ContinentFabricator {
        static Random _random = new Random();
        public static ZoneMap GenerateContinent(List<ZoneBuilder> generators, string groundTerrain, string wallTerrain, int size = 2, int height = 1) {
            if (generators == null || generators.Count == 0) {
                throw new ArgumentException("Generators list cannot be null or empty.");
            }

            if (size < 1 || height < 1) {
                throw new ArgumentException("Size and height must be at least 1.");
            }

            // Initialize a 2D list to hold the generated maps
            List<List<ZoneMap>> gridMaps = new List<List<ZoneMap>>();

            for (int row = 0; row < height; row++) {
                List<ZoneMap> rowMaps = new List<ZoneMap>();
                for (int col = 0; col < size; col++) {
                    // Select a random generator from the list
                    ZoneBuilder generator = generators[_random.Next(generators.Count)];
                    ZoneMap generatedMap = generator.Generate();
                    rowMaps.Add(generatedMap);
                }
                gridMaps.Add(rowMaps);
            }

            // Merge each row horizontally with precise path carving
            List<ZoneMap> mergedRows = new List<ZoneMap>();
            foreach (var rowMaps in gridMaps) {
                ZoneMap mergedRow = rowMaps[0];
                for (int i = 1; i < rowMaps.Count; i++) {
                    mergedRow = new MergeFabricator().MergeMapsHorizontally(mergedRow, rowMaps[i], groundTerrain, wallTerrain);
                }
                mergedRows.Add(mergedRow);
            }

            // Merge all rows vertically with precise path carving
            ZoneMap superMap = mergedRows[0];
            for (int i = 1; i < mergedRows.Count; i++) {
                superMap = new MergeFabricator().MergeMapsVertically(superMap, mergedRows[i], groundTerrain, wallTerrain);
            }

            BorderFabricator.BuildBorder(superMap, "border");
            return superMap;
        }
    }
}
