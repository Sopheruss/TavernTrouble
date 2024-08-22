using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components
{
    internal class Mug : Component
    {
        public bool isFilled;
        public Mug(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager)
            : base(texture, position, perspectiveManager)
        {
            isFilled = false;
        }
    }
}
