using System;
using XenWorld.Model;

namespace XenWorld.src.Service {
    public static class WeaponService {
        public static void CycleEquippedWeapon(Puppet puppet) {
            int weaponCount = puppet.Weapons.Count;

            if (weaponCount <= 1) {
                // No need to cycle if there's only one or no weapon
                Console.WriteLine("Weapon cycling not available.");
                return;
            }

            if (puppet.EquippedWeapon == null) {
                // If no weapon is currently equipped, equip the first one
                puppet.EquippedWeapon = puppet.Weapons[0];
                Console.WriteLine($"Equipped weapon: {puppet.EquippedWeapon.Name}");
                return;
            }

            // Find the index of the currently equipped weapon
            int currentIndex = puppet.Weapons.IndexOf(puppet.EquippedWeapon);

            if (currentIndex == -1) {
                // EquippedWeapon is not in the Weapons list, equip the first weapon
                puppet.EquippedWeapon = puppet.Weapons[0];
                Console.WriteLine($"Equipped weapon: {puppet.EquippedWeapon.Name}");
                return;
            }

            // Calculate the next index, looping back to the first weapon if at the end
            int nextIndex = (currentIndex + 1) % weaponCount;

            // Assign the next weapon as the equipped weapon
            puppet.EquippedWeapon = puppet.Weapons[nextIndex];
        }
    }
}
