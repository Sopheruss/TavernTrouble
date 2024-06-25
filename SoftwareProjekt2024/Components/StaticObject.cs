using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components
{
    internal class StaticObject : Component
    {
        Rectangle dest;
        Rectangle src;

        public StaticObject(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
            : base(texture, position, perspectiveManager)
        {
            dest = _dest;
            src = _src;
        }

        //Höhe des Destination Rectangle
        public override int getHeight()
        {
            return dest.Height;
        }

        public override void draw(SpriteBatch _spriteBatch, AnimationManager _animationManager)
        {
            _spriteBatch.Draw(texture, dest, src, Color.White);
        }
    }
}
