//Shoutout an Jan lol 

using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using System.Numerics;

namespace SoftwareProjekt2024.Managers;

internal class InputManager
{
    Game1 _game;
    Player _ogerCook;
    CollisionManager _collisionManager;
    AnimationManager _animationManager;

    Vector2 _previous_direction; //zitat Jan: "misleading name" -> was es macht ist sich die Direction zu speichern, die davor wichtig war 

    readonly Vector2 _left = new(-1, 0);
    readonly Vector2 _right = new(1, 0);
    readonly Vector2 _up = new(0, -1);
    readonly Vector2 _down = new(0, 1);

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
        Vector2 _currentDirection = ConvertKeyToVector();

        //stopps movement if no key is pressed 
        if (_currentDirection.Length() == 0)
        {
            StopMovement();
            return;
        }

        //if one key is pressed -> movement in one direction 
        if (_currentDirection.Length() == 1)
        {
            _previous_direction = _currentDirection;
        }
        //if more than one key is pressed -> first direction is saved if still pressed and second direction gets executed 
        else if (_currentDirection.Length() > 1)
        {
            if (_previous_direction.Length() == 0) //forbids diagonal movement 
            {
                StopMovement();
                return;
            }

            _currentDirection -= _previous_direction;
        }

        _ogerCook.position += _currentDirection;
        _animationManager.PlayAnimation = true;
        AnimationRow(_currentDirection); //sets row for animation 
    }

    private Vector2 ConvertKeyToVector()
    {
        Vector2 _currentDirection = Vector2.Zero;

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            _currentDirection += _left;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            _currentDirection += _right;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            _currentDirection += _up;
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            _currentDirection += _down;
        }

        return _currentDirection;
    }

    private void AnimationRow(Vector2 currentDirection)
    {
        if (currentDirection == _left)
        {
            _animationManager.RowPos = 0; //changes Animation to Left
        }
        else if (currentDirection == _right)
        {
            _animationManager.RowPos = 1; //changes Animation to right
        }
        else if (currentDirection == _up)
        {
            _animationManager.RowPos = 2; //changes Animation to up
        }
        else if (currentDirection == _down)
        {
            _animationManager.RowPos = 3; //changes Animation to down 
        }
    }

    private void StopMovement()
    {
        _previous_direction = Vector2.Zero;
        _animationManager.PlayAnimation = false;
    }
}
