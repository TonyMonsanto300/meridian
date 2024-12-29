using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XenWorld.Factory.Map;
using XenWorld.Model.Map;

namespace XenWorld.src.Manager {
    internal class MapManager {
        public static ZoneMap ActiveMap = null;
        public static void SetDefaultMap() {
            ActiveMap = MapFactory.GetDefaultMap();
        }
    }
}
