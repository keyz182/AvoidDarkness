using HarmonyLib;
using Verse;

namespace AvoidDarkness;

[HarmonyPatch(typeof(Area))]
public static class Area_Patch
{
    [HarmonyPatch(nameof(Area.Delete))]
    [HarmonyPrefix]
    public static bool Delete_Patch(Area __instance)
    {
        return __instance is not LightArea;
    }

    [HarmonyPatch(nameof(Area.Invert))]
    [HarmonyPrefix]
    public static bool Invert_Patch(Area __instance)
    {
        return __instance is not LightArea;
    }

    [HarmonyPatch(nameof(Area.AreaUpdate))]
    [HarmonyPostfix]
    public static void AreaUpdate_Patch(Area __instance)
    {
        if (__instance is LightArea area)
        {
            area.CheckNeedsUpdate();
        }
    }
}
