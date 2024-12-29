using Microsoft.Xna.Framework.Graphics;

namespace XenWorld.Model.Map {
    public class MapTerrain {
        public string Name { get; private set; }
        public Texture2D Sprite { get; private set; }
        public bool Obstacle { get; private set; }
        public bool Wall { get; private set; }
        public MapTerrain(string name, Texture2D sprite, bool obstacle = false, bool wall = false) {
            Name = name;
            Sprite = sprite;
            Obstacle = obstacle;
            Wall = wall;
        }
    }
}
