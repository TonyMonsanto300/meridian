using System.Collections.Generic;
using XenWorld.src.Model.Puppet.Equipment;
using XenWorld.src.Repository.Puppet;

namespace XenWorld.src.Loader.Puppet {
    public static class AccessorySetLoader {
        public static void SeedAccessorySets() {
            AccessorySetDictionary.LoadAccessorySet(AccessorySetEnum.DEFAULT, new List<PuppetAccessory>());

            AccessorySetDictionary.LoadAccessorySet(AccessorySetEnum.PLAYER, new List<PuppetAccessory> {
                new PuppetAccessory("Bronze Shield", 1, AccessoryType.None)
            });
        }
    }
}