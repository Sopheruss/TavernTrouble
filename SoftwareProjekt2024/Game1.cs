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

    //it is possible to initialize a List of Sprites!!!
    Player ogerCook;

    int screenWidth = 720;
    int screenHeight = 480;

    int midScreenWidth;
    int midScreenHeight;

    AnimationManager _animationManager;
    TileManager _tileManager;
    CameraManager _cameraManager;
    CollisionManager _collisionManager;

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
        midScreenWidth = _graphics.PreferredBackBufferWidth / 2 + 175; // higer val => right
        midScreenHeight = _graphics.PreferredBackBufferHeight / 2 + 100; // lower val => up


        _cameraManager = new CameraManager(Window, GraphicsDevice, screenWidth, screenHeight);



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
                              _animationManager); //oger Position 

        _tileManager = new TileManager(Content, GraphicsDevice);

        _collisionManager = new CollisionManager(_tileManager._tiledMap);
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();


        //Helper bzgl Position and Bounds: Ausgabe -> Debuggen
        Debug.WriteLine($"Player Position: {ogerCook.position}");
        Debug.WriteLine($"Map Bounds: {_collisionManager.MapBounds}");


        if (_collisionManager.IsPositionWithinBounds(ogerCook.position))
        {
            Debug.WriteLine("Player is within bounds.");

        }
        else
        {
            Debug.WriteLine("Player is out of bounds.");

        }


        _animationManager.Update();
        _tileManager.Update(gameTime);
        _cameraManager.Update(gameTime);
        ogerCook.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(Color.Black);

        // Sharp images while scaling
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _tileManager.Draw(_cameraManager.GetViewMatrix());

        _spriteBatch.Draw(
            ogerCook.texture,                                //texture 
            ogerCook.Rect,                                  //destinationRectangle
            _animationManager.GetFrame(),                   //sourceRectangle (frame) 
            Color.White,                                   //color
            0f,                                           //rotation 
            new Vector2(                                 //origin -> to place center texture correctly
                ogerCook.texture.Width / 4,
                ogerCook.texture.Width / 4),
            SpriteEffects.None,                        //effects
            1f);                                      //layer depth

        _spriteBatch.End();

        base.Draw(gameTime);

    }
}
