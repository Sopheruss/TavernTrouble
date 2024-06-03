using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Managers;

internal class InputManager
{
    Game1 _game;
    Player _ogerCook;
    CollisionManager _collisionManager;
    AnimationManager _animationManager;
    int halftileOffsetX = 32 / 2;       //offset oger middle to left
    int halftileOffsetY = 32 / 2;       //offset oger míddle to top
    int cosmeticOffsetX = 8;            //for the looks
    int cosmeticOffsetY = 8;            //for the looks
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
        int ogerXwithOffset = (int)_ogerCook.position.X - halftileOffsetX;
        int ogerYwithOffset = (int)_ogerCook.position.Y - halftileOffsetY;
        Rectangle leftBounds = new Rectangle(ogerXwithOffset - cosmeticOffsetX, ogerYwithOffset, 19, 32);
        Rectangle rightBounds = new Rectangle(ogerXwithOffset + cosmeticOffsetX, ogerYwithOffset, 19, 32);
        Rectangle upBounds = new Rectangle(ogerXwithOffset, ogerYwithOffset - cosmeticOffsetY, 19, 32);
        Rectangle downBounds = new Rectangle(ogerXwithOffset, ogerYwithOffset + cosmeticOffsetY, 19, 32);

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            if (!_collisionManager.checkCollision(leftBounds))
            {
                _ogerCook.position.X -= 1;
                _animationManager.PlayAnimation = true; //playes Animation
                _animationManager.RowPos = 0; //changes Animation to Left
            }
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            if (!_collisionManager.checkCollision(rightBounds))
            {
                _ogerCook.position.X += 1;
                _animationManager.PlayAnimation = true;
                _animationManager.RowPos = 1; //changes Animation to right
            }
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            if (!_collisionManager.checkCollision(upBounds))
            {
                _ogerCook.position.Y -= 1;
                _animationManager.PlayAnimation = true;
                _animationManager.RowPos = 2; //changes Animation to up
            }
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            if (!_collisionManager.checkCollision(downBounds))
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
