using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.Ingredients
{
    internal class Potato : Ingredient
    {
        public bool cooked;
        public bool chopped;
        public Potato(Vector2 position, PerspectiveManager perspectiveManager)
            : base(Plate.withBun, position, perspectiveManager)
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

    }
}
