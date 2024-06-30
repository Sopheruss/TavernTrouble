using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;

namespace SoftwareProjekt2024.Screens;

public class SplashScreen
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int _screenWidth;
    readonly int _screenHeight;
    readonly int midScreenWidth;
    readonly int midScreenHeight;

    BitmapFont bmfont;

    public SplashScreen(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        _screenHeight = screenHeight;
        _screenWidth = screenWidth;
        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        bmfont = Content.Load<BitmapFont>("Fonts/font_new"); // load font from content-manager using monogame.ext importer/exporter
    }

    public void Update()
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            _game.activeScene = Scenes.MAINMENU;
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _spriteBatch.DrawString(bmfont, "press  space", new Vector2(midScreenWidth - 100, midScreenHeight + 200), Color.Black);

        _spriteBatch.End();
    }
}