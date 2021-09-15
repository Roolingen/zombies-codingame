namespace Codingame
{
    using System;
    using Codingame.Constants;
    using Codingame.InputsProcessing;
    using Codingame.Statistics;

    internal class Player
    {
        public static void Main(string[] args)
        {
            string[] inputs;

            // game loop
            while (true)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();

                var shooter = Inputs.GetShooter(inputs[0], inputs[1]);
                var humans = Inputs.GetNPCs(ref inputs);
                var zombies = Inputs.GetNPCs(ref inputs, NPCTypes.Zombie);

                var npcStatistics = Stats.GenerateStatistics(shooter, humans, zombies);

                var priorityList = npcStatistics.OrderBy(x => x.KilledInTurns)
                    .GroupBy(x => x.HumanId)
                    .Select(x => x.First()).ToList();

                var prioritizedHumans = Stats.PrioritizeHumans(npcStatistics);

                var zombieId = prioritizedHumans.First().ZombieId;


                // Write an action using Console.WriteLine()
                // Console.Error.WriteLine("{humans}", humans);

                var zombieToKill = zombies.First(x => x.Id == zombieId);
                Console.WriteLine($"{zombieToKill.Location.X} {zombieToKill.Location.Y}"); // Your destination coordinates
            }
        }
    }
}
