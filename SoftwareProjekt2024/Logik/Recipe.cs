using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Logik
{
    public class Recipe
    {

        public static Texture2D beer;
        public static Texture2D burger;
        public static Texture2D fries;
        public Texture2D currTexture;

        public static Dictionary<string, List<Type>> recipes = new Dictionary<string, List<Type>>()
        {
            {"Burger" , new List<Type>() {typeof(Bun), typeof(Meat), typeof(Salad) } },
            {"Fries", new List<Type>() {typeof(Potato) } }
        };

        public string name;
        public bool isFinished;
        public List<Type> recipeContents;

        public Recipe(string _recipeName)
        {
            name = _recipeName;
            isFinished = false;
            recipeContents = recipes[name];


            // Load appropriate texture based on the recipe name
            switch (name)
            {
                case "Burger":
                    currTexture = burger;
                    Debug.WriteLine("burgir");
                    break;
                case "Fries":
                    currTexture = fries;
                    break;
                case "Beer":
                    currTexture = beer;
                    break;
            }
        }
    }
}
