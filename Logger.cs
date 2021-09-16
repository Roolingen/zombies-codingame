namespace Codingame.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Codingame.Models;

    internal class Log
    {
        internal static void Write<T>(T o)
        {
            Console.Error.WriteLine(JsonSerializer.Serialize(o));
        }

        internal static void WriteTargets(IEnumerable<ZombieNPC> zombies)
        {
            foreach (var zombie in zombies)
            {
                Write($"Zombie{zombie.Id} target chain:");
                WriteTargets(zombie.TargetChain);
            }
        }

        internal static void WriteTargets(IEnumerable<Target> list)
        {
            foreach (var item in list)
            {
                Console.Error.WriteLine($"Target: {item.NPC.Id}, turns to target: {item.KilledInTurns}, distance: {(int)item.Distance}");
            }
        }

        internal static void WriteHoomans(string whatHoomans, IEnumerable<LivingNPC> list)
        {
            Write(whatHoomans);
            foreach (var item in list)
            {
                Console.Error.WriteLine($"Hooman: {item.Id}.");
            }
        }

        internal static void WriteMatrix(List<DistanceMatrix> distanceMatrix)
        {
            foreach (var item in distanceMatrix)
            {
                Console.Error.WriteLine($"{item.NPCHuman.NPCType}{item.NPCHuman.Id}, {item.NPCMixed.NPCType}{item.NPCMixed.Id}, {item.TurnsToTarget}, {(int)item.Distance}");
            }
        }
    }
}
