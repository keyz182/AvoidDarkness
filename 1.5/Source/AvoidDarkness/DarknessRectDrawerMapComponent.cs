using Verse;

namespace AvoidDarkness;

public class DarknessRectDrawerMapComponent : MapComponent
{
    private DarknessRectDrawer _DarknessRectDrawer;
    public DarknessRectDrawer DarknessRectDrawer
    {
        get
        {
            if (_DarknessRectDrawer == null)
            {
                _DarknessRectDrawer = new DarknessRectDrawer(this.map);
            }

            return _DarknessRectDrawer;
        }
    }

    public DarknessRectDrawerMapComponent(Map map)
        : base(map)
    {
        if (_DarknessRectDrawer == null)
            _DarknessRectDrawer = new DarknessRectDrawer(this.map);
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref DarknessRectDrawer.IsVisible, "DarknessRectDrawer_IsVisible");
    }
}
