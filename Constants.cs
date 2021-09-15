namespace Codingame.Constants
{
    internal static class AreaDimensions
    {
        const int Width = 16_000;
        const int Height = 9_000;
    }

    internal enum NPCTypes
    {
        Shooter,
        Human,
        Zombie
    }

    internal static class Ranges
    {
        public const int ShooterMove = 1_000;
        public const int ShooterKill = 2_000;
        public const int ZombieMove = 400;
        public const int ZombieKill = 400;
    }
}
