using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoftwareProjekt2024.SpriteClasses
{
    internal class ScaledSprite : Sprite
    {
        public int width = 40; //player width in pixel
        public int height = 50; //player height in pixel 
        public Rectangle Rect //dependent on position 
        {
            get
            {
                //property that is dependent on position 
                return new Rectangle((int)position.X, (int)position.Y, width, height);
            }
        }

        public ScaledSprite(Texture2D texture, Vector2 position)
                    : base(texture, position) //calls base constructor 
        { }
    }
}
