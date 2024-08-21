using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class Kessel : StaticObject
{
    AnimationManager _kesselAnimationManager;

    public static Texture2D _kesselTextureEmpty;
    public static Texture2D _kesselTextureAnimation;
    public static Texture2D _kesselTextureFull;

    public Kessel(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    { }

    public void Load()
    {
        _kesselAnimationManager = new AnimationManager(3, 1, new Vector2(32, 32));
    }

    public override void Update()
    {
        _kesselAnimationManager.Update();
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public void HandleInteraction()
    {
        /* what should happen:
            - interaction with potato in kessel
            - start Animation 
            - plays Animation for 10 seconds (maybe more?)
            - stops Animation and has texture of kessel done
            - interaction with done kessel makes it empty again 
         */
    }
}
