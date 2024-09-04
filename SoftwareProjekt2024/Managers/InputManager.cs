using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Managers;

internal class InputManager
{
    readonly Game1 _game;
    readonly Player _ogerCook;
    readonly CollisionManager _collisionManager;
    readonly InteractionManager _interactionManager;
    readonly PerspectiveManager _perspectiveManager;

    readonly Vector2 _left = new(-1, 0);
    readonly Vector2 _right = new(1, 0);
    readonly Vector2 _up = new(0, -1);
    readonly Vector2 _down = new(0, 1);

    enum Direction { Left, Right, Up, Down, None };

    readonly List<Direction> curDirs;

    public static KeyboardState _currentKeyState;
    public static KeyboardState _previousKeyState;

    public float speed = 2.0f;

    public bool pressedE = false;

    public InputManager(Game1 game, Player ogerCook, CollisionManager collisionManager, InteractionManager interactionManager, PerspectiveManager perspectiveManager)
    {
        _game = game;
        _ogerCook = ogerCook;
        _collisionManager = collisionManager;
        _interactionManager = interactionManager;
        _perspectiveManager = perspectiveManager;

        curDirs = new List<Direction>();
    }

    public void Update()
    {
        GetKeyboardState();
        Moving();
    }

    public void Moving()
    {
        UpdateCurDirs();

        //stopps movement if no key is pressed
        if (curDirs.Count == 0)
        {
            Player._playerAnimationManager.PlayAnimation = false;
            return;
        }

        // Check best direction to move to:
        // CurDirs contains all possible movements with the last entry being the best option, the second last entry being the second 
        // best option and so on. If the best option is not possible, choose the second best option, etc. If no option is possible
        // (e.g. bc character is standing in a corner) the target direction is 'None'.
        Direction targetDir = Direction.None;
        (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) = _collisionManager.CalcPlayerBounds(_ogerCook);

        for (int i = curDirs.Count - 1; i >= 0; i--)
        {
            targetDir = curDirs[i];
            if (!HasCollisionInDirection(targetDir, leftBounds, rightBounds, upBounds, downBounds)) break;
            if (i == 0) targetDir = Direction.None;
        }

        // If character is stuck (i.e. target direction is None), do not move at all:
        if (targetDir == Direction.None)
        {
            Player._playerAnimationManager.PlayAnimation = false;
            return;
        }

        // Move character:
        _ogerCook.position += DirectionToVector(targetDir) * speed; // also dictates speed, multiply currDir with float
        Player._playerAnimationManager.PlayAnimation = true;
        AnimationRow(DirectionToVector(targetDir)); //sets row for animation

    } //moving close bracket


    // Updates current directions by checking direction buttons:
    private void UpdateCurDirs()
    {

        // If 'A' is pressed and has not been pressed during the last frame, 'Left' is the best movement direction.
        // => Append 'Left' to current directions.
        if ((Keyboard.GetState().IsKeyDown(Keys.A) || Keyboard.GetState().IsKeyDown(Keys.Left)) && !curDirs.Contains(Direction.Left))
        {
            curDirs.Add(Direction.Left);
        }

        // If 'A' is not pressed and has been pressed during the last frame, the current directions need to be updated.
        // => 'Left' is removed from list.
        else if ((!Keyboard.GetState().IsKeyDown(Keys.A) && !Keyboard.GetState().IsKeyDown(Keys.Left)) && curDirs.Contains(Direction.Left))
        {
            curDirs.Remove(Direction.Left);
        }

        // Repeat process for all other directions:
        if ((Keyboard.GetState().IsKeyDown(Keys.W) || Keyboard.GetState().IsKeyDown(Keys.Up)) && !curDirs.Contains(Direction.Up))
        {
            curDirs.Add(Direction.Up);
        }
        else if ((!Keyboard.GetState().IsKeyDown(Keys.W) && !Keyboard.GetState().IsKeyDown(Keys.Up)) && curDirs.Contains(Direction.Up))
        {
            curDirs.Remove(Direction.Up);
        }

        if ((Keyboard.GetState().IsKeyDown(Keys.D) || Keyboard.GetState().IsKeyDown(Keys.Right)) && !curDirs.Contains(Direction.Right))
        {
            curDirs.Add(Direction.Right);
        }
        else if ((!Keyboard.GetState().IsKeyDown(Keys.D) && !Keyboard.GetState().IsKeyDown(Keys.Right)) && curDirs.Contains(Direction.Right))
        {
            curDirs.Remove(Direction.Right);
        }

        if ((Keyboard.GetState().IsKeyDown(Keys.S) || Keyboard.GetState().IsKeyDown(Keys.Down)) && !curDirs.Contains(Direction.Down))
        {
            curDirs.Add(Direction.Down);
        }
        else if ((!Keyboard.GetState().IsKeyDown(Keys.S) && !Keyboard.GetState().IsKeyDown(Keys.Down)) && curDirs.Contains(Direction.Down))
        {
            curDirs.Remove(Direction.Down);
        }

        // Check for interaction:
        if (HasBeenPressed(Keys.E))
        {
            pressedE = true;
        }
        else
        {
            pressedE = false;
        }
    }

    public static void GetKeyboardState()
    {
        _previousKeyState = _currentKeyState;
        _currentKeyState = Keyboard.GetState();
    }

    public static bool HasBeenPressed(Keys key)
    {
        return _currentKeyState.IsKeyDown(key) && !_previousKeyState.IsKeyDown(key);
    }

    // Check if moving the character to given direction results in collision:
    private bool HasCollisionInDirection(Direction dir, Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) => dir switch
    {
        Direction.Left => _collisionManager.CheckCollision(leftBounds),
        Direction.Up => _collisionManager.CheckCollision(upBounds),
        Direction.Right => _collisionManager.CheckCollision(rightBounds),
        Direction.Down => _collisionManager.CheckCollision(downBounds),
        _ => false,
    };

    // Translates Direction enum into 2D Vector:
    private Vector2 DirectionToVector(Direction dir) => dir switch
    {
        Direction.Left => _left,
        Direction.Up => _up,
        Direction.Right => _right,
        Direction.Down => _down,
        _ => Vector2.Zero,
    };

    private void AnimationRow(Vector2 currentDirection)
    {
        if (currentDirection == _left)
        {
            Player._playerAnimationManager.RowPos = 0; //changes Animation to Left
        }
        else if (currentDirection == _right)
        {
            Player._playerAnimationManager.RowPos = 1; //changes Animation to right

        }
        else if (currentDirection == _up)
        {
            Player._playerAnimationManager.RowPos = 2; //changes Animation to up
        }
        else if (currentDirection == _down)
        {
            Player._playerAnimationManager.RowPos = 3; //changes Animation to down
        }
    }
}
