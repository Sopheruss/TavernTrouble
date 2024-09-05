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

    public void addIngreditentToBar(Player _ogerCook)
    {
        Component item = _ogerCook.inventory[0];
        Debug.WriteLine("item: " + item);
        _ogerCook.inventory.Clear();
        _ogerCook.changeAppearence(1);  //reset appearence

        workStationContents.Add(item);
        item.position = new Vector2(position.X + 11, position.Y + 22);
    }


    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public void HandleInteraction(PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp, Player _ogerCook, InteractionManager interactionManager, InputManager inputManager)
    {
        if (!_ogerCook.inventoryIsEmpty())
        {
            if (isEmpty() && ((_ogerCook.inventory[0] is Plate) || (_ogerCook.inventory[0] is Mug))) //oger is holding a plate or mug
            {
                if (_ogerCook.inventory[0] is Plate && (_ogerCook.inventory[0] as Plate).recipe != null && (_ogerCook.inventory[0] as Plate).recipe.isFinished)
                {
                    interactionManager._interactionTextline = "Press [E] to put meal on bar space";
                    interactionManager._allowedInteraction = true;
                }
                else if (_ogerCook.inventory[0] is Plate && (_ogerCook.inventory[0] as Plate).recipe != null && !(_ogerCook.inventory[0] as Plate).recipe.isFinished)
                {
                    interactionManager._interactionTextline = "Press [E] to put unfinished meal on bar space";
                    interactionManager._allowedInteraction = true;
                }
                else if (_ogerCook.inventory[0] is Plate)
                {
                    interactionManager._interactionTextline = "Press [E] to put plate on bar space";
                    interactionManager._allowedInteraction = true;
                }
                else if (_ogerCook.inventory[0] is Mug && (_ogerCook.inventory[0] as Mug).isFilled)
                {
                    interactionManager._interactionTextline = "Press [E] to put beer on bar space";
                    interactionManager._allowedInteraction = true;
                }
                else if(_ogerCook.inventory[0] is Mug)
                {
                    interactionManager._interactionTextline = "Press [E] to put tankard on bar space";
                    interactionManager._allowedInteraction = true;
                }
                if (inputManager.pressedE)
                {
                    addPlateOrMug(_ogerCook);
                }
            }
            else if (!isEmpty() && _ogerCook.inventory[0] is Ingredient && workStationContents[0] is Plate && (workStationContents[0] as Plate).canAddIngredient(_ogerCook.inventory[0]))
            {
                interactionManager._interactionTextline = "Press [E] to put ingredient on plate";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    (workStationContents[0] as Plate).addIngredient(_ogerCook);
                }
            }
            else if (isEmpty() && _ogerCook.inventory[0] is Ingredient)
            {
                interactionManager._interactionTextline = "Press [E] to put ingredient on bar space";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    addIngreditentToBar(_ogerCook);
                }
            }
            else
            {
                interactionManager._allowedInteraction = false;
            }
        }
        else if (_ogerCook.inventoryIsEmpty() && !isEmpty()) //Implementation of plate method to see if plate/recipe is finished needed
        {
            if (workStationContents[0] is Plate && (workStationContents[0] as Plate).recipe == null)
            {
                interactionManager._interactionTextline = "Press [E] to grab plate";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    ClearBar(_ogerCook, positionWhilePickedUp);
                }
            }
            else if (workStationContents[0] is Plate && (workStationContents[0] as Plate).recipe != null && (workStationContents[0] as Plate).recipe.isFinished)
            {
                interactionManager._interactionTextline = "Press [E] to grab meal";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    ClearBar(_ogerCook, positionWhilePickedUp);
                }
            }
            else if (workStationContents[0] is Plate && (workStationContents[0] as Plate).recipe != null && !(workStationContents[0] as Plate).recipe.isFinished)
            {
                interactionManager._interactionTextline = "Press [E] to grab unfinished meal";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    ClearBar(_ogerCook, positionWhilePickedUp);
                }
            }
            else if (workStationContents[0] is Mug && (workStationContents[0] as Mug).isFilled)
            {
                interactionManager._interactionTextline = "Press [E] to grab beer";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    ClearBar(_ogerCook, positionWhilePickedUp);
                }
            }
            else if (workStationContents[0] is Mug)
            {
                interactionManager._interactionTextline = "Press [E] to grab tankard";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    ClearBar(_ogerCook, positionWhilePickedUp);
                }
            }
            else if (workStationContents[0] is Ingredient)
            {
                interactionManager._interactionTextline = "Press [E] to grab ingredient";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    ClearBar(_ogerCook, positionWhilePickedUp);
                }
            }
            else
            {
                interactionManager._allowedInteraction = false;
            }
        }
        else
        {
            interactionManager._allowedInteraction = false;
        }
    }

    public void ClearBar(Player _ogerCook, Vector2 positionWhilePickedUp)
    {
        Debug.WriteLine("Picking up");
        Component item = workStationContents[0];
        workStationContents.Clear();
        _ogerCook.pickUp(item);
        item.position = positionWhilePickedUp;
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
