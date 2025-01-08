using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Input;

namespace XenWorld.src.Controller.Player {
    public static class KeyMapper {
        public static readonly Dictionary<Keys, int> NumberKeys = new Dictionary<Keys, int> {
            { Keys.D1, 1 },
            { Keys.D2, 2 },
            { Keys.D3, 3 },
            { Keys.D4, 4 },
            { Keys.D5, 5 },
            { Keys.D6, 6 },
            { Keys.D7, 7 },
            { Keys.D8, 8 },
            { Keys.D9, 9 }
        };
    }
}
