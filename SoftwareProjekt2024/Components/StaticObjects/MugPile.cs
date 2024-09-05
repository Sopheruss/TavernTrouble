using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Linq;

namespace SoftwareProjekt2024.Components.StaticObjects
{
    internal class MugPile : StaticObject
    {
        public MugPile(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
        { }

        public override int getHeight()
        {
            return dest.Height - 10;
        }

        public static void HandleInteraction(Player _ogerCook, PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp, InteractionManager interactionManager, InputManager inputManager)
        {
            if (_ogerCook.inventoryIsEmpty())
            {
                interactionManager._interactionTextline = "Press [E] to grab tankard";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    _perspectiveManager._dynamicObjects.Add(new Mug(Mug.beerEmpty, positionWhilePickedUp, _perspectiveManager));
                    _ogerCook.pickUp(_perspectiveManager._dynamicObjects.Last());
                }
            }
            else if (_ogerCook.inventory[0] is Mug && !(_ogerCook.inventory[0] as Mug).isFilled)
            {
                interactionManager._interactionTextline = "Press [E] to put tankard back";
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
