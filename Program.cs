namespace Codingame
{
    using System;
    using Codingame.InputsProcessing;
    using Codingame.Logger;
    using Codingame.Models;
    using Codingame.TargetCalculations;

    internal class Player
    {
        public static void Main(string[] args)
        {
            string[] inputs;

            while (true)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();

                var shooter = Inputs.GetShooter(inputs[0], inputs[1]);
                var humans = Inputs.GetNPCs<LivingNPC>(ref inputs);
                humans.Add(shooter);
                var zombies = Inputs.GetNPCs<ZombieNPC>(ref inputs);

                var distanceMatrix = Targets.GetDistanceMatrix(humans, zombies);
                Targets.CalculateZombieTargetChains(zombies, humans, distanceMatrix);
                Log.WriteTargets(zombies);

                Console.WriteLine($"{shooter!.Location.X} {shooter!.Location.Y}"); // Your destination coordinates
            }
        }
    }
}
