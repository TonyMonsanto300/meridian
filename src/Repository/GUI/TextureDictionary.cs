using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace XenWorld.src.Repository.GUI {
    internal class TextureDictionary {
        public static Dictionary<string, Texture2D> Context { get; private set; } = new Dictionary<string, Texture2D>();

        public static void LoadTexture(string name, Texture2D texture) {
            Context.Add(name, texture);
        }
    }
}
