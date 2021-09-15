namespace Codingame
{
    using System;
    using System.Linq;
    using Codingame.Constants;
    using Codingame.DistanceCalculations;
    using Codingame.InputsProcessing;
    using Codingame.Logger;
    using Codingame.Models;
    using Codingame.Statistics;

    internal class Player
    {
        public static void Main(string[] args)
        {
            string[] inputs;

            while (true)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();

                var shooter = Inputs.GetShooter(inputs[0], inputs[1]);
                var humans = Inputs.GetNPCs<HumanNPC>(ref inputs, shooter);
                var zombies = Inputs.GetNPCs<ZombieNPC>(ref inputs, shooter);

                var humanStatistics = Stats.GenerateRawStatistics(humans, zombies);
                var zombie = zombies.First();
                var i1 = Distances.FindClosestIntersection(zombie.Location, Ranges.ShooterKill, shooter.Location, zombie.Location);
                Log.Write(i1);

                var prioritizedHumans = Stats.PrioritizeHumans(humanStatistics);

                var target = prioritizedHumans.First().Zombie;

                Console.WriteLine($"{target!.Location.X} {target!.Location.Y}"); // Your destination coordinates
            }
        }
    }
}
