using Microsoft.Xna.Framework;
using XenWorld.Model;
using XenWorld.src.Manager;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer.Widget {
    public static class HealthBarWidget {
        public static void DrawHealthBar(Puppet occupant, Rectangle barArea) {
            if (occupant.Health == null || occupant.Health.Max == 0) return;

            float healthPercent = (float)occupant.Health.Current / occupant.Health.Max;
            Color barColor = healthPercent > 0.5f ? Color.Green : healthPercent > 0.25f ? Color.Yellow : Color.Red;
            int healthBarWidth = (int)(barArea.Width * healthPercent);

            RendererManager.SpriteBatch.Draw(TextureDictionary.Context["blackTexture"], barArea, Color.Black);
            RendererManager.SpriteBatch.Draw(TextureDictionary.Context["whiteTexture"], new Rectangle(barArea.X, barArea.Y, healthBarWidth, barArea.Height), barColor);
        }
    }
}
