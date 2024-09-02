using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.Ingredients
{
    internal class Meat : Ingredient
    {
        public bool cooked;
        public Meat(Vector2 position, PerspectiveManager perspectiveManager)
            : base(Plate.withBun, position, perspectiveManager)
        {
            state = (int)States.Meat;
            cooked = false;
        }

        public void cook()
        {
            cooked = true;
            state = (int)States.DoneMeat;
        }

        public override bool isPrepared()
        {
            return cooked;
        }

        public void cook()
        {
            cooked = true;
            state = (int)Component.States.MeatDone;
        }
    }
}
