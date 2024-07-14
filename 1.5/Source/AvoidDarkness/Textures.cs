using UnityEngine;
using Verse;

namespace AvoidDarkness;

[StaticConstructorOnStartup]
public class Textures
{
    public static readonly Texture2D ShowLightOverlay = ContentFinder<Texture2D>.Get("UI/Buttons/ShowLightOverlay");

}