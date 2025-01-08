using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.src.Repository.GUI;

namespace XenWorld.src.Loader {
    public static class TextureLoader {
        public static void SeedTextures(GraphicsDevice graphicsDevice) {
            Texture2D blackTexture = new Texture2D(graphicsDevice, 1, 1);
            blackTexture.SetData(new Color[] { Color.Black });
            TextureDictionary.LoadTexture("blackTexture", blackTexture);

            // Create a 1x1 white texture
            Texture2D whiteTexture = new Texture2D(graphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });
            TextureDictionary.LoadTexture("whiteTexture", whiteTexture);
        }
    }
}
