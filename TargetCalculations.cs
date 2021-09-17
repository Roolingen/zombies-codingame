namespace Codingame.TargetCalculations
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Codingame.Constants;
    using Codingame.Logger;
    using Codingame.Models;

    internal class Targets
    {
        internal static int GetZombieTurnsToTarget(double distanceToTarget) => (int)Math.Ceiling(distanceToTarget / Ranges.ZombieMove);
        internal static int GetShooterTurnsToTarget(double distanceToTarget) => (int)Math.Ceiling((distanceToTarget - Ranges.ShooterKill) / Ranges.ShooterMove);

        private static Point GetVector(Point p1, Point p2) => new Point(p2.X - p1.X, p2.Y - p1.Y);
        internal static double GetDistance(Point vector) => Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        internal static double GetDistance(Point p1, Point p2)
        {
            var vector = GetVector(p1, p2);
            return Math.Sqrt(Math.Pow(vector.X, 2) + Math.Pow(vector.Y, 2));
        }

        internal static Point GetCoordinates(Point p1, Point p2, double distance)
        {
            if (p1 == p2) return p1;
            Log.Write($"p1 x:{p1.X} y:{p1.Y} - p2 x:{p2.X} y:{p2.Y}. Distance: {distance}");
            var vector = GetVector(p1, p2);
            Log.Write($"Vector: {vector}");
            var fullDistance = GetDistance(vector);
            Log.Write($"Full distance: {fullDistance}");
            var requiredDistance = fullDistance / distance;
            Log.Write($"Required distance: {requiredDistance}");
            return new Point((int)Math.Abs(p1.X + vector.X / requiredDistance), (int)Math.Abs(p1.Y + vector.Y / requiredDistance));
        }

        internal static Point GetMaxDistanceFromTarget(DistanceMatrix lastToDie, DistanceMatrix lastToSave, Point target)
        {
            return GetCoordinates(lastToSave.SecondaryNPC.Location, target, lastToDie.TurnsToTarget * Ranges.ShooterMove);
        }

        internal static List<DistanceMatrix> GetZombieDistanceMatrix(LivingNPC shooter, IEnumerable<LivingNPC> humans, IEnumerable<ZombieNPC> zombies)
        {
            var distanceMatrix = new List<DistanceMatrix>();
            foreach (var human in humans)
            {
                foreach (var zombie in zombies)
                {
                    var distanceForZombie = GetDistance(zombie.Location, human.Location);
                    var distanceForShooter = GetDistance(shooter.Location, human.Location);
                    var turnsToTargetForZombie = GetZombieTurnsToTarget(distanceForZombie);
                    var turnsToTargetForShooter = GetShooterTurnsToTarget(distanceForShooter);
                    distanceMatrix.Add(new DistanceMatrix
                    {
                        PrimaryNPC = zombie,
                        SecondaryNPC = human,
                        Distance = distanceForZombie,
                        TurnsToTarget = turnsToTargetForZombie,
                        TurnsToSave = turnsToTargetForShooter
                    });
                }
            }
            return distanceMatrix;
        }

        internal static List<DistanceMatrix> GetShooterDistanceMatrix(LivingNPC shooter, IEnumerable<LivingNPC> humans, IEnumerable<ZombieNPC> zombies)
        {
            var distanceMatrix = new List<DistanceMatrix>();
            foreach (var human in humans)
            {
                var distance = GetDistance(shooter.Location, human.Location);
                var turnsToTarget = GetShooterTurnsToTarget(distance);
                distanceMatrix.Add(new DistanceMatrix
                {
                    PrimaryNPC = shooter,
                    SecondaryNPC = human,
                    Distance = distance,
                    TurnsToTarget = turnsToTarget
                });
            }
            foreach (var zombie in zombies)
            {
                var distance = GetDistance(shooter.Location, zombie.Location);
                var turnsToTarget = GetShooterTurnsToTarget(distance);
                distanceMatrix.Add(new DistanceMatrix
                {
                    PrimaryNPC = shooter,
                    SecondaryNPC = zombie,
                    Distance = distance,
                    TurnsToTarget = turnsToTarget
                });
            }
            return distanceMatrix;
        }

        internal static List<DistanceMatrix> GetHumanDistanceMatrix(IEnumerable<LivingNPC> humans, IEnumerable<ZombieNPC> zombies)
        {
            var distanceMatrix = new List<DistanceMatrix>();
            foreach (var human in humans)
            {
                foreach (var human2 in humans)
                {
                    if (distanceMatrix.Find(x => x.SecondaryNPC == human && x.PrimaryNPC == human2) != null || human == human2) continue;

                    var distance = GetDistance(human2.Location, human.Location);
                    var turnsToTarget = GetZombieTurnsToTarget(distance);
                    distanceMatrix.Add(new DistanceMatrix
                    {
                        PrimaryNPC = human,
                        SecondaryNPC = human2,
                        Distance = distance,
                        TurnsToTarget = turnsToTarget
                    });
                }
            }
            return distanceMatrix;
        }
    }
}
