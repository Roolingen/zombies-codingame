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

        internal static void WriteHoomans(string whatHoomans, IEnumerable<LivingNPC> list)
        {
            Write(whatHoomans);
            foreach (var item in list)
            {
                Console.Error.WriteLine($"Hooman: {item.Id}.");
            }
        }

        internal static void WriteMatrix(IEnumerable<DistanceMatrix> distanceMatrix)
        {
            foreach (var item in distanceMatrix)
            {
                WriteMatrixItem(item);
            }
        }

        internal static void WriteMatrixItem(DistanceMatrix item)
        {
            Console.Error.WriteLine($"{item.PrimaryNPC.NPCType}{item.PrimaryNPC.Id} can reach {item.SecondaryNPC.NPCType}{item.SecondaryNPC.Id} in {item.TurnsToTarget} turns ({(int)item.Distance} units away)");
        }
    }
}
