using Verse;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orbital_Cannons
{
    public class CompApparelReloadableTargeter : CompApparelReloadable
    {
        private Verb selectedVerb;

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

            selectedVerb = VerbTracker.AllVerbs.First();    //Setting default verb
        }

        public override bool CanBeUsed(out string reason)   //Checking if gizmo disabled
        {
            reason = "";

            if(!base.CanBeUsed(out reason))
            {
                return false;
            }

            if(WeaponComp == null)
            {
                ModLog.Error("No GameComponent_ShipWeapons found");
                return false;
            }

            if(WeaponComp != null)
            {
                if(selectedVerb.verbProps.verbClass.ToString() == parent.def.GetModExtension<BombardmentExtension>().beamVerb)  //Checking each verb
                {
                    if(WeaponComp.beamWeapons.NullOrEmpty())    //If weapon list is empty
                    {
                        reason = "OC_BeamNotFound".Translate();
                        return false;
                    }
                    if(!WeaponComp.beamWeapons.Any((Thing thing) => thing.TryGetComp<CompPowerTrader>().PowerOn))   //If no weapons have power
                    {
                        reason = "OC_BeamNotOnline".Translate();
                        return false;
                    }
                }
                if(selectedVerb.verbProps.verbClass.ToString() == parent.def.GetModExtension<BombardmentExtension>().bombardmentVerb)
                {
                    if(WeaponComp.bombardWeapons.NullOrEmpty())
                    {
                        reason = "OC_BombardmentNotFound".Translate();
                        return false;
                    }
                    if(!WeaponComp.bombardWeapons.Any((Thing thing) => thing.TryGetComp<CompPowerTrader>().PowerOn))
                    {
                        reason = "OC_BombardmentNotOnline".Translate();
                        return false;
                    }
                }
                if(selectedVerb.verbProps.verbClass.ToString() == parent.def.GetModExtension<BombardmentExtension>().antigrainVerb)
                {
                    if(WeaponComp.antigrainWeapons.NullOrEmpty())
                    {
                        reason = "OC_AntigrainNotFound".Translate();
                        return false;
                    }
                    if(!WeaponComp.antigrainWeapons.Any((Thing thing) => thing.TryGetComp<CompPowerTrader>().PowerOn))
                    {
                        reason = "OC_AntigrainNotOnline".Translate();
                        return false;
                    }
                }

                if(!WeaponComp.inSpace) //If ship is not in space
                {
                    reason = "OC_ShipNotInSpace".Translate();
                    return false;
                }

                if(WeaponComp.shipMap == parent.MapHeld)    //If ship is on the same map
                {
                    reason = "OC_ShipOnMap".Translate();
                    return false;
                }
            }

            return true;
        }

        public override IEnumerable<Gizmo> CompGetWornGizmosExtra()
        {
            if (DebugSettings.ShowDevGizmos)
            {
                Command_Action command_Action = new Command_Action();
                command_Action.defaultLabel = "DEV: Reload to full";
                command_Action.action = delegate
                {
                    remainingCharges = MaxCharges;
                };
                yield return command_Action;
            }

            bool drafted = Wearer.Drafted;
            if ((drafted && !Props.displayGizmoWhileDrafted) || (!drafted && !Props.displayGizmoWhileUndrafted))
            {
                yield break;
            }

            if(VerbTracker.AllVerbs.Count > 1)  //If there is more than one verb
            {
                yield return new Command_Action //New gizmo ...
                {
                    defaultLabel = "Select weapon",
                    action = delegate
                    {
                        List<FloatMenuOption> list = new List<FloatMenuOption>();   //... with a pop-up menu ...
                        foreach (Verb allVerb in VerbTracker.AllVerbs)  //... of all possible verbs
                        {
                            list.Add(new FloatMenuOption(allVerb.verbProps.label, delegate
                            {
                                selectedVerb = allVerb;
                            }));
                        }
                        Find.WindowStack.Add(new FloatMenu(list));
                    },
                    icon = ContentFinder<Texture2D>.Get("UI/Icons/SelectWeapon"),  //Path to the icon of the gizmo
                };

                yield return CreateVerbTargetCommand(parent, selectedVerb); //Then make the actual gizmo for the bombardment (same as vanilla)
            }
            else
            {
                yield return CreateVerbTargetCommand(parent, selectedVerb);
            }
        }

        private Command_VerbTarget CreateVerbTargetCommand(Thing gear, Verb verb)
        {
            Command_VerbOwner command_VerbOwner = new Command_VerbOwner(this);
            command_VerbOwner.defaultDesc = gear.def.description;
            command_VerbOwner.hotKey = Props.hotKey;
            command_VerbOwner.defaultLabel = verb.verbProps.label;
            command_VerbOwner.verb = verb;
            if (verb.verbProps.defaultProjectile != null && verb.verbProps.commandIcon == null)
            {
                command_VerbOwner.icon = verb.verbProps.defaultProjectile.uiIcon;
                command_VerbOwner.iconAngle = verb.verbProps.defaultProjectile.uiIconAngle;
                command_VerbOwner.iconOffset = verb.verbProps.defaultProjectile.uiIconOffset;
                command_VerbOwner.overrideColor = verb.verbProps.defaultProjectile.graphicData.color;
            }
            else
            {
                command_VerbOwner.icon = ((verb.UIIcon != BaseContent.BadTex) ? verb.UIIcon : gear.def.uiIcon);
                command_VerbOwner.iconAngle = gear.def.uiIconAngle;
                command_VerbOwner.iconOffset = gear.def.uiIconOffset;
                command_VerbOwner.defaultIconColor = gear.DrawColor;
            }
            string reason;
            if (!Wearer.IsColonistPlayerControlled)
            {
                command_VerbOwner.Disable("CannotOrderNonControlled".Translate());
            }
            else if (verb.verbProps.violent && Wearer.WorkTagIsDisabled(WorkTags.Violent))
            {
                command_VerbOwner.Disable("IsIncapableOfViolenceLower".Translate(Wearer.LabelShort, Wearer).CapitalizeFirst() + ".");
            }
            else if (!CanBeUsed(out reason))
            {
                command_VerbOwner.Disable(reason);
            }
            return command_VerbOwner;
        }
    }
}