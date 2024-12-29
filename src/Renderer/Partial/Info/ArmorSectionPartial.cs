using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.Model;
using XenWorld.Renderer;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet.Equipment;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class ArmorSectionPartial {
        public static PrintCursor Render(SpriteFont font, SpriteBatch spriteBatch, PrintCursor cursor) {
            string armorHeader = "Armor";
            Vector2 armorHeaderSize = font.MeasureString(armorHeader);
            float armorHeaderX = InfoRendererConfig.LeftBorder + (RenderConfig.InfoViewPortX * RenderConfig.CellSize - armorHeaderSize.X) / 2;
            float armorHeaderY = cursor.Y;

            spriteBatch.DrawString(font, armorHeader, new Vector2(armorHeaderX, armorHeaderY), Color.Black);

            cursor.Y += armorHeaderSize.Y + 5; // Move down for armor entry

            // **Positions and Dimensions for Armor**
            float armorBoxWidth = (RenderConfig.InfoViewPortX * RenderConfig.CellSize) - 20; // Total width with padding
            float armorBoxHeight = 30; // Height of the armor entry
            float armorBoxX = InfoRendererConfig.LeftBorder + 10; // Left padding

            // Left and right column widths
            float armorLabelColumnWidth = armorBoxWidth / 2;
            float armorValueColumnWidth = armorBoxWidth / 2;

            // Draw armor in a box
            if (PlayerManager.Controller.Puppet.Armor != null) {
                // Rectangle for the armor entry
                Rectangle armorBox = new Rectangle(
                    (int)armorBoxX,
                    (int)cursor.Y,
                (int)armorBoxWidth,
                    (int)armorBoxHeight
                );

                // Draw the box background
                spriteBatch.Draw(
                    TextureDictionary.Context["whiteTexture"],
                armorBox,
                    Color.LightGray
                );

                // Draw the box border
                InfoRenderService.DrawRectangleBorder(spriteBatch, armorBox, Color.Black);

                // Draw the vertical line separating label and value
                float separatorX = armorBoxX + armorLabelColumnWidth;
                spriteBatch.Draw(
                    TextureDictionary.Context["blackTexture"],
                    new Rectangle((int)separatorX, (int)cursor.Y, 1, (int)armorBoxHeight),
                    Color.Black
                );

                // **Draw Armor Label (Name)**
                string armorLabel = PlayerManager.Controller.Puppet.Armor.Name;
                Vector2 armorLabelSize = font.MeasureString(armorLabel);
                float armorLabelX = armorBoxX + 5; // Left padding inside the box
                float armorLabelY = cursor.Y + (armorBoxHeight - armorLabelSize.Y) / 2;

                spriteBatch.DrawString(font, armorLabel, new Vector2(armorLabelX, armorLabelY), Color.Black);

                // **Draw Armor Value (Defense and Type)**
                string armorValue = GenerateArmorValueString(PlayerManager.Controller.Puppet.Armor);
                Vector2 armorValueSize = font.MeasureString(armorValue);
                float armorValueX = separatorX + 5; // Left padding after separator
                float armorValueY = cursor.Y + (armorBoxHeight - armorValueSize.Y) / 2;

                spriteBatch.DrawString(font, armorValue, new Vector2(armorValueX, armorValueY), Color.Black);

                cursor.Y += 35; // Add some spacing after the armor section
            }
            return cursor;
        }
        private static string GenerateArmorValueString(PuppetArmor armor) {
            // Defense
            int defense = armor.Defense;
            string defenseString = (defense >= 0) ? $"+{defense}" : $"{defense}";

            // Armor Type
            string armorType = armor.ArmorClass.ToString();

            // Combine the parts
            string armorValue = $"{defenseString} ({armorType})";

            return armorValue;
        }

    }
}
