using System;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace AvoidDarkness;

[HarmonyPatch(typeof(AreaUtility))]
public static class AreaUtility_Patch
{
    [HarmonyPatch(nameof(AreaUtility.MakeAllowedAreaListFloatMenu))]
    [HarmonyPostfix]
    public static void MakeAllowedAreaListFloatMenu_Patch()
    {
        if (Find.WindowStack.windows.Any(win => win is MainTabWindow_Architect))
            return;

        var window = Find.WindowStack.windows.Last();
        if (window is not FloatMenu fm)
            return;

        FloatMenuOption opt = null;
        foreach (FloatMenuOption floatMenuOption in fm.options)
        {
            if (floatMenuOption.Label == "AvoidDarkness_LightArea".Translate())
                opt = floatMenuOption;
        }

        if (opt != null)
        {
            fm.options.Remove(opt);
        }
    }
}
