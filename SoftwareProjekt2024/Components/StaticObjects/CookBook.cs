using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class CookBook : StaticObject
{
    public CookBook(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    { }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public static void HandleInteraction(Game1 _game, Stopwatch _timer)
    {
        _game.activeScene = Scenes.COOKBOOKSCREEN;
        _timer.Stop();
    }
}
