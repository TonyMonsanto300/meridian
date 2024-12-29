using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using XenWorld.Config;
using XenWorld.Controller;
using XenWorld.Factory.Map;
using XenWorld.Loader;
using XenWorld.Model;
using XenWorld.Model.Map;
using XenWorld.Renderer;
using XenWorld.Repository.GUI;
using XenWorld.Service;
using XenWorld.src.Loader;
using XenWorld.src.Loader.Puppet;
using XenWorld.src.Manager;
using XenWorld.src.Repository.GUI;
using XenWorld.src.Repository.Map;
using XenWorld.src.Service;

namespace XenWorld
{
    public class XenWorldGame : Game {
        public XenWorldGame() {
            _setupGame();
        }

        protected override void LoadContent() {

            TerrainLoader.SeedTerrain(Content);
            SpriteLoader.SeedSprites(Content);
            FontLoader.SeedFonts(Content);
            PuppetClassLoader.SeedClasses();

            Texture2D blackTexture = new Texture2D(GraphicsDevice, 1, 1);
            blackTexture.SetData(new Color[] { Color.Black });
            TextureDictionary.LoadTexture("blackTexture", blackTexture);

            // Create a 1x1 white texture
            Texture2D whiteTexture = new Texture2D(GraphicsDevice, 1, 1);
            whiteTexture.SetData(new Color[] { Color.White });
            TextureDictionary.LoadTexture("whiteTexture", whiteTexture);

            MapManager.SetDefaultMap();

            PlayerManager.InitializePlayer();
            PuppetService.InitializeNPCs();
            RendererManager.Initialize(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            PlayerManager.Controller.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) {
            GraphicsDevice.Clear(Color.White);

            RendererManager.MasterRenderer.DrawGame();

            base.Draw(gameTime);
        }

        private void _setupGame() {
            GraphicsDeviceManager graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            this.IsMouseVisible = true;

            graphics.PreferredBackBufferWidth = RenderConfig.WindowSizeX * RenderConfig.CellSize;
            graphics.PreferredBackBufferHeight = RenderConfig.WindowSizeY * RenderConfig.CellSize;

            graphics.ApplyChanges();
        }
    }
}
