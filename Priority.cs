namespace Codingame.PriorityProcessor
{
    using System.Collections.Generic;
    using System.Linq;
    using Codingame.Models;

    internal class Priorities
    {
        internal static IEnumerable<DistanceMatrix> GetKillCountDownForAll(List<DistanceMatrix> distanceMatrix)
        {
            return distanceMatrix
                .Where(x => x.TurnsToTarget >= x.TurnsToSave)
                .OrderBy(x => x.TurnsToTarget)
                .GroupBy(x => x.SecondaryNPC)
                .Select(x => x.First());
        }

        internal static DistanceMatrix GetSaveCountDown(List<DistanceMatrix> distanceMatrix, BaseNPC source, BaseNPC target)
        {
            return distanceMatrix.First(x => x.SecondaryNPC.Id == source.Id && x.PrimaryNPC.Id == target.Id || x.SecondaryNPC.Id == target.Id && x.PrimaryNPC.Id == source.Id);
        }
    }
}
