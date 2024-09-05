using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;
using System.Linq;

namespace SoftwareProjekt2024.Components.StaticObjects
{
    internal class BunCrate : StaticObject
    {
        public BunCrate(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
        { }

        public override int getHeight()
        {
            return dest.Height - 10;
        }

        public static void HandleInteraction(PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp, Player _ogerCook, InteractionManager interactionManager, InputManager inputManager)
        {
            if (_ogerCook.inventoryIsEmpty())
            {
                interactionManager._interactionTextline = "Press [E] to grab bun";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    _perspectiveManager._dynamicObjects.Add(new Bun(positionWhilePickedUp, _perspectiveManager));
                    _ogerCook.pickUp(_perspectiveManager._dynamicObjects.Last());
                }
            }
            else
            {
                interactionManager._allowedInteraction = false;
            }
        }
    }
}
