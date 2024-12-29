using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenWorld.src.Model.Puppet {
    public enum PuppetDirection {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest,
        None
    }

    public enum InteractionMode {
        None,
        Attack,
        Scan,
        Mine,
        Cast,
        Skill
    }
}
