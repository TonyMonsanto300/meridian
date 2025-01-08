using System.Collections.Generic;
using XenWorld.src.Model.Puppet.Equipment;
using XenWorld.src.Repository.Puppet;
using XenWorld.src.Service;

namespace XenWorld.src.Loader.Puppet {
    public static class WeaponSetLoader {
        public static void SeedWeaponSets() {
            WeaponSetDictionary.LoadWeaponSet(WeaponSetEnum.PLAYER, new List<PuppetWeapon>() {
                new PuppetWeapon("Wood Bow", new List<Dice> { Dice.D4 }, 0, 0, PuppetWeaponType.LongBow, true, null, null, 2),
                new PuppetWeapon("Bronze Sword", new List<Dice> { Dice.D6 }, 0, 0, PuppetWeaponType.LongSword)
            });

            WeaponSetDictionary.LoadWeaponSet(WeaponSetEnum.DEFAULT, new List<PuppetWeapon>() {
                new PuppetWeapon("Bronze Dagger", new List<Dice> { Dice.D4 }, 0, 0, PuppetWeaponType.Dagger)
            });
        }
    }
}