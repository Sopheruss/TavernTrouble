using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Managers;

internal class InputManager
{
    Game1 _game;
    Player _ogerCook;
    float newOgerPosX;
    float newOgerPosY;
    CollisionManager _collisionManager;
    AnimationManager _animationManager;
    public InputManager(Game1 game, Player ogerCook, CollisionManager collisionManager, AnimationManager animationManager)
    {
        _game = game;
        _ogerCook = ogerCook;
        _collisionManager = collisionManager;
        _animationManager = animationManager;
    }

    public void Update()
    {
        Commands();
        Moving();
    }
    
    public void Commands()
    {
        // exit, pause, ...
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            _game.Exit();
        }
    }
    public void Moving()
    {
        newOgerPosX = _ogerCook.position.X - _collisionManager._offsetX;
        newOgerPosY = _ogerCook.position.Y - _collisionManager._offsetY;
        
        Rectangle newPlayerBounds = new Rectangle((int)newOgerPosX, (int)newOgerPosY, 19, 32);
        
        Rectangle leftBounds = new Rectangle((int)newOgerPosX - 1, (int)newOgerPosY, 19, 32);
        Rectangle rightBounds = new Rectangle((int)newOgerPosX + 1, (int)newOgerPosY, 19, 32);
        Rectangle upBounds = new Rectangle((int)newOgerPosX, (int)newOgerPosY - 1, 19, 32);
        Rectangle downBounds = new Rectangle((int)newOgerPosX, (int)newOgerPosY + 1, 19, 32);

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            if (!_collisionManager.CheckCollision(leftBounds))
            {
                _ogerCook.position.X -= 1;
                _animationManager.PlayAnimation = true; //playes Animation
                _animationManager.RowPos = 0; //changes Animation to Left
            } 
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            if (!_collisionManager.CheckCollision(rightBounds))
            {
                _ogerCook.position.X += 1;
                _animationManager.PlayAnimation = true;
                _animationManager.RowPos = 1; //changes Animation to right
            } 
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            if (!_collisionManager.CheckCollision(upBounds))
            {
                _ogerCook.position.Y -= 1;
                _animationManager.PlayAnimation = true;
                _animationManager.RowPos = 2; //changes Animation to up
            }
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            if (!_collisionManager.CheckCollision(downBounds))
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
