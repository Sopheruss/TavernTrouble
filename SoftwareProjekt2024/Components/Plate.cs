using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Logik;
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

        public Recipe recipe;

        public Plate(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager)
            : base(texture, position, perspectiveManager)
        {
            plateContents = new List<Component>();
            state = (int)States.Plate;
            _perspectiveManager = perspectiveManager;
        }

        ~Plate()
        {
            foreach (Component item in plateContents)
            {
                _perspectiveManager._dynamicObjects.Remove(item);   //delete all PlateContent items upon destruction
            }
        }

        public bool needsIngredient(Component ingredient)
        {
            Type ingredientType = ingredient.GetType();
            // Check if the recipe contains the type of ingredient and if plateContents has any items of that type
            if (recipe.recipeContents.Contains(ingredientType) && !plateContents.Any(item => item.GetType() == ingredientType))
            {
                return true;
            }
            return false;
        }

        public bool canAddIngredient(Component ingredient)
        {
            if ((ingredient as Ingredient).isPrepared()) //Ingredient has to be prepared
            {
                if (recipe is null)    //if plate is empty
                {
                    return true;
                }
                if (!(recipe is null) && needsIngredient(ingredient))
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
                            recipe = new Recipe("Burger");
                            break;
                        case (int)Component.States.DoneMeat:
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
                            recipe.isFinished = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case (int)Component.States.DoneMeat:
                    switch (state)
                    {
                        case (int)Component.States.Plate:
                            state = (int)Component.States.DoneMeat;
                            texture = withMeat;
                            recipe = new Recipe("Burger");
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
                            recipe.isFinished = true;
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
                            recipe = new Recipe("Burger");
                            break;
                        case (int)Component.States.Bun:
                            state = (int)Component.States.PlateWSaladBun;
                            texture = withBun_Salad;
                            break;
                        case (int)Component.States.DoneMeat:
                            state = (int)Component.States.PlateWSaladMeat;
                            texture = withMeat_Salad;
                            break;
                        case (int)Component.States.PlateWMeatBun:
                            state = (int)Component.States.PlateWBurger;
                            texture = withFullBurger;
                            recipe.isFinished = true;
                            break;
                        default:
                            break;
                    }
                    break;
                case (int)Component.States.Potato:
                    if (state == (int)Component.States.Plate)
                    {
                        state = (int)Component.States.PlateWFries;
                        texture = withFries;
                        recipe = new Recipe("Fries");
                        recipe.isFinished = true;
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
