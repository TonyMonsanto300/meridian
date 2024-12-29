using XenWorld.Model.Map;
using XenWorld.Model;
using XenWorld.src.Model;

public class MapCell {
    public Coordinate Coordinate { get; set; }
    public MapTerrain Terrain { get; set; }
    public Puppet Occupant { get; set; }
    public bool Highlighted { get; set; }
    public bool Cursor { get; set; } // New cursor property
    public bool Indoor { get; set; }

    public bool Occupied {
        get => Occupant != null;
    }

    public MapCell(MapTerrain terrain, int x, int y) {
        Terrain = terrain;
        Occupant = null;
        Highlighted = false;
        Cursor = false; // Initialize as false
        Indoor = false;
        Coordinate= new Coordinate(x, y);
    }
}