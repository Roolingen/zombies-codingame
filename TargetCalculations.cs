namespace Codingame.TargetCalculations
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Codingame.Constants;
    using Codingame.Models;

    internal class Targets
    {
        internal static int GetZombieTurnsToTarget(double distanceToTarget) => (int)Math.Ceiling(distanceToTarget / Ranges.ZombieMove);
        internal static int GetShooterTurnsToTarget(double distanceToTarget) => (int)Math.Ceiling((distanceToTarget - Ranges.ShooterKill) / Ranges.ShooterMove);

        internal static double GetDistance(Point p1, Point p2)
        {
            var line = new Point(p1.X - p2.X, p1.Y - p2.Y);
            return Math.Sqrt(Math.Pow(line.X, 2) + Math.Pow(line.Y, 2));
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
