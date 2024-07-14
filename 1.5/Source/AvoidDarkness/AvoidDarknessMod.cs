using Verse;
using UnityEngine;
using HarmonyLib;

namespace AvoidDarkness;

public class AvoidDarknessMod : Mod
{
    public static Settings settings;

    public AvoidDarknessMod(ModContentPack content) : base(content)
    {

        // initialize settings
        settings = GetSettings<Settings>();
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new Harmony("keyz182.rimworld.AvoidDarkness.main");	
        harmony.PatchAll();
    }

    public override void DoSettingsWindowContents(Rect inRect)
    {
        base.DoSettingsWindowContents(inRect);
        settings.DoWindowContents(inRect);
    }

    public override string SettingsCategory()
    {
        return "AvoidDarkness_SettingsCategory".Translate();
    }
}
