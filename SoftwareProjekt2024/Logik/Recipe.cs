using SoftwareProjekt2024.Components.Ingredients;
using System;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Logik
{
    public class Recipe
    {
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
        }
    }
}
