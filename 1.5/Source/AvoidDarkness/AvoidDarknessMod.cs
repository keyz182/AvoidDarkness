﻿using HarmonyLib;
using UnityEngine;
using Verse;

namespace AvoidDarkness;

public class AvoidDarknessMod : Mod
{
    public static AD_Settings settings;

    public AvoidDarknessMod(ModContentPack content)
        : base(content)
    {
        // initialize settings
        settings = GetSettings<AD_Settings>();
#if DEBUG
        Harmony.DEBUG = true;
#endif
        Harmony harmony = new("keyz182.rimworld.AvoidDarkness.main");
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
