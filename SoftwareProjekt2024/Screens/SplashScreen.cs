//Sophie träumt von einem Splashscreen 

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;

namespace SoftwareProjekt2024.Screens;

internal class SplashScreen
{
    readonly Game1 _game;

    readonly int _screenWidth;
    readonly int _screenHeight;

    readonly SpriteBatch _spriteBatch;

    BitmapFont bmfont;
    public SplashScreen(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
        _spriteBatch = spriteBatch;

        bmfont = Content.Load<BitmapFont>("Fonts/font_new"); // load font from content-manager using monogame.ext importer/exporter
    }
    public void Update()
    {
        if (Keyboard.GetState().IsKeyUp(Keys.Space))
        {
            _game.activeScene = Scenes.MAINMENU;
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin();

        _spriteBatch.DrawString(bmfont, "Press space to Start", new Vector2(_screenWidth / 2, _screenHeight - 200), Color.White);

        _spriteBatch.End();
    }
}


