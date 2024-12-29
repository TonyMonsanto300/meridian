using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XenWorld.Model.Map
{
    public class ZoneMap {
        public int Width { get; private set; }
        public int Height { get; private set; }
        public MapCell[,] Grid { get; private set; }
        public List<Building> Buildings { get; set; } = new List<Building>();

        public List<MapCell> Cells { get => Grid.Cast<MapCell>().ToList(); }

        public ZoneMap(int width, int height) {
            Width = width;
            Height = height;
            Grid = new MapCell[Width, Height];
        }

        public bool IsWithinBounds(int x, int y) {
            return x >= 0 && x < this.Width && y >= 0 && y < this.Height;
        }

        public MapCell FindPuppetCell(Puppet puppet) {
            return Cells.FirstOrDefault(cell => cell.Occupant == puppet);
        }
    }
}
