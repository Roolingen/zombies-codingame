namespace Codingame.TargetCalculations
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Codingame.Constants;
    using Codingame.Logger;
    using Codingame.Models;

    internal class Targets
    {
        internal static double GetDistance(Point p1, Point p2)
        {
            var line = new Point(p1.X - p2.X, p1.Y - p2.Y);
            return Math.Sqrt(Math.Pow(line.X, 2) + Math.Pow(line.Y, 2));
        }
        internal class TargetInfo
        {
            internal TargetInfo(LivingNPC living, ZombieNPC zombie, double distance)
            {
                Target = living; Zombie = zombie; Distance = distance;
            }
            internal LivingNPC Target { get; set; }
            internal ZombieNPC Zombie { get; set; }
            internal double Distance { get; set; }
        }

        internal static void SetTargets(LivingNPC shooter, List<LivingNPC> humans, List<ZombieNPC> zombies)
        {
            var targetList = new List<TargetInfo>();
            foreach (var zombie in zombies)
            {
                foreach (var human in humans)
                {
                    var intersection = FindClosestIntersection(human.Location, SceneSettings.TargetDiameter, zombie.Location, zombie.NextLocation);
                    if (intersection != PointF.Empty)
                    {
                        var distance = GetDistance(zombie.Location, human.Location);
                        targetList.Add(new TargetInfo(human, zombie, distance));
                    }
                }
                var intersection2 = FindClosestIntersection(shooter.Location, SceneSettings.TargetDiameter, zombie.Location, zombie.NextLocation);
                if (intersection2 != PointF.Empty)
                {
                    var distance = GetDistance(zombie.Location, shooter.Location);
                    targetList.Add(new TargetInfo(shooter, zombie, distance));
                }
            }
            var prioritizedTargetList = targetList
                .OrderBy(x => x.Distance)
                .GroupBy(x => x.Zombie.Id)
                .Select(x => x.First());
            // Log.Write(prioritizedTargetList);
            foreach (var item in prioritizedTargetList)
            {
                item.Zombie.CurrentTarget = item.Target;
                item.Zombie.DistanceToTarget = item.Distance;
                item.Target.TargetedBy.Add(item.Zombie);
            }
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
                return PointF.Empty;
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
