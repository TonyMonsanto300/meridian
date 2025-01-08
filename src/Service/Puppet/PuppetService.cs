using System.Collections.Generic;
using XenWorld.src.Manager;
using XenWorld.src.Service;
using XenWorld.src.Service.Spawner;

namespace XenWorld.Service {
    public static class PuppetService {
        public static void InitializePuppets() {
            new MasterSpawner(new List<AbstractSpawner> {
                new VillagerSpawner(),
                new ThrallSpawner(),
                new MerchantSpawner(),
                new BanditSpawner(1)
            }).Execute();
            PlacerService.PlaceDummies(MapManager.ActiveMap);
        }
    }
}