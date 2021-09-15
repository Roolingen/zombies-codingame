namespace Codingame.Models
{
    using System;
    using System.Drawing;
    using Codingame.Constants;

    internal class ShooterNPC
    {
        public Point Location { get; set; }
    }

    internal class HumanNPC : ShooterNPC
    {
        public int Id { get; set; }
        public int DistanceToShooter { get; set; }
    }

    internal class ZombieNPC : HumanNPC
    {
        public Point NextLocation { get; set; }
        public PointF ClosestIntersectionNext { get; set; }
    }

    internal class HumanRawStatistic
    {
        public HumanNPC? Human { get; set; }
        public ZombieNPC? Zombie { get; set; }

        public int DistanceZombieToHuman { get; set; }
        public int DistanceShooterToZombie { get; set; }
        public int DistanceShooterToHuman { get; set; }

        public int KilledInTurns => (int)Math.Ceiling((DistanceZombieToHuman - Ranges.ZombieKill) / (double)Ranges.ZombieMove) + 1;
        public int SavedInTurns => (int)Math.Ceiling((DistanceShooterToHuman - Ranges.ShooterKill) / (double)Ranges.ShooterMove);
        public bool PossibleToSave => SavedInTurns <= KilledInTurns;
    }
}
