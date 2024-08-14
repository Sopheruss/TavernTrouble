using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Components
{
    internal class Plate : DynamicObject
    {
        public static Texture2D plain;
        public static Texture2D withMeat;
        public static Texture2D withBun;
        public static Texture2D withSalad;
        public static Texture2D withMeat_Bun;
        public static Texture2D withMeat_Salad;
        public static Texture2D withBun_Salad;
        public static Texture2D withFullBurger;

        public List<Component> plateContents;

        public Plate(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager)
            : base(texture, position, perspectiveManager)
        {
            plateContents = new List<Component>();
            state = (int)States.Plate;
        }

    }
}
