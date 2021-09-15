namespace Codingame.InputsProcessing
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Codingame.Constants;
    using Codingame.Models;

    internal class Inputs
    {
        internal static LivingNPC GetShooter(string x, string y)
        {
            return new LivingNPC
            {
                Id = SceneSettings.ShooterId,
                Location = new Point
                {
                    X = int.Parse(x),
                    Y = int.Parse(y)
                }
            };
        }

        internal static List<T> GetNPCs<T>(ref string[] inputs) where T : BaseNPC
        {
            var npcs = new List<T>();
            if (!int.TryParse(Console.ReadLine(), out var npcCount)) return npcs;

            for (int i = 0; i < npcCount; i++)
            {
                inputs = Console.ReadLine()?.Split(' ') ?? Array.Empty<string>();

                var npc = Activator.CreateInstance<T>();
                npc.Id = int.Parse(inputs[0]);
                npc.Location = new Point(int.Parse(inputs[1]), int.Parse(inputs[2]));

                if (npc is ZombieNPC zombieNPC)
                {
                    zombieNPC.NextLocation = new Point(int.Parse(inputs[3]), int.Parse(inputs[4]));
                }

                npcs.Add(npc);
            }

            return npcs;
        }
    }
}
