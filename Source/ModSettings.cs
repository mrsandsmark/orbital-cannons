using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

[StaticConstructorOnStartup]
public class CannonSettings : Verse.ModSettings
{
        // Gauss Cannon settings
        public float gaussCannon_Powerusage = 1000f;
        public float gaussCannon_Range = 56;
        public float gaussCannon_Firerate = 5f;
        public float gaussCannon_Burst = 1f;
        public string gaussCannon_Projectile = "Bullet_AntigrainGravship";
        public List<string> gaussCannon_Projectiles = new List<string> { "Bullet_AntigrainGravship", "Bullet_GravShipCannon", "Gravship_Rocket" };
        public List<string> gaussCannon_Ammotypes = new List<string> { "Steel", "Shell_HighExplosive"};
        public string gaussCannon_Ammotype = "Shell_HighExplosive";
        public float gaussCannon_Magazine = 10f;
        //Autocannon settings
        public float gaussAutoCannon_Powerusage = 600f;
        public float gaussAutoCannon_Range = 50.9f;
        public float gaussAutoCannon_Firerate = 15f;
        public float gaussAutoCannon_Burst = 1f;
        public List<string> gaussAutoCannon_Projectiles = new List<string> { "Bullet_AntigrainGravship", "Bullet_GravShipCannon", "Gravship_Rocket" };
        public List<string> gaussAutoCannon_Ammotypes = new List<string> { "Steel", "Shell_HighExplosive" };
        public string gaussAutoCannon_Projectile = "GravShip_Rocket";
        public string gaussAutoCannon_Ammotype = "Steel";
        public float gaussAutoCannon_Magazine = 50f;
        //CIWS settings
        public float gaussCIWS_Powerusage = 600f;
        public float gaussCIWS_Range = 50.9f;
        public float gaussCIWS_Firerate = 10f;
        public float gaussCIWS_Burst = 10f;
        public List<string> gaussCIWS_Projectiles = new List<string> { "Bullet_AntigrainGravship", "Bullet_GravShipCannon", "Gravship_Rocket" };
        public List<string> gaussCIWS_Ammotypes = new List<string> { "Steel", "Shell_HighExplosive" };
        public string gaussCIWS_Projectile = "Bullet_GravShipCannon";
        public string gaussCIWS_Ammotype = "Steel";
        public float gaussCIWS_Magazine = 100f;
        //Artillery settings
        public float gaussArtillery_Firerate = 1f;
        //Big Beam settings
        public float gaussBigBeam_Powerusage = 4000f;
        public float gaussBigBeam_Range =51f;
        public float gaussBigBeam_Firerate = 22f;
        public float gaussBigBeam_Burst = 5f;
        //Small Beam settings
        public float gaussSmallBeam_Powerusage = 2000f;
        public float gaussSmallBeam_Range = 50.9f;
        public float gaussSmallBeam_Firerate = 22f;
        public float gaussSmallBeam_Burst = 5f;

    public override void ExposeData()
        {
            Scribe_Values.Look(ref gaussCannon_Powerusage, "gaussCannon_Powerusage", 1000f);
            Scribe_Values.Look(ref gaussCannon_Range, "gaussCannon_Range", 56f);
            Scribe_Values.Look(ref gaussCannon_Firerate, "gaussCannon_Firerate", 5f);
            Scribe_Values.Look(ref gaussCannon_Projectile, "gaussCannon_Projectile", "Bullet_AntigrainGravship");
            Scribe_Values.Look(ref gaussCannon_Ammotype, "gaussCannon_Ammotype", "Shell_HighExplosive");
            Scribe_Values.Look(ref gaussCannon_Magazine, "gaussCannon_Magazine", 10f);
            Scribe_Values.Look(ref gaussCannon_Burst, "gaussCannon_Burst", 1f);

            Scribe_Values.Look(ref gaussAutoCannon_Powerusage, "gaussAutoCannon_Powerusage", 600f);
            Scribe_Values.Look(ref gaussAutoCannon_Range, "gaussAutoCannon_Range", 50.9f);
            Scribe_Values.Look(ref gaussAutoCannon_Firerate, "gaussAutoCannon_Firerate", 15f);
            Scribe_Values.Look(ref gaussAutoCannon_Projectile, "gaussAutoCannon_Projectile", "Gravship_Rocket");
            Scribe_Values.Look(ref gaussAutoCannon_Ammotype, "gaussAutoCannon_Ammotype", "Steel");
            Scribe_Values.Look(ref gaussAutoCannon_Magazine, "gaussAutoCannon_Magazine", 50f);
            Scribe_Values.Look(ref gaussAutoCannon_Burst, "gaussAutoCannon_Burst", 1f);

            Scribe_Values.Look(ref gaussCIWS_Powerusage, "gaussCIWS_Powerusage", 600f);
            Scribe_Values.Look(ref gaussCIWS_Range, "gaussCIWS_Range", 50.9f);
            Scribe_Values.Look(ref gaussCIWS_Firerate, "gaussCIWS_Firerate", 10f);
            Scribe_Values.Look(ref gaussCIWS_Projectile, "gaussCIWS_Projectile", "Bullet_GravShipCannon");
            Scribe_Values.Look(ref gaussCIWS_Ammotype, "gaussCIWS_Ammotype", "Steel");
            Scribe_Values.Look(ref gaussCIWS_Magazine, "gaussCIWS_Magazine", 100f);
            Scribe_Values.Look(ref gaussCIWS_Burst, "gaussCIWS_Burst", 10f);

    }
    }
        