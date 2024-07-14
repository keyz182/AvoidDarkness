using System;
using UnityEngine;
using Verse;

namespace AvoidDarkness;

public class GlowValueGrid(Map map, GlowGrid glowGrid)
{
    public GlowGrid glowGrid = glowGrid;
    private Map map = map;
    

    public void GlowValueGridUpdate()
    {
        // if (Find.PlaySettings.showFertilityOverlay)
        DarknessDrawer.DarknessDrawerOnGUI();
    }
    
}