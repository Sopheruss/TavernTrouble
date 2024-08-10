using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;
using System;

namespace SoftwareProjekt2024.Screens;

public class OptionMenuPause
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int _midScreenWidth;
    readonly int _midScreenHeight;

    private MouseState _currentMouse;
    private MouseState _previousMouse;

    readonly Button _returnButton;

    readonly int _edgeSpacer;

    readonly string _fullScreen;
    readonly string _fullScreenOnText;
    readonly Vector2 _onSize;
    readonly Vector2 _offSize;
    readonly string _fullScreenOffText;
    readonly Vector2 _fullScreenTextSize;
    readonly Texture2D _fullScreenOn;
    readonly Texture2D _fullScreenOff;
    readonly Rectangle _fullScreenRect;

    bool _fullIsClicked;

    readonly string _volume;
    readonly Vector2 _volumeTextSize;
    readonly Texture2D _volumeBarTexture;
    readonly Rectangle _volumeBarRect;
    readonly Texture2D _volumeButtonTexture;
    Rectangle _volumeButtonRect;

    readonly BitmapFont bmfont;

    private bool _isDraggingVolumeButton;
    private int _volumeButtonOffsetX;
    


    public OptionMenuPause(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        _midScreenWidth = screenWidth / 2;
        _midScreenHeight = screenHeight / 2;

        _returnButton = new Button(
            Content.Load<Texture2D>("Buttons/returnButton"),
            Content.Load<Texture2D>("Buttons/returnButtonHovering"),
            new Vector2(screenWidth - 70, screenHeight - 70));

        _edgeSpacer = 50;

        _fullScreen = "Fullscreen:";
        _fullScreenOffText = "Off";
        _fullScreenOnText = "On";
        _fullScreenOn = Content.Load<Texture2D>("Buttons/fullScreenButtonOn"); //not hovering = on
        _fullScreenOff = Content.Load<Texture2D>("Buttons/fullScreenButtonOff"); //hovering = off

        _volume = "Volume:";
        _volumeBarTexture = Content.Load<Texture2D>("Buttons/volumeBar");
        _volumeButtonTexture = Content.Load<Texture2D>("Buttons/volumeButton");

        // texture as well as fnt file have to be imported via content-pipline and monogame.extended importer. Beware of filestructure
        bmfont = Content.Load<BitmapFont>("Fonts/font_new");

        _fullScreenTextSize = bmfont.MeasureString(_fullScreen);
        _onSize = bmfont.MeasureString(_fullScreenOnText);
        _offSize = bmfont.MeasureString(_fullScreenOffText);
        _volumeTextSize = bmfont.MeasureString(_volume);

        _fullScreenRect = new Rectangle(_midScreenWidth - _fullScreenOn.Width / 2, 245, _fullScreenOn.Width, _fullScreenOn.Height);

        // init button and bar
        _volumeBarRect = new Rectangle(_midScreenWidth - (_volumeBarTexture.Width / 2) * 4, _midScreenHeight + 50, _volumeBarTexture.Width * 4, _volumeBarTexture.Height * 4);
        _volumeButtonRect = new Rectangle(_volumeBarRect.X, _midScreenHeight - _volumeButtonTexture.Height / 2 + 50, _volumeButtonTexture.Width * 4, _volumeButtonTexture.Height * 4);
    }

    public void Update()
    {
        _returnButton.Update();

        if (_returnButton.isClicked || _returnButton._escIsPressed)
        {
            _game.activeScene = Scenes.MAINMENU;
        }

        FullScreenIntersect();
        HandleVolumeButtonDragging();

        if (_fullIsClicked)
        {
            _game.fullScreen = !_game.fullScreen;
            _game._graphics.IsFullScreen = _game.fullScreen;
            _game._graphics.ApplyChanges();
            _fullIsClicked = false;
        }

    }

    public void FullScreenIntersect()
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


    private void HandleVolumeButtonDragging()
    {
        _previousMouse = _currentMouse;
        _currentMouse = Mouse.GetState();

        var mouseRect = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

        // Start dragging
        if (_volumeButtonRect.Contains(_previousMouse.X, _previousMouse.Y) && _currentMouse.LeftButton == ButtonState.Pressed)
        {
            _isDraggingVolumeButton = true;
            _volumeButtonOffsetX = _currentMouse.X - _volumeButtonRect.X;
        }

        // Stop dragging
        if (_currentMouse.LeftButton == ButtonState.Released)
        {
            _isDraggingVolumeButton = false;
        }

        // Dragging logic
        if (_isDraggingVolumeButton)
        {
            int newX = _currentMouse.X - _volumeButtonOffsetX;
            int minX = _volumeBarRect.X;
            int maxX = _volumeBarRect.X + _volumeBarRect.Width - _volumeButtonRect.Width;
            newX = Math.Clamp(newX, minX, maxX);

            _volumeButtonRect.X = newX;
            _game.VolumeLevel = (float)(newX - minX) / (maxX - minX); // Update volume level

        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _returnButton.Draw(_spriteBatch);

        _spriteBatch.DrawString(bmfont, _fullScreen, new Vector2(_midScreenWidth - _fullScreenTextSize.X / 2, 200), Color.Black);
        _spriteBatch.DrawString(bmfont, _fullScreenOffText, new Vector2(_midScreenWidth - _fullScreenRect.Width - 20, 250), Color.Black);
        _spriteBatch.DrawString(bmfont, _fullScreenOnText, new Vector2(_midScreenWidth + _fullScreenRect.Width + 20, 250), Color.Black);
        _spriteBatch.Draw(_fullScreenOn, _fullScreenRect, Color.White);

        _spriteBatch.DrawString(bmfont, _volume, new Vector2(_midScreenWidth - _volumeTextSize.X / 2, _midScreenHeight), Color.Black);

        _spriteBatch.Draw(_volumeBarTexture, _volumeBarRect, Color.White);
        _spriteBatch.Draw(_volumeButtonTexture, _volumeButtonRect, Color.White);

        // Ensure the volume button position is updated according to shared volume level
        int minX = _volumeBarRect.X;
        int maxX = _volumeBarRect.X + _volumeBarRect.Width - _volumeButtonRect.Width;
        _volumeButtonRect.X = (int)(minX + _game.VolumeLevel * (maxX - minX));

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