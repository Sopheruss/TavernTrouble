using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components;

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


        Moving(); 

        //if collision stop moving 
    }

    public void Moving()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            position.X -= 1;
            _animManager.PlayAnimation = true; //playes Animation
            _animManager.RowPos = 0; //changes Animation to Left 
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            position.X += 1;
            _animManager.PlayAnimation = true;
            _animManager.RowPos = 1; //changes Animation to right 
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            position.Y -= 1;
            _animManager.PlayAnimation = true;
            _animManager.RowPos = 2; //changes Animation to up
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            position.Y += 1;
            _animManager.PlayAnimation = true;
            _animManager.RowPos = 3; //changes Animation to down 
        }
        else
        {
            _animManager.PlayAnimation = false; //stopps Animation
        }
    }

    public void NotMovingCollision()
    {
    }
}
