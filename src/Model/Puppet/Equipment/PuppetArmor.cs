using XenWorld.Model;
using XenWorld.src.Model.Puppet.Stats;

namespace XenWorld.src.Model.Puppet.Equipment {
    public enum PuppetArmorClass {
        None,
        Light,
        Medium,
        Heavy
    }
    public class PuppetArmor : EquipmentBase {
        public PuppetArmorClass ArmorClass { get; set; }

        public PuppetArmor(string name, int defense=0, PuppetArmorClass armorClass=PuppetArmorClass.None, StatBlock bonus = null, StatBlock requirement = null) : base(name, bonus, requirement, defense) {
            Name = name;
            ArmorClass = armorClass;
        }
    }
}
