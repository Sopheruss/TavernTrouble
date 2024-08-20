using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class Grill : StaticObject
{
    AnimationManager _grillAnimationManager;
    public static Texture2D _grillTextureFull;
    public static Texture2D _grillTextureEmpty;
    public static Texture2D _grillTextureAnimation;

    public Grill(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    { }

    public void Load()
    {
        _grillAnimationManager = new AnimationManager(3, 1, new Vector2(32, 32));
    }

    public override void Update()
    {
        _grillAnimationManager.Update();
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public void HandleInteraction()
    {
        /* what should happen:
            - interaction with meat in grill
            - start Animation 
            - plays Animation for 10 seconds (maybe more?)
            - stops Animation and has texture of grill done
            - interaction with done grill makes it empty again 
        */
    }
}
