namespace Codingame.PriorityProcessor
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Codingame.Constants;
    using Codingame.Models;
    using Codingame.TargetCalculations;

    internal class Priorities
    {
        internal static IEnumerable<DistanceMatrix> GetKillCountDownForAll(List<DistanceMatrix> distanceMatrix)
        {
            return distanceMatrix
                .OrderBy(x => x.TurnsToTarget)
                .GroupBy(x => x.SecondaryNPC)
                .Select(x => x.First())
                .OrderByDescending(x => x.TurnsToTarget);
        }

        internal static DistanceMatrix GetSaveCountDown(List<DistanceMatrix> distanceMatrix, BaseNPC source, BaseNPC target)
        {
            return distanceMatrix.First(x => x.SecondaryNPC.Id == source.Id && x.PrimaryNPC.Id == target.Id || x.SecondaryNPC.Id == target.Id && x.PrimaryNPC.Id == source.Id);
        }

        internal static Point GetNextCoordinates(List<DistanceMatrix> zombieMatrix)
        {
            if (zombieMatrix.Count() < 2) return zombieMatrix.First().PrimaryNPC.Location;

            var orderedList = zombieMatrix
                .OrderBy(x => x.TurnsToTarget)
                .ThenBy(x => x.Distance);

            var closestZombieToHumanMatrix = orderedList.First();
            var secondClosestZombieToHumanMatrix = orderedList.ElementAt(1);

            var closestZombie = closestZombieToHumanMatrix.PrimaryNPC as ZombieNPC;
            var secondClosestZombie = secondClosestZombieToHumanMatrix.PrimaryNPC as ZombieNPC;
            return Targets.GetCoordinates(closestZombie!.NextLocation, secondClosestZombie!.NextLocation, Ranges.ShooterKill * 0.9f);
        }
    }
}
