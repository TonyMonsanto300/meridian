namespace XenWorld.src.Manager
{
    internal class PlayerManager {
        public static bool PartyDead { get; set; } = false;
        public static PlayerController Controller = null;

        public void KillParty() {
            PartyDead = true;
        }
        public static void InitializePlayer() {
            new PlayerSpawner().Spawn();
        }
    }
}
