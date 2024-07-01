using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components
{
    internal class Bar_Links : StaticObject
    {
        public Bar_Links(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
            : base(texture, position, _dest, _src, perspectiveManager)
        {

        }
    }
}
