using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Model.Map;
using XenWorld.src.Manager;
using XenWorld.src.Model;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model.Puppet.Ability;
using XenWorld.src.Model.Puppet.Equipment;
using XenWorld.src.Model.Puppet.Stats;
using XenWorld.src.Service;

namespace XenWorld.Model {
    public enum AmmoClass {
        Arrow,
        Javelin,
        Shuriken
    }

    public class MissileAmmo {
        public AmmoClass Class;
        public string Name { get; set; }
        public string Description { get; set; }
        public int HitBonus { get; set; }
        public int DamageBonus { get; set; }

        public MissileAmmo(string name, string description="", int hitBonus=0, int damageBonus=0, AmmoClass ammoClass = AmmoClass.Arrow) {
            Class = ammoClass;
            Name = name; 
            Description = description;
            HitBonus = hitBonus;
            DamageBonus = damageBonus;
        }
    }
    public class ThrownWeapon : MissileAmmo {
        Dice DamageDie { get; set; }

        public ThrownWeapon(string name, Dice damageDie, AmmoClass ammoClass, string description = "", int hitBonus = 0, int damageBonus = 0) : base(name, description, hitBonus, damageBonus, ammoClass) {
            Name = name;
            Description = description;
            HitBonus = hitBonus;
            DamageBonus = damageBonus;
            DamageDie = damageDie;
        }
    }

    public class Quiver : PuppetAccessory {
        MissileAmmo QuiverType { get; set; }
        int Capacity { get; set; }
        int Current { get; set; }

        public Quiver(string name, MissileAmmo quiverType, int max, bool fill=true) : base(name, 0, AccessoryType.Quiver, null, null) {
            QuiverType = quiverType;
            Capacity = max;
            if(fill) {
                Current = Capacity;
            } else {
                Current = 0;
            }
        }
    }

    public class Puppet {
        // Personalization
        public string Name { get; set; }
        public Texture2D Sprite { get; set; }
        // Identity
        public PuppetClass Class { get; set; }
        public int Level { get; set; } = 1;
        public int Experience { get; set; } = 0;
        // Stats
        public StatBlock Stats { get; set; } = new StatBlock();
        public int Speed = 3;
        // Resources
        public Health Health { get; set; }
        public Hunger Hunger { get; set; }
        public Dictionary<string, PuppetResource> Resources = new Dictionary<string, PuppetResource>();
        // Abilities
        public List<Ability> Abilities { get; set; }
        // Location
        public Coordinate Coordinate { get; set; }
        // Property
        public Building Home { get; set; }
        // Weapons
        public int WeaponSlots { get; set; } = 2;
        public List<PuppetWeapon> Weapons { get; set; } = new List<PuppetWeapon>();
        public PuppetWeapon EquippedWeapon { get; set; } = null;
        // Armor
        public PuppetArmor Armor { get; set; } = null;
        // Accessories
        public int AccessorySlots { get; set; } = 3;
        public List<PuppetAccessory> Accessories { get; set; } = new List<PuppetAccessory>();

        public Puppet(string name, Texture2D sprite, PuppetClass puppetClass, int x = -1, int y = -1) {
            Name = name;
            Sprite = sprite;
            Class = puppetClass;
            Health = new Health();
            Hunger = new Hunger();

            Resources.Add("Mana", new PuppetResource(ResourceType.Mana, 10));
            Resources.Add("Energy", new PuppetResource(ResourceType.Energy, 10));
            //Resources.Add("Rune", new PuppetResource(ResourceType.Rune, 10));
            //Resources.Add("Prayer", new PuppetResource(ResourceType.Prayer, 10));

            Abilities = new List<Ability>() {
                new Spell("Fireball", "A ball of fire.", ResourceType.Mana, 5, SpellSchool.Common),
                new Spell("Blood Surge", "A surge of blood magic.", ResourceType.Mana, 7, SpellSchool.Blood),
                new Skill("Power Strike", "A powerful strike.", ResourceType.Combo, 5, SkillClass.Martial),
                new Skill("Tracking", "A survival skill for tracking.", ResourceType.Energy, 3, SkillClass.Survival),
                new Song("Hymn of Valor", "A song of courage.", ResourceType.Prayer, 4, SongKey.Dorian)
            };

            Coordinate = new Coordinate(x, y);
            AddWeapon(new PuppetWeapon(
                "Bronze Dagger",
                new List<Dice> { Dice.D4 },
                0,
                0,
                PuppetWeaponType.Dagger
            ));
            AddWeapon(new PuppetWeapon(
                "Bronze Sword",
                new List<Dice> { Dice.D6 },
                0,
                0,
                PuppetWeaponType.LongSword
            ));

            Armor = new PuppetArmor("Hide Tunic", 1, PuppetArmorClass.Light);
            AddAccessory(new PuppetAccessory("Bronze Shield", 1, AccessoryType.None));
        }

        public void AddWeapon(PuppetWeapon weapon) {
            if (Weapons.Count < WeaponSlots) {
                Weapons.Add(weapon);
                if (EquippedWeapon == null) {
                    EquippedWeapon = weapon;
                }
            }
        }

        public void AddAccessory(PuppetAccessory accessory) {
            if (Accessories.Count < AccessorySlots) {
                Accessories.Add(accessory);
            }
        }

        public void AddExperience(int experience) {
            Experience += experience;
            if (Level <= 10 && Experience >= ExperienceService.LevelTable[Level + 1]) {
                Level++;
                LogRenderer.AddLogMessage($"{Name} has reached level {Level}");
            }
        }

        public int Defense {
            get {
                int armorDefense = (Armor != null) ? Armor.Defense : 0;
                int accessoriesDefense = Accessories.Sum(accessory => accessory.Defense);
                return armorDefense + accessoriesDefense;
            }
        }

        public bool TakeDamage(int damage) {
            if (damage <= Health.Current) {
                Health.Current -= damage;
            } else {
                Health.Current = 0;
            }

            if (Health.Current <= 0) {
                Die();
                return true;
            }
            return false;
        }

        private void Die() {
            if(this != PlayerManager.Controller.Puppet) {
                NPCManager.NPCControllers.Remove(NPCManager.NPCControllers.Where(controller => controller.Puppet == this).First());
            }
            MapManager.ActiveMap.Grid[Coordinate.X, Coordinate.Y].Occupant = null;
        }
    }
}