using System.Linq;
using Microsoft.Xna.Framework.Input;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model.Puppet.Ability;
using System;
using System.Collections.Generic;

namespace XenWorld.src.Controller.Player {
    public static class KeyValidator {
        private static readonly Dictionary<InteractionMode, Func<int>> modeAbilityCountMap =
            new Dictionary<InteractionMode, Func<int>> {
                {
                    InteractionMode.Cast, () => PlayerManager.Controller.Puppet.Abilities.Where(ability => ability.Class == AbilityClass.Spell).Count()
                },
                {
                    InteractionMode.Skill, () => PlayerManager.Controller.Puppet.Abilities.Where(ability => ability.Class == AbilityClass.Skill).Count()
                }
            };

        public static bool IsValidActionKey(Keys key, InteractionMode currentMode) {
            if (KeyMapper.NumberKeys.TryGetValue(key, out int numberPressed)) {
                int spellIndex = numberPressed - 1;

                if (modeAbilityCountMap.TryGetValue(currentMode, out Func<int> abilityCountFunc)) {
                    int validCount = abilityCountFunc();
                    return spellIndex < validCount;
                }
            }

            return false;
        }
    }
}