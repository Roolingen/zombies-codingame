namespace Codingame.InputsProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Codingame.Constants;
    using Codingame.Models;
    using Codingame.Distances;

    internal class Inputs
    {
        internal static NPC GetShooter(string x, string y) =>
            new NPC
            {
                Id = 0,
                Location = new Point
                {
                    X = int.Parse(x),
                    Y = int.Parse(y)
                },
                DistanceToShooter = 0
            };

        internal static List<NPC> GetNPCs(ref string[] inputs, NPC shooter, NPCTypes? type = default)
        {
            var npcs = new List<NPC>();
            if (!int.TryParse(Console.ReadLine(), out var zombieCount)) return npcs;

            for (int i = 0; i < zombieCount; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();
                var npc = new NPC
                {
                    Id = int.Parse(inputs[0]),
                    Location = new Point(int.Parse(inputs[1]), int.Parse(inputs[2])),
                    NextLocation = type == NPCTypes.Zombie
                        ? new Point(int.Parse(inputs[3]), int.Parse(inputs[4]))
                        : new Point()
                };
                npc.DistanceToShooter = Distances.GetDistance(npc.Location, shooter.Location);
                npcs.Add(npc);
            }

            return npcs;
        }
    }
}
