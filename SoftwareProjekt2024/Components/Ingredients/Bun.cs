using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.Ingredients;

internal class Bun : Ingredient
{
    public static Texture2D bun;

    public Bun(Vector2 position, PerspectiveManager perspectiveManager)
        : base(bun, position, perspectiveManager)
    {
        state = (int)States.Bun;
    }

    public override bool isPrepared()
    {
        return true;
    }

    public override void draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(bun, position, Color.White);
    }
}
