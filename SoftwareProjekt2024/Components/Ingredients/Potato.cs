using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.Ingredients;

internal class Potato : Ingredient
{
    public static Texture2D potato;
    public static Texture2D potatoChopped;
    public static Texture2D potatoCooked;

    public bool cooked;
    public bool chopped;
    public Potato(Vector2 position, PerspectiveManager perspectiveManager)
        : base(potato, position, perspectiveManager)
    {
        state = (int)States.Potato;
        cooked = false;
        chopped = false;
    }

    public override bool isPrepared()
    {
        return chopped && cooked;
    }

    public void chop()
    {
        chopped = true;
        state = (int)Component.States.Fries;
    }

    public void cook()
    {
        cooked = true;
        state = (int)Component.States.FriesDone;
    }

    public override void draw(SpriteBatch _spriteBatch)
    {
        if (!cooked && !chopped)
        {
            _spriteBatch.Draw(potato, position, Color.White);
        }
        else if (!cooked && chopped)
        {
            _spriteBatch.Draw(potatoChopped, position, Color.White);
        }
        else if (cooked && chopped)
        {
            _spriteBatch.Draw(potatoCooked, position, Color.White);
        }
    }
}
