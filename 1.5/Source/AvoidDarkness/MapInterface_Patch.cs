using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;

namespace AvoidDarkness;

[HarmonyPatch(typeof(MapInterface))]
public static class MapInterface_Patch
{
    [HarmonyPatch(nameof(MapInterface.MapInterfaceOnGUI_BeforeMainTabs))]
    [HarmonyPostfix]
    public static void MapInterfaceOnGUI_BeforeMainTabs_Patch()
    {
        if (Find.CurrentMap == null)
            return;
        if (!WorldRendererUtility.WorldRenderedNow)
            DarknessDrawer.DarknessDrawerOnGUI();
    }

    [HarmonyPatch(nameof(MapInterface.MapInterfaceUpdate))]
    [HarmonyPostfix]
    public static void MapInterfaceUpdate_Patch()
    {
        if (Find.CurrentMap == null || WorldRendererUtility.WorldRenderedNow)
            return;

        Find.CurrentMap.GetComponent<DarknessRectDrawerMapComponent>()
            .DarknessRectDrawer.DarkGridUpdate();
    }
}
