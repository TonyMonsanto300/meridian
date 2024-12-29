using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.Model;
using XenWorld.Renderer;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet.Equipment;
using XenWorld.src.Repository.GUI;
using XenWorld.src.Service;

namespace XenWorld.src.Renderer.Partial.Info {
    public static class WeaponsSectionPartial {
        public static PrintCursor Render(SpriteFont font, SpriteBatch spriteBatch, PrintCursor cursor) {
            string weaponsHeader = "Weapons";
            Vector2 weaponsHeaderSize = font.MeasureString(weaponsHeader);
            float weaponsHeaderX = InfoRendererConfig.LeftBorder +
                (RenderConfig.InfoViewPortX * RenderConfig.CellSize - weaponsHeaderSize.X) / 2;
            float weaponsHeaderY = cursor.Y;

            // Draw "Weapons" Header
            spriteBatch.DrawString(font, weaponsHeader, new Vector2(weaponsHeaderX, weaponsHeaderY), Color.Black);

            // Move cursor down for weapon entries
            cursor.Y += weaponsHeaderSize.Y + 5; // 5 pixels padding

            // **Positions and Dimensions for Weapons**
            float weaponBoxWidth = (RenderConfig.InfoViewPortX * RenderConfig.CellSize) - 20; // Total width with padding
            float weaponBoxHeight = 30; // Height of each weapon entry
            float weaponBoxX = InfoRendererConfig.LeftBorder + 10; // Left padding

            // Left and right column widths
            float weaponLabelColumnWidth = weaponBoxWidth / 2;
            float weaponValueColumnWidth = weaponBoxWidth / 2;

            // Retrieve the equipped weapon
            PuppetWeapon equippedWeapon = PlayerManager.Controller.Puppet.EquippedWeapon;

            // Draw each weapon in a box
            foreach (var weapon in PlayerManager.Controller.Puppet.Weapons) {
                // Rectangle for the weapon entry
                Rectangle weaponBox = new Rectangle(
                    (int)weaponBoxX,
                    (int)cursor.Y,
                    (int)weaponBoxWidth,
                    (int)weaponBoxHeight
                );

                // **Determine if this weapon is equipped**
                bool isEquipped = weapon == equippedWeapon;

                // **Set colors based on equipped status**
                Color boxBackgroundColor = isEquipped ? Color.IndianRed : Color.LightGray;
                Color boxBorderColor = isEquipped ? Color.Red : Color.Black;
                Color textColor = isEquipped ? Color.White : Color.Black;

                // **Draw the box background**
                spriteBatch.Draw(
                    TextureDictionary.Context["whiteTexture"],
                    weaponBox,
                    boxBackgroundColor
                );

                // **Draw the box border**
                InfoRenderService.DrawRectangleBorder(spriteBatch, weaponBox, boxBorderColor);

                // **Draw the vertical line separating label and value**
                float separatorX = weaponBoxX + weaponLabelColumnWidth;
                spriteBatch.Draw(
                    TextureDictionary.Context["blackTexture"],
                    new Rectangle((int)separatorX, (int)cursor.Y, 1, (int)weaponBoxHeight),
                    Color.Black
                );

                // **Draw Weapon Label (Name)**
                string weaponLabel = weapon.Name;
                Vector2 weaponLabelSize = font.MeasureString(weaponLabel);
                float weaponLabelX = weaponBoxX + 5; // Left padding inside the box
                float weaponLabelY = cursor.Y + (weaponBoxHeight - weaponLabelSize.Y) / 2;

                spriteBatch.DrawString(font, weaponLabel, new Vector2(weaponLabelX, weaponLabelY), textColor);

                // **Draw Weapon Value (Stats)**
                string weaponValue = GenerateWeaponValueString(weapon);
                Vector2 weaponValueSize = font.MeasureString(weaponValue);
                float weaponValueX = separatorX + 5; // Left padding after separator
                float weaponValueY = cursor.Y + (weaponBoxHeight - weaponValueSize.Y) / 2;

                spriteBatch.DrawString(font, weaponValue, new Vector2(weaponValueX, weaponValueY), textColor);

                // Move cursor down for next weapon
                cursor.Y += weaponBoxHeight + 5; // Move down for next weapon
            }

            return cursor;
        }

        private static string GenerateWeaponValueString(PuppetWeapon weapon) {
            // HitBonus
            int hitBonus = weapon.HitBonus;
            string hitBonusString = (hitBonus >= 0) ? $"+{hitBonus}" : $"{hitBonus}";

            // Damage Dice
            // Group dice by type and count them
            var diceGroups = weapon.DamageDice.GroupBy(d => d);

            List<string> diceStrings = new List<string>();

            foreach (var group in diceGroups) {
                int count = group.Count();
                Dice dieType = group.Key;

                // Convert die type to string, e.g., D6 -> "d6"
                string dieString = $"d{DiceService.GetDieValue(dieType)}";

                if (count > 1) {
                    dieString = $"{count}{dieString}";
                }

                diceStrings.Add(dieString);
            }

            // Join the dice strings with '+'
            string damageDiceString = string.Join("+", diceStrings);

            // DamageBonus
            int damageBonus = weapon.DamageBonus;
            string damageBonusString = "";
            if (damageBonus != 0) {
                damageBonusString = (damageBonus > 0) ? $"+{damageBonus}" : $"{damageBonus}";
            } else {
                damageBonusString = "+0";
            }

            // Combine the parts
            string weaponValue = $"({hitBonusString}) {damageDiceString}{damageBonusString}";

            return weaponValue;
        }
    }
}
