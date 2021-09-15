namespace Codingame.Statistics
{
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Codingame.Distances;
    using Codingame.Models;

    internal class Stats
    {
        internal static List<NPCStatistic> GenerateStatistics(NPC shooter, List<NPC> humans, List<NPC> zombies)
        {
            var npcStatistics = new List<NPCStatistic>();
            foreach (var human in humans)
            {
                foreach (var zombie in zombies)
                {
                    npcStatistics.Add(new NPCStatistic
                    {
                        HumanId = human.Id,
                        ZombieId = zombie.Id,
                        DistanceZombieToHuman = Distances.GetDistance(zombie.Location, human.Location),
                        DistanceShooterToZombie = Distances.GetDistance(shooter.Location, (Point)zombie.Location),
                        DistanceShooterToHuman = Distances.GetDistance(shooter.Location, human.Location)
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
                .GroupBy(x => x.HumanId)
                .Select(x => x.First());
    }
}
