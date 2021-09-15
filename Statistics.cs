namespace Codingame.Statistics
{
    using System.Collections.Generic;
    using System.Linq;
    using Codingame.Distances;
    using Codingame.Models;

    internal class Stats
    {
        internal static List<NPCStatistic> GenerateStatistics(List<NPC> humans, List<NPC> zombies)
        {
            var npcStatistics = new List<NPCStatistic>();
            foreach (var human in humans)
            {
                foreach (var zombie in zombies)
                {
                    npcStatistics.Add(new NPCStatistic
                    {
                        Human = human,
                        Zombie = zombie,
                        DistanceZombieToHuman = Distances.GetDistance(zombie.Location, human.Location),
                        DistanceShooterToZombie = zombie.DistanceToShooter,
                        DistanceShooterToHuman = human.DistanceToShooter
                    });
                }
            }

            return npcStatistics;
        }

        internal static IEnumerable<NPCStatistic> PrioritizeHumans(List<NPCStatistic> npcStatistics) =>
            npcStatistics
                .Where(x => x.PossibleToSave)
                .OrderBy(x => x.KilledInTurns)
                .ThenBy(x => x.DistanceZombieToHuman)
                .ThenBy(x => x.DistanceShooterToZombie)
                .GroupBy(x => x.Human!.Id)
                .Select(x => x.First());
    }
}
