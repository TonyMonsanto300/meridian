using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Config;
using XenWorld.src.Manager;
using XenWorld.src.Renderer.Partial.Info;

namespace XenWorld.Renderer {
    public class PrintCursor {
        public float X;
        public float Y;

        public PrintCursor(float x, float y) {
            X = x;
            Y = y;
        }
    }

    public static class InfoRendererConfig {
        public static int LeftBorder = RenderConfig.MapViewPortX * RenderConfig.CellSize;
        public static int TopBorder = 0; // Assuming the info viewport starts at the top
        public static int Height = RenderConfig.WindowSizeY * RenderConfig.CellSize;
        public static int ResourceBarPadding = 5;

    }
    public class InfoRenderer {
        private SpriteBatch spriteBatch;
        private SpriteFont font;
        private PrintCursor _printCursor;

        public InfoRenderer(SpriteBatch spriteBatch, SpriteFont font) {
            this.spriteBatch = spriteBatch;
            this.font = font;

             _printCursor = new PrintCursor(InfoRendererConfig.LeftBorder, InfoRendererConfig.TopBorder);
        }

        public void DrawInfo() {
            // Move into own ResourceBarRenderer
            int labelWidth = (int)font.MeasureString("XXXXXX").X + 10;
            int resourceBarWidth = (int)(RenderConfig.InfoViewPortX * RenderConfig.CellSize - 20 - labelWidth); // Adjusted bar width
            int resourceBarOffset = InfoRendererConfig.LeftBorder + 10 + labelWidth; // Bar starts after label and left padding
            int resourceLabelOffset = InfoRendererConfig.LeftBorder + 10; // Label starts after left padding

            InfoBorderPartial.Render(spriteBatch);
            _printCursor = PlayerNamePartial.Render(_printCursor);
            _printCursor = LevelClassPartial.Render(_printCursor);
            _printCursor = HealthBarPartial.Render(_printCursor, resourceLabelOffset, resourceBarOffset, resourceBarWidth);
            _printCursor = HungerBarPartial.Render(_printCursor, resourceLabelOffset, resourceBarOffset, resourceBarWidth);
            _printCursor = ResourceListPartial.Render(_printCursor, resourceLabelOffset, resourceBarOffset, resourceBarWidth);
            _printCursor = StatsSectionPartial.Render(_printCursor);
            _printCursor = WeaponsSectionPartial.Render(font, spriteBatch, _printCursor);
            _printCursor = ArmorSectionPartial.Render(font, spriteBatch, _printCursor);
            _printCursor = AccessoriesSectionPartial.Render(font, spriteBatch, _printCursor);
            _printCursor = ExperienceBarPartial.Render(font, spriteBatch, _printCursor);

        }
    }
}