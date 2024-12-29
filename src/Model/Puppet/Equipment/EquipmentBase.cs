using XenWorld.Model;
using XenWorld.src.Model.Puppet.Stats;

namespace XenWorld.src.Model.Puppet.Equipment {
    public class EquipmentBase {
        public string Name {  get; set; }
        public StatBlock Bonus { get; set; } = new StatBlock();
        public StatBlock Requirement { get; set; } = new StatBlock();
        public int Defense { get; set; }

        public EquipmentBase(string name, StatBlock bonus = null, StatBlock requirement = null, int defense = 0) {
            Name = name;
            Defense = defense;
            if (bonus != null) {
                Bonus = bonus;
            }
            if(requirement != null) {
                Requirement = requirement;
            }
        }
    }
}
