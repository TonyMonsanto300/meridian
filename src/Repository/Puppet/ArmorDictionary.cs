using System.Collections.Generic;
using XenWorld.src.Model.Puppet.Equipment;

namespace XenWorld.src.Repository.Puppet {
    public enum ArmorEnum {
        DEFAULT,
        PLAYER
    }

    public static class ArmorDictionary {
        public static Dictionary<ArmorEnum, PuppetArmor> Context
            = new Dictionary<ArmorEnum, PuppetArmor>();

        public static void LoadArmor(ArmorEnum key, PuppetArmor armor) {
            Context.Add(key, armor);
        }
    }
}