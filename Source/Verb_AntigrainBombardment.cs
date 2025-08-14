using Verse;
using RimWorld;
using Verse.AI;

namespace Orbital_Cannons
{
    public class Verb_AntigrainBombardment : Verb_CastBase
    {
        public const int DurationTicks = 60;

        protected override bool TryCastShot()
        {
            if (currentTarget.HasThing && currentTarget.Thing.Map != caster.Map)
            {
                return false;
            }
            AntigrainBombardment obj = (AntigrainBombardment)GenSpawn.Spawn(OCDefOfs.OC_AntigrainBombardment, currentTarget.Cell, caster.Map);  //Clone of vanilla Verb_Bombardment, just changed the THing spawned
            obj.duration = 60;
            obj.instigator = caster;
            obj.weaponDef = ((base.EquipmentSource != null) ? base.EquipmentSource.def : null);
            base.ReloadableCompSource?.UsedOnce();
            return true;
        }

        public override float HighlightFieldRadiusAroundTarget(out bool needLOSToCenter)
        {
            needLOSToCenter = false;
            return 16f; //And changed the targeting target circle size
        }
    }
}