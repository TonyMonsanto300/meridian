using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.Renderer;
using XenWorld.src.Manager;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class PlayerNamePartial {
        public static PrintCursor Render(PrintCursor position) {
            position.X = InfoRendererConfig.LeftBorder + (RenderConfig.InfoViewPortX * RenderConfig.CellSize - RendererManager.DefaultFont.MeasureString(PlayerManager.Controller.Puppet.Name).X) / 2;
            position.Y = 10;

            RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, PlayerManager.Controller.Puppet.Name, new Vector2(position.X, position.Y), Color.Black);
            return position;
        }
    }
}
