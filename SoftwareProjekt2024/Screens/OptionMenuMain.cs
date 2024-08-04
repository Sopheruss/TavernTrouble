﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;
internal class OptionMenuMain
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int midScreenWidth;
    readonly int midScreenHeight;

    private MouseState _currentMouse;
    private MouseState _previousMouse;

    readonly Button _returnButton;

    Texture2D _controls;
    Rectangle _controlsRect;

    Texture2D _fullScreenOn;
    Texture2D _fullScreenOff;
    Rectangle _fullScreenRect;

    bool _fullIsClicked;

    //BitmapFont bmfont;

    public OptionMenuMain(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _returnButton = new Button(
            Content.Load<Texture2D>("Buttons/returnButton"),
            Content.Load<Texture2D>("Buttons/returnButtonHovering"),
            new Vector2(screenWidth - 70, screenHeight - 70));

        _controls = Content.Load<Texture2D>("OptionMenu/controls");
        _controlsRect = new Rectangle((screenWidth / 2) - 250, (screenHeight / 2) - 140, _controls.Width * 4, _controls.Height * 4);

        _fullScreenOn = Content.Load<Texture2D>("Buttons/fullScreenButtonOn"); //not hovering = on
        _fullScreenOff = Content.Load<Texture2D>("Buttons/fullScreenButtonOff"); //hovering = off
        _fullScreenRect = new Rectangle((screenWidth / 2) - 40, screenHeight - 150, _fullScreenOn.Width, _fullScreenOn.Height);

        // texture as well as fnt file have to be imported via content-pipline and monogame.extended importer. Beware of filestructure
        //bmfont = Content.Load<BitmapFont>("Fonts/font_new");
    }

    public void Update()
    {
        _returnButton.Update();

        if (_returnButton.isClicked || _returnButton._escIsPressed)
        {
            _game.activeScene = Scenes.MAINMENU;
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


        /*// Ensure bmfont is not null before drawing
        if (bmfont != null)
        {

            // Add the key bindings display
            string[] keys = { "W", "A", "S", "D", "E", "Esc" };
            string[] descriptions = {
                "Move Up",
                "Move Left",
                "Move Down",
                "Move Right",
                "Interact",
                "Pause/Menu"
            };

            Vector2 position = new Vector2(midScreenWidth - 150, midScreenHeight - 150); // Starting position for the table
            float lineHeight = bmfont.LineHeight + 5; // Line height with a small gap

            for (int i = 0; i < keys.Length; i++)
            {
                _spriteBatch.DrawString(bmfont, keys[i], position, Color.White);
                _spriteBatch.DrawString(bmfont, descriptions[i], new Vector2(position.X + 150, position.Y), Color.Black);
                position.Y += lineHeight; // Move down for the next line
            }
        }*/

        _spriteBatch.End();
    }
}

