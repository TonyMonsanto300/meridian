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

namespace XenWorld.src.Renderer.Partial.Info {
    public static class StatsSectionPartial {
        public static PrintCursor Render(PrintCursor position) {
            string statsHeader = "Stats";
            Vector2 statsHeaderSize = RendererManager.DefaultFont.MeasureString(statsHeader);
            float statsHeaderX = InfoRendererConfig.LeftBorder + (RenderConfig.InfoViewPortX * RenderConfig.CellSize - statsHeaderSize.X) / 2;
            float statsHeaderY = position.Y + 5; // Adjusted position

            RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, statsHeader, new Vector2(statsHeaderX, statsHeaderY), Color.Black);

            // Draw the 2x3 grid of stats with alignment and boxes
            // Positions and spacing
            float gridStartY = statsHeaderY + statsHeaderSize.Y + 10; // Starting Y position for the grid
            float rowSpacing = 40; // Vertical spacing between rows (adjusted for box height)

            // Dimensions for the stat boxes
            float boxWidth = (RenderConfig.InfoViewPortX * RenderConfig.CellSize) / 2 - 20; // Subtract padding
            float boxHeight = 30; // Height of each stat box

            // Left and right column X positions
            float leftColumnX = InfoRendererConfig.LeftBorder + 10; // Left padding
            float rightColumnX = InfoRendererConfig.LeftBorder + (RenderConfig.InfoViewPortX * RenderConfig.CellSize) / 2 + 10; // Middle + padding

            // Define the stats
            var leftStats = new (string Label, int Value)[] {
                ("STR", PlayerManager.Controller.Puppet.Stats.Strength),
                ("MND", PlayerManager.Controller.Puppet.Stats.Mind),
                ("SKL", PlayerManager.Controller.Puppet.Stats.Skill),
                ("DEF", PlayerManager.Controller.Puppet.Defense) // Use the Defense property we just verified
            };

            var rightStats = new (string Label, int Value)[] {
                ("AGI", PlayerManager.Controller.Puppet.Stats.Agility),
                ("SPR", PlayerManager.Controller.Puppet.Stats.Spirit),
                ("FVR", PlayerManager.Controller.Puppet.Stats.Favor),
                ("SPD", PlayerManager.Controller.Puppet.Speed)
            };

            // Calculate maximum label widths
            float maxLabelWidthLeft = 0f;
            foreach (var stat in leftStats) {
                var labelSize = RendererManager.DefaultFont.MeasureString(stat.Label);
                if (labelSize.X > maxLabelWidthLeft) {
                    maxLabelWidthLeft = labelSize.X;
                }
            }

            float maxLabelWidthRight = 0f;
            foreach (var stat in rightStats) {
                var labelSize = RendererManager.DefaultFont.MeasureString(stat.Label);
                if (labelSize.X > maxLabelWidthRight) {
                    maxLabelWidthRight = labelSize.X;
                }
            }

            // Define the position where the value will be drawn (same for each stat)
            float valueOffsetLeft = leftColumnX + maxLabelWidthLeft + 10; // 10 pixels padding
            float valueOffsetRight = rightColumnX + maxLabelWidthRight + 10; // 10 pixels padding

            // Draw left column stats
            for (int i = 0; i < leftStats.Length; i++) {
                // Rectangle for the stat box
                Rectangle statBox = new Rectangle(
                    (int)leftColumnX,
                    (int)(gridStartY + i * rowSpacing),
                (int)boxWidth,
                    (int)boxHeight
                );

                // Draw the box background
                RendererManager.SpriteBatch.Draw(
                    TextureDictionary.Context["whiteTexture"],
                statBox,
                    Color.LightGray
                );

                // Draw the box border
                InfoRenderService.DrawRectangleBorder(RendererManager.SpriteBatch, statBox, Color.Black);

                // Draw the vertical line between label and value
                float lineX = valueOffsetLeft - 5; // Slightly before the value
                RendererManager.SpriteBatch.Draw(
                    TextureDictionary.Context["blackTexture"],
                    new Rectangle((int)lineX, statBox.Y, 1, statBox.Height),
                    Color.Black
                );

                // Draw the label
                string labelText = leftStats[i].Label;
                Vector2 labelPosition = new Vector2(leftColumnX + 5, statBox.Y + (boxHeight - RendererManager.DefaultFont.LineSpacing) / 2);
                RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, labelText, labelPosition, Color.Black);

                // Draw the value aligned
                string valueText = leftStats[i].Value.ToString();
                Vector2 valuePosition = new Vector2(valueOffsetLeft, statBox.Y + (boxHeight - RendererManager.DefaultFont.LineSpacing) / 2);
                RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, valueText, valuePosition, Color.Black);
            }

            // Draw right column stats
            for (int i = 0; i < rightStats.Length; i++) {
                // Rectangle for the stat box
                Rectangle statBox = new Rectangle(
                    (int)rightColumnX,
                    (int)(gridStartY + i * rowSpacing),
                (int)boxWidth,
                    (int)boxHeight
                );

                // Draw the box background
                RendererManager.SpriteBatch.Draw(
                    TextureDictionary.Context["whiteTexture"],
                statBox,
                    Color.LightGray
                );

                // Draw the box border
                InfoRenderService.DrawRectangleBorder(RendererManager.SpriteBatch, statBox, Color.Black);

                // Draw the vertical line between label and value
                float lineX = valueOffsetRight - 5; // Slightly before the value
                RendererManager.SpriteBatch.Draw(
                    TextureDictionary.Context["blackTexture"],
                    new Rectangle((int)lineX, statBox.Y, 1, statBox.Height),
                    Color.Black
                );

                // Draw the label
                string labelText = rightStats[i].Label;
                Vector2 labelPosition = new Vector2(rightColumnX + 5, statBox.Y + (boxHeight - RendererManager.DefaultFont.LineSpacing) / 2);
                RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, labelText, labelPosition, Color.Black);

                // Draw the value aligned
                string valueText = rightStats[i].Value.ToString();
                Vector2 valuePosition = new Vector2(valueOffsetRight, statBox.Y + (boxHeight - RendererManager.DefaultFont.LineSpacing) / 2);
                RendererManager.SpriteBatch.DrawString(RendererManager.DefaultFont, valueText, valuePosition, Color.Black);
            }

            // **Update currentY after drawing stats**
            float gridHeight = (rowSpacing * leftStats.Length); // Total height of the stats grid
            position.Y = gridStartY + gridHeight; // Update currentY to position below the stats grid
            return position;
        }
    }
}
