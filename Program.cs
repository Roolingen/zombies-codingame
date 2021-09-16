namespace Codingame
{
    using System;
    using System.Linq;
    using Codingame.Constants;
    using Codingame.InputsProcessing;
    using Codingame.Logger;
    using Codingame.Models;
    using Codingame.PriorityProcessor;
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
                var zombies = Inputs.GetNPCs<ZombieNPC>(ref inputs);

                var zombieMatrix = Targets.GetZombieDistanceMatrix(shooter, humans, zombies);
                var shooterMatrix = Targets.GetShooterDistanceMatrix(shooter, humans, zombies);

                var humanKillList = Priorities.GetKillCountDownForAll(zombieMatrix);
                var lastToDie = humanKillList.First();
                Log.WriteMatrix(humanKillList);

                var lastToSave = shooterMatrix.First(x => x.SecondaryNPC.NPCType == NPCType.Human && x.SecondaryNPC.Id == lastToDie.SecondaryNPC.Id);
                Log.WriteMatrixItem(lastToSave);

                if (lastToDie.TurnsToTarget <= lastToSave.TurnsToTarget)
                {
                    Console.WriteLine($"{lastToDie.SecondaryNPC!.Location.X} {lastToDie.SecondaryNPC!.Location.Y}"); // Your destination coordinates
                }
                else
                {
                    Console.WriteLine($"{shooter!.Location.X} {shooter!.Location.Y}"); // Your destination coordinates
                }
            }
        }
    }
}
