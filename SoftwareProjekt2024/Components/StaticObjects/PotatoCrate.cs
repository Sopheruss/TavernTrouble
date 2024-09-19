using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;
using System.Linq;

namespace SoftwareProjekt2024.Components.StaticObjects
{
    internal class PotatoCrate : StaticObject
    {
        public PotatoCrate(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
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
                interactionManager._interactionTextline = "Press [E] to grab potato";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    _perspectiveManager._dynamicObjects.Add(new Potato(positionWhilePickedUp, _perspectiveManager));
                    _ogerCook.pickUp(_perspectiveManager._dynamicObjects.Last());
                }
            }
            else if (!_ogerCook.inventoryIsEmpty() && _ogerCook.inventory[0] is Potato && !(_ogerCook.inventory[0] as Potato).chopped)
            {
                interactionManager._interactionTextline = "Press [E] to put potato back";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    Component item = _ogerCook.inventory[0];
                    _perspectiveManager._dynamicObjects.Remove(item);
                    //int index = perspectiveManager._dynamicObjects.FindIndex(x => x == item);
                    //Debug.WriteLine(perspectiveManager._dynamicObjects.Count);
                    _ogerCook.inventory.Clear();
                    _ogerCook.texture = Player.plain;
                }
            }
            else
            {
                interactionManager._allowedInteraction = false;
            }
        }
    }
}
