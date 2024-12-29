using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace XenWorld.Repository.GUI {
    internal class FontRepository {
        public static Dictionary<string, SpriteFont> Context { get; private set; } = new Dictionary<string, SpriteFont>();

        public static void LoadFont(string name, SpriteFont font) {
            Context.Add(name, font);
        }
    }
}
