namespace Codingame.Models
{
    using System;
    using System.Drawing;
    using Codingame.Constants;

    internal class NPC
    {
        public int Id { get; set; }
        public Point Location { get; set; }
        public Point? NextLocation { get; set; }
    }

    internal class NPCStatistic
    {
        public int HumanId { get; set; }
        public int ZombieId { get; set; }
        public int DistanceZombieToHuman { get; set; }
        public int KilledInTurns => (int)Math.Ceiling((DistanceZombieToHuman - Ranges.ZombieKill) / (double)Ranges.ZombieMove) + 1;
        public int DistanceShooterToZombie { get; set; }
        public int SavedInTurns => (int)Math.Ceiling((DistanceShooterToHuman - Ranges.ShooterKill) / (double)Ranges.ShooterMove);
        public bool PossibleToSave => SavedInTurns <= KilledInTurns;
        public int DistanceShooterToHuman { get; set; }
    }
}
