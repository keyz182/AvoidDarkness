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
        if (Find.CurrentMap == null)
            return;

        row.ToggleableIcon(
            ref DarknessDrawer.IsVisible,
            Textures.ShowLightOverlay,
            "AvoidDarkness_ShowLightLevelOverlayToggleButton".Translate(),
            SoundDefOf.Mouseover_ButtonToggle
        );

        DarknessRectDrawerMapComponent comp =
            Find.CurrentMap.GetComponent<DarknessRectDrawerMapComponent>();

        if (comp == null)
            return;

        row.ToggleableIcon(
            ref comp.DarknessRectDrawer.IsVisible,
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
