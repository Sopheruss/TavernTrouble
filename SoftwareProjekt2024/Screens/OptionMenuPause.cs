using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

public class OptionMenuPause
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly MouseState _mouse;

    readonly int midScreenWidth;
    readonly int midScreenHeight;

    readonly Button _returnButton;

    public OptionMenuPause(ContentManager Content, int screenWidth, int screenHeight, MouseState mouse, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        _mouse = mouse;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _returnButton = new Button(Content.Load<Texture2D>("Buttons/returnButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight), _mouse);
    }

    public void Update()
    {
        _returnButton.Update(_mouse);

        if (_returnButton.isClicked)
        {
            _game.activeScene = Scenes.PAUSEMENU;
        }
    }

    public void Draw()
    {
        _returnButton.Draw(_spriteBatch);
    }
}
