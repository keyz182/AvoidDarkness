using HarmonyLib;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AvoidDarkness;

[HarmonyPatch(typeof(Pawn_PathFollower))]
public static class Pawn_PathFollower_Patch
{
    public class DarknessCustomTuning(Map map) : PathFinderCostTuning.ICustomizer
    {
        private Map map = map;

        public int CostOffset(IntVec3 from, IntVec3 to)
        {
            float fromGroundGlow = map.glowGrid.GroundGlowAt(from);
            float toGroundGlow = map.glowGrid.GroundGlowAt(to);

            float glowDelta = toGroundGlow - fromGroundGlow;

            if (Mathf.Approximately(fromGroundGlow, 0) && Mathf.Approximately(toGroundGlow, 0))
            {
                // force full darkness to darkness move to be costly  
                glowDelta = 1;
            }
            
            return Mathf.RoundToInt(AvoidDarknessMod.settings.ScaledCostMultiplier * Mathf.Clamp01(glowDelta));
        }
    }
    [HarmonyPatch(nameof(Pawn_PathFollower.GenerateNewPath))]
    [HarmonyPrefix]
    public static bool GenerateNewPath_Patched(Pawn_PathFollower __instance, ref PawnPath __result)
    {
        if (!AvoidDarknessMod.settings.EnableAvoidDarkness) return true;
        __instance.lastPathedTargetPosition = __instance.destination.Cell;
        PathFinderCostTuning tuning = new PathFinderCostTuning();
        tuning.custom = new DarknessCustomTuning(__instance.pawn.Map);
        
        __result = __instance.pawn.Map.pathFinder.FindPath(__instance.pawn.Position, __instance.destination, __instance.pawn, __instance.peMode, tuning);
        return false;
    }
}