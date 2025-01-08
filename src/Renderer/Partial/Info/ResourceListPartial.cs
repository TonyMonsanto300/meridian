using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.Model;
using XenWorld.Renderer;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class ResourceListPartial {
        public static PrintCursor Render(PrintCursor position, int labelX, int barX, int barWidth) {
            foreach (var resource in PlayerManager.Controller.Puppet.Resources) {
                // Draw the label for the resource
                string resourceLabel = resource.Type.ToString();
                Vector2 resourceLabelSize = RendererManager.DefaultFont.MeasureString(resourceLabel);
                float resourceLabelY = position.Y + (RenderConfig.CellSize - resourceLabelSize.Y) / 2;

                RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, resourceLabel, new Vector2(labelX, resourceLabelY), Color.Black);

                // Draw the resource bar
                int resourceBarY = (int)position.Y;

                // Draw background bar
                Rectangle resourceBarBackgroundRect = new Rectangle(barX, resourceBarY, barWidth, RenderConfig.CellSize);
                RendererManager.SpriteBatch.Draw(
                    TextureDictionary.Context["whiteTexture"],
                    resourceBarBackgroundRect,
                    Color.Gray
                );

                // Draw filled portion
                float resourcePercent = (float)resource.Current / resource.Max;
                int resourceFilledWidth = (int)(barWidth * resourcePercent);
                Rectangle resourceBarFilledRect = new Rectangle(barX, resourceBarY, resourceFilledWidth, RenderConfig.CellSize);

                Color resourceBarColor = GetResourceColor(resource.Type);
                RendererManager.SpriteBatch.Draw(
                    TextureDictionary.Context["whiteTexture"],
                    resourceBarFilledRect,
                    resourceBarColor
                );

                // Draw border around the resource bar
                InfoRenderService.DrawRectangleBorder(RendererManager.SpriteBatch, new Rectangle(barX, resourceBarY, barWidth, RenderConfig.CellSize), Color.Black);

                // **Draw notches for Combo resource**
                if (resource.Type == ResourceTypeEnum.Combo && resource.Max > 1) {
                    int notchCount = resource.Max - 1;
                    float notchSpacing = (float)barWidth / resource.Max;
                    for (int i = 1; i <= notchCount; i++) {
                        float notchX = barX + i * notchSpacing;
                        RendererManager.SpriteBatch.Draw(
                            TextureDictionary.Context["blackTexture"],
                            new Rectangle((int)notchX, resourceBarY, 1, RenderConfig.CellSize),
                            Color.Black
                        );
                    }
                }

                // **Do not draw numbers inside Combo resource bar**
                if (resource.Type != ResourceTypeEnum.Combo) {
                    // Draw resource text inside the bar, centered
                    string resourceText = $"{resource.Current} / {resource.Max}";
                    Vector2 resourceTextSize = RendererManager.DefaultFont.MeasureString(resourceText);
                    float resourceTextX = barX + (barWidth - resourceTextSize.X) / 2;
                    float resourceTextY = resourceBarY + (RenderConfig.CellSize - resourceTextSize.Y) / 2;

                    Color textColor = GetContrastingColor(resourceBarColor);

                    RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, resourceText, new Vector2(resourceTextX, resourceTextY), textColor);
                }

                position.Y += RenderConfig.CellSize + InfoRendererConfig.ResourceBarPadding; // Move down for the next resource
            }
            return position;
        }


        private static Color GetResourceColor(ResourceTypeEnum resourceType) {
            switch (resourceType) {
                case ResourceTypeEnum.Mana:
                    return Color.Blue;
                case ResourceTypeEnum.Energy:
                    return Color.Orange;
                case ResourceTypeEnum.Prayer:
                    return Color.Cyan;
                case ResourceTypeEnum.Miasma:
                    return Color.MediumPurple; // Light Purple
                case ResourceTypeEnum.Combo:
                    return Color.Yellow;
                case ResourceTypeEnum.Fury:
                    return Color.Red;
                case ResourceTypeEnum.Rune:
                    return Color.Magenta;
                default:
                    return Color.White;
            }
        }

        // Helper method to get contrasting text color
        private static Color GetContrastingColor(Color backgroundColor) {
            // Simple algorithm to determine if white or black text would contrast better
            double luminance = (0.299 * backgroundColor.R + 0.587 * backgroundColor.G + 0.114 * backgroundColor.B) / 255;

            if (luminance > 0.5)
                return Color.Black; // bright colors - black font
            else
                return Color.White; // dark colors - white font
        }
    }
}
