using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI;

namespace AvoidDarkness;

[HarmonyPatch(typeof(Pawn_PathFollower))]
public static class Pawn_PathFollower_Patch
{
    public static List<Faction> FactionsAllowedInDarkness = new List<Faction>()
    {
        Faction.OfEntities,
        Faction.OfInsects,
        Faction.OfMechanoids
    };
    
    public class DarknessCustomTuning(Map map) : PathFinderCostTuning.ICustomizer
    {
        private Map map = map;

        public Dictionary<IntVec3, float> CachedGroundGlow = new Dictionary<IntVec3, float>();

        public float GetGroundGlow(IntVec3 at)
        {
            if (CachedGroundGlow.TryGetValue(at, out float glow))
                return glow;
            
            CachedGroundGlow.Add(at, map.glowGrid.GroundGlowAt(at));

            return CachedGroundGlow[at];
        }

        public int CostOffset(IntVec3 from, IntVec3 to)
        {
            if (map.IsHashIntervalTick(60))
            {
                CachedGroundGlow.Clear();
            }
            
            float fromGroundGlow = GetGroundGlow(from);
            float toGroundGlow = GetGroundGlow(to);

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
        if (AvoidDarknessMod.settings.IgnoreNonPlayerFaction && !__instance.pawn.Faction.IsPlayerSafe()) return true;
        if (AvoidDarknessMod.settings.IgnoreAnimals && __instance.pawn.RaceProps.Animal) return true;
        if (__instance.pawn.Faction == null || FactionsAllowedInDarkness.Contains(__instance.pawn.Faction)) return true;
        
        __instance.lastPathedTargetPosition = __instance.destination.Cell;
        PathFinderCostTuning tuning = new PathFinderCostTuning();
        tuning.custom = new DarknessCustomTuning(__instance.pawn.Map);
        
        __result = __instance.pawn.Map.pathFinder.FindPath(__instance.pawn.Position, __instance.destination, __instance.pawn, __instance.peMode, tuning);
        return false;
    }
}