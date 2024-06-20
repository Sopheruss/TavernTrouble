using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoftwareProjekt2024.SpriteClasses
{
    internal class ScaledSprite : Sprite
    {
        public Rectangle Rect //dependent on position 
        {
            get
            {
                //100/200 has to change acordingly (is the scale of the sprite)
                //property that is dependent on position 
                return new Rectangle((int)position.X, (int)position.Y, 25, 50);
            }
        }

        public ScaledSprite(Texture2D texture, Vector2 position)
                    : base(texture, position) //calls base constructor 
        { }
    }
}
