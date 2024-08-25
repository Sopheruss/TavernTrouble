using SoftwareProjekt2024.Components.Ingredients;
using System;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Logik
{
    public class Recipe
    {

        public Dictionary<string, List<Type>> recipes = new Dictionary<string, List<Type>>()
        {
            {"Burger" , new List<Type>() {typeof(Bun), typeof(Meat), typeof(Salad) } },
            {"Fries", new List<Type>() {typeof(Potato) } }
        };

        public string recipeName;
        public bool recipeIsFinished;

        public Recipe(string _recipeName)
        {
            recipeName = _recipeName;
            recipeIsFinished = false;
        }
    }
}
