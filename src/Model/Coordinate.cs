namespace XenWorld.src.Model {
    public class Coordinate {
        private int _x;
        private int _y;

        public Coordinate(int x, int y) {
            _x = x;
            _y = y;
        }

        public int X { get { return _x; } set { _x = value; } }
        public int Y { get { return _y; } set { _y = value; } }
    }
}
