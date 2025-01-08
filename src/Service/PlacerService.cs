using XenWorld.src.Manager;
using XenWorld.Model.Map;

namespace XenWorld.src.Service {
    public static class PlacerService {
        public static void PlaceDummies(ZoneMap activeMap) {
            foreach (var npcController in DummyManager.DummyControllers) {
                var puppet = npcController.Puppet;
                int x = puppet.Location.X;
                int y = puppet.Location.Y;

                // Ensure the position is valid and unoccupied
                if (!activeMap.Grid[x, y].Terrain.Obstacle && activeMap.Grid[x, y].Occupant == null) {
                    activeMap.Grid[x, y].Occupant = puppet;
                }
            }
        }
    }
}