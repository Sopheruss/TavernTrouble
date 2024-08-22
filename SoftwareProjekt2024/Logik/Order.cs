using System.Collections.Generic;

namespace SoftwareProjekt2024.Logik
{
    public class Order
    {
        public List<Recipe> recipes;
        public bool hasDrink;
        public Order(bool _hasDrink, List<Recipe> _recipes)
        {
            recipes = _recipes;
            hasDrink = _hasDrink;
        }
    }
}
