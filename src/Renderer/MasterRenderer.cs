using Microsoft.Xna.Framework.Graphics;

namespace XenWorld.Renderer {
    public class MasterRenderer {
        private MapRenderer mapRenderer;
        private InfoRenderer infoRenderer;
        private LogRenderer logRenderer; // Add LogRenderer
        private SpriteBatch spriteBatch;

        public MasterRenderer(MapRenderer mapRenderer, InfoRenderer infoRenderer, LogRenderer logRenderer, SpriteBatch spriteBatch) {
            this.mapRenderer = mapRenderer;
            this.infoRenderer = infoRenderer;
            this.logRenderer = logRenderer; // Initialize LogRenderer
            this.spriteBatch = spriteBatch;
        }

        public void DrawGame() {
            spriteBatch.Begin();

            mapRenderer.DrawMap();
            infoRenderer.DrawInfo();
            logRenderer.DrawLog(); // Draw the log area

            spriteBatch.End();
        }
    }
}