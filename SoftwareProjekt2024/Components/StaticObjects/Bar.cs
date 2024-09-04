using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class Bar : StaticObject
{
    public List<Component> barContents;
    public static bool _allowedInteraction = false;
    public Bar(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        barContents = new List<Component>();
        state = (int)States.Empty;
    }

    public bool isEmpty()
    {
        return barContents.Count == 0;
    }

    public void addItemToBar(Player _ogerCook)
    {
        Component item = _ogerCook.inventory[0];
        Debug.WriteLine("item: " + item);
        _ogerCook.inventory.Clear();
        _ogerCook.changeAppearence(1);  //reset appearence

        barContents.Add(item);
        item.position = new Vector2(position.X + 9, position.Y + 9);
    }

    public Bar GetBar()
    {
        return this;
    }

    public void HandleInteraction(PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp, Player _ogerCook)
    {
        if (!_ogerCook.inventoryIsEmpty())
        {
            if (isEmpty() && ((_ogerCook.inventory[0] is Plate) || (_ogerCook.inventory[0] is Mug))) //oger is holding a plate
            {
                addItemToBar(_ogerCook);
            }
            else if (!isEmpty() && _ogerCook.inventory[0] is Ingredient && barContents[0] is Plate && (barContents[0] as Plate).canAddIngredient(_ogerCook.inventory[0]))
            {
                (barContents[0] as Plate).addIngredient(_ogerCook);
            }
            else if (isEmpty() && _ogerCook.inventory[0] is Ingredient)
            {
                addItemToBar(_ogerCook);
            }
        }

        else if (_ogerCook.inventoryIsEmpty() && !isEmpty())

        {
            if (barContents[0] is Plate && (!(barContents[0] as Plate).plateContents.Any() || (barContents[0] as Plate).recipe.isFinished))
            {
                ClearBar(_ogerCook, positionWhilePickedUp);
            }
            else if (barContents[0] is Mug)
            {
                ClearBar(_ogerCook, positionWhilePickedUp);
            }
            else if (barContents[0] is Ingredient)
            {
                ClearBar(_ogerCook, positionWhilePickedUp);
            }
        }
    }

    public void ClearBar(Player _ogerCook, Vector2 positionWhilePickedUp)
    {
        Debug.WriteLine("Picking up");
        Component item = barContents[0];
        barContents.Clear();
        _ogerCook.pickUp(item);
        item.position = positionWhilePickedUp;
    }

    public bool AllowedInteraction(Player _ogerCook)
    {
        if (!_ogerCook.inventoryIsEmpty())
        {
            if (isEmpty() && ((_ogerCook.inventory[0] is Plate) || (_ogerCook.inventory[0] is Mug))) //oger is holding a plate
            {
                return true;
            }
            else if (!isEmpty() && _ogerCook.inventory[0] is Ingredient && barContents[0] is Plate && (barContents[0] as Plate).canAddIngredient(_ogerCook.inventory[0]))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (_ogerCook.inventoryIsEmpty() && !isEmpty()) //Implementation of plate method to see if plate/recipe is finished needed
        {
            if (barContents[0] is Plate && (!(barContents[0] as Plate).plateContents.Any() || (barContents[0] as Plate).recipe.isFinished) || barContents[0] is Mug)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public override void draw(SpriteBatch _spriteBatch)
    {
        _spriteBatch.Draw(texture, dest, src, Color.White);

        if (!isEmpty())
        {
            barContents[0].draw(_spriteBatch);   //drawing Contents here to make them appear on top of bar
        }
    }
}
