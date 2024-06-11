using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using System.Diagnostics;
//using System.Numerics;

namespace SoftwareProjekt2024.Managers;

internal class InputManager
{
    Game1 _game;
    Player _ogerCook;
    CollisionManager _collisionManager;
    InteractionManager _interactionManager;
    AnimationManager _animationManager;

    Vector2 _previous_direction; //zitat Jan: "misleading name" -> was es macht ist sich die Direction zu speichern, die davor wichtig war 

    readonly Vector2 _left = new(-1, 0);
    readonly Vector2 _right = new(1, 0);
    readonly Vector2 _up = new(0, -1);
    readonly Vector2 _down = new(0, 1);

    readonly int halftileOffset = 16;       // half a tile -> 32px / 2 = 16px

    readonly int halfOgerOffsetX = 19/2;    // player rectangle draws in the middle, offset to left edge
    readonly int halfOgerOffsetY = 32/2;    // player rectangle draws in the middle, offset to left top

    readonly int cosmeticOffsetX = 5;       // for the looks (hardcoding)
    readonly int cosmeticOffsetY = 4;       // for the looks (hardcoding)

    Rectangle playerBounds;

    
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
        Commands();
        Moving();
        Interacting();
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
    } //moving close bracket 

    private Vector2 ConvertKeyToVector()
    {
        Vector2 _currentDirection = Vector2.Zero;

        playerBounds = _ogerCook.Rect;
        playerBounds.X -= (halftileOffset + halfOgerOffsetX + cosmeticOffsetX);
        playerBounds.Y -= (halftileOffset + halfOgerOffsetY + cosmeticOffsetY);

        Rectangle leftBounds = playerBounds;
        leftBounds.X -= 1;

        Rectangle rightBounds = playerBounds;
        rightBounds.X += playerBounds.Width;        // +width because manager tracks left side of rectangle (dunno why)

        Rectangle upBounds = playerBounds;
        upBounds.Y -= 1;

        Rectangle downBounds = playerBounds;
        downBounds.Y += halftileOffset;

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

    public void DrawRectHollow(SpriteBatch spriteBatch, Rectangle rect, int thickness, Texture2D rectangleTexture)
    {
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Bottom - thickness,
                rect.Width,
                thickness
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.X,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
        spriteBatch.Draw(
            rectangleTexture,
            new Rectangle(
                rect.Right - thickness,
                rect.Y,
                thickness,
                rect.Height
            ),
            Color.White
        );
    }

    public Rectangle GetPlayerbounds()
    {
        return playerBounds;
    }

    public void Interacting() // needs some changes, will not work atm
    {
        if (_interactionManager.CheckInteraction(playerBounds))
        {
            Debug.WriteLine("Interaction possible");
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                Debug.WriteLine("INTERACTION");
            }
        }
    }
}
