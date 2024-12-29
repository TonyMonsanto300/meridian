using XenWorld.Model.Map;
using XenWorld.src.Factory.MapFactory.MapFabricator;

namespace XenWorld.src.Factory.MapFactory.Architect {
    public static class MountainPathArchitect {
        public static void Carve(ZoneMap map, string borderTerrain, string groundTerrain) {
            FillFabricator.FillZone(map, borderTerrain);
            PathFabricator.CreateMainPath(map, groundTerrain);
            BranchFabricator.CreateBranches(map, 6, borderTerrain, groundTerrain);
            PathFabricator.CreateMainPath(map, groundTerrain);
            BranchFabricator.CreateBranches(map, 6, borderTerrain, groundTerrain);
        }
    }
}
