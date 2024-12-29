using System;
using System.Linq;
using Microsoft.Xna.Framework;
using XenWorld.Config;
using XenWorld.Repository.GUI;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Renderer.Window {
    public static class AbilityWindow {
        private static Vector2 GetWindowPositionRelativeToPlayer(int windowWidth, int windowHeight) {
            // Retrieve PlayerController and Map properties
            var playerController = PlayerManager.Controller;
            var map = MapManager.ActiveMap;

            // Player's position on the map
            int playerX = playerController.Puppet.Coordinate.X;
            int playerY = playerController.Puppet.Coordinate.Y;

            // Viewport and map properties
            int viewportWidth = RenderConfig.MapViewPortX;
            int viewportHeight = RenderConfig.MapViewPortY;
            int cellSize = RenderConfig.CellSize;

            // Calculate player's position on the screen
            int halfViewportX = viewportWidth / 2;
            int halfViewportY = viewportHeight / 2;

            int startX = Math.Max(0, Math.Min(playerX - halfViewportX, map.Width - viewportWidth));
            int startY = Math.Max(0, Math.Min(playerY - halfViewportY, map.Height - viewportHeight));

            int playerScreenX = (playerX - startX) * cellSize;
            int playerScreenY = (playerY - startY) * cellSize;

            // Determine if the player is near map boundaries
            bool renderWindowLeft = (map.Width - 1) - playerX <= 15; // Near right edge
            bool nearTopEdge = playerY <= 15;                        // Near top edge
            bool nearBottomEdge = (map.Height - 1) - playerY <= 15;  // Near bottom edge

            // Default window position slightly to the right of the player
            Vector2 windowPosition = new Vector2(playerScreenX + 50, playerScreenY - (windowHeight / 2));

            if (nearTopEdge) {
                // Place the window below the player if near the top edge
                windowPosition = new Vector2(playerScreenX - (windowWidth / 2), playerScreenY + 50);
            } else if (nearBottomEdge) {
                // Place the window above the player if near the bottom edge
                windowPosition = new Vector2(playerScreenX - (windowWidth / 2), playerScreenY - windowHeight - 50);
            } else if (renderWindowLeft) {
                // Place the window to the left of the player if near the right edge
                windowPosition = new Vector2(playerScreenX - windowWidth - 50, playerScreenY - (windowHeight / 2));
            }

            // Ensure the window stays within the viewport horizontally
            windowPosition.X = Math.Clamp(windowPosition.X, 0, (viewportWidth * cellSize) - windowWidth);

            // Ensure the window stays within the viewport vertically
            windowPosition.Y = Math.Clamp(windowPosition.Y, 0, (viewportHeight * cellSize) - windowHeight);

            return windowPosition;
        }
        public static void DrawAbilityClassList() {
            PlayerController playerController = PlayerManager.Controller;
            var abilityClasses = playerController.Puppet.Abilities.Select(a => a.Class).Distinct().ToList();

            int windowWidth = 200;
            int windowHeight = (abilityClasses.Count * 20) + 15;

            Vector2 windowPosition = GetWindowPositionRelativeToPlayer(windowWidth, windowHeight);
            DrawWindowBackground(windowPosition, windowWidth, windowHeight);

            for (int i = 0; i < abilityClasses.Count; i++) {
                RendererManager.SpriteBatch.DrawString(FontRepository.Context["default"], $"{i + 1}. {abilityClasses[i]}", new Vector2(windowPosition.X + 10, windowPosition.Y + 10 + i * 20), Color.Black);
            }
        }

        public static void DrawAbilityList() {
            PlayerController playerController = PlayerManager.Controller;
            var selectedClass = playerController.Puppet.Abilities.Select(a => a.Class).Distinct().ElementAt(playerController.SelectedAbilityClassIndex);
            var abilities = playerController.Puppet.Abilities.Where(a => a.Class == selectedClass).ToList();

            int windowWidth = 200;
            int windowHeight = (abilities.Count * 20) + 15;

            Vector2 windowPosition = GetWindowPositionRelativeToPlayer(windowWidth, windowHeight);

            DrawWindowBackground(windowPosition, windowWidth, windowHeight);

            for (int i = 0; i < abilities.Count; i++) {
                var ability = abilities[i];
                string costText = ability.Cost.Value.ToString();
                Vector2 costSize = FontRepository.Context["default"].MeasureString(costText);
                Vector2 textPosition = new Vector2(windowPosition.X + 10, windowPosition.Y + 10 + i * 20);
                Vector2 costPosition = new Vector2(windowPosition.X + windowWidth - 10 - costSize.X, textPosition.Y);

                RendererManager.SpriteBatch.DrawString(FontRepository.Context["default"], $"{i + 1}. {ability.Name}", textPosition, Color.Black);
                RendererManager.SpriteBatch.DrawString(FontRepository.Context["default"], costText, costPosition, GetResourceColor(ability.Cost.Type));
            }
        }

        private static void DrawWindowBackground(Vector2 position, int width, int height) {
            RendererManager.SpriteBatch.Draw(TextureDictionary.Context["blackTexture"], new Rectangle((int)position.X, (int)position.Y, width, height), Color.Black);
            RendererManager.SpriteBatch.Draw(TextureDictionary.Context["whiteTexture"], new Rectangle((int)position.X + 5, (int)position.Y + 5, width - 10, height - 10), Color.White);
        }

        private static Color GetResourceColor(ResourceType resourceType) {
            return resourceType switch {
                ResourceType.Mana => Color.Blue,
                ResourceType.Miasma => Color.Purple,
                ResourceType.Fury => Color.Red,
                ResourceType.Rune => Color.HotPink,
                ResourceType.Energy => Color.Coral,
                ResourceType.Combo => Color.Yellow,
                ResourceType.Prayer => Color.Teal,
                _ => Color.White,
            }; ;
        }
    }
}
