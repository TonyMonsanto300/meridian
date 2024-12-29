using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Model.Map;

namespace XenWorld.src.Repository.Map {
    internal class SpriteDictionary {
        public static Dictionary<string, Texture2D> Context { get; private set; } = new Dictionary<string, Texture2D>();

        public static void LoadSprite(string name, Texture2D texture, bool obstacle = false) {
            Context.Add(name, texture);
        }
    }
}
