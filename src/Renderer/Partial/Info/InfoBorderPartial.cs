using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.Renderer;
using XenWorld.src.Manager;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer.Partial.Info {
    public class InfoBorderPartial {
        public static void Render(SpriteBatch spriteBatch) {
            int borderWidth = 2; // Width of the border (adjust as needed)
            Rectangle leftBorderRect = new Rectangle(InfoRendererConfig.LeftBorder, InfoRendererConfig.TopBorder, borderWidth, InfoRendererConfig.Height);

            spriteBatch.Draw(
                TextureDictionary.Context["blackTexture"],
                leftBorderRect,
                Color.Black
            );
        }
    }
}
