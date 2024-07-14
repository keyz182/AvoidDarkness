using UnityEngine;
using Verse;

namespace AvoidDarkness;

public class DarknessRectDrawer(Map map) : ICellBoolGiver
{
    public IntVec3[] darkCells = new IntVec3[map.cellIndices.NumGridCells];

    public CellBoolDrawer drawerInt;

    public bool IsVisible = false;

    public Map map = map;

    public CellBoolDrawer Drawer
    {
        get
        {
            if (drawerInt == null)
                drawerInt = new CellBoolDrawer(this, map.Size.x, map.Size.z, 13620);
            return drawerInt;
        }
    }

    public Color Color => Color.red;

    public bool GetCellBool(int index)
    {
        return !map.fogGrid.IsFogged(index) &&
               Mathf.Approximately(Find.CurrentMap.glowGrid.GroundGlowAt(map.cellIndices.IndexToCell(index)), 0);
    }


    public Color GetCellExtraColor(int index)
    {
        return Mathf.Approximately(Find.CurrentMap.glowGrid.GroundGlowAt(map.cellIndices.IndexToCell(index)), 0)
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