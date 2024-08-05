using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;
internal class OptionMenuMain
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int _screenWidth;
    readonly int _screenHeight;
    readonly int _midScreenWidth;
    readonly int _midScreenHeight;

    private MouseState _currentMouse;
    private MouseState _previousMouse;

    readonly Button _returnButton;

    readonly int _edgeSpacer;

    readonly string _controls;
    readonly Texture2D _controlsTexture;
    readonly Rectangle _controlsRect;
    readonly Vector2 _controlsTextSize;

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
    readonly Texture2D _volumeButton;
    readonly Rectangle _volumeButtonRect;

    readonly BitmapFont bmfont;

    public OptionMenuMain(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
        _midScreenWidth = screenWidth / 2;
        _midScreenHeight = screenHeight / 2;

        _returnButton = new Button(
            Content.Load<Texture2D>("Buttons/returnButton"),
            Content.Load<Texture2D>("Buttons/returnButtonHovering"),
            new Vector2(screenWidth - 70, screenHeight - 70));

        _edgeSpacer = 50;

        _controls = "Controls:";
        _controlsTexture = Content.Load<Texture2D>("OptionMenu/controls");
        int controlsTextureWidth = _controlsTexture.Width * 3;
        int controlsTextureHeight = _controlsTexture.Height * 3;
        _controlsRect = new Rectangle(_edgeSpacer, _midScreenHeight - controlsTextureHeight / 2, controlsTextureWidth, controlsTextureHeight);

        _fullScreen = "Fullscreen:";
        _fullScreenOffText = "Off";
        _fullScreenOnText = "On";
        _fullScreenOn = Content.Load<Texture2D>("Buttons/fullScreenButtonOn"); //not hovering = on
        _fullScreenOff = Content.Load<Texture2D>("Buttons/fullScreenButtonOff"); //hovering = off

        _volume = "Volume:";

        // texture as well as fnt file have to be imported via content-pipline and monogame.extended importer. Beware of filestructure
        bmfont = Content.Load<BitmapFont>("Fonts/font_new");
        _fullScreenTextSize = bmfont.MeasureString(_fullScreen);
        _controlsTextSize = bmfont.MeasureString(_controls);
        _onSize = bmfont.MeasureString(_fullScreenOnText);
        _offSize = bmfont.MeasureString(_fullScreenOffText);
        _volumeTextSize = bmfont.MeasureString(_volume);

        _fullScreenRect = new Rectangle(_midScreenWidth + _edgeSpacer + (int)_offSize.X + 20, 245, _fullScreenOn.Width, _fullScreenOn.Height);
    }

    public void Update()
    {
        _returnButton.Update();

        if (_returnButton.isClicked || _returnButton._escIsPressed)
        {
            _game.activeScene = Scenes.MAINMENU;
        }

        FullScreenIntersect();
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

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); // to make sharp images while scaling

        _returnButton.Draw(_spriteBatch);

        _spriteBatch.DrawString(bmfont, _fullScreen, new Vector2(_midScreenWidth + _edgeSpacer, 200), Color.Black);
        _spriteBatch.DrawString(bmfont, _fullScreenOffText, new Vector2(_midScreenWidth + _edgeSpacer, 250), Color.Black);
        _spriteBatch.DrawString(bmfont, _fullScreenOnText, new Vector2(_midScreenWidth + _edgeSpacer + _offSize.Y + 50 + _fullScreenRect.Width, 250), Color.Black);
        _spriteBatch.Draw(_fullScreenOn, _fullScreenRect, Color.White);

        _spriteBatch.DrawString(bmfont, _volume, new Vector2(_midScreenWidth + _edgeSpacer, _midScreenHeight), Color.Black);

        _spriteBatch.DrawString(bmfont, _controls, new Vector2(_edgeSpacer, 200), Color.Black);
        _spriteBatch.Draw(_controlsTexture, _controlsRect, Color.White);


        if (_game.fullScreen && _fullIsClicked)
        {
            _spriteBatch.Draw(_fullScreenOff, _fullScreenRect, Color.White);
            _game.fullScreen = false;
        }
        else if (_game.fullScreen == false && _fullIsClicked)
        {
            _spriteBatch.Draw(_fullScreenOn, _fullScreenRect, Color.White);
            _game.fullScreen = true;
        }
        else if (_game.fullScreen)
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

