using RimWorld;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace AvoidDarkness;

public class DarknessRectDrawer(Map map) : ICellBoolGiver
{
    public IntVec3[] darkCells = new IntVec3[map.cellIndices.NumGridCells];

    public bool IsVisible = false;
    
    public CellBoolDrawer drawerInt;

    public Map map = map;

    public Color Color => Color.red;

    public CellBoolDrawer Drawer
    {
        get
        {
            if (drawerInt == null)
                drawerInt = new CellBoolDrawer( this, map.Size.x, map.Size.z, 13620, 0.33f);
            return drawerInt;
        }
    }
    
    public bool GetCellBool(int index)
    {
        return !this.map.fogGrid.IsFogged(index) && Mathf.Approximately(Find.CurrentMap.glowGrid.GroundGlowAt(this.map.cellIndices.IndexToCell(index)), 0);
    }
    

    public Color GetCellExtraColor(int index)
    {
        return Mathf.Approximately(Find.CurrentMap.glowGrid.GroundGlowAt(this.map.cellIndices.IndexToCell(index)), 0)
            ? Color.red
            : Color.white;
    }
    
    public void DarkGridUpdate()
    {
        if (IsVisible)
            Drawer.MarkForDraw();
        Drawer.CellBoolDrawerUpdate();
    }
}