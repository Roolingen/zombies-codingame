namespace Codingame.Models
{
    using System.Collections.Generic;
    using System.Drawing;
    using Codingame.Constants;
    using Codingame.TargetCalculations;

    internal class BaseNPC
    {
        internal int Id { get; set; }
        internal Point Location { get; set; }
        // internal double DistanceToShooter { get; set; }
        internal NPCType NPCType { get; set; }
    }

    internal class ZombieNPC : BaseNPC
    {
        internal LivingNPC CurrentTarget { get; set; } = new LivingNPC();
        internal Stack<Target> TargetChain { get; set; } = new Stack<Target>();
        // internal double DistanceToTarget { get; set; }
        // internal int TurnsToKill => Targets.GetZombieTurnsToTarget(DistanceToTarget) + 1;
        internal Point NextLocation { get; set; }
    }

    internal class LivingNPC : BaseNPC
    {
        internal List<ZombieNPC> TargetedBy { get; set; } = new List<ZombieNPC>();
        // internal double DistanceToClosestZombie { get; set; }
        // internal int KilledInTurns => Targets.GetZombieTurnsToTarget(DistanceToClosestZombie) + 1;
        // internal int SavedInTurns => Targets.GetZombieTurnsToTarget(DistanceToShooter);
        // internal bool PossibleToSave => SavedInTurns <= KilledInTurns;
    }

    internal class DistanceMatrix
    {
        internal BaseNPC NPCHuman { get; set; } = new BaseNPC();
        internal BaseNPC NPCMixed { get; set; } = new BaseNPC();
        internal double Distance { get; set; }
        internal int TurnsToTarget { get; set; }
    }

    internal class Target
    {
        internal LivingNPC NPC { get; set; } = new LivingNPC();
        internal double Distance { get; set; } = double.MaxValue;
        internal int KilledInTurns;
    }

    internal class TargetInfo
    {
        internal TargetInfo(LivingNPC living, ZombieNPC zombie, double distance) { Target = living; Zombie = zombie; Distance = distance; }
        internal LivingNPC Target { get; set; }
        internal ZombieNPC Zombie { get; set; }
        internal double Distance { get; set; }
    }

    internal class HumanRawStatistic
    {
        internal LivingNPC? Human { get; set; }
        internal ZombieNPC? Zombie { get; set; }

        internal double DistanceZombieToHuman { get; set; }
        internal double DistanceShooterToZombie { get; set; }
        internal double DistanceShooterToHuman { get; set; }

        internal int KilledInTurns => Targets.GetZombieTurnsToTarget(DistanceZombieToHuman) + 1;
        internal int SavedInTurns => Targets.GetZombieTurnsToTarget(DistanceShooterToHuman);
        internal bool PossibleToSave => SavedInTurns <= KilledInTurns;
    }
}
