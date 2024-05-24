using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareProjekt2024.Components
{
    internal class Component : SpriteClasses.ScaledSprite,IComparable<Component> 
    {
        public Texture2D texture;
        public Vector2 position;

        public Component(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position)
        {
            perspectiveManager.sortedComponents.Add(this);
        }

        public void draw(SpriteBatch _spriteBatch)
        {
            _spriteBatch.Draw(texture,
            position,
            null,
            Color.White,
            0f,
            new Vector2(texture.Width / 2, texture.Height / 2),
            0.2f,
            SpriteEffects.None,
            0f);
        }


        public int CompareTo(Component other)
        {
            if (this.position.Y < other.position.Y) return -1;
            if (this.position.Y == other.position.Y) return 0;
            return 1;
        }
    
    }
}
