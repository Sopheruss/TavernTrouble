using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class CookBook : StaticObject
{
    AnimationManager _cookBookAnimationManager;
    public static Texture2D _cookBookAnimation;
    public static Texture2D _cookBookClose;

    public CookBook(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    { }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public static void HandleInteraction(Game1 _game, Stopwatch _timer)
    {
        /* what esle should happen: 
            - before opening new scene, play cookBook animation 
            - after closing scene, close book again
        */

        _game.activeScene = Scenes.COOKBOOKSCREEN;
        _timer.Stop();
    }
}
