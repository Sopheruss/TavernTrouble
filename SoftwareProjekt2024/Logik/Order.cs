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
            return Enumerable.SequenceEqual(recipes.OrderBy(t => t), other.recipes.OrderBy(t => t)) && hasDrink == other.hasDrink;
        }
    }
}
