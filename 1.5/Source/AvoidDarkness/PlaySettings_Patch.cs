using HarmonyLib;
using RimWorld;
using Verse;

namespace AvoidDarkness;

[HarmonyPatch(typeof(PlaySettings))]
public static class PlaySettings_Patch
{
    [HarmonyPatch(nameof(PlaySettings.DoPlaySettingsGlobalControls))]
    [HarmonyPostfix]
    public static void DoPlaySettingsGlobalControls_Patch(WidgetRow row, bool worldView)
    {
        row.ToggleableIcon(
            ref DarknessDrawer.IsVisible,
            Textures.ShowLightOverlay,
            "AvoidDarkness_ShowLightLevelOverlayToggleButton".Translate(),
            SoundDefOf.Mouseover_ButtonToggle
        );

        DarknessRectDrawer drawer = Find
            .CurrentMap.GetComponent<DarknessRectDrawerMapComponent>()
            .DarknessRectDrawer;

        if (drawer == null)
            return;

        row.ToggleableIcon(
            ref Find.CurrentMap.GetComponent<DarknessRectDrawerMapComponent>().DarknessRectDrawer.IsVisible,
            Textures.ShowLightRectOverlay,
            "AvoidDarkness_ShowLightLevelVisualOverlayToggleButton".Translate(),
            SoundDefOf.Mouseover_ButtonToggle
        );
    }

    [HarmonyPatch(nameof(PlaySettings.ExposeData))]
    [HarmonyPostfix]
    public static void ExposeData_Patch()
    {
        Scribe_Values.Look(ref DarknessDrawer.IsVisible, "DarknessDrawer_IsVisible");
    }
}
