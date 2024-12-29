using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenWorld.src.Model.Puppet {
    public enum ResourceType {
        Mana,
        Energy,
        Prayer,
        Miasma,
        Combo,
        Fury,
        Rune
    }
    public class PuppetResource {
        public ResourceType Type { get; set; }
        public int Max { get; set; } = 0;
        public int Current { get; set; } = 0;

        public PuppetResource(ResourceType type, int max, bool fill = true) {
            Type = type;
            Max = max;
            if (fill) {
                Current = max;
            } else {
                Current = 0;
            }
        }
    }
}
