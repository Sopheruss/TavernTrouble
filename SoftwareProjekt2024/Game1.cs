using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024;

public class Game1 : Game

{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Player ogerCook;



    Player _ogerCook;
    Player testDummy;

    int screenWidth = 1080;
    int screenHeight = 720;

    AnimationManager _animationManager;
    TileManager _tileManager;

    CameraManager _cameraManager;
    CollisionManager _collisionManager;
    PerspectiveManager _perspectiveManager;
    InputManager _inputManager;



    public Scenes activeScene;  

    private MainMenu _mainMenu;
    private GamePlay _gamePlay;
    private PauseMenu _pauseMenu;
    private OptionMenu _optionMenu;

    int midScreenHeight;
    int midScreenWidth;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        this._graphics.PreferredBackBufferWidth = screenWidth;
        this._graphics.PreferredBackBufferHeight = screenHeight;

        //this._graphics.IsFullScreen = true;
    }

    protected override void Initialize()
    {
        //calc for middle of screen 

        midScreenWidth = _graphics.PreferredBackBufferWidth / 2; // higer val => right
        midScreenHeight = _graphics.PreferredBackBufferHeight / 2; // lower val => up

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

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

        _gamePlay.LoadContent(Content, this, Window, GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        _animationManager.Update();
        _cameraManager.Update(gameTime, _ogerCook.position);
        _ogerCook.Update();
        _inputManager.Update();
        
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
        }

        _spriteBatch.End();

        base.Draw(gameTime);

    }
}

