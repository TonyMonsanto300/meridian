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

            //Game Assets
            TerrainLoader.SeedTerrain(Content);
            SpriteLoader.SeedSprites(Content);
            FontLoader.SeedFonts(Content);
            TextureLoader.SeedTextures(GraphicsDevice);

            //Pre-Built Objects
            PuppetClassLoader.SeedClasses();
            ArmorLoader.SeedArmors();

            //Pre-Built Sets
            AbilitySetLoader.SeedAbilitySets();
            WeaponSetLoader.SeedWeaponSets();
            AccessorySetLoader.SeedAccessorySets();
            ResourceSetLoader.SeedResourceSets();

            MapManager.SetDefaultMap();

            PlayerManager.InitializePlayer();
            PuppetService.InitializePuppets();
            RendererManager.Initialize(GraphicsDevice);
        }

        protected override void Update(GameTime gameTime) {
            PlayerManager.Controller.Update(gameTime);

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
