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
    readonly Button _creditsButton;

    readonly int midScreenWidth;
    readonly int midScreenHeight;

    readonly Texture2D _background;
    readonly Rectangle _backgroundRect;

    readonly Texture2D _backgroundBord;
    readonly Rectangle _backgroundBordRect;

    public MainMenu(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _startButton = new Button(
            Content.Load<Texture2D>("Buttons/playButton"),
            Content.Load<Texture2D>("Buttons/playButtonHovering"),
            new Vector2(screenWidth / 2, midScreenHeight - 100));
        _optionButton = new Button(
            Content.Load<Texture2D>("Buttons/settingsButton"),
            Content.Load<Texture2D>("Buttons/settingsButtonHovering"),
            new Vector2(screenWidth / 2, midScreenHeight));
        _quitButton = new Button(
            Content.Load<Texture2D>("Buttons/quitButton"),
            Content.Load<Texture2D>("Buttons/quitButtonHovering"),
            new Vector2(screenWidth / 2, midScreenHeight + 100));
        _creditsButton = new Button(
            Content.Load<Texture2D>("Buttons/creditsButton"),
            Content.Load<Texture2D>("Buttons/creditsButtonHovering"),
            new Vector2(screenWidth - 70, screenHeight - 70));

        _background = Content.Load<Texture2D>("Background/background");
        _backgroundRect = new Rectangle(0, 0, screenWidth, screenHeight);

        _backgroundBord = Content.Load<Texture2D>("Background/backgroundBord");
        _backgroundBordRect = new Rectangle(midScreenWidth - (_backgroundBord.Width / 2),
                                            midScreenHeight - (_backgroundBord.Height / 2) + 5,
                                            _backgroundBord.Width,
                                            _backgroundBord.Height);

        _spriteBatch = spriteBatch;
    }

    public void Update()
    {
        _startButton.Update();
        _optionButton.Update();
        _quitButton.Update();
        _creditsButton.Update();

        if (_startButton.isClicked)
        {
            Game1.activeScene = Scenes.GAMEPLAY;
        }
        else if (_optionButton.isClicked)
        {
            Game1.activeScene = Scenes.OPTIONMENUMAIN;
        }
        else if (_quitButton.isClicked || _startButton._escIsPressed)
        {
            _game.Quit();
        }
        else if (_creditsButton.isClicked)
        {
            _game.activeScene = Scenes.CREDITSSCREEN;
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _spriteBatch.Draw(_background, _backgroundRect, Color.White);
        _spriteBatch.Draw(_backgroundBord, _backgroundBordRect, Color.White);
        _startButton.Draw(_spriteBatch);
        _optionButton.Draw(_spriteBatch);
        _quitButton.Draw(_spriteBatch);
        _creditsButton.Draw(_spriteBatch);

        _spriteBatch.End();
    }
}
