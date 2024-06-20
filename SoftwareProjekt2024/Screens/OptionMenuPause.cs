﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

public class OptionMenuPause
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int midScreenWidth;
    readonly int midScreenHeight;

    readonly Button _returnButton;

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
    }

    public void Update()
    {
        _returnButton.Update();

        if (_returnButton.isClicked)
        {
            _game.activeScene = Scenes.PAUSEMENU;
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _returnButton.Draw(_spriteBatch);

        _spriteBatch.End();
    }
}
