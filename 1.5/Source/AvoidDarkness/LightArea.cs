using RimWorld;
using UnityEngine;
using Verse;

namespace AvoidDarkness;

public class LightArea : Area
{
    public string labelInt = "AvoidDarkness_LightArea".Translate();
    public int lastUpdateTick = -1;
    public int TicksToCacheGlowGrid => AvoidDarknessMod.settings.TicksToCacheGlowGrid;

    public LightArea(AreaManager areaManager)
        : base(areaManager) { }

    public LightArea() { }

    public void CheckNeedsUpdate()
    {
        // split the map cells into chunks of size TicksToCacheGlowGrid
        // this lets us spread the calls to the glow grid over the duration of TicksToCacheGlowGrid, rather than
        // having a spike every TicksToCacheGlowGrid ticks.

        // The longer TicksToCacheGlowGrid is, the weirder/glitchier the area might appear between full updates,
        // but it's eventually consistent, and should be good enough for gameplay

        // Skip if we've already run this tick
        if (lastUpdateTick == Find.TickManager.TicksGame)
            return;

        lastUpdateTick = Find.TickManager.TicksGame;

        // Grab the cells to process this iteration
        int chunkIdx = Find.TickManager.TicksGame % TicksToCacheGlowGrid;

        for (
            int idx = chunkIdx;
            idx < areaManager.map.cellIndices.NumGridCells - TicksToCacheGlowGrid;
            idx += TicksToCacheGlowGrid
        )
        {
            IntVec3 cell = areaManager.map.cellIndices.IndexToCell(idx);

            // If we have a door, check both sides, if both sides of the door are lit, we allow the door.
            Building_Door door = areaManager.map.thingGrid.ThingAt<Building_Door>(cell);
            if (door != null)
            {
                float glowA = 0f;
                float glowB = 0f;

                if (door.Rotation == Rot4.North || door.Rotation == Rot4.South)
                {
                    glowA = areaManager.map.glowGrid.GroundGlowAt(
                        new IntVec3(cell.x, cell.y, cell.z + 1)
                    );
                    glowB = areaManager.map.glowGrid.GroundGlowAt(
                        new IntVec3(cell.x, cell.y, cell.z - 1)
                    );
                }
                else
                {
                    glowA = areaManager.map.glowGrid.GroundGlowAt(
                        new IntVec3(cell.x + 1, cell.y, cell.z)
                    );
                    glowB = areaManager.map.glowGrid.GroundGlowAt(
                        new IntVec3(cell.x - 1, cell.y, cell.z)
                    );
                }

                if (Mathf.Approximately(Mathf.Min(glowA, glowB), 0))
                {
                    innerGrid.Set(cell, false);
                }
                else
                {
                    innerGrid.Set(cell, !Mathf.Approximately(glowA + glowB, 0));
                }
            }
            else
            {
                innerGrid.Set(cell, areaManager.map.glowGrid.GroundGlowAt(cell) > 0f);
            }

            MarkDirty(cell);
        }
    }

    public override string Label
    {
        get => labelInt;
    }
    public override Color Color
    {
        get => new Color(0.7f, 0.7f, 0.1f);
    }
    public override int ListPriority
    {
        get => 1;
    }

    public override bool Mutable => false;

    public override bool AssignableAsAllowed() => true;

    public override void Set(IntVec3 c, bool val)
    {
        return;
    }

    public override string GetUniqueLoadID()
    {
        return "Area_" + ID + "_Named_" + labelInt;
    }
}
