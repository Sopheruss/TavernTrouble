using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.Ingredients
{
    internal class Salad : Ingredient
    {
        public bool chopped;
        public Salad(Vector2 position, PerspectiveManager perspectiveManager)
            : base(Plate.withBun, position, perspectiveManager)
        {
            state = (int)States.Salad;
        }

        public override bool isPrepared()
        {
            return true;
        }

        public void chop()
        {
            chopped = true;
            state = (int)Component.States.SaladChopped;
        }
    }
}
