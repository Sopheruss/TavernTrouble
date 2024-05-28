using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Screens;

internal class GamePlay
{
    Button _pauseButton;
    PerspectiveManager _perspectiveManager;
    AnimationManager _animationManager;

    //it is possible to initialize a List of Sprites!!!
    Player ogerCook;

    MouseState _mouse;

    int _screenWidth;
    int _screenHeight;

    public GamePlay(int screenWidth, int screenHeight, MouseState mouse, PerspectiveManager perspectiveManager) { 
        _mouse = mouse;
        _perspectiveManager = perspectiveManager;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public void LoadContent(ContentManager Content)
    {
        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image 
        _animationManager = new(4, 4, new Vector2(19, 32));

        //local implementation, cuz acces to texture via Sprite class 
        Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");
        ogerCook = new Player(_ogerCookSpritesheet,
                              new Vector2(_screenWidth/2, _screenHeight/2),
                              _perspectiveManager); //oger Position 

        Texture2D _pauseButtonTexture = Content.Load<Texture2D>("Buttons/pauseButton");
        _pauseButton = new Button(_pauseButtonTexture, _screenWidth, _screenHeight, new Vector2(20, 20), _mouse);
    }

    public void Update(Game1 game)
    {
        _pauseButton.Update(_mouse);

        if (_pauseButton.isClicked)
        {
            game.activeScene = Scenes.PAUSEMENU;
        }

        ogerCook.Update();
        _animationManager.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _pauseButton.Draw(spriteBatch);

        spriteBatch.Draw(ogerCook.texture,               //texture 
                         ogerCook.Rect,                  //destinationRectangle
                         _animationManager.GetFrame(),   //sourceRectangle (frame) 
                         Color.White,                    //color
                         0f,                             //rotation 
                         new Vector2(ogerCook.texture.Width / 4, //origin -> to place center texture correctly
                                     ogerCook.texture.Width / 4),
                         SpriteEffects.None,             //effects
                         0f);                            //layer depth
    }
}
