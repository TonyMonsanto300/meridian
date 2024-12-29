using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenWorld.src.Model.Puppet.Stats;

namespace XenWorld.src.Model.Puppet.Equipment {
    public enum AccessoryType {
        None,
        Shield,
        Artifact,
        Quiver,
        Relic
    }
    public class PuppetAccessory: EquipmentBase {
        public AccessoryType AccessoryType { get; set; }

        public PuppetAccessory(string name, int defense, AccessoryType type, StatBlock bonus = null, StatBlock requirement = null) : base(name, bonus, requirement, defense)  {
            Defense = defense;
            AccessoryType = type;
        }
    }
}
