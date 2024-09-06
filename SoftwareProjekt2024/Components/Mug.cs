using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components
{
    internal class Mug : DynamicObject
    {
        PerspectiveManager _perspectiveManager;

        public static Texture2D beerEmpty;
        public static Texture2D beerFull;

        public bool isFilled;
        public Mug(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager)
            : base(texture, position, perspectiveManager)
        {
            _perspectiveManager = perspectiveManager;
            state = (int)States.BeerEmpty;
            isFilled = false;
        }

        public void fill()
        {
            isFilled = true;
            texture = beerFull;
            state = (int)Component.States.BeerFull;
        }

        public void empty()
        {
            isFilled = false;
            texture = beerEmpty;
            state = (int)Component.States.BeerEmpty;
        }
    }
}
