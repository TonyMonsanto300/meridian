using System.Collections.Generic;
using XenWorld.src.Model.Puppet.Equipment;

namespace XenWorld.src.Repository.Puppet {
    public enum AccessorySetEnum {
        DEFAULT,
        PLAYER
    }

    public static class AccessorySetDictionary {
        public static Dictionary<AccessorySetEnum, List<PuppetAccessory>> Context
            = new Dictionary<AccessorySetEnum, List<PuppetAccessory>>();

        public static void LoadAccessorySet(AccessorySetEnum key, List<PuppetAccessory> accessories) {
            Context.Add(key, accessories);
        }
    }
}