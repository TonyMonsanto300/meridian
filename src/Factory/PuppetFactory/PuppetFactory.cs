using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using XenWorld.Model;
using XenWorld.src.Repository.Map;
using XenWorld.src.Repository.Puppet;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Model.Puppet.Ability;
using XenWorld.src.Model.Puppet.Equipment;
using XenWorld.src.Model.Puppet.Stats;
using XenWorld.src.Loader.Puppet;
using XenWorld.src.Model;

namespace XenWorld.src.Factory {
    public static class PuppetFactory {
        private static Random random = new Random();

        public static Puppet CreateVillager(Coordinate location) {
            string name = "Villager";
            Texture2D sprite = SpriteDictionary.Context["peasant"];
            PuppetClass puppetClass = PuppetClassDictionary.Context[PuppetClassEnum.None];
            Health health = new Health(10);
            List<PuppetResource> resources = ResourceSetDictionary.Context[ResourceSetEnum.Default];
            List<Ability> abilities = AbilitySetDictionary.Context[AbilitySetEnum.DEFAULT];
            List<PuppetWeapon> weapons = WeaponSetDictionary.Context[WeaponSetEnum.DEFAULT];
            PuppetArmor armor = ArmorDictionary.Context[ArmorEnum.DEFAULT];
            List<PuppetAccessory> accessories = AccessorySetDictionary.Context[AccessorySetEnum.DEFAULT];
            return new Puppet(name, sprite, puppetClass, health, resources, abilities, weapons, armor, accessories, location);
        }

        public static Puppet CreateShopOwner(Coordinate location) {
            string name = "Shop Owner";
            Texture2D sprite = SpriteDictionary.Context["merchant"];
            PuppetClass puppetClass = PuppetClassDictionary.Context[PuppetClassEnum.None];
            Health health = new Health(10);
            List<PuppetResource> resources = ResourceSetDictionary.Context[ResourceSetEnum.Default];
            List<Ability> abilities = AbilitySetDictionary.Context[AbilitySetEnum.DEFAULT];
            List<PuppetWeapon> weapons = WeaponSetDictionary.Context[WeaponSetEnum.DEFAULT];
            PuppetArmor armor = ArmorDictionary.Context[ArmorEnum.DEFAULT];
            List<PuppetAccessory> accessories = AccessorySetDictionary.Context[AccessorySetEnum.DEFAULT];
            return new Puppet(name, sprite, puppetClass, health, resources, abilities, weapons, armor, accessories, location);
        }

        public static Puppet CreateShopKeeper(Coordinate location) {
            string name = "Shop Keeper";
            Texture2D sprite = SpriteDictionary.Context["assistant"];
            PuppetClass puppetClass = PuppetClassDictionary.Context[PuppetClassEnum.None];
            Health health = new Health(10);
            List<PuppetResource> resources = ResourceSetDictionary.Context[ResourceSetEnum.Default];
            List<Ability> abilities = AbilitySetDictionary.Context[AbilitySetEnum.DEFAULT];
            List<PuppetWeapon> weapons = WeaponSetDictionary.Context[WeaponSetEnum.DEFAULT];
            PuppetArmor armor = ArmorDictionary.Context[ArmorEnum.DEFAULT];
            List<PuppetAccessory> accessories = AccessorySetDictionary.Context[AccessorySetEnum.DEFAULT];
            return new Puppet(name, sprite, puppetClass, health, resources, abilities, weapons, armor, accessories, location);
        }

        public static Puppet CreateTaskMaster(Coordinate location) {
            string name = "Task Master";
            Texture2D sprite = SpriteDictionary.Context["taskmaster"];
            PuppetClass puppetClass = PuppetClassDictionary.Context[PuppetClassEnum.None];
            Health health = new Health(15);
            List<PuppetResource> resources = ResourceSetDictionary.Context[ResourceSetEnum.Default];
            List<Ability> abilities = AbilitySetDictionary.Context[AbilitySetEnum.DEFAULT];
            List<PuppetWeapon> weapons = WeaponSetDictionary.Context[WeaponSetEnum.DEFAULT];
            PuppetArmor armor = ArmorDictionary.Context[ArmorEnum.DEFAULT];
            List<PuppetAccessory> accessories = AccessorySetDictionary.Context[AccessorySetEnum.DEFAULT];
            return new Puppet(name, sprite, puppetClass, health, resources, abilities, weapons, armor, accessories, location);
        }

        public static Puppet CreateThrall(Coordinate location) {
            string name = "Thrall";
            Texture2D sprite = SpriteDictionary.Context["thrall"];
            PuppetClass puppetClass = PuppetClassDictionary.Context[PuppetClassEnum.None];
            Health health = new Health(10);
            List<PuppetResource> resources = ResourceSetDictionary.Context[ResourceSetEnum.Default];
            List<Ability> abilities = AbilitySetDictionary.Context[AbilitySetEnum.DEFAULT];
            List<PuppetWeapon> weapons = WeaponSetDictionary.Context[WeaponSetEnum.DEFAULT];
            PuppetArmor armor = ArmorDictionary.Context[ArmorEnum.DEFAULT];
            List<PuppetAccessory> accessories = AccessorySetDictionary.Context[AccessorySetEnum.DEFAULT];
            return new Puppet(name, sprite, puppetClass, health, resources, abilities, weapons, armor, accessories, location);
        }

        public static Puppet CreateBandit(Coordinate location) {
            string name = "Bandit";
            var banditSprites = new[] { "bandit_1", "bandit_2" };
            string chosenSprite = banditSprites[random.Next(banditSprites.Length)];
            Texture2D sprite = SpriteDictionary.Context[chosenSprite];
            PuppetClass puppetClass = PuppetClassDictionary.Context[PuppetClassEnum.None];
            Health health = new Health(10);
            List<PuppetResource> resources = ResourceSetDictionary.Context[ResourceSetEnum.Default];
            List<Ability> abilities = AbilitySetDictionary.Context[AbilitySetEnum.DEFAULT];
            List<PuppetWeapon> weapons = WeaponSetDictionary.Context[WeaponSetEnum.DEFAULT];
            PuppetArmor armor = ArmorDictionary.Context[ArmorEnum.DEFAULT];
            List<PuppetAccessory> accessories = AccessorySetDictionary.Context[AccessorySetEnum.DEFAULT];
            return new Puppet(name, sprite, puppetClass, health, resources, abilities, weapons, armor, accessories, location);
        }

        public static Puppet CreatePlayer(Coordinate location) {
            string name = "Player";
            Texture2D sprite = SpriteDictionary.Context["player"];
            PuppetClass puppetClass = PuppetClassDictionary.Context[PuppetClassEnum.None];
            Health health = new Health(30);
            List<PuppetResource> resources = ResourceSetDictionary.Context[ResourceSetEnum.Default];
            List<Ability> abilities = AbilitySetDictionary.Context[AbilitySetEnum.PLAYER];
            List<PuppetWeapon> weapons = WeaponSetDictionary.Context[WeaponSetEnum.PLAYER];
            PuppetArmor armor = ArmorDictionary.Context[ArmorEnum.PLAYER];
            List<PuppetAccessory> accessories = AccessorySetDictionary.Context[AccessorySetEnum.PLAYER];
            return new Puppet(name, sprite, puppetClass, health, resources, abilities, weapons, armor, accessories, location);
        }
    }
}
