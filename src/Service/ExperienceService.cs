using System.Collections.Generic;
using XenWorld.Model;

namespace XenWorld.src.Service {
    public static class ExperienceService {
        public static Dictionary<int, int> LevelTable = new Dictionary<int, int>() {
            {1, 0},
            {2, 100},
            {3, 300},
            {4, 600},
            {5, 1000},
            {6, 1500},
            {7, 2100},
            {8, 2800},
            {9, 3600},
            {10, 4500}
        };

        public static int GetExpToNextLevel(Puppet puppet) {
            if(puppet.Level == 10) {
                return 0;
            }
            return LevelTable[puppet.Level+1] - puppet.Experience;
        }

        public static int GetNextLevelRequirement(Puppet puppet) {
            if (puppet.Level < 1 || puppet.Level >= 10) {
                return 0;
            }
            return LevelTable[puppet.Level+1] - LevelTable[puppet.Level];
        }

        public static int GetExpProgress(Puppet puppet) {
            if(puppet.Level >= 10) {
                return 0;
            }
            return puppet.Experience - LevelTable[puppet.Level];
        }

        public static float GetPercentToNextLevel(Puppet puppet) {
            if (puppet.Level == 10) {
                return 100f;
            }
            float progressExp = GetExpProgress(puppet);
            float nextLevelExp = GetNextLevelRequirement(puppet);
            return progressExp / nextLevelExp;
        }
    }
}
