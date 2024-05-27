using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

internal class GamePlay
{
    readonly Button _pauseButton;

    MouseState _mouse;
    public GamePlay(ContentManager Content, int screenWidth, int screenHeight, MouseState mouse) { 
        _mouse = mouse;

        //no texture yet -> retry button as place holder
        _pauseButton = new Button(Content.Load<Texture2D>("Buttons/pauseButton"), screenWidth, screenHeight, new Vector2(10, screenHeight - 20), _mouse);
    }

    public void Update(Game1 game)
    {
        _pauseButton.Update(_mouse);

        if(_pauseButton.isClicked || Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            game.activeScene = Scenes.PAUSEMENU;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _pauseButton.Equals(spriteBatch);
    }
}
