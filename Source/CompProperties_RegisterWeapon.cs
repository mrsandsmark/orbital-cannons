using Verse;

namespace Orbital_Cannons
{
    public class CompProperties_RegisterWeapon : CompProperties
    {
        public CompProperties_RegisterWeapon()
        {
            compClass = typeof(CompRegisterWeapon);
        }

        public bool isBeam;

        public bool isBombardment;

        public bool isAntigrain;
    }
}