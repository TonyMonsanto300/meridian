using System.Collections.Generic;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model.Puppet.Ability;
using XenWorld.src.Repository.Puppet;

namespace XenWorld.src.Loader.Puppet {
    public class AbilitySetLoader {
        public AbilitySetLoader() { }

        public static void SeedAbilitySets() {
            AbilitySetDictionary.LoadAbilitySet(AbilitySetEnum.PLAYER, new List<Ability>() {
                new Spell("Fireball", "A ball of fire.", ResourceTypeEnum.Mana, 5, SpellSchool.Common),
                new Spell("Blood Surge", "A surge of blood magic.", ResourceTypeEnum.Mana, 7, SpellSchool.Blood),
                new Skill("Power Strike", "A powerful strike.", ResourceTypeEnum.Combo, 5, SkillClass.Martial),
                new Skill("Tracking", "A survival skill for tracking.", ResourceTypeEnum.Energy, 3, SkillClass.Survival),
                new Song("Hymn of Valor", "A song of courage.", ResourceTypeEnum.Prayer, 4, SongKey.Dorian)
            });

            AbilitySetDictionary.LoadAbilitySet(AbilitySetEnum.DEFAULT, new List<Ability>() { });
        }
    }
}