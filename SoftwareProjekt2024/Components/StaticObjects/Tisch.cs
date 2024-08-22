using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class Tisch : StaticObject
{
    public static int capacity = 4;
    public int occupiedSpots;
    public bool hasGuest;
    public Guest guest;
    int tablenumber;
    int tablenumberCount = 1;
    List<Component> tableContents;
    public Tisch(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        occupiedSpots = 0;
        hasGuest = false;
        tablenumber = tablenumberCount;
        tablenumberCount++;
        tableContents = new List<Component>();
    }

    public bool isClean()
    {
        return occupiedSpots == 0;
    }

    public void HandleInteraction(PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp, Player _ogerCook)
    {
        if (hasGuest)
        {
            if (!guest.hasOrdered)
            {
                guest.takeOrder();
                Debug.WriteLine("Order taken");
            }
            else if (guest.hasOrdered && occupiedSpots < capacity && _ogerCook.inventory[0] is Plate)
            {
                addOrderItem(_ogerCook);
            }
            else if (guest.hasOrdered && occupiedSpots < capacity && _ogerCook.inventory[0] is Mug)
            {
                //addOrderItem?
            }
        }
    }
    public override int getHeight()
    {
        return dest.Height - 13;
    }

    public Vector2 freePosition()
    {
        switch (occupiedSpots)
        {
            case 0:
                return new Vector2(position.X, position.Y);
            case 1:
                return new Vector2(position.X, position.Y);
            case 2:
                return new Vector2(position.X, position.Y);
            case 3:
                return new Vector2(position.X, position.Y);
            default:
                return new Vector2(position.X, position.Y);
        }
    }

    public void addOrderItem(Player _ogerCook)
    {
        Component item = _ogerCook.inventory[0];
        _ogerCook.inventory.Clear();
        _ogerCook.changeAppearence(1);

        tableContents.Add(item);
        item.position = freePosition();
    }


}
