using Microsoft.Xna.Framework;
using XenWorld.Config;
using XenWorld.Renderer;
using XenWorld.src.Manager;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class LevelClassPartial {
        public static PrintCursor Render(PrintCursor cursor) {
            int level = PlayerManager.Controller.Puppet.Level;
            string className = PlayerManager.Controller.Puppet.Class.Name.ToString();

            string levelClassText = $"Lvl. {level} {className}";
            Vector2 levelClassSize = RendererManager.DefaultFont.MeasureString(levelClassText);
            cursor.X = InfoRendererConfig.LeftBorder + (RenderConfig.InfoViewPortX * RenderConfig.CellSize - levelClassSize.X) / 2;
            cursor.Y = cursor.Y + RendererManager.DefaultFont.MeasureString(PlayerManager.Controller.Puppet.Name).Y + 5; // Add some vertical spacing

            RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, levelClassText, new Vector2(cursor.X, cursor.Y), Color.Black);
            cursor.Y += RenderConfig.CellSize;
            return cursor;
        }
    }
}
