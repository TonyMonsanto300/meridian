using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenWorld.src.Model.Puppet.Stats {
    public class Health {
        public int Max = 0;
        public int Current = 0;
        public Health(int max = 5) {
            Max = max;
            Current = max;
        }
    }
}
