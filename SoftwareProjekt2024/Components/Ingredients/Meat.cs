using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.Ingredients;

internal class Meat : Ingredient
{
    public static Texture2D meat;
    public static Texture2D meatCooked;

    public bool cooked;

    public Meat(Vector2 position, PerspectiveManager perspectiveManager)
        : base(meat, position, perspectiveManager)
    {
        state = (int)States.Meat;
        cooked = false;
    }

    public override bool isPrepared()
    {
        return cooked;
    }

    public void cook()
    {
        cooked = true;
        state = (int)States.MeatDone;
    }

    public override void draw(SpriteBatch _spriteBatch)
    {
        if (!cooked)
        {
            _spriteBatch.Draw(meat, position, Color.White);
        }
        else
        {
            _spriteBatch.Draw(meatCooked, position, Color.White);
        }
    }
}
