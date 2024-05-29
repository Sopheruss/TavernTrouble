using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components
{
    internal class Component : SpriteClasses.ScaledSprite, IComparable<Component>
    {

        public Component(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position)
        {
            perspectiveManager._sortedComponents.Add(this);
        }

        public void draw(SpriteBatch _spriteBatch, AnimationManager _animationManager)
        {
            _spriteBatch.Draw(
            this.texture,
            this.Rect,                                 //destinationRectangle
            _animationManager.GetFrame(),              //sourceRectangle (frame) 
            Color.White,
            0f,                                        //rotation 
            new Vector2(                               //origin -> to place center texture correctly
                this.texture.Width / 4,
                this.texture.Width / 4),
            SpriteEffects.None,
            1f);                                      //layer depth
        }


        public int CompareTo(Component other)
        {
            if (this.position.Y < other.position.Y) return -1;
            if (this.position.Y == other.position.Y) return 0;
            return 1;
        }

    }
}
