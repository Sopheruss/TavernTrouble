using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.StaticObjects
{
    internal class BeerBarrel : StaticObject
    {
        public BeerBarrel(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
        { }

        public override int getHeight()
        {
            return dest.Height - 10;
        }

        public static bool AllowedInteraction(Player ogerCook)
        {
            if (!ogerCook.inventoryIsEmpty() && ogerCook.inventory[0] is Mug && (ogerCook.inventory[0] as Mug).isFilled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void HandleInteraction()
        {

        }
    }
}
