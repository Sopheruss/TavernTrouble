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
    public bool tableOrderfinished;

    public static Texture2D one;
    public static Texture2D two;
    public static Texture2D three;
    public static Texture2D four;
    public static Texture2D five;
    public static Texture2D six;
    public static Texture2D seven;
    public static Texture2D eight;

    public Table(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        occupiedSpots = 0;
        tableID = tableIDCount;
        tableIDCount++;
        tableContents = new List<Component>();
        tableOrderfinished = false;
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
            else if (!_ogerCook.inventoryIsEmpty() && guest.hasOrdered && occupiedSpots < capacity && (_ogerCook.inventory[0] is Plate || _ogerCook.inventory[0] is Mug))
            {
                addOrderItem(_ogerCook);
            }
        }
        else if (!isClean())
        {
            //dreckiges Geschirr wegr�umen logik hier

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

        if (item is Plate && (item as Plate).recipe is not null)
        {
            guest.order.addRecipe((item as Plate).recipe.name);
        }
        else if (item is Plate && (item as Plate).recipe is null)
        {
            guest.order.wrongComponentsCount++;
        }
        else if (item is Mug)
        {
            guest.order.addDrink(item as Mug);
        }

        if (orderFinished())
        {
            Debug.WriteLine("Order now finished!");
            tableOrderfinished = true;
            guest.eat(_ogerCook);
        }
    }

    public bool orderFinished()
    {
        if (occupiedSpots == capacity) return true;
        if (guest.order.isFinished) return true;
        return false;
    }

    public override void draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(texture, dest, src, Color.White);

        if (!isClean())
        {
            foreach (Component item in tableContents)
            {
                item.draw(_spriteBatch); //drawing Contents here to make them appear on top of table
            }
        }

        drawNumber(tableID, 22, 41, _spriteBatch, position, 1);

    }

    public static void drawNumber(int tableNumber, int spacerX, int spacerY, SpriteBatch _spriteBatch, Vector2 position, int scale)
    {
        switch (tableNumber)
        {
            case 0:
                _spriteBatch.Draw(one, new Rectangle((int)position.X + spacerX, (int)position.Y + spacerY, one.Width * scale, one.Height * scale), Color.White);
                break;
            case 1:
                _spriteBatch.Draw(two, new Rectangle((int)position.X + spacerX, (int)position.Y + spacerY, one.Width * scale, one.Height * scale), Color.White);
                break;
            case 2:
                _spriteBatch.Draw(three, new Rectangle((int)position.X + spacerX, (int)position.Y + spacerY, one.Width * scale, one.Height * scale), Color.White);
                break;
            case 3:
                _spriteBatch.Draw(four, new Rectangle((int)position.X + spacerX, (int)position.Y + spacerY, one.Width * scale, one.Height * scale), Color.White);
                break;
            case 4:
                _spriteBatch.Draw(five, new Rectangle((int)position.X + spacerX, (int)position.Y + spacerY, one.Width * scale, one.Height * scale), Color.White);
                break;
            case 5:
                _spriteBatch.Draw(six, new Rectangle((int)position.X + spacerX, (int)position.Y + spacerY, one.Width * scale, one.Height * scale), Color.White);
                break;
            case 6:
                _spriteBatch.Draw(seven, new Rectangle((int)position.X + spacerX, (int)position.Y + spacerY, one.Width * scale, one.Height * scale), Color.White);
                break;
            case 7:
                _spriteBatch.Draw(eight, new Rectangle((int)position.X + spacerX, (int)position.Y + spacerY, one.Width * scale, one.Height * scale), Color.White);
                break;
        }
    }
}
