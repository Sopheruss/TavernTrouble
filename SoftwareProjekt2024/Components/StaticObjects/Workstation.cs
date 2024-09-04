using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class Workstation : StaticObject
{
    public List<Component> workStationContents;
    public Workstation(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        workStationContents = new List<Component>();
        state = (int)States.Empty;
    }
    public bool isEmpty()
    {
        return workStationContents.Count == 0;
    }

    public void addPlateOrMug(Player _ogerCook)
    {
        Component item = _ogerCook.inventory[0];
        _ogerCook.inventory.Clear();
        _ogerCook.changeAppearence(1);  //reset appearence

        workStationContents.Add(item);
        item.position = new Vector2(position.X + 9, position.Y + 20);
    }


    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public void HandleInteraction(PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp, Player _ogerCook)
    {
        if (!_ogerCook.inventoryIsEmpty())
        {
            if (isEmpty() && ((_ogerCook.inventory[0] is Plate) || (_ogerCook.inventory[0] is Mug))) //oger is holding a plate
            {
                addPlateOrMug(_ogerCook);
            }
            else if (!isEmpty() && _ogerCook.inventory[0] is Ingredient && workStationContents[0] is Plate && (workStationContents[0] as Plate).canAddIngredient(_ogerCook.inventory[0]))
            {
                (workStationContents[0] as Plate).addIngredient(_ogerCook);
            }
        }


        else if (_ogerCook.inventoryIsEmpty() && !isEmpty()) //Implementation of plate method to see if plate/recipe is finished needed
        {
            if (workStationContents[0] is Plate && (workStationContents[0] as Plate).recipeIsFinished)
            {
                Debug.WriteLine("Picking up");
                Component item = workStationContents[0];
                workStationContents.Clear();
                _ogerCook.pickUp(item);
                item.position = positionWhilePickedUp;
            }
            else if (workStationContents[0] is Mug)
            {
                Debug.WriteLine("Picking up");
                Component item = workStationContents[0];
                workStationContents.Clear();
                _ogerCook.pickUp(item);
                item.position = positionWhilePickedUp;
            }
        }
    }

    public bool AllowedInteraction(Player _ogerCook)
    {
        if (!_ogerCook.inventoryIsEmpty())
        {
            if (isEmpty() && ((_ogerCook.inventory[0] is Plate) || (_ogerCook.inventory[0] is Mug))) //oger is holding a plate
            {
                return true;
            }
            else if (!isEmpty() && _ogerCook.inventory[0] is Ingredient && workStationContents[0] is Plate && (workStationContents[0] as Plate).canAddIngredient(_ogerCook.inventory[0]))
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
            if ((workStationContents[0] is Plate && ((workStationContents[0] as Plate).recipeIsFinished) || workStationContents[0] is Mug))
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
            workStationContents[0].draw(_spriteBatch);   //drawing Contents here to make them appear on top of bar
        }
    }
}
