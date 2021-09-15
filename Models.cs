namespace Codingame.Models
{
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using Codingame.Constants;

    internal class BaseNPC
    {
        internal int Id { get; set; }
        internal Point Location { get; set; }
        internal double DistanceToShooter { get; set; }
    }

    internal class ZombieNPC : BaseNPC
    {
        internal LivingNPC? CurrentTarget { get; set; }
        internal double DistanceToTarget { get; set; }
        internal Point NextLocation { get; set; }
    }

    internal class LivingNPC : BaseNPC
    {
        public List<ZombieNPC> TargetedBy { get; set; } = new List<ZombieNPC>();
    }

    internal class HumanRawStatistic
    {
        public LivingNPC? Human { get; set; }
        public ZombieNPC? Zombie { get; set; }

        public int DistanceZombieToHuman { get; set; }
        public int DistanceShooterToZombie { get; set; }
        public int DistanceShooterToHuman { get; set; }

        public int KilledInTurns => (int)Math.Ceiling((DistanceZombieToHuman - Ranges.ZombieKill) / (double)Ranges.ZombieMove) + 1;
        public int SavedInTurns => (int)Math.Ceiling((DistanceShooterToHuman - Ranges.ShooterKill) / (double)Ranges.ShooterMove);
        public bool PossibleToSave => SavedInTurns <= KilledInTurns;
    }
}
