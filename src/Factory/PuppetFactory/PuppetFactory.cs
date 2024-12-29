using System;
using System.Collections.Generic;
using XenWorld.Model;
using XenWorld.src.Loader.Puppet;
using XenWorld.src.Model.Puppet.Stats;
using XenWorld.src.Repository.Map;
using XenWorld.src.Repository.Puppet;

namespace XenWorld.src.Factory.PuppetFactory {
    public class PuppetFactory {
        private static readonly Random random = new Random();
        public static Puppet CreateBandit(int x, int y) {
            var banditSprites = new List<string>() { "bandit_1", "bandit_2"};
            return new Puppet("Bandit", SpriteDictionary.Context[banditSprites[random.Next(banditSprites.Count)]], PuppetClassDictionary.Context[PuppetClassEnum.Blackguard], x, y);
        }

        // Function to generate the player puppet
        public static Puppet GeneratePlayerPuppet() {
            var playerPuppet = new Puppet("Muwatili", SpriteDictionary.Context["player"], PuppetClassDictionary.Context[PuppetClassEnum.Sentinel]);
            playerPuppet.Health = new Health(30);
            return playerPuppet;
        }
    }
}
