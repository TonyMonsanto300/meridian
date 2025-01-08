using System.Collections.Generic;
using XenWorld.src.Model.Puppet.Ability;

namespace XenWorld.src.Repository.Puppet {
    public enum AbilitySetEnum {
        DEFAULT,
        PLAYER
    }
    public class AbilitySetDictionary {
        public static Dictionary<AbilitySetEnum, List<Ability>> Context = new Dictionary<AbilitySetEnum, List<Ability>>();

        public static void LoadAbilitySet(AbilitySetEnum name, List<Ability> abilitySet) {
            Context.Add(name, abilitySet);
        }
    }
}
