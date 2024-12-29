using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Repository.GUI;

namespace XenWorld.src.Loader {
    internal class FontLoader {
        public static void SeedFonts(ContentManager content) {
            SpriteFont defaultFont = content.Load<SpriteFont>("rpg-font");
            FontRepository.LoadFont("default", defaultFont);
        }
    }
}
