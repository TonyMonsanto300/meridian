using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.Renderer;
using XenWorld.src.Manager;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class AccessoriesSectionPartial {
        public static PrintCursor Render(SpriteFont font, SpriteBatch spriteBatch, PrintCursor cursor) {
            string accessoriesHeader = "Accessories";
            Vector2 accessoriesHeaderSize = font.MeasureString(accessoriesHeader);
            float accessoriesHeaderX = InfoRendererConfig.LeftBorder +
                (RenderConfig.InfoViewPortX * RenderConfig.CellSize - accessoriesHeaderSize.X) / 2;
            float accessoriesHeaderY = cursor.Y;

            // Draw "Accessories" Header
            spriteBatch.DrawString(font, accessoriesHeader, new Vector2(accessoriesHeaderX, accessoriesHeaderY), Color.Black);

            // Move cursor down for accessory slots
            cursor.Y += accessoriesHeaderSize.Y + 5; // 5 pixels padding

            // **Positions and Dimensions for Accessory Slots**
            int accessoryCount = PlayerManager.Controller.Puppet.AccessorySlots;
            int maxAccessorySlots = 4; // Maximum slots allowed
            int displayCount = Math.Min(accessoryCount, maxAccessorySlots);
            int accessorySlotWidth = 120;
            int accessorySlotHeight = 30; // Height of each accessory box
            int spacing = 10; // Spacing between boxes

            List<Vector2> positions = new List<Vector2>();

            if (displayCount == 1) {
                // **1 Accessory: Centered**
                float x = InfoRendererConfig.LeftBorder +
                    (RenderConfig.InfoViewPortX * RenderConfig.CellSize - accessorySlotWidth) / 2;
                float y = cursor.Y;
                positions.Add(new Vector2(x, y));
            } else if (displayCount == 2) {
                // **2 Accessories: Centered and Spaced Equally**
                float totalWidth = accessorySlotWidth * 2 + spacing;
                float startX = InfoRendererConfig.LeftBorder +
                    (RenderConfig.InfoViewPortX * RenderConfig.CellSize - totalWidth) / 2;
                float y = cursor.Y;
                positions.Add(new Vector2(startX, y));
                positions.Add(new Vector2(startX + accessorySlotWidth + spacing, y));
            } else if (displayCount == 3) {
                // **3 Accessories: Two on First Row, One Centered Below**
                // First Row
                float firstRowTotalWidth = accessorySlotWidth * 2 + spacing;
                float firstRowStartX = InfoRendererConfig.LeftBorder +
                    (RenderConfig.InfoViewPortX * RenderConfig.CellSize - firstRowTotalWidth) / 2;
                float firstRowY = cursor.Y;
                positions.Add(new Vector2(firstRowStartX, firstRowY));
                positions.Add(new Vector2(firstRowStartX + accessorySlotWidth + spacing, firstRowY));

                // Second Row (Centered)
                float secondRowStartX = InfoRendererConfig.LeftBorder +
                    (RenderConfig.InfoViewPortX * RenderConfig.CellSize - accessorySlotWidth) / 2;
                float secondRowY = cursor.Y + accessorySlotHeight + spacing;
                positions.Add(new Vector2(secondRowStartX, secondRowY));
            } else if (displayCount >= 4) {
                // **4 Accessories: Two Rows of Two**
                float rowTotalWidth = accessorySlotWidth * 2 + spacing;
                float startX = InfoRendererConfig.LeftBorder +
                    (RenderConfig.InfoViewPortX * RenderConfig.CellSize - rowTotalWidth) / 2;
                float firstRowY = cursor.Y;
                float secondRowY = cursor.Y + accessorySlotHeight + spacing;
                positions.Add(new Vector2(startX, firstRowY));
                positions.Add(new Vector2(startX + accessorySlotWidth + spacing, firstRowY));
                positions.Add(new Vector2(startX, secondRowY));
                positions.Add(new Vector2(startX + accessorySlotWidth + spacing, secondRowY));
            }

            // **Draw Accessory Boxes**
            for (int i = 0; i < displayCount; i++) {
                Vector2 pos = positions[i];
                Rectangle accessoryBox = new Rectangle(
                    (int)pos.X,
                    (int)pos.Y,
                    accessorySlotWidth,
                    accessorySlotHeight
                );

                // Draw the box background
                spriteBatch.Draw(
                    TextureDictionary.Context["whiteTexture"],
                    accessoryBox,
                    Color.LightGray
                );

                // Draw the box border
                InfoRenderService.DrawRectangleBorder(spriteBatch, accessoryBox, Color.Black);

                // **Draw Accessory Name if Available**
                if (i < PlayerManager.Controller.Puppet.Accessories.Count) {
                    string accessoryName = PlayerManager.Controller.Puppet.Accessories[i].Name;
                    Vector2 accessoryNameSize = font.MeasureString(accessoryName);
                    float accessoryNameX = pos.X + (accessorySlotWidth - accessoryNameSize.X) / 2;
                    float accessoryNameY = pos.Y + (accessorySlotHeight - accessoryNameSize.Y) / 2;

                    spriteBatch.DrawString(font, accessoryName, new Vector2(accessoryNameX, accessoryNameY), Color.Black);
                }
                // If no accessory is equipped in this slot, leave it blank
            }

            // **Update cursor.Y after Accessories**
            if (displayCount == 3) {
                // Two rows
                cursor.Y += accessorySlotHeight * 2 + spacing + 10; // 10 pixels padding after
            } else if (displayCount >= 4) {
                // Two rows
                cursor.Y += accessorySlotHeight * 2 + spacing + 10; // 10 pixels padding after
            } else {
                // Single or two accessories
                cursor.Y += accessorySlotHeight + spacing + 10; // 10 pixels padding after
            }

            return cursor;
        }
    }
}
