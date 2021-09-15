namespace Codingame.InputsProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Codingame.Constants;
    using Codingame.Models;
    using Codingame.DistanceCalculations;
    using System.Threading.Tasks;

    internal class Inputs
    {
        internal static HumanNPC GetShooter(string x, string y) =>
            new HumanNPC
            {
                Id = 0,
                Location = new Point
                {
                    X = int.Parse(x),
                    Y = int.Parse(y)
                },
                DistanceToShooter = 0
            };

        internal static List<T> GetNPCs<T>(ref string[] inputs, ShooterNPC shooter) where T : HumanNPC
        {
            var npcs = new List<T>();
            if (!int.TryParse(Console.ReadLine(), out var npcCount)) return npcs;

            for (int i = 0; i < npcCount; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();

                var npc = Activator.CreateInstance<T>();
                npc.Id = int.Parse(inputs[0]);
                npc.Location = new Point(int.Parse(inputs[1]), int.Parse(inputs[2]));
                npc.DistanceToShooter = Distances.GetDistance(npc.Location, shooter.Location);

                if (npc is ZombieNPC zombieNPC)
                {
                    zombieNPC.NextLocation = new Point(int.Parse(inputs[3]), int.Parse(inputs[4]));
                    zombieNPC.ClosestIntersectionNext = Distances.FindClosestIntersection(zombieNPC.NextLocation, Ranges.ShooterKill, shooter.Location, zombieNPC.NextLocation);
                }

                npcs.Add(npc);
            }

            return npcs;
        }
    }
}
