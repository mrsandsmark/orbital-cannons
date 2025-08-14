using Verse;

namespace Orbital_Cannons
{
    public class DefEditGC : GameComponent
    {
        public DefEditGC(Game game)
        {
        }

        public override void LoadedGame()
        {
            BuildableOrbitalCannon.ApplySettingsNow();
        }

        public override void StartedNewGame()
        {
            BuildableOrbitalCannon.ApplySettingsNow();
        }
    }
}