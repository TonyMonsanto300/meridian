using System.Linq;
using Microsoft.Xna.Framework.Input;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model.Puppet.Ability;

namespace XenWorld.src.Controller.Player {
    public static class KeyValidator {
        public static bool IsValidActionKey(Keys key, InteractionMode currentMode) {
            // Check if the key exists in the keyToSpellIndex dictionary
            if (KeyMapper.NumberKeys.ContainsKey(key)) {
                // Check if the spell index is within the valid range of available spells
                int spellIndex = KeyMapper.NumberKeys[key]-1;
                if (currentMode == InteractionMode.Cast) {
                    if (spellIndex <= PlayerManager.Controller.Puppet.Abilities.Where(ability => ability.Class == AbilityClass.Skill).Count()) {
                        return true;
                    }
                } else if (currentMode == InteractionMode.Skill) {
                    if (spellIndex <= PlayerManager.Controller.Puppet.Abilities.Where(ability => ability.Class == AbilityClass.Skill).Count()) {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
