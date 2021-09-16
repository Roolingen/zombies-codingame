namespace Codingame.Models
{
    using System.Drawing;
    using Codingame.Constants;

    internal class BaseNPC
    {
        internal int Id { get; set; }
        internal Point Location { get; set; }
        internal NPCType NPCType { get; set; }
    }

    internal class ZombieNPC : BaseNPC
    {
        internal Point NextLocation { get; set; }
    }

    internal class LivingNPC : BaseNPC { }

    internal class DistanceMatrix
    {
        internal BaseNPC PrimaryNPC { get; set; } = new BaseNPC();
        internal BaseNPC SecondaryNPC { get; set; } = new BaseNPC();
        internal double Distance { get; set; }
        internal int TurnsToTarget { get; set; }
        public int TurnsToSave { get; set; }
    }
}
