using Verse;

namespace AvoidDarkness;

public class DarknessRectDrawerMapComponent : MapComponent
{
    public DarknessRectDrawer DarknessRectDrawer;

    public DarknessRectDrawerMapComponent(Map map) : base(map)
    {
        DarknessRectDrawer = new DarknessRectDrawer(this.map);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref DarknessRectDrawer.IsVisible, "DarknessRectDrawer_IsVisible");
    }
}