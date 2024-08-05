using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

public class OptionMenuPause
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int midScreenWidth;
    readonly int midScreenHeight;

    private MouseState _currentMouse;
    private MouseState _previousMouse;

    readonly Button _returnButton;

    Texture2D _fullScreenOn;
    Texture2D _fullScreenOff;
    Rectangle _fullScreenRect;

    bool _fullIsClicked;

    public OptionMenuPause(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _returnButton = new Button(
            Content.Load<Texture2D>("Buttons/returnButton"),
            Content.Load<Texture2D>("Buttons/returnButtonHovering"),
            new Vector2(screenWidth - 70, screenHeight - 70));

        _fullScreenOn = Content.Load<Texture2D>("Buttons/fullScreenButtonOn"); //not hovering = on
        _fullScreenOff = Content.Load<Texture2D>("Buttons/fullScreenButtonOff"); //hovering = off
        _fullScreenRect = new Rectangle((screenWidth / 2) - 40, screenHeight / 2, _fullScreenOn.Width, _fullScreenOn.Height);
    }

    public void FullScreenIntersaddect()
    {
        _fullIsClicked = false;

        _previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();

        var mouseRect = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

        if (mouseRect.Intersects(_fullScreenRect))
        {
            if (_currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
            {
                _fullIsClicked = true;
            }
        }
    }

    public void Update()
    {
        _returnButton.Update();

        if (_returnButton.isClicked || _returnButton._escIsPressed)
        {
            _game.activeScene = Scenes.PAUSEMENU;
        }

        FullScreenIntersect();

        if (_fullIsClicked)
        {
            _game.fullScreen = !_game.fullScreen;
            _game._graphics.IsFullScreen = _game.fullScreen;
            _game._graphics.ApplyChanges();
            _fullIsClicked = false;
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _returnButton.Draw(_spriteBatch);

        _spriteBatch.Draw(_fullScreenOn, _fullScreenRect, Color.White);

        _spriteBatch.Draw(_controls, _controlsRect, Color.White);


        if (_game.fullScreen)
        {
            _spriteBatch.Draw(_fullScreenOn, _fullScreenRect, Color.White);
        }
        else
        {
            _spriteBatch.Draw(_fullScreenOff, _fullScreenRect, Color.White);
        }

        _spriteBatch.End();
    }
}
