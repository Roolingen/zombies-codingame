namespace Codingame.InputsProcessing
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Drawing;
    using Codingame.Constants;
    using Codingame.Models;

    internal class Inputs
    {
        internal static List<NPC> GetNPCs(ref string[] inputs, NPCTypes? type = default)
        {
            var zombies = new List<NPC>();
            if (!int.TryParse(Console.ReadLine(), out var zombieCount)) return zombies;

            for (int i = 0; i < zombieCount; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();
                var zombie = new NPC
                {
                    Id = int.Parse(inputs[0]),
                    Location = new Point(int.Parse(inputs[1]), int.Parse(inputs[2])),
                    NextLocation = type == NPCTypes.Zombie
                        ? new Point(int.Parse(inputs[3]), int.Parse(inputs[4]))
                        : (Point?)null
                };
                zombies.Add(zombie);
            }

            return zombies;
        }

        internal static NPC GetShooter(string x, string y) =>
            new NPC
            {
                Location = new Point
                {
                    X = int.Parse(x),
                    Y = int.Parse(y)
                }
            };
    }
}
