using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Renderer;
using XenWorld.Repository.GUI;
namespace XenWorld.src.Manager {
    internal class RendererManager {
        public static MapRenderer MapRenderer = null;
        public static InfoRenderer InfoRenderer = null;
        public static MasterRenderer MasterRenderer = null;
        public static LogRenderer LogRenderer = null;

        public static SpriteBatch SpriteBatch;
        public static SpriteFont DefaultFont;
        public static void Initialize(GraphicsDevice graphicsDevice) {
            SpriteBatch = new SpriteBatch(graphicsDevice);
            DefaultFont = FontRepository.Context["default"];
            Texture2D highlightTexture = new Texture2D(graphicsDevice, 1, 1);
            highlightTexture.SetData(new[] { Color.White });

            MapRenderer = new MapRenderer(MapManager.ActiveMap, highlightTexture, SpriteBatch);
            InfoRenderer = new InfoRenderer(SpriteBatch, DefaultFont);
            LogRenderer = new LogRenderer(SpriteBatch, DefaultFont);
            MasterRenderer = new MasterRenderer(MapRenderer, InfoRenderer, LogRenderer, SpriteBatch);
        }
    }
}