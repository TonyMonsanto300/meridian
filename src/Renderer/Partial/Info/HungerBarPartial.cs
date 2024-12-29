using Microsoft.Xna.Framework;
using XenWorld.Config;
using XenWorld.Renderer;
using XenWorld.src.Manager;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class HungerBarPartial {
        public static PrintCursor Render(PrintCursor position, int labelX, int barX, int barWidth) {
            int currentHunger = PlayerManager.Controller.Puppet.Hunger.Current;
            int maxHunger = PlayerManager.Controller.Puppet.Hunger.Max;

            float hungerPercent = (float)currentHunger / maxHunger;

            Color hungerBarColor = new Color(210, 180, 140); // Light brown color (Tan)

            // Draw the label for Hunger
            string hungerLabel = "Hunger";
            Vector2 hungerLabelSize = RendererManager.DefaultFont.MeasureString(hungerLabel);
            float hungerLabelY = position.Y + (RenderConfig.CellSize - hungerLabelSize.Y) / 2;

            RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, hungerLabel, new Vector2(labelX, hungerLabelY), Color.Black);

            // Draw the hunger bar
            int hungerBarY = (int)position.Y;

            // Draw background bar
            Rectangle hungerBarBackgroundRect = new Rectangle(barX, hungerBarY, barWidth, RenderConfig.CellSize);
            RendererManager.SpriteBatch.Draw(
                TextureDictionary.Context["whiteTexture"],
                hungerBarBackgroundRect,
                Color.Gray
            );

            // Draw filled portion
            int hungerFilledWidth = (int)(barWidth * hungerPercent);
            Rectangle hungerBarFilledRect = new Rectangle(barX, hungerBarY, hungerFilledWidth, RenderConfig.CellSize);

            RendererManager.SpriteBatch.Draw(
                TextureDictionary.Context["whiteTexture"],
            hungerBarFilledRect,
                hungerBarColor
            );

            // Draw border around the hunger bar
            InfoRenderService.DrawRectangleBorder(RendererManager.SpriteBatch, new Rectangle(barX, hungerBarY, barWidth, RenderConfig.CellSize), Color.Black);

            // Draw hunger text inside the bar, centered
            string hungerText = $"{currentHunger} / {maxHunger}";
            Vector2 hungerTextSize = RendererManager.DefaultFont.MeasureString(hungerText);
            float hungerTextX = barX + (barWidth - hungerTextSize.X) / 2;
            float hungerTextY = hungerBarY + (RenderConfig.CellSize - hungerTextSize.Y) / 2;

            RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, hungerText, new Vector2(hungerTextX, hungerTextY), Color.Black);

            position.Y += RenderConfig.CellSize + InfoRendererConfig.ResourceBarPadding; // Move down for the next bar
            return position;
        }
    }
}
