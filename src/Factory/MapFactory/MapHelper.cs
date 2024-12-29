using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenWorld.src.Factory.MapFactory {
    public static class MapHelper {
        public static IEnumerable<(int x, int y)> GetNeighbors(int x, int y) {
            yield return (x + 1, y);
            yield return (x - 1, y);
            yield return (x, y + 1);
            yield return (x, y - 1);
        }
    }
}
