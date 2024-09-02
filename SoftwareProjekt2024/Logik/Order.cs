using System.Collections.Generic;
using System.Linq;

namespace SoftwareProjekt2024.Logik
{
    public class Order
    {
        public List<Recipe> recipes;
        public bool hasDrink;
        public bool isFinished;
        public Order(bool _hasDrink, List<Recipe> _recipes)
        {
            recipes = _recipes;
            hasDrink = _hasDrink;
        }

        public bool Equals(Order other)
        {
            List<Recipe> myRecipes = recipes;
            List<Recipe> otherRecipes = other.recipes;
            foreach (Recipe recipe in myRecipes)
            {
                int matchingRecipeIndex = otherRecipes.FindIndex(t => t.name == recipe.name);
                if (matchingRecipeIndex == -1) return false;
                else
                {

                    otherRecipes.RemoveAt(matchingRecipeIndex);
                }
            }
            return !otherRecipes.Any() && hasDrink == other.hasDrink;
            //does not work for now
        }
    }
}
