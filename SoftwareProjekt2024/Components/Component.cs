using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System;

namespace SoftwareProjekt2024.Components
{
    internal class Component : SpriteClasses.ScaledSprite, IComparable<Component> //erbt von ScaledSprite, implementiert Comparable
    {
        int heigth;
        public Component(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position)
        {
            heigth = getHeight();
            perspectiveManager._sortedComponents.Add(this); //bei Erstellung von Components werden sie in Liste geaddet
        }

        public virtual int getHeight() { return 0; }


        public int CompareTo(Component other) //sortiere nach Y Werten
        {
            if (this.position.Y + this.heigth < other.position.Y + other.heigth) return -1;
            if (this.position.Y + this.heigth == other.position.Y + other.heigth) return 0;
            return 1;
        }

        public virtual void draw(SpriteBatch spriteBatch, AnimationManager animationManager) { }
    }
}
