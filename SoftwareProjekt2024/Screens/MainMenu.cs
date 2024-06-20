using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

public class MainMenu
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly Button _startButton;
    readonly Button _optionButton;
    readonly Button _quitButton;

    readonly int midScreenWidth;
    readonly int midScreenHeight;


    public MainMenu(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _startButton = new Button(
            Content.Load<Texture2D>("Buttons/startButton"),
            Content.Load<Texture2D>("Buttons/startButton"),
            new Vector2(midScreenWidth, midScreenHeight - 100));
        _optionButton = new Button(
            Content.Load<Texture2D>("Buttons/optionsButton"),
            Content.Load<Texture2D>("Buttons/optionsButton"),
            new Vector2(midScreenWidth, midScreenHeight));
        _quitButton = new Button(
            Content.Load<Texture2D>("Buttons/quitButton"),
            Content.Load<Texture2D>("Buttons/quitButton"),
            new Vector2(midScreenWidth, midScreenHeight + 100));

        _spriteBatch = spriteBatch;
    }

    public void Update()
    {
        _startButton.Update();
        _optionButton.Update();
        _quitButton.Update();

        if (_startButton.isClicked)
        {
            _game.activeScene = Scenes.GAMEPLAY;
        }
        else if (_optionButton.isClicked)
        {
            _game.activeScene = Scenes.OPTIONMENUMAIN;
        }
        else if (_quitButton.isClicked)
        {
            _game.Quit();
        }
    }

    public void Draw()
    {
        _startButton.Draw(_spriteBatch);
        _optionButton.Draw(_spriteBatch);
        _quitButton.Draw(_spriteBatch);
    }
}
