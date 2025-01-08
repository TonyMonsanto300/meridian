namespace XenWorld.src.Model.Puppet.Ability {
    public enum AbilityClass {
        Spell,
        Skill,
        Miracle,
        Wish,
        Omen,
        Power,
        Song,
        Dance,
        Summon,
        Curse
    }

    public class Ability {
        public string Name { get; set; }
        public string Description { get; set; }
        public AbilityClass Class { get; set; }
        public AbilityCost Cost { get; set; }

        public Ability(string name, string description, AbilityClass abilityClass, AbilityCost cost) {
            Name = name;
            Description = description;
            Class = abilityClass;
            Cost = cost;
        }
    }


    public enum SkillClass {
        Martial,
        Survival,
        Natural,
        Tactic,
        Technical
    }

    public class Skill : Ability {
        public SkillClass Class { get; set; }

        public Skill(string name, string description, ResourceTypeEnum abilityType, int abilityCost, SkillClass skillClass)
            : base(name, description, AbilityClass.Skill, new AbilityCost(abilityType, abilityCost)) {
            Class = skillClass;
        }
    }

    public enum SpellSchool {
        Common, // Known as "Common Magic" is the main school of magic
        Blood
    }

    public class Spell : Ability {
        public SpellSchool School { get; set; }

        public Spell(string name, string description, ResourceTypeEnum abilityType, int abilityCost, SpellSchool spellSchool)
            : base(name, description, AbilityClass.Spell, new AbilityCost(abilityType, abilityCost)) {
            School = spellSchool;
        }
    }

    public enum SongKey {
        Ionian,
        Dorian,
        Phrygian,
        Lydian,
        Mixolydian,
        Aeolian,
        Locrian
    }

    public class Song : Ability {
        public SongKey Key { get; set; }

        public Song(string name, string description, ResourceTypeEnum abilityType, int abilityCost, SongKey key)
            : base(name, description, AbilityClass.Song, new AbilityCost(abilityType, abilityCost)) {
            Key = key;
        }
    }

    public class AbilityCost {
        public ResourceTypeEnum Type { get; set; }
        public int Value { get; set; }

        public AbilityCost(ResourceTypeEnum type, int value) {
            Type = type;
            Value = value;
        }
    }
}
