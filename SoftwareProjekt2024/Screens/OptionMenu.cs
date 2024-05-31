using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using System.Diagnostics;

namespace SoftwareProjekt2024.Screens;

internal class OptionMenu
{
    MouseState _mouse;

    int midScreenWidth; 
    int midScreenHeight;

    Button _returnButton; 
    public OptionMenu(ContentManager Content, int screenWidth, int screenHeight, MouseState mouse) { 
        _mouse = mouse;

        midScreenWidth = screenWidth / 2;
        midScreenHeight = screenHeight / 2;

        _returnButton = new Button(Content.Load<Texture2D>("Buttons/returnButton"), screenWidth, screenHeight, new Vector2(midScreenWidth, midScreenHeight), _mouse);
    }

    public void Update(Game1 game)
    {
        _returnButton.Update(_mouse);

        if (_returnButton.isClicked)
        {
            //game.activeScene = Scenes.PAUSEMENU; 
            Debug.WriteLine("return pressed");
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _returnButton.Draw(spriteBatch);
    }
}
