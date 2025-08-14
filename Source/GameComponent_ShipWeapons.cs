using Verse;
using RimWorld;
using System.Collections.Generic;

namespace Orbital_Cannons
{
    public class GameComponent_ShipWeapons : GameComponent
    {
        public List<Thing> beamWeapons = new List<Thing>();

        public List<Thing> antigrainWeapons = new List<Thing>();

        public List<Thing> bombardWeapons = new List<Thing>();

        public bool inSpace = false;

        public Map shipMap = null;

        public GameComponent_ShipWeapons(Game game)
        {
        }

        public void RegisterWeapon(CompRegisterWeapon weapon)    //If fits in category, add to list and set appropriate map
        {
            ModLog.Log("Registering weapon");
            if(weapon.Props.isBeam)
            {
                ModLog.Log("Registered beam weapon");
                beamWeapons.Add(weapon.parent);

                if(shipMap == null)
                {
                    shipMap = weapon.parent.Map;
                }
            }
            else if(weapon.Props.isBombardment)
            {
                bombardWeapons.Add(weapon.parent);

                if(shipMap == null)
                {
                    shipMap = weapon.parent.Map;
                }
            }
            else if(weapon.Props.isAntigrain)
            {
                antigrainWeapons.Add(weapon.parent);

                if(shipMap == null)
                {
                    shipMap = weapon.parent.Map;
                }
            }
            else
            {
                ModLog.Error("Could not register weapon "+weapon.parent.Label+". CompProperties_RegisterWeapon was not defined correctly.");    //Else throw error
            }
        }

        public void DeregisterWeapon(CompRegisterWeapon weapon)    //Same as above, just remove
        {
            if(weapon.Props.isBeam)
            {
                beamWeapons.Remove(weapon.parent);
            }
            else if(weapon.Props.isBombardment)
            {
                bombardWeapons.Remove(weapon.parent);
            }
            else if(weapon.Props.isAntigrain)
            {
                antigrainWeapons.Remove(weapon.parent);
            }
            else
            {
                ModLog.Error("Could not deregister weapon "+weapon.parent.Label+". CompProperties_RegisterWeapon was not defined correctly.");    //Else throw error
            }
        }
    }
}