using System.Collections.Generic;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Repository.Puppet;

namespace XenWorld.src.Loader.Puppet {
    public static class ResourceSetLoader {
        public static void SeedResourceSets() {
            ResourceSetDictionary.LoadResourceSet(ResourceSetEnum.Default, new List<PuppetResource>() {
                new PuppetResource(ResourceTypeEnum.Mana, 10),
                new PuppetResource(ResourceTypeEnum.Energy, 10)
            });
        }
    }
}