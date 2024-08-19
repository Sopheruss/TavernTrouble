using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.Ingredients
{
    internal class Meat : Ingredient
    {
        public Meat(Vector2 position, PerspectiveManager perspectiveManager)
            : base(Plate.withBun, position, perspectiveManager)
        {
            state = (int)States.Meat;
        }

        public override bool isPrepared()
        {
            return true;
        }
    }
}
