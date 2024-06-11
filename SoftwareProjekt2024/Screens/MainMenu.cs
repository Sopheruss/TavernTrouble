using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

    readonly MouseState _mouse;

    public MainMenu(ContentManager Content, int screenWidth, int screenHeight, MouseState mouse, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        _mouse = mouse;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _startButton = new Button(Content.Load<Texture2D>("Buttons/startButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight - 100), _mouse);
        _optionButton = new Button(Content.Load<Texture2D>("Buttons/optionsButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight), _mouse);
        _quitButton = new Button(Content.Load<Texture2D>("Buttons/quitButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight + 100), _mouse);
        _spriteBatch = spriteBatch;
    }

    public void Update()
    {
        _startButton.Update(_mouse);
        _optionButton.Update(_mouse);
        _quitButton.Update(_mouse);

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
