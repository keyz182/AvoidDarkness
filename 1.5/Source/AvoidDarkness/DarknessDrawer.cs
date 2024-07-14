using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AvoidDarkness;

public static class DarknessDrawer
{
    
    public static List<IntVec3> darkRelevantCells = new List<IntVec3>();
    public static HashSet<Thing> darkCountedThings = new HashSet<Thing>();
    public static readonly int SampleNumCells_Dark = GenRadial.NumCellsInRadius(8.9f);
    
    public static Color ColourLight = Color.green;
    public static Color ColourDark = Color.red;

    public static bool IsVisible = false;
    
    public static void DarknessDrawerOnGUI()
    {
        if (Event.current.type != EventType.Repaint || !ShouldShow())
            return;
        DrawDarknessAroundMouse();
    }

    public static bool ShouldShow()
    {
        return IsVisible 
               && !Mouse.IsInputBlockedNow 
               && UI.MouseCell().InBounds(Find.CurrentMap) 
               && !UI.MouseCell().Fogged(Find.CurrentMap);
    }
    
    public static void FillDarkRelevantCells(IntVec3 root, Map map)
    {
        darkRelevantCells.Clear();
        
        for (int index1 = 0; index1 < SampleNumCells_Dark; ++index1)
        {
            IntVec3 intVec3 = root + GenRadial.RadialPattern[index1];
            if (intVec3.InBounds(map) && !intVec3.Fogged(map) && intVec3.GetTerrain(map).passability != Traversability.Impassable)
            {
                darkRelevantCells.Add(intVec3);
            }
        }
    }

    public static void DrawDarknessAroundMouse()
    {
        FillDarkRelevantCells(UI.MouseCell(), Find.CurrentMap);
        
        for (int index = 0; index < darkRelevantCells.Count; ++index)
        {
            IntVec3 darkRelevantCell = darkRelevantCells[index];
            float num = Find.CurrentMap.glowGrid.GroundGlowAt(darkRelevantCell);
            GenMapUI.DrawThingLabel((Vector3) GenMapUI.LabelDrawPosFor(darkRelevantCell), num.ToString("0.00"), DarknessColor(num));
        }
        darkCountedThings.Clear();
    }

    public static Color DarknessColor(float light)
    {
        float t = Mathf.Clamp01(Mathf.InverseLerp(0, 1, light));
        return Color.Lerp(Color.Lerp(ColourDark, ColourLight, t), Color.white, 0.25f);
    }
}