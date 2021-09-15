namespace Codingame.Distances
{
    using System;
    using System.Drawing;

    internal class Distances
    {
        internal static int GetDistance(Point p1, Point p2)
        {
            var distanceX = p1.X - p2.X;
            var distanceY = p1.Y - p2.Y;
            var distance = Math.Sqrt(Math.Pow(distanceX, 2) + Math.Pow(distanceY, 2));

            return (int)Math.Ceiling(distance);
        }
    }
}
