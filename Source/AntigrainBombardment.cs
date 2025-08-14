using Verse;
using Verse.Sound;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Orbital_Cannons
{
    [StaticConstructorOnStartup]
    public class AntigrainBombardment : Bombardment
    {
        //Mostly just cloned from vanilla, to prevent the vanilla one to run

        public new float impactAreaRadius = 1f; //Area where projectiles can impact

        public new FloatRange explosionRadiusRange = new FloatRange(14f, 16f);  //Explosion size min/max

        public new int bombIntervalTicks = 18;  //Time in ticks between shots

        public new int explosionCount = 1;  //Number of shots

	    private int ticksToNextEffect;

        private IntVec3 nextExplosionCell = IntVec3.Invalid;

        private List<BombardmentProjectile> projectiles = new List<BombardmentProjectile>();

        private const int StartRandomFireEveryTicks = 20;

        private const int EffectDuration = 60;  //Time in ticks that a projectile will take from start to finish

        private static readonly Material ProjectileMaterial = MaterialPool.MatFrom("Things/Projectile/ShellAntigrainWarhead", ShaderDatabase.Transparent, Color.white); //Projectile texture set to antigrain shell

        private int explosionsDone = 0;

        public override void StartStrike()
        {
            base.StartStrike();
            duration = EffectDuration * explosionCount;
        }

        protected override void Tick()
        {
            if (Destroyed)
            {
                return;
            }
            if (warmupTicks > 0)
            {
                warmupTicks--;
                if (warmupTicks <= 0)
                {
                    StartStrike();
                }
            }
            else
            {
                if (TicksPassed >= duration)
                {
                    Destroy();
                }
                if (TicksLeft > 0 && this.IsHashIntervalTick(20))
                {
                    StartRandomFire();
                }
            }
            EffectTick();
        }

        private void EffectTick()
        {
            if (!nextExplosionCell.IsValid)
            {
                ticksToNextEffect = warmupTicks - bombIntervalTicks;
                GetNextExplosionCell();
            }
            ticksToNextEffect--;
            if (ticksToNextEffect <= 0 && TicksLeft >= bombIntervalTicks && explosionsDone < explosionCount)    //If all projectiles already launched, don't launch more
            {
                SoundDefOf.Bombardment_PreImpact.PlayOneShot(new TargetInfo(nextExplosionCell, Map));
                projectiles.Add(new BombardmentProjectile(60, nextExplosionCell));
                ticksToNextEffect = bombIntervalTicks;
                explosionsDone++;
                GetNextExplosionCell();
            }
            for (int num = projectiles.Count - 1; num >= 0; num--)
            {
                projectiles[num].Tick();
                if (projectiles[num].LifeTime <= 0)
                {
                    TryDoExplosion(projectiles[num]);
                    projectiles.RemoveAt(num);
                }
            }
        }

        private void TryDoExplosion(BombardmentProjectile proj)
        {
            List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].TryGetComp<CompProjectileInterceptor>().CheckBombardmentIntercept(this, proj))
                {
                    return;
                }
            }
            Effecter effecter = OCDefOfs.GiantExplosion.Spawn();    //Explosion effect of antigrain shell
            effecter.Trigger(new TargetInfo(base.Position, Map), new TargetInfo(base.Position, Map));
			effecter.Cleanup();
            GenExplosion.DoExplosion(proj.targetCell, base.Map, explosionRadiusRange.RandomInRange, OCDefOfs.BombSuper, instigator, projectile: def, weapon: weaponDef, preExplosionSpawnSingleThingDef: ThingDefOf.CraterMedium, explosionSound: OCDefOfs.Explosion_GiantBomb, chanceToStartFire: 0.22f);
            //Explosion generated with parameters closest to antigrain shell
        }

        protected override void DrawAt(Vector3 drawLoc, bool flip = false)
        {
            Comps_PostDraw();
            if (!projectiles.NullOrEmpty())
            {
                for (int i = 0; i < projectiles.Count; i++)
                {
                    projectiles[i].Draw(ProjectileMaterial);
                }
            }
        }

        private void StartRandomFire()
        {
            IntVec3 intVec = (from x in GenRadial.RadialCellsAround(base.Position, randomFireRadius, useCenter: true)
                where x.InBounds(base.Map)
                select x).RandomElementByWeight((IntVec3 x) => DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position)));
            List<Thing> list = base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ProjectileInterceptor);
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].TryGetComp<CompProjectileInterceptor>().BombardmentCanStartFireAt(this, intVec))
                {
                    return;
                }
            }
            RoofDef roof = intVec.GetRoof(base.Map);
            if (roof == null || !roof.isThickRoof)
            {
                FireUtility.TryStartFireIn(intVec, base.Map, Rand.Range(0.1f, 0.925f), instigator);
            }
        }

        private void GetNextExplosionCell()
        {
            nextExplosionCell = (from x in GenRadial.RadialCellsAround(base.Position, impactAreaRadius, useCenter: true)
                where x.InBounds(base.Map)
                select x).RandomElementByWeight((IntVec3 x) => DistanceChanceFactor.Evaluate(x.DistanceTo(base.Position) / impactAreaRadius));
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref impactAreaRadius, "impactAreaRadius", 1f);
            Scribe_Values.Look(ref explosionRadiusRange, "explosionRadiusRange", new FloatRange(14f, 16f));
            Scribe_Values.Look(ref randomFireRadius, "randomFireRadius", 0);
            Scribe_Values.Look(ref explosionsDone, "explosionsDone", 0);
        }
    }
}