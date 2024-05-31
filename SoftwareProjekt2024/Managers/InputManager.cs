using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Managers;

internal class InputManager
{
    Game1 _game;
    Player _ogerCook;
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



        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {


            _ogerCook.position.X -= 1;
            _animationManager.PlayAnimation = true; //playes Animation
            _animationManager.RowPos = 0; //changes Animation to Left

        }
        else if (Keyboard.GetState().IsKeyDown(Keys.D))
        {


            _ogerCook.position.X += 1;
            _animationManager.PlayAnimation = true;
            _animationManager.RowPos = 1; //changes Animation to right

        }
        else if (Keyboard.GetState().IsKeyDown(Keys.W))
        {


            _ogerCook.position.Y -= 1;
            _animationManager.PlayAnimation = true;
            _animationManager.RowPos = 2; //changes Animation to up

        }
        else if (Keyboard.GetState().IsKeyDown(Keys.S))
        {


            _ogerCook.position.Y += 1;
            _animationManager.PlayAnimation = true;
            _animationManager.RowPos = 3; //changes Animation to down 

        }
        else
        {
            _animationManager.PlayAnimation = false; //stopps Animation
        }
    }
}
