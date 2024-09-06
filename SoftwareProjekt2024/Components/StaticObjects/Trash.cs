using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Diagnostics;
using System.Linq;

namespace SoftwareProjekt2024.Components.StaticObjects
{
    internal class Trash : StaticObject
    {

        public Trash(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
        {

        }

        public override int getHeight()
        {
            return dest.Height - 10;
        }

        public static void HandleInteraction(Player _ogerCook, PerspectiveManager perspectiveManager, Vector2 positionWhilePickedUp, InteractionManager interactionManager, InputManager inputManager)
        {
            if (!_ogerCook.inventoryIsEmpty() && _ogerCook.inventory[0] is Plate && (_ogerCook.inventory[0] as Plate).state != 2)  //plate.state == 2 -> empty plate
            {
                Debug.WriteLine(_ogerCook.inventory[0].state);
                interactionManager._interactionTextline = "Press [E] to clear plate";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    Component item = _ogerCook.inventory[0];
                    perspectiveManager._dynamicObjects.Remove(item);
                    //int index = perspectiveManager._dynamicObjects.FindIndex(x => x == item);
                    //Debug.WriteLine(perspectiveManager._dynamicObjects.Count);
                    _ogerCook.inventory.Clear();
                    _ogerCook.texture = Player.plain;
                    perspectiveManager._dynamicObjects.Add(new Plate(Plate.plain, positionWhilePickedUp, perspectiveManager));
                    _ogerCook.pickUp(perspectiveManager._dynamicObjects.Last());
                }
            }
            else if (!_ogerCook.inventoryIsEmpty() && _ogerCook.inventory[0] is Mug && (_ogerCook.inventory[0] as Mug).isFilled)
            {
                interactionManager._interactionTextline = "Press [E] to pour out beer";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    Component item = _ogerCook.inventory[0];
                    perspectiveManager._dynamicObjects.Remove(item);
                    //int index = perspectiveManager._dynamicObjects.FindIndex(x => x == item);
                    //Debug.WriteLine(perspectiveManager._dynamicObjects.Count);
                    _ogerCook.inventory.Clear();
                    _ogerCook.texture = Player.plain;
                    perspectiveManager._dynamicObjects.Add(new Mug(Mug.beerEmpty, positionWhilePickedUp, perspectiveManager));
                    _ogerCook.pickUp(perspectiveManager._dynamicObjects.Last());
                }
                else
                {
                    interactionManager._allowedInteraction = false;
                }
            }
            else if (!_ogerCook.inventoryIsEmpty())
            {
                interactionManager._interactionTextline = "Press [E] to throw this away";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    Component item = _ogerCook.inventory[0];
                    perspectiveManager._dynamicObjects.Remove(item);
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
