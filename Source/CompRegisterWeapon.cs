using Verse;
using RimWorld;
using System.Collections.Generic;

namespace Orbital_Cannons
{
    public class CompRegisterWeapon : ThingComp
    {
        public CompProperties_RegisterWeapon Props => (CompProperties_RegisterWeapon)props;

        public GameComponent_ShipWeapons WeaponComp
        {
            get
            {
                return Current.Game.GetComponent<GameComponent_ShipWeapons>();
            }
        }
        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            WeaponComp.RegisterWeapon(this);  //Register on spawn

            if(parent.Map.Biome == BiomeDefOf.Orbit)
            {
                WeaponComp.inSpace = true;  //If in space, set to true
            }
        }

        public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
        {
            base.PostDeSpawn(map, mode);

            WeaponComp.DeregisterWeapon(this);    //Deregister on despawn
        }

        public override void PostSwapMap()
        {
            base.PostSwapMap();

            if(parent.Map.Biome == BiomeDefOf.Orbit)
            {
                WeaponComp.inSpace = true;  //If changed to space, set to true
                WeaponComp.shipMap = parent.Map;    //And set map to current map
            }
            else if(parent.Map.Biome != BiomeDefOf.Orbit)
            {
                WeaponComp.inSpace = false; //If no longer in space, set to false
                WeaponComp.shipMap = parent.Map;
            }
        }
    }
}