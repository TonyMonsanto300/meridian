using Microsoft.Xna.Framework;
using XenWorld.Config;
using XenWorld.Repository.GUI;
using XenWorld.src.Manager;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer {
    static class GameOverScreen {
        public static void DrawScreen() {
            RendererManager.SpriteBatch.Draw(TextureDictionary.Context["blackTexture"], new Rectangle(0, 0, RenderConfig.MapViewPortX * RenderConfig.CellSize, RenderConfig.MapViewPortY * RenderConfig.CellSize), Color.Black);

            string gameOverText = "Game Over\nThanks for playing the demo!";
            Vector2 textSize = FontRepository.Context["default"].MeasureString(gameOverText);
            Vector2 textPosition = new Vector2((RenderConfig.MapViewPortX * RenderConfig.CellSize - textSize.X) / 2, (RenderConfig.MapViewPortY * RenderConfig.CellSize - textSize.Y) / 2);

            RendererManager.SpriteBatch.DrawString(FontRepository.Context["default"], gameOverText, textPosition, Color.White);
        }
    }
}
