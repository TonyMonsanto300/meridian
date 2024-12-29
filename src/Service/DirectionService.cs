using System.Collections.Generic;
using XenWorld.src.Model.Puppet;

namespace XenWorld.src.Service {
    public class DirectionService {
        public static readonly Dictionary<PuppetDirection, (int x, int y)> directionToCoordinates = new Dictionary<PuppetDirection, (int x, int y)> {
            { PuppetDirection.North, (1, 0) },
            { PuppetDirection.NorthEast, (2, 0) },
            { PuppetDirection.East, (2, 1) },
            { PuppetDirection.SouthEast, (2, 2) },
            { PuppetDirection.South, (1, 2) },
            { PuppetDirection.SouthWest, (0, 2) },
            { PuppetDirection.West, (0, 1) },
            { PuppetDirection.NorthWest, (0, 0) }
        };
    }
}
