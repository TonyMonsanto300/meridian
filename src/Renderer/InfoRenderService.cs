using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer {
    internal class InfoRenderService {
        public static void DrawRectangleBorder(SpriteBatch spriteBatch, Rectangle rect, Color color) {
            // Top border
            spriteBatch.Draw(
                TextureDictionary.Context["blackTexture"],
                new Rectangle(rect.X, rect.Y, rect.Width, 1),
                color
            );
            // Bottom border
            spriteBatch.Draw(
                TextureDictionary.Context["blackTexture"],
                new Rectangle(rect.X, rect.Y + rect.Height - 1, rect.Width, 1),
                color
            );
            // Left border
            spriteBatch.Draw(
                TextureDictionary.Context["blackTexture"],
                new Rectangle(rect.X, rect.Y, 1, rect.Height),
                color
            );
            // Right border
            spriteBatch.Draw(
                TextureDictionary.Context["blackTexture"],
                new Rectangle(rect.X + rect.Width - 1, rect.Y, 1, rect.Height),
                color
            );
        }
    }
}
