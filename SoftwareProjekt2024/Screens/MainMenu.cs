using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

public class MainMenu
{
    readonly Button _startButton;
    readonly Button _optionButton;
    readonly Button _quitButton;

    readonly int midScreenWidth;
    readonly int midScreenHeight;

    readonly MouseState _mouse; 

    public MainMenu(ContentManager Content, int screenWidth, int screenHeight, MouseState mouse)
    {
        _mouse = mouse;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2; 

        _startButton = new Button(Content.Load<Texture2D>("Buttons/startButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight - 100), _mouse);
        //not texture yet for option Button, hier grade retry als ersatz
        _optionButton = new Button(Content.Load<Texture2D>("Buttons/retryButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight), _mouse);
        _quitButton = new Button(Content.Load<Texture2D>("Buttons/quitButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight + 100), _mouse);
    }

    public void Update(Game1 game)
    {
        _startButton.Update(_mouse);
        _optionButton.Update(_mouse);
        _quitButton.Update(_mouse);

        if (_startButton.isClicked)
        {
            game.activeScene = Scenes.GAMEPLAY;
        } else if (_optionButton.isClicked)
        {
            game.activeScene = Scenes.OPTIONMENU;
        } else if (_quitButton.isClicked)
        {
            game.Quit();
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _startButton.Draw(spriteBatch);
        _optionButton.Draw(spriteBatch);
        _quitButton.Draw(spriteBatch);
    }
}
