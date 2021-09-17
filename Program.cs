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

                var lastToDie = Priorities.GetKillCountDownForAll(zombieMatrix).ToList().First();
                var lastToSave = shooterMatrix.First(x => x.SecondaryNPC.NPCType == NPCType.Human && x.SecondaryNPC.Id == lastToDie.SecondaryNPC.Id);

                var target = Priorities.GetNextCoordinates(zombieMatrix);
                if (Targets.GetDistance(lastToSave.SecondaryNPC.Location, target) > lastToSave.Distance && zombies.Count() > 1)
                {
                    target = Targets.GetMaxDistanceFromTarget(lastToDie, lastToSave, target);
                }
                Log.Write($"-----------------------------");
                Log.Write($"lastToDie.SecondaryNPC.Id: {lastToDie.SecondaryNPC.Id}.");
                Log.Write($"lastToDie.TurnsToTarget: {lastToDie.TurnsToTarget}.");
                Log.Write($"lastToSave.SecondaryNPC.Id: {lastToSave.SecondaryNPC.Id}.");
                Log.Write($"lastToSave.TurnsToTarget: {lastToSave.TurnsToTarget}.");
                Log.Write($"-----------------------------");

                Console.WriteLine(lastToDie.TurnsToTarget > lastToSave.TurnsToTarget
                        ? $"{target.X} {target.Y}"
                        : $"{lastToDie.SecondaryNPC!.Location.X} {lastToDie.SecondaryNPC!.Location.Y}");
            }
        }
    }
}
