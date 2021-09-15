namespace Codingame.Logger
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json;
    using Codingame.Models;

    internal class Log
    {
        internal static void Write(object o)
        {
            Console.Error.WriteLine(JsonSerializer.Serialize(o));
        }
        internal static void CurrentTargetInfo(LivingNPC shooter, List<LivingNPC> humans, List<ZombieNPC> zombies)
        {
            foreach (var commingZombie in shooter.TargetedBy)
            {
                Write($"Shooter is targeted by zombie (Id:{commingZombie.Id}). Distance: {commingZombie.DistanceToTarget}");
            }
            foreach (var human in humans)
            {
                foreach (var commingZombie in human.TargetedBy)
                {
                    Write($"Human (Id:{human.Id}) is targeted by zombie (Id:{commingZombie.Id}). Distance: {commingZombie.DistanceToTarget}");
                }
            }
            foreach (var zombie in zombies)
            {
                Write($"Zombie (Id:{zombie.Id}) current target id:{zombie.CurrentTarget?.Id}). Distance: {zombie.DistanceToTarget}");
            }
        }
    }
}
