// GravshipSize, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// GravshipSize.GravshipSizeSettings
using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

[StaticConstructorOnStartup]
public class BuildableOrbitalCannon: Mod
{
    public static CannonSettings settings = null;

    private static Vector2 scrollPosition = Vector2.zero;
    private float maxRange = 56f;

    // Gauss Cannon settings
    private string gaussCannon_PowerusageBuffer;
    private string gaussCannon_RangeBuffer;
    private string gaussCannon_FirerateBuffer;
    private string gaussCannon_MagazineBuffer;
    private string gaussCannon_BurstBuffer;
    //AutoCannon settings
    private string gaussAutoCannon_PowerusageBuffer;
    private string gaussAutoCannon_RangeBuffer;
    private string gaussAutoCannon_FirerateBuffer;
    private string gaussAutoCannon_MagazineBuffer;  
    private string gaussAutoCannon_BurstBuffer;
    //CIWS settings
    private string gaussCIWS_PowerusageBuffer;
    private string gaussCIWS_RangeBuffer;
    private string gaussCIWS_FirerateBuffer;
    private string gaussCIWS_MagazineBuffer;
    private string gaussCIWS_BurstBuffer;



    public BuildableOrbitalCannon(ModContentPack content) : base(content)
    {
        settings = GetSettings<CannonSettings>();
        if (ModsConfig.IsActive("Reel.TurretPipeline"))
        {
            if (!settings.gaussCannon_Ammotypes.Contains("ReelTurretAmmo"))
            {
                settings.gaussCannon_Ammotypes.Add("ReelTurretAmmo");
            }
            if (!settings.gaussAutoCannon_Ammotypes.Contains("ReelTurretAmmo"))
            {
                settings.gaussAutoCannon_Ammotypes.Add("ReelTurretAmmo");
            }
            if (!settings.gaussCIWS_Ammotypes.Contains("ReelTurretAmmo"))
            {
                settings.gaussCIWS_Ammotypes.Add("ReelTurretAmmo");
            }
        }
        if(ModsConfig.IsActive("OskarPotocki.VanillaFactionsExpanded.Core"))
        {
            maxRange = 240f;
        }
        else
        {
            maxRange = 56f;
        }
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        Listing_Standard listing_Standard = new Listing_Standard();
        Rect rect = inRect.ContractedBy(10f);
        rect.height -= listing_Standard.CurHeight;
        rect.y += listing_Standard.CurHeight;
        Widgets.DrawBoxSolid(rect, Color.grey);
        Rect rect2 = rect.ContractedBy(1f);
        Widgets.DrawBoxSolid(rect2, new ColorInt(42, 43, 44).ToColor);
        Rect rect3 = rect2.ContractedBy(5f);
        Rect rect4 = rect3;
        rect4.x = 0f;
        rect4.y = 0f;
        rect4.width -= 20f;
        rect4.height = 950f;
        Widgets.BeginScrollView(rect3, ref scrollPosition, rect4);
        listing_Standard.Begin(rect4.AtZero());
        if (listing_Standard.ButtonText("OC_Default".Translate()))
        {
            SetDefault();
            ApplySettingsNow();
        }
        if (listing_Standard.ButtonText("OC_ApplySettingsNow".Translate()))
        {
            ApplySettingsNow();
        }
        listing_Standard.GapLine();
        Text.Font = GameFont.Medium;
        listing_Standard.Label("OC_GaussCannon".Translate(), -1f);
        Text.Font = GameFont.Small;
        listing_Standard.GapLine();
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_GaussPower".Translate(), ref settings.gaussCannon_Powerusage, ref gaussCannon_PowerusageBuffer, 0f, 2000f, (float value) => $"{value:F0}");
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_GaussRange".Translate(), ref settings.gaussCannon_Range, ref gaussCannon_RangeBuffer, 1f, maxRange, (float value) => $"{value:F0}");
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_GaussFirerate".Translate(), ref settings.gaussCannon_Firerate, ref gaussCannon_FirerateBuffer, 1f, 60f, (float value) => $"{value:F0}");
        //SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_GaussBurst".Translate(), ref settings.gaussCannon_Burst, ref gaussCannon_BurstBuffer, 1f, 100f, (float value) => $"{value:F0}");
        //SettingsWidgets.CreateSettingsDropdown(listing_Standard,"OC_GaussProjectile".Translate(),settings.gaussCannon_Projectile,v => settings.gaussCannon_Projectile = v,settings.gaussCannon_Projectiles.ToArray());
        SettingsWidgets.CreateSettingsDropdown(listing_Standard, "OC_GaussAmmo".Translate(), settings.gaussCannon_Ammotype, v => settings.gaussCannon_Ammotype = v, settings.gaussCannon_Ammotypes.ToArray());
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_GaussMagazine".Translate(), ref settings.gaussCannon_Magazine, ref gaussCannon_MagazineBuffer, 1f, 500f, (float value) => $"{value:F0}");
        listing_Standard.GapLine();
        Text.Font = GameFont.Medium;
        listing_Standard.Label("OC_AutoCannon".Translate(), -1f);
        Text.Font = GameFont.Small;
        listing_Standard.GapLine();
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_AutoCannonPower".Translate(), ref settings.gaussAutoCannon_Powerusage, ref gaussAutoCannon_PowerusageBuffer, 0f, 2000f, (float value) => $"{value:F0}");
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_AutoCannonRange".Translate(), ref settings.gaussAutoCannon_Range, ref gaussAutoCannon_RangeBuffer, 1f, maxRange, (float value) => $"{value:F0}");
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_AutoCannonFirerate".Translate(), ref settings.gaussAutoCannon_Firerate, ref gaussAutoCannon_FirerateBuffer, 1f, 100f, (float value) => $"{value:F0}");
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_AutoCannonBurst".Translate(), ref settings.gaussAutoCannon_Burst, ref gaussAutoCannon_BurstBuffer, 1f, 100f, (float value) => $"{value:F0}");
        //SettingsWidgets.CreateSettingsDropdown(listing_Standard, "OC_AutoCannonProjectile".Translate(), settings.gaussAutoCannon_Projectile, v => settings.gaussAutoCannon_Projectile = v, settings.gaussAutoCannon_Projectiles.ToArray());
        SettingsWidgets.CreateSettingsDropdown(listing_Standard, "OC_AutoCannonAmmo".Translate(), settings.gaussAutoCannon_Ammotype, v => settings.gaussAutoCannon_Ammotype = v, settings.gaussAutoCannon_Ammotypes.ToArray());
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_AutoCannonMagazine".Translate(), ref settings.gaussAutoCannon_Magazine, ref gaussAutoCannon_MagazineBuffer, 1f, 500f, (float value) => $"{value:F0}");
        listing_Standard.GapLine();
        Text.Font = GameFont.Medium;
        listing_Standard.Label("OC_CIWS".Translate(), -1f);
        Text.Font = GameFont.Small;
        listing_Standard.GapLine();
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_CIWSPower".Translate(), ref settings.gaussCIWS_Powerusage, ref gaussCIWS_PowerusageBuffer, 0f, 2000f, (float value) => $"{value:F0}");
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_CIWSRange".Translate(), ref settings.gaussCIWS_Range, ref gaussCIWS_RangeBuffer, 1f, maxRange, (float value) => $"{value:F0}");
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_CIWSFirerate".Translate(), ref settings.gaussCIWS_Firerate, ref gaussCIWS_FirerateBuffer, 1f, 100f, (float value) => $"{value:F0}");
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_CIWSBurst".Translate(), ref settings.gaussCIWS_Burst, ref gaussCIWS_BurstBuffer, 1f, 100f, (float value) => $"{value:F0}");
        //SettingsWidgets.CreateSettingsDropdown(listing_Standard, "OC_CIWSProjectile".Translate(), settings.gaussCIWS_Projectile, v => settings.gaussCIWS_Projectile = v, settings.gaussCIWS_Projectiles.ToArray());
        SettingsWidgets.CreateSettingsDropdown(listing_Standard, "OC_CIWSAmmo".Translate(), settings.gaussCIWS_Ammotype, v => settings.gaussCIWS_Ammotype = v, settings.gaussCIWS_Ammotypes.ToArray());
        SettingsWidgets.CreateSettingsSlider(listing_Standard, "OC_CIWSMagazine".Translate(), ref settings.gaussCIWS_Magazine, ref gaussCIWS_MagazineBuffer, 1f, 500f, (float value) => $"{value:F0}");
        listing_Standard.End();
        Widgets.EndScrollView();
        base.DoSettingsWindowContents(inRect);
    }

