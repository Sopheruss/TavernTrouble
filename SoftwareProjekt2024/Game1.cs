using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024;

public class Game1 : Game

{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Player ogerCook;
    Player ogerCook2;
    Player _ogerCook;

    int screenWidth = 1080;
    int screenHeight = 720;

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
    }

    protected override void Initialize()
    {

        //calc for middle of screen + hack to spawn into middle of first iteration of map (TEMPORARY)
        midScreenWidth = _graphics.PreferredBackBufferWidth / 2 + 150; // higer val => right
        midScreenHeight = _graphics.PreferredBackBufferHeight / 2 + 100; // lower val => up


        _cameraManager = new CameraManager(Window, GraphicsDevice, screenWidth, screenHeight);
        _perspectiveManager = new PerspectiveManager();



        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image 
        _animationManager = new(4, 4, new Vector2(19, 32));

        //local implementation, cuz acces to texture via Sprite class 
        Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");

        ogerCook = new Player(_ogerCookSpritesheet,
                              new Vector2(midScreenWidth, midScreenHeight),
                              _animationManager,
                              _perspectiveManager); //oger Position 

        ogerCook2 = new Player(_ogerCookSpritesheet,
                              new Vector2(midScreenWidth-175, midScreenHeight-100),
                              _animationManager,
                              _perspectiveManager); //oger Position

        _ogerCook = new Player(_ogerCookSpritesheet,
                              new Vector2(midScreenWidth, midScreenHeight)); //oger Position 


        _tileManager = new TileManager(Content, GraphicsDevice);

        //Debug.WriteLine("Pos Oger X: " + ogerCook.position.X);
        //Debug.WriteLine("Pos Oger Y: " + ogerCook.position.Y);
        
        _collisionManager = new CollisionManager(_tileManager._tiledMap, "collisionlayer", _ogerCook.position.X, _ogerCook.position.Y);
        _inputManager = new InputManager(this, _ogerCook, _collisionManager, _animationManager);
    }

    protected override void Update(GameTime gameTime)
    {

        _animationManager.Update();
        _tileManager.Update(gameTime);
        _cameraManager.Update(gameTime, ogerCook.position);
        ogerCook.Update();
        _ogerCook.Update();
        _inputManager.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(Color.Black);

        // Sharp images while scaling
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tileManager.Draw(_cameraManager.GetViewMatrix());

        _perspectiveManager.draw(_spriteBatch, _animationManager);
        _spriteBatch.End();

        base.Draw(gameTime);

    }
}

