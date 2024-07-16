using HarmonyLib;
using Verse;

namespace AvoidDarkness;

[HarmonyPatch(typeof(AreaManager))]
public static class AreaManager_Patch
{
    [HarmonyPatch(nameof(AreaManager.AddStartingAreas))]
    [HarmonyPostfix]
    public static void AddStartingAreas_Patch(AreaManager __instance)
    {
        if (!__instance.areas.Any(area => area is LightArea))
        {
            __instance.areas.Add(new LightArea(__instance));
        }
    }

    [HarmonyPatch(nameof(AreaManager.AreaManagerUpdate))]
    [HarmonyPostfix]
    public static void AreaManagerUpdate_Patch(AreaManager __instance)
    {
        if (!__instance.areas.Any(area => area is LightArea))
        {
            __instance.areas.Add(new LightArea(__instance));
        }
    }
}
