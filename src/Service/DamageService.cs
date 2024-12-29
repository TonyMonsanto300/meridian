using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenWorld.Model;
using XenWorld.src.Model.Puppet.Equipment;

namespace XenWorld.src.Service {
    public static class DamageService {
        private static Random _random = new Random();

        public static int RollWeaponDamage(PuppetWeapon weapon) {
            int damage = 0;
            weapon.DamageDice.ForEach(die => {
                damage += _random.Next(1, DiceService.GetDieValue(die) + 1);
            });
            damage += weapon.DamageBonus;
            return damage;
        }
    }
}
