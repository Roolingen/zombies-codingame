namespace Codingame.Statistics
{
    using System.Collections.Generic;
    using System.Linq;
    using Codingame.Models;

    internal class Stats
    {
        // internal static List<HumanRawStatistic> GenerateRawStatistics(List<HumanNPC> humans, List<ZombieNPC> zombies)
        // {
        //     var npcStatistics = new List<HumanRawStatistic>();
        //     foreach (var human in humans)
        //     {
        //         var killedInTurns = 100;
        //         foreach (var zombie in zombies)
        //         {
        //             var statistic = new HumanRawStatistic
        //             {
        //                 Human = human,
        //                 Zombie = zombie,
        //                 DistanceZombieToHuman = Distances.GetDistance(zombie.Location, human.Location),
        //                 DistanceShooterToZombie = zombie.DistanceToShooter,
        //                 DistanceShooterToHuman = human.DistanceToShooter
        //             };
        //             if (statistic.KilledInTurns < killedInTurns)
        //             {
        //                 killedInTurns = statistic.KilledInTurns;
        //             }
        //             npcStatistics.Add(statistic);
        //         }
        //         human.KillInTurns = killedInTurns;
        //     }

        //     return npcStatistics;
        // }

        internal static void Generate(List<HumanRawStatistic> humanStatistics)
        {
            var a = humanStatistics
                .GroupBy(x => x.KilledInTurns)
                .Select(x => x.Count());

            foreach (var human in humanStatistics)
            {

            }
        }

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
