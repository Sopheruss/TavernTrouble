using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Managers;

internal class InputManager
{
    readonly Game1 _game;
    readonly Player _ogerCook;
    readonly CollisionManager _collisionManager;
    readonly InteractionManager _interactionManager;
    readonly AnimationManager _animationManager;

    Vector2 _previous_direction; //Direction speichern, die davor wichtig war

    readonly Vector2 _left = new(-1, 0);
    readonly Vector2 _right = new(1, 0);
    readonly Vector2 _up = new(0, -1);
    readonly Vector2 _down = new(0, 1);

    public InputManager(Game1 game, Player ogerCook, CollisionManager collisionManager, InteractionManager interactionManager, AnimationManager animationManager)
    {
        _game = game;
        _ogerCook = ogerCook;
        _collisionManager = collisionManager;
        _interactionManager = interactionManager;
        _animationManager = animationManager;
    }

    public void Update()
    {
        //Commands();
        Moving();
    }

    public void Commands()
    { }

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

            //if one key is pressed and than two in the opposite direction -> forbids diagonal movement 
            if (_currentDirection.Length() != 1)
            {
                StopMovement();
                return;
            }
        }

        _ogerCook.position += _currentDirection; // also dictates speed, multiply currDir with float
        _animationManager.PlayAnimation = true;
        AnimationRow(_currentDirection); //sets row for animation
    } //moving close bracket

    public Vector2 ConvertKeyToVector()
    {
        Vector2 _currentDirection = Vector2.Zero;
        (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) = _collisionManager.CalcPlayerBounds(_ogerCook);

        if (Keyboard.GetState().IsKeyDown(Keys.A))
        {
            if (!_collisionManager.CheckCollision(leftBounds))
            {
                _currentDirection += _left;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.D))
        {
            if (!_collisionManager.CheckCollision(rightBounds))
            {
                _currentDirection += _right;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.W))
        {
            if (!_collisionManager.CheckCollision(upBounds))
            {
                _currentDirection += _up;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.S))
        {
            if (!_collisionManager.CheckCollision(downBounds))
            {
                _currentDirection += _down;
            }
        }

        if (Keyboard.GetState().IsKeyDown(Keys.E))
        {
            int interactionState = _interactionManager.GetInteractionState();

            if (interactionState != 0) // != false
            {
                _interactionManager.HandleInteraction(interactionState); // gives ID of intersecting tile to interaction-handler
            }
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
