using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Logik;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Components;

internal class Guest : Component
{
    public bool hasOrdered;
    public Order guestOrder;
    public Guest(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)
    {
        hasOrdered = false;
    }

    public void takeOrder() //placeholder
    {
        guestOrder = new Order(false, new List<Recipe> { new Recipe("Burger") });
        hasOrdered = true;
    }


}