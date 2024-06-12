using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System;

namespace SoftwareProjekt2024.Components
{
    internal class Component : SpriteClasses.ScaledSprite, IComparable<Component> //erbt von ScaledSprite, implementiert Comparable
    {

        public Component(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position)
        {
            perspectiveManager._sortedComponents.Add(this); //bei Erstellung von Components werden sie in Liste geaddet
        }




        public int CompareTo(Component other) //sortiere nach Y Werten
        {
            if (this.position.Y < other.position.Y) return -1;
            if (this.position.Y == other.position.Y) return 0;
            return 1;
        }

        public void draw(SpriteBatch _spriteBatch, AnimationManager _animationManager) // generalisierter Aufruf der Spritedraw Methode
        {
            _spriteBatch.Draw(
            this.texture,                                //texture 
            this.Rect,                                  //destinationRectangle
            _animationManager.GetFrame(),                   //sourceRectangle (frame) 
            Color.White,                                   //color
            0f,                                           //rotation 
            new Vector2(                                 //origin -> to place center texture correctly
                this.texture.Width / 4,
                this.texture.Width / 4),
            SpriteEffects.None,                        //effects
            1f);                                      //layer depth
        }
    }
}
