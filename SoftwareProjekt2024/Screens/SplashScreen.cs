using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoftwareProjekt2024.Screens;

public class SplashScreen
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int _screenWidth;
    readonly int _screenHeight;
    readonly int midScreenWidth;
    readonly int midScreenHeight;

    readonly Texture2D _background;
    readonly Rectangle _backgroundRect;

    public SplashScreen(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        _screenHeight = screenHeight;
        _screenWidth = screenWidth;
        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _background = Content.Load<Texture2D>("Background/SplashScreen");
        _backgroundRect = new Rectangle(0, 0, screenWidth, screenHeight);
    }

    public void Update()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _game.activeScene = Scenes.MAINMENU;
        }
        else if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            _game.Quit();
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _spriteBatch.Draw(_background, _backgroundRect, Color.White);

        _spriteBatch.End();
    }
}