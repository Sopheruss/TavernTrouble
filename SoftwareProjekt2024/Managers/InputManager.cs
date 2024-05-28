using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Managers;

internal class InputManager
{
    Player _ogerCook;
    CollisionManager _collisionManager;
    AnimationManager _animationManager;
    public InputManager(Player ogerCook, CollisionManager collisionManager, AnimationManager animationManager)
    {
        _ogerCook = ogerCook;
        _collisionManager = collisionManager;
        _animationManager = animationManager;
    }

    public void Update()
    {
        Moving();
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            
        }
    }
    public void Moving()
    {
        Rectangle newPlayerBounds = new Rectangle((int)(_ogerCook.position.X - _collisionManager._offsetX),
                                                  (int)(_ogerCook.position.Y - _collisionManager._offsetY), 19, 32);

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            if (!_collisionManager.CheckCollision(newPlayerBounds))
            {
                _ogerCook.position.X -= 1;
                _animationManager.PlayAnimation = true; //playes Animation
                _animationManager.RowPos = 0; //changes Animation to Left
            } 
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            if (!_collisionManager.CheckCollision(newPlayerBounds))
            {
                _ogerCook.position.X += 1;
                _animationManager.PlayAnimation = true;
                _animationManager.RowPos = 1; //changes Animation to right
            } 
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            if (!_collisionManager.CheckCollision(newPlayerBounds))
            {
                _ogerCook.position.Y -= 1;
                _animationManager.PlayAnimation = true;
                _animationManager.RowPos = 2; //changes Animation to up
            }
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            if (!_collisionManager.CheckCollision(newPlayerBounds))
            {
                _ogerCook.position.Y += 1;
                _animationManager.PlayAnimation = true;
                _animationManager.RowPos = 3; //changes Animation to down 
            }
        }
        else
        {
            _animationManager.PlayAnimation = false; //stopps Animation
        }
    }
}
