using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.Model.Map;
using XenWorld.Repository.GUI;
using XenWorld.src.Config;
using XenWorld.src.Manager;
using XenWorld.src.Model.Puppet;
using XenWorld.src.Renderer;
using XenWorld.src.Renderer.Widget;
using XenWorld.src.Renderer.Window;
using XenWorld.src.Repository.GUI;
using XenWorld.src.Repository.Map;

namespace XenWorld.Renderer {
    public class MapRenderer {
        private ZoneMap map;
        private Texture2D highlightTexture;
        private SpriteBatch spriteBatch;

        public MapRenderer(ZoneMap map, Texture2D highlightTexture, SpriteBatch spriteBatch) {
            this.map = map;
            this.highlightTexture = highlightTexture;
            this.spriteBatch = spriteBatch;
        }

        public void DrawMap() {
            // Handle Game Over condition
            if (HandleGameOver()) {
                return;
            }

            PlayerController playerController = PlayerManager.Controller;

            // Get highlight color based on interaction mode
            Color highlightColor = GetHighlightColor(playerController.CurrentMode);

            // Calculate viewport bounds
            var (startX, startY) = CalculateViewportBounds(playerController);

            // Draw map and entities
            DrawCells(startX, startY, highlightColor);

            // Handle Ability Class or Ability List rendering based on state
            HandleAbilityRendering(playerController);

            // Draw viewport border
            DrawViewportBorder();
        }

        private bool HandleGameOver() {
            if (PlayerManager.PartyDead) {
                GameOverScreen.DrawScreen();
                return true; // Indicates that the game over screen was drawn
            }
            return false;
        }

        private (int startX, int startY) CalculateViewportBounds(PlayerController playerController) {
            int viewportWidth = RenderConfig.MapViewPortX;
            int viewportHeight = RenderConfig.MapViewPortY;
            int playerX = playerController.Puppet.Coordinate.X;
            int playerY = playerController.Puppet.Coordinate.Y;

            int startX = Math.Clamp(playerX - viewportWidth / 2, 0, map.Width - viewportWidth);
            int startY = Math.Clamp(playerY - viewportHeight / 2, 0, map.Height - viewportHeight);

            return (startX, startY);
        }

        private void DrawCells(int startX, int startY, Color highlightColor) {
            int viewportWidth = RenderConfig.MapViewPortX;
            int viewportHeight = RenderConfig.MapViewPortY;

            for (int x = 0; x < viewportWidth; x++) {
                for (int y = 0; y < viewportHeight; y++) {
                    int mapX = startX + x;
                    int mapY = startY + y;

                    if (mapX >= 0 && mapX < map.Width && mapY >= 0 && mapY < map.Height) {
                        MapCell cell = map.Grid[mapX, mapY];
                        Vector2 position = new Vector2(x * RenderConfig.CellSize, y * RenderConfig.CellSize);

                        DrawTerrain(cell, position);
                        DrawOccupant(cell, position);
                        DrawHighlights(cell, position, highlightColor);
                        DrawCursor(cell, position);
                    }
                }
            }
        }

        private void DrawTerrain(MapCell cell, Vector2 position) {
            spriteBatch.Draw(cell.Terrain.Sprite, position, Color.White);
        }

        private void DrawOccupant(MapCell cell, Vector2 position) {
            if (cell.Occupant != null) {
                spriteBatch.Draw(cell.Occupant.Sprite, position, Color.White);

                // Draw health bar if occupant is injured
                if (cell.Occupant.Health.Current < cell.Occupant.Health.Max) {
                    Rectangle healthBarPosition = new Rectangle(
                        (int)position.X + (RenderConfig.CellSize - 28) / 2,
                        (int)position.Y,
                        28,
                        3);

                    HealthBarWidget.DrawHealthBar(cell.Occupant, healthBarPosition);
                }
            }
        }

        private void DrawHighlights(MapCell cell, Vector2 position, Color highlightColor) {
            if (cell.Highlighted) {
                spriteBatch.Draw(
                    highlightTexture,
                    new Rectangle((int)position.X, (int)position.Y, RenderConfig.CellSize, RenderConfig.CellSize),
                    highlightColor);
            }
        }

        private void DrawCursor(MapCell cell, Vector2 position) {
            if (cell.Cursor) {
                spriteBatch.Draw(
                    SpriteDictionary.Context["cursor"],
                    new Rectangle((int)position.X, (int)position.Y, RenderConfig.CellSize, RenderConfig.CellSize),
                    Color.White);
            }
        }

        private void HandleAbilityRendering(PlayerController playerController) {
            if (playerController.IsChoosingClass) {
                AbilityWindow.DrawAbilityClassList();
            } else if (playerController.IsCasting) {
                AbilityWindow.DrawAbilityList();
            }
        }

        private void DrawViewportBorder() {
            spriteBatch.Draw(
                TextureDictionary.Context["blackTexture"],
                new Rectangle(
                    0,
                    RenderConfig.MapViewPortY * RenderConfig.CellSize,
                    RenderConfig.MapViewPortX * RenderConfig.CellSize,
                    2),
                Color.Black);
        }

        private Color GetHighlightColor(InteractionMode mode) {
            return mode switch {
                InteractionMode.Attack => Color.Red * 0.5f,
                InteractionMode.Scan => Color.Blue * 0.5f,
                InteractionMode.Mine => Color.Yellow * 0.5f,
                _ => Color.Transparent,
            };
        }
    }
}