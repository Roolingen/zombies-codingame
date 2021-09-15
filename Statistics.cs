namespace Codingame.Statistics
{
    using System.Collections.Generic;
    using System.Linq;
    using Codingame.Models;

    internal class Stats
    {
        internal static IEnumerable<HumanRawStatistic> PrioritizeHumans(List<HumanRawStatistic> npcStatistics) =>
            npcStatistics
                .Where(x => x.PossibleToSave)
                .OrderBy(x => x.KilledInTurns)
                .ThenBy(x => x.DistanceZombieToHuman)
                .ThenBy(x => x.DistanceShooterToZombie)
                .GroupBy(x => x.Human!.Id)
                .Select(x => x.First());
    }
}
