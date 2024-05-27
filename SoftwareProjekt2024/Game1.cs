using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.SpriteClasses;
using System.Diagnostics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using SoftwareProjekt2024.Screens;

namespace SoftwareProjekt2024;

public enum Scenes
{ 
    MAINMENU,
    GAMEPLAY,
    PAUSEMENU, 
    OPTIONMENU
};

public class Game1 : Game

{
    public bool _exit = false;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    //it is possible to initialize a List of Sprites!!!
    Player ogerCook; 

    int screenWidth = 720;
    int screenHeight = 480;

    int midScreenWidth;
    int midScreenHeight;

    AnimationManager _animationManager;

    public Scenes activeScene; 

    private MainMenu _mainMenu;
    private GamePlay _gamePlay;
    private PauseMenu _pauseMenu;
    private OptionMenu _optionMenu;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        this._graphics.PreferredBackBufferWidth = screenWidth;
        this._graphics.PreferredBackBufferHeight = screenHeight;

        //this._graphics.IsFullScreen = true;

        activeScene = Scenes.MAINMENU;
    }

    protected override void Initialize()
    {
        //to make using calc for middel of Screen shorter 
        midScreenWidth = _graphics.PreferredBackBufferWidth / 2;
        midScreenHeight = _graphics.PreferredBackBufferHeight / 2;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _mainMenu = new MainMenu(Content, screenWidth, screenHeight, Mouse.GetState());
        _gamePlay = new GamePlay(Content, screenWidth, screenHeight, Mouse.GetState());
        _pauseMenu = new PauseMenu(Content, screenWidth, screenHeight, Mouse.GetState());
        _optionMenu = new OptionMenu(Content, screenWidth, screenHeight, Mouse.GetState());

        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image 
        _animationManager = new(4, 4, new Vector2(19, 32));

        //local implementation, cuz acces to texture via Sprite class 
        Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");
        ogerCook = new Player(_ogerCookSpritesheet,
                              new Vector2(midScreenWidth, midScreenHeight), 
                              _animationManager); //oger Position 
    }

    protected override void Update(GameTime gameTime)
    {
        /*if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Quit();*/

        if (_exit)
        {
            Exit();
        }

        switch (activeScene)
        {
            case Scenes.MAINMENU:
                //main menu logic

                _mainMenu.Update(this);

                break;
            case Scenes.GAMEPLAY:
                //game logic 

                ogerCook.Update();
                _animationManager.Update();

                _gamePlay.Update(this);

                break;
            case Scenes.PAUSEMENU:
                //pause logic 

                _pauseMenu.Update(this);
                break;
            case Scenes.OPTIONMENU:
                //option logic 

                _optionMenu.Update(this);

                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        switch (activeScene)
        {
            case Scenes.MAINMENU:
                //main menu logic

                GraphicsDevice.Clear(Color.LightBlue);
                _mainMenu.Draw(_spriteBatch);

                break;
            case Scenes.GAMEPLAY:
                //game logic 
                GraphicsDevice.Clear(Color.Beige);

                _spriteBatch.Draw(ogerCook.texture,               //texture 
                                  ogerCook.Rect,                  //destinationRectangle
                                  _animationManager.GetFrame(),   //sourceRectangle (frame) 
                                  Color.White,                    //color
                                  0f,                             //rotation 
                                  new Vector2(ogerCook.texture.Width / 4, //origin -> to place center texture correctly
                                              ogerCook.texture.Width / 4),
                                  SpriteEffects.None,             //effects
                                  0f);                            //layer depth

                _gamePlay.Draw(_spriteBatch);

                break;
            case Scenes.PAUSEMENU:
                //pause logic 
                GraphicsDevice.Clear(Color.LightPink);

                _pauseMenu.Draw(_spriteBatch);

                break;
            case Scenes.OPTIONMENU:
                //option menu logic
                GraphicsDevice.Clear(Color.LightGreen);

                _optionMenu.Draw(_spriteBatch);

                break;
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);

    }

    public void Quit()
    {
        this.Exit();
    }
}
