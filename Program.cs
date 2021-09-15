namespace Codingame
{
    using System;
    using System.Linq;
    using Codingame.Constants;
    using Codingame.InputsProcessing;
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
                var humans = Inputs.GetNPCs(ref inputs, shooter);
                var zombies = Inputs.GetNPCs(ref inputs, shooter, NPCTypes.Zombie);

                var npcStatistics = Stats.GenerateStatistics(humans, zombies);

                var prioritizedHumans = Stats.PrioritizeHumans(npcStatistics);

                var target = prioritizedHumans.First().Zombie;

                Console.WriteLine($"{target.NextLocation.X} {target.NextLocation.Y}"); // Your destination coordinates
            }
        }
    }
}
