using Microsoft.Xna.Framework;
using XenWorld.Config;
using XenWorld.Renderer;
using XenWorld.src.Manager;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class HealthBarPartial {
        public static PrintCursor Render(PrintCursor position, int labelX, int barX, int barWidth) {
            int currentHealth = PlayerManager.Controller.Puppet.Health.Current;
            int maxHealth = PlayerManager.Controller.Puppet.Health.Max;

            float healthPercent = (float)currentHealth / maxHealth;

            Color healthBarColor;
            if (healthPercent > 0.5f) {
                healthBarColor = Color.Lime; // Bright green
            } else if (healthPercent > 0.25f) {
                healthBarColor = Color.Yellow;
            } else {
                healthBarColor = Color.Red;
            }

            // Draw the label for Health
            string healthLabel = "Health";
            Vector2 healthLabelSize = RendererManager.DefaultFont.MeasureString(healthLabel);
            float healthLabelY = position.Y + (RenderConfig.CellSize - healthLabelSize.Y) / 2;

            RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, healthLabel, new Vector2(labelX, healthLabelY), Color.Black);

            // Draw the health bar
            int healthBarY = (int)position.Y;

            // Draw background bar
            Rectangle healthBarBackgroundRect = new Rectangle(barX, healthBarY, barWidth, RenderConfig.CellSize);
            RendererManager.SpriteBatch.Draw(
                TextureDictionary.Context["whiteTexture"],
                healthBarBackgroundRect,
                Color.Gray
            );

            // Draw filled portion
            int healthFilledWidth = (int)(barWidth * healthPercent);
            Rectangle healthBarFilledRect = new Rectangle(barX, healthBarY, healthFilledWidth, RenderConfig.CellSize);

            RendererManager.SpriteBatch.Draw(
                TextureDictionary.Context["whiteTexture"],
                healthBarFilledRect,
                healthBarColor
            );

            // Draw border around the health bar
            InfoRenderService.DrawRectangleBorder(RendererManager.SpriteBatch, new Rectangle(barX, healthBarY, barWidth, RenderConfig.CellSize), Color.Black);

            // Draw health text inside the bar, always in black
            string healthText = $"{currentHealth} / {maxHealth}";
            Vector2 healthTextSize = RendererManager.DefaultFont.MeasureString(healthText);
            float healthTextX = barX + (barWidth - healthTextSize.X) / 2;
            float healthTextY = healthBarY + (RenderConfig.CellSize - healthTextSize.Y) / 2;

            RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, healthText, new Vector2(healthTextX, healthTextY), Color.Black);

            position.Y += RenderConfig.CellSize + InfoRendererConfig.ResourceBarPadding; // Move down for the next bar
            return position;
        }
    }
}
