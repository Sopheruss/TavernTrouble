using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

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

        public static bool AllowedInteraction(Player _ogerCook)
        {
            return !_ogerCook.inventoryIsEmpty();
        }

        public static void HandleInteraction(Player _ogerCook, PerspectiveManager perspectiveManager)
        {
            Component item = _ogerCook.inventory[0];
            perspectiveManager._dynamicObjects.Remove(item);
            //int index = perspectiveManager._dynamicObjects.FindIndex(x => x == item);
            //Debug.WriteLine(perspectiveManager._dynamicObjects.Count);
            _ogerCook.inventory.Clear();
            _ogerCook.texture = Player.plain;

        }
    }
}
