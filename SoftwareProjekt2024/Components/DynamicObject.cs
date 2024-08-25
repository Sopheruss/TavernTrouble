using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components;

internal class DynamicObject : Component
{
    bool pickedUp;
    public DynamicObject(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager)
        : base(texture, position, perspectiveManager)
    {
        pickedUp = true;
    }

    public override int getHeight()
    {
        return texture.Height;
    }
    public override int getLevel()
    {
        return 1;
    }

    public override void draw(SpriteBatch _spriteBatch, AnimationManager _animationManager)
    {
        _spriteBatch.Draw(texture, position, Color.White);
    }
}
