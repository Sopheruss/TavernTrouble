using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

internal class OptionMenuMain
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int midScreenWidth;
    readonly int midScreenHeight;

    readonly Button _returnButton;

    public OptionMenuMain(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _returnButton = new Button(Content.Load<Texture2D>("Buttons/returnButton"), new Vector2(midScreenWidth, midScreenHeight));
        _spriteBatch = spriteBatch;
    }

    public void Update()
    {
        _returnButton.Update();


        if (_returnButton.isClicked || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            _game.activeScene = Scenes.MAINMENU;
        }
    }

    public void Draw()
    {
        _returnButton.Draw(_spriteBatch);
    }
}
