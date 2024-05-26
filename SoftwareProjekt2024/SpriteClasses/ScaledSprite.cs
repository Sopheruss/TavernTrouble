using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                return new Rectangle((int)position.X, (int)position.Y, 50, 100);
            }
        }

        public ScaledSprite(Texture2D texture, Vector2 position)
                    : base(texture, position) //calls base constructor 
        { }
    }
}
