using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components
{
    internal class Ingredient : DynamicObject
    {
        public Ingredient(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager)
            : base(texture, position, perspectiveManager)
        {

        }

        public virtual bool isPrepared()
        {
            return false;
        }
    }
}
