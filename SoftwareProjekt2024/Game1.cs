using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
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

    int screenWidth = 1080;
    int screenHeight = 720;

    public Scenes activeScene;  

    private MainMenu _mainMenu;
    private GamePlay _gamePlay;
    private PauseMenu _pauseMenu;
    private OptionMenu _optionMenu;

    int midScreenWidth;
    int midScreenHeight;

    AnimationManager _animationManager;
    TileManager _tileManager;

    CameraManager _cameraManager;
    CollisionManager _collisionManager;
    PerspectiveManager _perspectiveManager;
    InputManager _inputManager;



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


        //calc for middle of screen 

        midScreenWidth = _graphics.PreferredBackBufferWidth / 2; // higer val => right
        midScreenHeight = _graphics.PreferredBackBufferHeight / 2; // lower val => up


        _cameraManager = new CameraManager(Window, GraphicsDevice, screenWidth, screenHeight);
        _perspectiveManager = new PerspectiveManager();



        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);


        _mainMenu = new MainMenu(Content, screenWidth, screenHeight, Mouse.GetState());
        _gamePlay = new GamePlay(screenWidth, screenHeight, Mouse.GetState());
        _pauseMenu = new PauseMenu(Content, screenWidth, screenHeight, Mouse.GetState());
        _optionMenu = new OptionMenu(Content, screenWidth, screenHeight, Mouse.GetState());

        _gamePlay.LoadContent(Content, this, Window, GraphicsDevice);
        

        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image 
        _animationManager = new(4, 4, new Vector2(19, 32));


        //local implementation, cuz acces to texture via Sprite class 
        Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");

        _ogerCook = new Player(_ogerCookSpritesheet, new Vector2(midScreenWidth, midScreenHeight), _perspectiveManager); //oger Position 
        testDummy = new Player(_ogerCookSpritesheet, new Vector2(midScreenWidth, midScreenHeight), _perspectiveManager);

        _inputManager = new InputManager(this, _ogerCook, _collisionManager, _animationManager);

        _tileManager = new TileManager();
        _tileManager.textureAtlas = Content.Load<Texture2D>("atlas");
        _tileManager.hitboxes = Content.Load<Texture2D>("hitboxes");


    }

    protected override void Update(GameTime gameTime)
    {

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Quit();

        if (_exit)
        {
            Exit();
        }

        switch (activeScene)
        {
            case Scenes.MAINMENU:
                _mainMenu.Update(this);
                break;
            case Scenes.GAMEPLAY:
                _gamePlay.Update(this, gameTime);
                break;
            case Scenes.PAUSEMENU:
                _pauseMenu.Update(this);
                break;
            case Scenes.OPTIONMENU:
                _optionMenu.Update(this);
                break;
        }


        _animationManager.Update();
        _cameraManager.Update(gameTime, _ogerCook.position);
        _ogerCook.Update();
        _inputManager.Update();



        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _tileManager.Draw(_spriteBatch, 32, 8, 32);

        _perspectiveManager.draw(_spriteBatch, _animationManager);
        
        switch (activeScene)
        {
            case Scenes.MAINMENU:

                GraphicsDevice.Clear(Color.LightBlue);
                _mainMenu.Draw(_spriteBatch);

                break;
            case Scenes.GAMEPLAY:
                GraphicsDevice.Clear(Color.Beige);

                _gamePlay.Draw(_spriteBatch);

                break;
            case Scenes.PAUSEMENU:
                GraphicsDevice.Clear(Color.LightPink);


                _pauseMenu.Draw(_spriteBatch);

                break;
            case Scenes.OPTIONMENU:
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

