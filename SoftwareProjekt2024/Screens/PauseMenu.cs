using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

public class PauseMenu
{
    MouseState _mouse;

    int midScreenWidth;
    int midScreenHeight;

    readonly Button _mainMenuButton;
    readonly Button _retryButton;
    readonly Button _quitButton;
    readonly Button _optionButton;
    readonly Button _returnButton;
    
    public PauseMenu(ContentManager Content, int screenWidth, int screenHeight, MouseState mouse) { 
        _mouse = mouse;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _mainMenuButton = new Button(Content.Load<Texture2D>("Buttons/menuButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight - 200), _mouse);
        _retryButton = new Button(Content.Load<Texture2D>("Buttons/retryButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight - 100), _mouse);
        _quitButton = new Button(Content.Load<Texture2D>("Buttons/quitButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight), _mouse);
        _optionButton = new Button(Content.Load<Texture2D>("Buttons/optionsButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight + 100), _mouse);
        _returnButton = new Button(Content.Load<Texture2D>("Buttons/returnButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight + 200), _mouse);
    }

    public void Update(Game1 game)
    {
        _mainMenuButton.Update(_mouse);
        _retryButton.Update(_mouse);
        _quitButton.Update(_mouse);
        _optionButton.Update(_mouse);
        _returnButton.Update(_mouse);

        if (_mainMenuButton.isClicked)
        {
            game.activeScene = Scenes.MAINMENU;
        }
        else if (_retryButton.isClicked)
        {
            //not right -> must start gamplay from beginning 
            game.activeScene = Scenes.GAMEPLAY;
        }
        else if(_optionButton.isClicked)
        {
            game.activeScene = Scenes.OPTIONMENU;
        } else if(_returnButton.isClicked || (Keyboard.GetState().IsKeyDown(Keys.Escape) && Keyboard.GetState().IsKeyUp(Keys.Escape)))
        {
            //works kinda? -> grafik fehler?; also pausiert spiel, aber sehr laggy?
            game.activeScene = Scenes.GAMEPLAY;
        } else if (_quitButton.isClicked)
        {
            game.Quit();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _mainMenuButton.Draw(spriteBatch);
        _retryButton.Draw(spriteBatch);
        _quitButton.Draw(spriteBatch);
        _optionButton.Draw(spriteBatch);
        _returnButton.Draw(spriteBatch);
    }
}
