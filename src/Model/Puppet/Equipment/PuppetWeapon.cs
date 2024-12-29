using System.Collections.Generic;
using XenWorld.src.Model.Puppet.Stats;
using XenWorld.src.Service;

namespace XenWorld.src.Model.Puppet.Equipment {
    public enum PuppetWeaponType {
        Dagger,
        Claw,
        Glove,
        LongSword, // GreatSword
        WaveSword, // Zanbato
        WhipSword,
        Polearm,
        Spear, // Lance
        Javelin,
        WarAxe,// BattleAxe
        Flail,
        Mace, // WarHammer
        Sickle, // Scythe
        Glaive,
        Whip,
        Chain,
        Staff,
        Septor,
        LongBow,
        CrossBow,
        Arquebus,
        Pistol,
        BladeClub,
        Instrument
    }
    public class PuppetWeapon: EquipmentBase {
        public List<Dice> DamageDice { get; set; }
        public int DamageBonus { get; set; } = 0;
        public int HitBonus { get; set; } = 0;
        public PuppetWeaponType Type { get; set; }
        public bool TwoHanded { get; set; } = false;
        public PuppetWeapon(string name, List<Dice> dice, int damageBonus, int hitBonus, PuppetWeaponType type, bool twoHanded = false, StatBlock bonus = null, StatBlock requirement = null) : base(name, bonus, requirement, 0) {
            DamageDice = dice;
            DamageBonus = damageBonus;
            HitBonus = hitBonus;
            Type = type;
            TwoHanded = twoHanded;
        }

    }
}
