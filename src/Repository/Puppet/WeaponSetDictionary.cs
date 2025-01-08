using System.Collections.Generic;
using XenWorld.src.Model.Puppet.Equipment;

namespace XenWorld.src.Repository.Puppet {
    public enum WeaponSetEnum {
        PLAYER,
        DEFAULT
    }

    public static class WeaponSetDictionary {
        public static Dictionary<WeaponSetEnum, List<PuppetWeapon>> Context
            = new Dictionary<WeaponSetEnum, List<PuppetWeapon>>();

        public static void LoadWeaponSet(WeaponSetEnum set, List<PuppetWeapon> weapons) {
            Context.Add(set, weapons);
        }
    }
}