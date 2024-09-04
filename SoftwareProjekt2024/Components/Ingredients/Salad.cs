using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.Ingredients;

internal class Salad : Ingredient
{
    public static Texture2D salad;
    public static Texture2D saladChoped;

    public bool chopped;
    public Salad(Vector2 position, PerspectiveManager perspectiveManager)
        : base(salad, position, perspectiveManager)
    {
        state = (int)States.Salad;
    }

    public override bool isPrepared()
    {
        return chopped;
    }

    public void chop()
    {
        chopped = true;
        state = (int)Component.States.SaladChopped;
    }

    public override void draw(SpriteBatch _spriteBatch)
    {
        if (!chopped)
        {
            _spriteBatch.Draw(salad, position, Color.White);
        }
        else
        {
            _spriteBatch.Draw(saladChoped, position, Color.White);

        }
    }
}