    private void SetDefault()
    {
        // Gauss Cannon settings
        settings.gaussCannon_Powerusage = 1000f;
        if (ModsConfig.IsActive("OskarPotocki.VanillaFactionsExpanded.Core"))
        {
            settings.gaussCannon_Range = 126f;
        }
        else
        {
            settings.gaussCannon_Range = 56f;
        }
        settings.gaussCannon_Firerate = 5f;
        //settings.gaussCannon_Projectile = "Bullet_AntigrainGravship";
        settings.gaussCannon_Ammotype = "Shell_HighExplosive";
        settings.gaussCannon_Magazine = 10f;
        //settings.gaussCannon_Burst = 1f;
        // Autocannon settings
        settings.gaussAutoCannon_Powerusage = 600f;
        settings.gaussAutoCannon_Range = 50.9f;
        settings.gaussAutoCannon_Firerate = 15f;
        //settings.gaussAutoCannon_Projectile = "GravShip_Rocket";
        settings.gaussAutoCannon_Ammotype = "Steel";
        settings.gaussAutoCannon_Magazine = 50f;
        settings.gaussAutoCannon_Burst = 1f;
        //CIWS settings
        settings.gaussCIWS_Powerusage = 600f;
        settings.gaussCIWS_Range = 50.9f;
        settings.gaussCIWS_Firerate = 10f;
        //settings.gaussCIWS_Projectile = "Bullet_GravShipCannon";
        settings.gaussCIWS_Ammotype = "Steel";
        settings.gaussCIWS_Magazine = 100f;
        settings.gaussCIWS_Burst = 10f;
    }
    public static void ApplySettingsNow()
    {
        //Turret = Power, Magazine, AmmoType and Cooldown
        ThingDef Turret_GaussCannonReal = OCDefOfs.Turret_GaussCannonReal;
        if (Turret_GaussCannonReal == null)
        {
            Log.Error("Turret_GaussCannonReal not found in DefOfs!");
            return;
        }
        CompProperties_Refuelable refuelableComp = Turret_GaussCannonReal.comps.OfType<CompProperties_Refuelable>().FirstOrDefault();
        string ammoDefName = settings.gaussCannon_Ammotype;
        ThingDef newAmmoDef = DefDatabase<ThingDef>.GetNamed(ammoDefName, errorOnFail: false);
        refuelableComp.fuelFilter.SetDisallowAll();
        refuelableComp.fuelFilter.SetAllow(newAmmoDef, true);
        refuelableComp.fuelCapacity = settings.gaussCannon_Magazine;
        Turret_GaussCannonReal.building.turretBurstCooldownTime = settings.gaussCannon_Firerate;
        Log.Message($"Successfully updated Turret_GaussCannonReal fuelFilter to {newAmmoDef}");
        //##########################################################
        CompProperties_Power powerComp = Turret_GaussCannonReal.comps.OfType<CompProperties_Power>().FirstOrDefault();
        powerComp.idlePowerDraw = settings.gaussCannon_Powerusage;
        //##########################################################

        ThingDef Turret_GaussCannon = OCDefOfs.Turret_GaussCannon;
        if (Turret_GaussCannon == null)
        {
            Log.Error("Turret_GaussCannon not found in DefOfs!");
            return;
        }
        CompProperties_Refuelable refuelableComp2 = Turret_GaussCannon.comps.OfType<CompProperties_Refuelable>().FirstOrDefault();
        string ammoDefName2 = settings.gaussAutoCannon_Ammotype;
        ThingDef newAmmoDef2 = DefDatabase<ThingDef>.GetNamed(ammoDefName2, errorOnFail: false);
        refuelableComp2.fuelFilter.SetDisallowAll();
        refuelableComp2.fuelFilter.SetAllow(newAmmoDef2, true);
        refuelableComp2.fuelCapacity = settings.gaussAutoCannon_Magazine;   
        Log.Message($"Successfully updated Turret_GaussCannon fuelFilter to {newAmmoDef2}");
        //###########################################################
        CompProperties_Power powerComp2 = Turret_GaussCannon.comps.OfType<CompProperties_Power>().FirstOrDefault();
        powerComp2.idlePowerDraw = settings.gaussAutoCannon_Powerusage;
        //##########################################################

        ThingDef Turret_Gauss = OCDefOfs.Turret_Gauss;
        if (Turret_Gauss == null)
        {
            Log.Error("Turret_Gauss not found in DefOfs!");
            return;
        }
        CompProperties_Refuelable refuelableComp3 = Turret_Gauss.comps.OfType<CompProperties_Refuelable>().FirstOrDefault();
        string ammoDefName3 = settings.gaussCIWS_Ammotype;
        ThingDef newAmmoDef3 = DefDatabase<ThingDef>.GetNamed(ammoDefName3, errorOnFail: false);
        refuelableComp3.fuelFilter.SetDisallowAll();
        refuelableComp3.fuelFilter.SetAllow(newAmmoDef3, true);
        refuelableComp3.fuelCapacity = settings.gaussCIWS_Magazine;
        Log.Message($"Successfully updated Turret_GaussCannonReal fuelFilter to {newAmmoDef3}");
        //###########################################################
        CompProperties_Power powerComp3 = Turret_Gauss.comps.OfType<CompProperties_Power>().FirstOrDefault();
        powerComp3.idlePowerDraw = settings.gaussCIWS_Powerusage;
        //##########################################################
    
        //Gun = Firerate, Projectile and Range
        ThingDef Gun_GaussCannonReal = OCDefOfs.Gun_GaussCannonReal;
        if (Gun_GaussCannonReal == null)
        {
            Log.Error("Gun_GaussCannonReal not found in DefOfs!");
            return;
        }
        VerbProperties rangeVerb = Gun_GaussCannonReal.Verbs?.FirstOrDefault(v => v.verbClass == typeof(Verb_Shoot));
        //string projectileDefName = settings.gaussCannon_Projectile;
        //ThingDef newProjectileDef = DefDatabase<ThingDef>.GetNamed(projectileDefName, errorOnFail: false);
        rangeVerb.range = settings.gaussCannon_Range;
        rangeVerb.ticksBetweenBurstShots = (int)settings.gaussCannon_Firerate;
        rangeVerb.burstShotCount = (int)settings.gaussCannon_Burst;
        //rangeVerb.defaultProjectile = newProjectileDef;
        //##########################################################

        ThingDef Gun_GravShipTurret = OCDefOfs.Gun_GravShipTurret;
        if (Gun_GravShipTurret == null)
        {
            Log.Error("Gun_GravShipTurret not found in DefOfs!");
            return;
        }
        VerbProperties rangeVerb2 = Gun_GravShipTurret.Verbs?.FirstOrDefault(v => v.verbClass == typeof(Verb_Shoot));
        //string projectileDefName2 = settings.gaussAutoCannon_Projectile;
        //ThingDef newProjectileDef2 = DefDatabase<ThingDef>.GetNamed(projectileDefName2, errorOnFail: false);
        rangeVerb2.range = settings.gaussAutoCannon_Range;
        rangeVerb2.ticksBetweenBurstShots = (int)settings.gaussCannon_Firerate;
        rangeVerb2.burstShotCount = (int)settings.gaussAutoCannon_Burst;
        //rangeVerb2.defaultProjectile = newProjectileDef2;
        //##########################################################

        ThingDef Gun_GravShipCannon = OCDefOfs.Gun_GravShipCannon;
        if (Gun_GravShipCannon == null)
        {
            Log.Error("Gun_GravShipCannon not found in DefOfs!");
            return;
        }
        VerbProperties rangeVerb3 = Gun_GravShipCannon.Verbs?.FirstOrDefault(v => v.verbClass == typeof(Verb_Shoot));
        //string projectileDefName3 = settings.gaussCIWS_Projectile;
        //ThingDef newProjectileDef3 = DefDatabase<ThingDef>.GetNamed(projectileDefName3, errorOnFail: false);
        rangeVerb3.range = settings.gaussCIWS_Range;
        rangeVerb3.ticksBetweenBurstShots = (int)settings.gaussCIWS_Firerate;
        rangeVerb3.burstShotCount = (int)settings.gaussCIWS_Burst;
        //rangeVerb3.defaultProjectile = newProjectileDef3;

    }

    public override string SettingsCategory()
    {
        return "OC_Menu".Translate();
    }
}
