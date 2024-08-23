using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class Table : StaticObject
{
    public static int capacity = 4;
    public int occupiedSpots;
    public Guest guest;
    public int tableID;
    static int tableIDCount = 0;
    List<Component> tableContents;
    public Table(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        occupiedSpots = 0;
        tableID = tableIDCount;
        tableIDCount++;
        tableContents = new List<Component>();
    }

    public bool isClean()
    {
        return occupiedSpots == 0;
    }

    public bool hasGuest()
    {
        return guest != null;
    }

    public void HandleInteraction(PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp, Player _ogerCook)
    {
        if (hasGuest())
        {
            if (!guest.hasOrdered)
            {
                guest.takeOrder();
                Debug.WriteLine("Order taken");
            }
            else if (!_ogerCook.inventoryIsEmpty() && guest.hasOrdered && occupiedSpots < capacity && _ogerCook.inventory[0] is Plate)
            {
                addOrderItem(_ogerCook);
            }
            else if (!_ogerCook.inventoryIsEmpty() && guest.hasOrdered && occupiedSpots < capacity && _ogerCook.inventory[0] is Mug)
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
                return new Vector2(position.X + 15, position.Y + 16); //oben links
            case 1:
                return new Vector2(position.X + 32 + 2, position.Y + 16);   //oben rechts
            case 2:
                return new Vector2(position.X + 15, position.Y + 27); //unten links
            case 3:
                return new Vector2(position.X + 32 + 2, position.Y + 27); //unten rechts
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
        occupiedSpots++;
    }


}
