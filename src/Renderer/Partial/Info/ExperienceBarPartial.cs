using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.Renderer;
using XenWorld.src.Manager;
using XenWorld.src.Repository.GUI;
using XenWorld.src.Service;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class ExperienceBarPartial {
        public static PrintCursor Render(SpriteFont font, SpriteBatch spriteBatch, PrintCursor cursor) {
            int currentExpProgress = ExperienceService.GetExpProgress(PlayerManager.Controller.Puppet);
            int nextLevelExpRequirement = ExperienceService.GetNextLevelRequirement(PlayerManager.Controller.Puppet);
            float percentToNextLevel = ExperienceService.GetPercentToNextLevel(PlayerManager.Controller.Puppet) * 100f; // Convert to percentage

            // Format the experience text to two decimal places
            string experienceText = $"{currentExpProgress}/{nextLevelExpRequirement} ({percentToNextLevel:F2}%)";

            // Define Experience Bar dimensions
            int experienceBarWidth = (int)(RenderConfig.InfoViewPortX * RenderConfig.CellSize - 20); // Full width minus padding
            int experienceBarHeight = 20; // Height of the experience bar
            int experienceBarX = InfoRendererConfig.LeftBorder + 10; // Left padding
            int experienceBarY = (int)(cursor.Y + 20); // Slight padding from the last element

            // **Draw "Experience" Label**
            string experienceLabel = "Experience";
            Vector2 experienceLabelSize = font.MeasureString(experienceLabel);
            float experienceLabelX = InfoRendererConfig.LeftBorder + (RenderConfig.InfoViewPortX * RenderConfig.CellSize - experienceLabelSize.X) / 2;
            float experienceLabelY = cursor.Y;

            spriteBatch.DrawString(font, experienceLabel, new Vector2(experienceLabelX, experienceLabelY), Color.Black);

            // **Draw Experience Bar Background (Gray)**
            Rectangle experienceBarBackground = new Rectangle(
            experienceBarX,
                (int)experienceBarY,
                experienceBarWidth,
                experienceBarHeight
            );

            spriteBatch.Draw(
                TextureDictionary.Context["whiteTexture"],
                experienceBarBackground,
                Color.Gray
            );

            // **Draw Filled Portion of Experience Bar (Yellow)**
            int experienceFilledWidth = nextLevelExpRequirement > 0 ? (int)(experienceBarWidth * ((float)currentExpProgress / nextLevelExpRequirement)) : 0;
            Rectangle experienceBarFilled = new Rectangle(
            experienceBarX,
                (int)experienceBarY,
                experienceFilledWidth,
                experienceBarHeight
            );

            spriteBatch.Draw(
                TextureDictionary.Context["whiteTexture"],
            experienceBarFilled,
                Color.Yellow
            );

            // **Draw Border Around Experience Bar**
            InfoRenderService.DrawRectangleBorder(spriteBatch, experienceBarBackground, Color.Black);

            // **Draw Experience Text Inside the Bar, Centered**
            Vector2 experienceTextSize = font.MeasureString(experienceText);
            Vector2 experienceTextPosition = new Vector2(
                experienceBarX + (experienceBarWidth - experienceTextSize.X) / 2,
                experienceBarY + (experienceBarHeight - experienceTextSize.Y) / 2
            );

            // Choose text color based on bar fill (ensure readability)
            Color experienceTextColor = Color.Black;

            spriteBatch.DrawString(font, experienceText, experienceTextPosition, experienceTextColor);

            // Update position.Y if needed (optional, since it's the last element)
            cursor.Y += experienceBarHeight + 10;
            return cursor;
        }
    }
}
