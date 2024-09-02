using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Linq;

namespace SoftwareProjekt2024.Components.StaticObjects
{
    internal class PlatePile : StaticObject
    {
        public PlatePile(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
        { }

        public override int getHeight()
        {
            return dest.Height - 10;
        }

        public static void HandleInteraction(Player _ogerCook, PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp)
        {
            if (_ogerCook.inventoryIsEmpty())
            {
                _perspectiveManager._dynamicObjects.Add(new Plate(Plate.plain, positionWhilePickedUp, _perspectiveManager));
                _ogerCook.pickUp(_perspectiveManager._dynamicObjects.Last());
            }
        }
    }
}
