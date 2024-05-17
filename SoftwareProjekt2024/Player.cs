using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareProjekt2024
{
    internal class Player : SpriteClasses.ScaledSprite
    {
        AnimationManager _animManager;
        public Player(Texture2D texture, Vector2 position, AnimationManager animationManager) : base(texture, position)
        { 
            _animManager = animationManager;
        }

        public override void Update()
        {
            base.Update();

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                position.X -= 1;
                _animManager.RowPos = 0; //changes Animation to Left 
            }
            
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                position.X += 1;
                    _animManager.RowPos = 1; //changes Animation to right 
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                position.Y -= 1;
                _animManager.RowPos = 2; //changes Animation to up
            }

            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                position.Y += 1;
                _animManager.RowPos = 3; //changes Animation to down 
            }
        }
    }
}
