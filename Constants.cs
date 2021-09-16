namespace Codingame.Constants
{
    internal static class SceneSettings
    {
        internal const int Width = 16_000;
        internal const int Height = 9_000;
        internal const int ShooterId = int.MaxValue;
        internal const float TargetDiameter = 40f;
    }

    internal static class Ranges
    {
        internal const int ShooterMove = 1_000;
        internal const int ShooterKill = 2_000;
        internal const int ZombieMove = 400;
        internal const int ZombieKill = 400;
    }

    internal enum NPCType
    {
        Shooter,
        Human,
        Zombie
    }
}
