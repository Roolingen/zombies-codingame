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
        internal static int GetTurnsToTarget(double distanceToTarget) => (int)Math.Ceiling((distanceToTarget - Ranges.ZombieKill) / Ranges.ZombieMove);

        internal static double GetDistance(Point p1, Point p2)
        {
            var line = new Point(p1.X - p2.X, p1.Y - p2.Y);
            return Math.Sqrt(Math.Pow(line.X, 2) + Math.Pow(line.Y, 2));
        }

        internal static void CalculateZombieTargetChains(IEnumerable<ZombieNPC> zombies, IEnumerable<LivingNPC> humans)
        {
            foreach (var zombie in zombies)
            {
                Console.Error.WriteLine($"{zombie.Id} zombie target: {zombie.CurrentTarget.Id}");
                CalculateZombieTargetChain(zombie, humans.ToList());
                Console.Error.WriteLine($"{zombie.Id} zombie target chain:");
                Log.Write(zombie.TargetChain.Select(x => x.NPC.Id));
            }
        }

        internal static void CalculateZombieTargetChain(ZombieNPC zombie, IEnumerable<LivingNPC> humans)
        {
            var list = humans.Except(zombie.TargetChain.Select(x => x.NPC));
            if (!list.Any()) return;
            var lastNPCOnChain = zombie.TargetChain.LastOrDefault()?.NPC ?? zombie.CurrentTarget;
            var target = FindClosestTarget(lastNPCOnChain, list);
            // Log.Write(target);
            zombie.TargetChain.Push(target);
            CalculateZombieTargetChain(zombie, humans);
        }

        private static Target FindClosestTarget(LivingNPC current, IEnumerable<LivingNPC> npcList)
        {
            var target = new Target();
            foreach (var npc in npcList)
            {
                var npcDistance = GetDistance(current.Location, npc.Location);
                if (target.Distance > npcDistance)
                {
                    target.Distance = npcDistance;
                    target.NPC = npc;
                }
            }

            return target;
        }


        internal static List<TargetInfo> CalculateZombieTargetList(LivingNPC shooter, List<LivingNPC> humans, List<ZombieNPC> zombies)
        {
            var targetList = new List<TargetInfo>();
            foreach (var zombie in zombies)
            {
                foreach (var human in humans)
                {
                    CalculateDistanceAndTarget(targetList, zombie, human);
                }
                CalculateDistanceAndTarget(targetList, zombie, shooter);
            }
            var revisedPriorityList = CalculatePriorities(targetList);
            UpdateNPCTargets(revisedPriorityList);

            return revisedPriorityList;
        }

        private static void CalculateDistanceAndTarget(List<TargetInfo> targetList, ZombieNPC zombie, LivingNPC npc)
        {
            var intersection = FindClosestIntersection(npc.Location, SceneSettings.TargetDiameter, zombie.Location, zombie.NextLocation);
            var distanceNpcToZombie = GetDistance(zombie.Location, npc.Location);
            if (intersection != PointF.Empty)
            {
                targetList.Add(new TargetInfo(npc, zombie, distanceNpcToZombie));
            }
            UpdateClosestDistanceToZombie(npc, distanceNpcToZombie);
        }

        private static void UpdateClosestDistanceToZombie(LivingNPC npc, double distanceNpcToZombie)
        {
            if (npc.DistanceToClosestZombie > distanceNpcToZombie)
            {
                npc.DistanceToClosestZombie = distanceNpcToZombie;
            }
        }

        private static void UpdateNPCTargets(List<TargetInfo> revisedPriorityList)
        {
            foreach (var item in revisedPriorityList)
            {
                item.Zombie.CurrentTarget = item.Target;
                item.Zombie.DistanceToTarget = item.Distance;
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
