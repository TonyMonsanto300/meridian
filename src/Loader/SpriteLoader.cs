using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.src.Repository.Map;

namespace XenWorld.Loader {
    internal class SpriteLoader {
        private static List<string> sprites = new List<string>() {
            "assistant",
            "bandit_1",
            "bandit_2",
            "cursor",
            "enemy",
            "merchant",
            "peasant",
            "player",
            "taskmaster",
            "thrall"
        };
        public static void SeedSprites(ContentManager content) {
            sprites.ForEach(sprite => {
                Texture2D spriteTexture = content.Load<Texture2D>(sprite);
                SpriteDictionary.LoadSprite(spriteTexture.Name, spriteTexture);
            });
        }
    }
}
