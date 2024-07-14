using UnityEngine;
using Verse;

namespace AvoidDarkness;

public class AD_Settings : ModSettings
{
    
    public bool EnableAvoidDarkness = true;
    public bool IgnoreAnimals = true;
    public bool IgnoreNonPlayerFaction = true;
    public float ScaledCostMultiplier = 2500f;

    public void DoWindowContents(Rect wrect)
    {
        Listing_Standard options = new();
        options.Begin(wrect);

        options.CheckboxLabeled("AvoidDarkness_EnableDarknessAvoid".Translate(), ref EnableAvoidDarkness);
        options.Gap();
        
        options.CheckboxLabeled("AvoidDarkness_IgnoreNonPlayerFaction".Translate(), ref IgnoreNonPlayerFaction);
        options.Gap();
        
        options.CheckboxLabeled("AvoidDarkness_IgnoreAnimals".Translate(), ref IgnoreAnimals);
        options.Gap();
        
        
        options.Label("AvoidDarkness_ScaledCostMultiplier".Translate(ScaledCostMultiplier.ToString("0.00")));
        ScaledCostMultiplier = options.Slider(ScaledCostMultiplier, 0f, 10000f);
        options.Gap();
        
        if (options.ButtonText("AvoidDarkness_Reset".Translate()))
        {
            EnableAvoidDarkness = true;
            ScaledCostMultiplier = 100f;
        }

        options.End();
    }

    public override void ExposeData()
    {
        Scribe_Values.Look(ref EnableAvoidDarkness, "EnableAvoidDarkness", true);
        Scribe_Values.Look(ref IgnoreNonPlayerFaction, "IgnoreNonPlayerFaction", true);
        Scribe_Values.Look(ref IgnoreAnimals, "IgnoreAnimals", true);
        Scribe_Values.Look(ref ScaledCostMultiplier, "ScaledCostMultiplier", 100f);
    }
}