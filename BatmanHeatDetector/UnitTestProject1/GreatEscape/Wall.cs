using System.Drawing;

namespace UnitTestProject1.GreatEscape
{
    public class Wall
    {
        public Point WallPosition { get; }

        public Orientation WallOrientation { get; }

        public Wall(Point position, Orientation orientation)
        {
            WallPosition = position;
            WallOrientation = orientation;
        }
    }
}