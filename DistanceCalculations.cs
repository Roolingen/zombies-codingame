namespace Codingame.DistanceCalculations
{
    using System;
    using System.Drawing;

    internal class Distances
    {
        internal static int GetDistance(Point p1, Point p2)
        {
            var line = new Point(p1.X - p2.X, p1.Y - p2.Y);
            var distance = Math.Sqrt(Math.Pow(line.X, 2) + Math.Pow(line.Y, 2));

            return (int)Math.Ceiling(distance);
        }

        internal static PointF FindClosestIntersection(
            Point target, float targetSearchRadius,
            Point source, Point sourceDestination)
        {
            float A, B, C, det;
            var line = new Point(sourceDestination.X - source.X, sourceDestination.Y - source.Y);

            A = line.X * line.X + line.Y * line.Y;
            B = 2 * (line.X * (source.X - target.X) + line.Y * (source.Y - target.Y));
            C = (source.X - target.X) * (source.X - target.X) +
                (source.Y - target.Y) * (source.Y - target.Y) -
                targetSearchRadius * targetSearchRadius;

            det = B * B - 4 * A * C;
            if ((A <= 0.0000001) || (det < 0)) // No intersections.
            {
                return new PointF(float.NaN, float.NaN);
            }
            else if (det == 0) // One intersection.
            {
                var t = -B / (2 * A);
                return new PointF(source.X + t * line.X, source.Y + t * line.Y);
            }
            else // Two intersections, returning the closest
            {
                var t2 = (float)((-B - Math.Sqrt(det)) / (2 * A));
                // return (new PointF(lineStart.X + t1 * line.X, lineStart.Y + t1 * line.Y), new PointF(lineStart.X + t2 * line.X, lineStart.Y + t2 * line.Y));
                return new PointF(source.X + t2 * line.X, source.Y + t2 * line.Y);
            }
        }
    }
}
