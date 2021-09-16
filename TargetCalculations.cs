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
        internal static int GetZombieTurnsToTarget(double distanceToTarget) => (int)Math.Ceiling((distanceToTarget - Ranges.ZombieKill) / Ranges.ZombieMove);

        internal static double GetDistance(Point p1, Point p2)
        {
            var line = new Point(p1.X - p2.X, p1.Y - p2.Y);
            return Math.Sqrt(Math.Pow(line.X, 2) + Math.Pow(line.Y, 2));
        }

        internal static List<DistanceMatrix> GetDistanceMatrix(IEnumerable<LivingNPC> humans, IEnumerable<ZombieNPC> zombies)
        {
            var distanceMatrix = new List<DistanceMatrix>();
            foreach (var human in humans)
            {
                foreach (var zombie in zombies)
                {
                    var distance = GetDistance(zombie.Location, human.Location);
                    var killedInTurns = GetZombieTurnsToTarget(distance);
                    distanceMatrix.Add(new DistanceMatrix
                    {
                        NPCHuman = human,
                        NPCMixed = zombie,
                        Distance = distance,
                        TurnsToTarget = killedInTurns
                    });
                }

                foreach (var human2 in humans)
                {
                    if (distanceMatrix.Find(x => x.NPCMixed == human && x.NPCHuman == human2) != null || human == human2) continue;

                    var distance = GetDistance(human2.Location, human.Location);
                    var killedInTurns = GetZombieTurnsToTarget(distance);
                    distanceMatrix.Add(new DistanceMatrix
                    {
                        NPCHuman = human,
                        NPCMixed = human2,
                        Distance = distance,
                        TurnsToTarget = killedInTurns
                    });
                }
            }
            return distanceMatrix;
        }



        internal static Target FindLastSurvivor(IEnumerable<ZombieNPC> zombies)
        {
            return zombies
                .SelectMany(x => x.TargetChain)
                .Where(x => x.NPC.Id != SceneSettings.ShooterId)
                .OrderBy(x => x.KilledInTurns)
                .GroupBy(x => x.NPC)
                .Select(x => x.First())
                .OrderByDescending(x => x.KilledInTurns)
                .FirstOrDefault();
        }

        internal static void CalculateZombieTargetChains(IEnumerable<ZombieNPC> zombies, IEnumerable<LivingNPC> humans, IEnumerable<DistanceMatrix> matrix)
        {
            foreach (var zombie in zombies)
            {
                Log.Write($"CalculateZombieTargetChains.Zombie{zombie.Id}");
                // CalculateZombieTargetChain(zombie, humans.ToList(), matrix, 0);
                var zombieToHumanMatrix = matrix.Where(x => x.NPCHuman.NPCType == NPCType.Zombie && x.NPCMixed.NPCType != NPCType.Zombie
                    || x.NPCHuman.NPCType != NPCType.Zombie && x.NPCMixed.NPCType == NPCType.Zombie);
                CalculateZombieTargetChain(zombie, humans, zombieToHumanMatrix);

                var humanMatrix = matrix.Where(x => x.NPCHuman.NPCType != NPCType.Zombie && x.NPCMixed.NPCType != NPCType.Zombie);
                CalculateHumanTargetChain(zombie, humans, humanMatrix, 0);
                // zombie.TargetChain.Reverse(); // sets so first element is the first target
            }
        }

        private static void CalculateZombieTargetChain(ZombieNPC zombie, IEnumerable<LivingNPC> remainingHumans, IEnumerable<DistanceMatrix> matrix)
        {
            Log.Write($"CalculateZombieTargetChain.Start");
            Log.WriteTargets(zombie.TargetChain);

            var targetedHumans = zombie.TargetChain.Select(x => x.NPC);
            Log.WriteHoomans("targeted:", targetedHumans);
            var unprocessedHumans = remainingHumans.Except(targetedHumans);
            Log.WriteHoomans("unprocessed:", unprocessedHumans);

            var nextTarget = FindClosest(zombie, unprocessedHumans, matrix);
            zombie.TargetChain.Push(nextTarget);
            Log.WriteTargets(zombie.TargetChain);
            Log.Write("CalculateZombieTargetChain.End");
        }

        private static void CalculateHumanTargetChain(ZombieNPC zombie, IEnumerable<LivingNPC> remainingHumans, IEnumerable<DistanceMatrix> matrix, int depth)
        {
            Log.Write($"Depth: {depth}");
            Log.WriteTargets(zombie.TargetChain);

            var targetedHumans = zombie.TargetChain.Select(x => x.NPC);
            Log.WriteHoomans("targeted:", targetedHumans);
            var unprocessedHumans = remainingHumans.Except(targetedHumans);
            Log.WriteHoomans("unprocessed:", unprocessedHumans);

            var nextTarget = FindClosest(zombie, unprocessedHumans, matrix);
            zombie.TargetChain.Push(nextTarget);
            CalculateHumanTargetChain(zombie, unprocessedHumans, matrix, ++depth);
        }

        private static Target FindClosest(BaseNPC referenceNPC, IEnumerable<LivingNPC> possibleTargets, IEnumerable<DistanceMatrix> humanMatrix)
        {
            var target = new Target();
            foreach (var possibleTarget in possibleTargets)
            {
                var humanRelationData = humanMatrix
                    .FirstOrDefault(x =>
                        (x.NPCHuman.Id == referenceNPC.Id && x.NPCMixed.Id == possibleTarget.Id)
                        || (x.NPCHuman.Id == possibleTarget.Id && x.NPCMixed.Id == referenceNPC.Id));

                if (humanRelationData != null && target.Distance > humanRelationData.Distance)
                {
                    target.Distance = humanRelationData.Distance;
                    target.NPC = possibleTarget;
                    target.KilledInTurns = humanRelationData.TurnsToTarget;
                }
            }

            return target;
        }

        // private static void CalculateZombieTargetChain(ZombieNPC zombie, IEnumerable<LivingNPC> remainingHumans, IEnumerable<DistanceMatrix> matrix, int depth)
        // {
        //     Log.Write($"Depth: {depth}");
        //     Log.WriteTargets(zombie.TargetChain);
        //     var targetedHumans = zombie.TargetChain.Select(x => x.NPC);
        //     var unprocessedHumans = remainingHumans.Except(targetedHumans);

        //     if (!unprocessedHumans.Any()) return;

        //     var lastNPCOnChain = zombie.TargetChain.FirstOrDefault()?.NPC ?? zombie.CurrentTarget;
        //     Log.Write($"lastNPCOnChain: {lastNPCOnChain.Id}");

        //     var target = FindClosestTarget(lastNPCOnChain, unprocessedHumans, matrix);
        //     zombie.TargetChain.Push(target);
        //     CalculateZombieTargetChain(zombie, remainingHumans, matrix, ++depth);
        // }

        // private static Target FindClosestTarget(LivingNPC current, IEnumerable<LivingNPC> npcList, IEnumerable<DistanceMatrix> matrix)
        // {
        //     var target = new Target();
        //     foreach (var npc in npcList)
        //     {
        //         var relationData = matrix.FirstOrDefault(x => (x.Npc1.Id == current.Id && x.Npc2.Id == npc.Id) || (x.Npc1.Id == npc.Id && x.Npc2.Id == current.Id));
        //         if (relationData != null && target.Distance > relationData.Distance)
        //         {
        //             target.Distance = relationData.Distance;
        //             target.NPC = npc;
        //             target.KilledInTurns = relationData.TurnsToTarget;
        //         }
        //     }

        //     return target;
        // }


        // internal static List<TargetInfo> CalculateZombieTargetList(LivingNPC shooter, List<LivingNPC> humans, List<ZombieNPC> zombies)
        // {
        //     var targetList = new List<TargetInfo>();
        //     foreach (var zombie in zombies)
        //     {
        //         foreach (var human in humans)
        //         {
        //             CalculateDistanceAndTarget(targetList, zombie, human);
        //         }
        //         CalculateDistanceAndTarget(targetList, zombie, shooter);
        //     }
        //     var revisedPriorityList = CalculatePriorities(targetList);
        //     UpdateNPCTargets(revisedPriorityList);

        //     return revisedPriorityList;
        // }

        // private static void CalculateDistanceAndTarget(List<TargetInfo> targetList, ZombieNPC zombie, LivingNPC npc)
        // {
        //     var intersection = FindClosestIntersection(npc.Location, SceneSettings.TargetDiameter, zombie.Location, zombie.NextLocation);
        //     var distanceNpcToZombie = GetDistance(zombie.Location, npc.Location);
        //     if (intersection != PointF.Empty)
        //     {
        //         targetList.Add(new TargetInfo(npc, zombie, distanceNpcToZombie));
        //     }
        // UpdateClosestDistanceToZombie(npc, distanceNpcToZombie);
        // }

        // private static void UpdateClosestDistanceToZombie(LivingNPC npc, double distanceNpcToZombie)
        // {
        //     if (npc.DistanceToClosestZombie > distanceNpcToZombie)
        //     {
        //         npc.DistanceToClosestZombie = distanceNpcToZombie;
        //     }
        // }

        private static void UpdateNPCTargets(List<TargetInfo> revisedPriorityList)
        {
            foreach (var item in revisedPriorityList)
            {
                item.Zombie.CurrentTarget = item.Target;
                // item.Zombie.DistanceToTarget = item.Distance;
                item.Target.TargetedBy.Add(item.Zombie);
            }
        }

        private static List<TargetInfo> CalculatePriorities(List<TargetInfo> targetList) =>
            targetList
                .OrderBy(x => x.Distance)
                .GroupBy(x => x.Zombie.Id)
                .Select(x => x.First())
                .ToList();

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
