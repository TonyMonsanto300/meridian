using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XenWorld.Repository.Map;

namespace XenWorld.Loader {
    public static class TerrainLoader {
        public static void SeedTerrain(ContentManager content) {
            Texture2D grassTexture = content.Load<Texture2D>("grass");
            Texture2D borderTexture = content.Load<Texture2D>("border");
            Texture2D woodFloorTexture = content.Load<Texture2D>("floor_wood");
            Texture2D panelFloorTexture = content.Load<Texture2D>("floor_panel");
            Texture2D whitePathTexture = content.Load<Texture2D>("path_white");
            Texture2D panelWallTexture = content.Load<Texture2D>("wall_panel");

            TerrainDictionary.LoadTerrain("grass", grassTexture);
            TerrainDictionary.LoadTerrain("border", borderTexture, true);
            TerrainDictionary.LoadTerrain("floor_wood", woodFloorTexture);
            TerrainDictionary.LoadTerrain("floor_panel", panelFloorTexture);
            TerrainDictionary.LoadTerrain("path_white", whitePathTexture);
            TerrainDictionary.LoadTerrain(panelWallTexture.Name, panelWallTexture, true, true);
        }
    }
}
