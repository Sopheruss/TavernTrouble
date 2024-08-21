using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class Grill : StaticObject
{
    public static List<Component> grillContents;
    public static int capacity = 4;
    public Grill(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        grillContents = new List<Component>();
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public static void HandleInteraction(Player _ogerCook)
    {
        if (!_ogerCook.inventoryIsEmpty() && _ogerCook.inventory[0] is Meat && grillContents.Count < capacity)
        {
            Component item = _ogerCook.inventory[0];
            _ogerCook.inventory.Clear();
            _ogerCook.changeAppearence(1);

            grillContents.Add(item);
            (item as Meat).cooked = true; //implementation of proper cooking method needed
        }
    }
}
