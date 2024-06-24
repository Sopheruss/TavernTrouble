using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Screens;

public class PauseMenu
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int midScreenWidth;
    readonly int midScreenHeight;

    readonly Button _mainMenuButton;
    readonly Button _retryButton;
    readonly Button _quitButton;
    readonly Button _optionButton;
    readonly Button _returnButton;

    readonly InputManager _inputManager;

    public PauseMenu(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _mainMenuButton = new Button(
            Content.Load<Texture2D>("Buttons/menuButton"),
            Content.Load<Texture2D>("Buttons/menuButtonHovering"),
            new Vector2(midScreenWidth, midScreenHeight - 150));
        _retryButton = new Button(
            Content.Load<Texture2D>("Buttons/retryButton"),
            Content.Load<Texture2D>("Buttons/retryButtonHovering"),
            new Vector2(midScreenWidth, midScreenHeight - 50));
        _optionButton = new Button(
            Content.Load<Texture2D>("Buttons/settingsButton"),
            Content.Load<Texture2D>("Buttons/settingsButtonHovering"),
            new Vector2(midScreenWidth, midScreenHeight + 50));
        _quitButton = new Button(
            Content.Load<Texture2D>("Buttons/quitButton"),
            Content.Load<Texture2D>("Buttons/quitButtonHovering"),
            new Vector2(midScreenWidth, midScreenHeight + 150));
        _returnButton = new Button(
            Content.Load<Texture2D>("Buttons/returnButton"),
            Content.Load<Texture2D>("Buttons/returnButtonHovering"),
            new Vector2(screenWidth - 70, screenHeight - 70));
    }

    public void Update()
    {
        _mainMenuButton.Update();
        _retryButton.Update();
        _quitButton.Update();
        _optionButton.Update();
        _returnButton.Update();

        if (_mainMenuButton.isClicked)
        {
            _game.activeScene = Scenes.MAINMENU;
        }
        else if (_retryButton.isClicked)
        {
            //not right -> must start gamplay from beginning 
            _game.activeScene = Scenes.GAMEPLAY;
        }
        else if (_optionButton.isClicked)
        {
            _game.activeScene = Scenes.OPTIONMENUPAUSE;
        }
        else if (_returnButton.isClicked || _game._escIsPressed)
        {
            _game.activeScene = Scenes.GAMEPLAY;
        }
        else if (_quitButton.isClicked)
        {
            _game.Quit();
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _mainMenuButton.Draw(_spriteBatch);
        _retryButton.Draw(_spriteBatch);
        _quitButton.Draw(_spriteBatch);
        _optionButton.Draw(_spriteBatch);
        _returnButton.Draw(_spriteBatch);

        _spriteBatch.End();
    }
}
