﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class Bar : StaticObject
{
    public List<Component> barContents;
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

    public void addPlate(Player _ogerCook)
    {
        Component item = _ogerCook.inventory[0];
        _ogerCook.inventory.Clear();
        _ogerCook.changeAppearence(1);  //reset appearence

        barContents.Add(item);
        item.position = new Vector2(position.X + 9, position.Y + 9);
    }

    public void HandleInteraction(PerspectiveManager _perspectiveManager, Vector2 positionWhilePickedUp, Player _ogerCook)
    {
        if (!_ogerCook.inventoryIsEmpty())
        {
            if (isEmpty() && _ogerCook.inventory[0] is Plate) //oger is holding a plate
            {
                addPlate(_ogerCook);
            }
            else if (_ogerCook.inventory[0] is Ingredient && barContents[0] is Plate && (barContents[0] as Plate).canAddIngredient(_ogerCook.inventory[0]))
            {
                (barContents[0] as Plate).addIngredient(_ogerCook);
            }
        }


        else if (_ogerCook.inventoryIsEmpty() && !isEmpty() && (barContents[0] as Plate).recipeIsFinished) //Implementation of plate method to see if plate/recipe is finished needed
        {
            Debug.WriteLine("Picking up");
            Component item = barContents[0];
            barContents.Clear();
            _ogerCook.pickUp(item);
            item.position = positionWhilePickedUp;
        }
    }
}