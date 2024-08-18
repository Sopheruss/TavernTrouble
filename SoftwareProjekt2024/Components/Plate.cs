using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SoftwareProjekt2024.Components
{
    internal class Plate : DynamicObject
    {
        PerspectiveManager _perspectiveManager;

        public static Texture2D plain;
        public static Texture2D withMeat;
        public static Texture2D withBun;
        public static Texture2D withSalad;
        public static Texture2D withMeat_Bun;
        public static Texture2D withMeat_Salad;
        public static Texture2D withBun_Salad;
        public static Texture2D withFullBurger;
        public static Texture2D withFries;

        public List<Component> plateContents;

        public Dictionary<string, List<Type>> recipes = new Dictionary<string, List<Type>>()
        {
            {"Burger" , new List<Type>() {typeof(Bun), typeof(Meat), typeof(Salad) } },
            {"Fries", new List<Type>() {typeof(Potato) } }
        };

        public string recipe;
        public bool recipeIsFinished;

        public Plate(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager)
            : base(texture, position, perspectiveManager)
        {
            plateContents = new List<Component>();
            state = (int)States.Plate;
            recipe = "Not defined";
            _perspectiveManager = perspectiveManager;
            recipeIsFinished = false;
        }

        ~Plate()
        {
            foreach (Component item in plateContents)
            {
                _perspectiveManager._dynamicObjects.Remove(item);   //delete all PlateContent items upon destruction
            }
        }

        public bool needsIngredient(string recipe, Component ingredient)
        {
            Type ingredientType = ingredient.GetType();
            // Check if the recipe contains the type of ingredient and if plateContents has any items of that type
            if (recipes[recipe].Contains(ingredientType) && !plateContents.Any(item => item.GetType() == ingredientType))
            {
                return true;
            }
            return false;
        }

        public bool canAddIngredient(Component ingredient)
        {
            if ((ingredient as Ingredient).isPrepared()) //Ingredient has to be prepared
            {
                if (recipe == "Not defined")    //if plate is empty
                {
                    return true;
                }
                if (!(recipe == "Not defined") && needsIngredient(recipe, ingredient))
                { //if plate is not empty and ingredient is needed for recipe
                    return true;
                }
                return false;
            }
            return false;
        }

        public void addIngredient(Player _ogerCook)
        {
            Component item = _ogerCook.inventory[0];
            _ogerCook.inventory.Clear();
            _ogerCook.changeAppearence(1);

            plateContents.Add(item);
            changeAppearence(item.state);
        }

        public void changeAppearence(int itemState)
        {
            switch (itemState)
            {
                case (int)Component.States.Bun:
                    switch (state)
                    {
                        case (int)Component.States.Plate:
                            state = (int)Component.States.Bun;
                            texture = withBun;
                            break;
                        case (int)Component.States.Meat:
                            state = (int)Component.States.PlateWMeatBun;
                            texture = withMeat_Bun;
                            break;
                        case (int)Component.States.Salad:
                            state = (int)Component.States.PlateWSaladBun;
                            texture = withBun_Salad;
                            break;
                        case (int)Component.States.PlateWSaladMeat:
                            state = (int)Component.States.PlateWBurger;
                            texture = withFullBurger;
                            recipeIsFinished = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case (int)Component.States.Meat:
                    switch (state)
                    {
                        case (int)Component.States.Plate:
                            state = (int)Component.States.Meat;
                            texture = withMeat;
                            break;
                        case (int)Component.States.Bun:
                            state = (int)Component.States.PlateWMeatBun;
                            texture = withMeat_Bun;
                            break;
                        case (int)Component.States.Salad:
                            state = (int)Component.States.PlateWSaladMeat;
                            texture = withMeat_Salad;
                            break;
                        case (int)Component.States.PlateWSaladBun:
                            state = (int)Component.States.PlateWBurger;
                            texture = withFullBurger;
                            recipeIsFinished = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case (int)Component.States.Salad:
                    switch (state)
                    {
                        case (int)Component.States.Plate:
                            state = (int)Component.States.Salad;
                            texture = withSalad;
                            break;
                        case (int)Component.States.Bun:
                            state = (int)Component.States.PlateWSaladBun;
                            texture = withBun_Salad;
                            break;
                        case (int)Component.States.Meat:
                            state = (int)Component.States.PlateWSaladMeat;
                            texture = withMeat_Salad;
                            break;
                        case (int)Component.States.PlateWMeatBun:
                            state = (int)Component.States.PlateWBurger;
                            texture = withFullBurger;
                            recipeIsFinished = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case (int)Component.States.Potato:
                    state = (int)Component.States.PlateWFries;
                    texture = withFries;
                    recipeIsFinished = true;
                    break;
                default:
                    break;
            }
        }
    }
}
