using System.Diagnostics;
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
using System.Collections.Generic;
using System;

namespace SoftwareProjekt2024;

public class Game1 : Game


{   /*
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
      */

        public class TestTexture : IComparable<TestTexture>
        {
            public Texture2D texture;
            public Vector2 position;

            public void draw(SpriteBatch _spriteBatch)
            {
                _spriteBatch.Draw(texture,
                position,
                null,
                Color.White,
                0f,
                new Vector2(texture.Width / 2, texture.Height / 2),
                0.2f,
                SpriteEffects.None,
                0f);
            }

            public int CompareTo(TestTexture other)
            {
                if (this.position.Y < other.position.Y) return -1;
                if (this.position.Y == other.position.Y) return 0;
                return 1;
            }
        }

        SortedSet<TestTexture> sortedTextures;
        TestTexture texture1 = new TestTexture();
        TestTexture texture2 = new TestTexture();
        TestTexture texture3 = new TestTexture();
        

        float ballSpeed;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        TiledMap _tiledMap;
        TiledMapRenderer _tiledMapRenderer;
        private OrthographicCamera _camera;
        private Vector2 _cameraPosition;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);

            this._graphics.PreferredBackBufferWidth = 1920;
            this._graphics.PreferredBackBufferHeight = 1080;

            //this._graphics.IsFullScreen = true;

            Content.RootDirectory = "Content";

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //starting position centered based on screen dimensions 

            texture1.position = new Vector2(800, 540);
            texture2.position = new Vector2(900, 500);
            texture3.position = new Vector2(1000, 600);

            ballSpeed = 300f;
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
            _camera = new OrthographicCamera(viewportadapter);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _tiledMap = Content.Load<TiledMap>("samplemap");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            texture1.texture = Content.Load<Texture2D>("Oger_Koch");
            texture2.texture = Content.Load<Texture2D>("Oger_Koch");
            texture3.texture = Content.Load<Texture2D>("Oger_Koch");
        }


        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.Down))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Up))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.Left))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.Right))
            {
                movementDirection += Vector2.UnitX;
            }

            // Can't normalize the zero vector so test for it before normalizing
            if (movementDirection != Vector2.Zero)
            {
                movementDirection.Normalize();
            }

            return movementDirection;
        }

        private void MoveCamera(GameTime gameTime)
        {
            var speed = 200;
            var seconds = gameTime.GetElapsedSeconds();
            var movementDirection = GetMovementDirection();
            texture1.position += speed * movementDirection * seconds;
        }


        protected override void Update(GameTime gameTime)
        {
            sortedTextures = new SortedSet<TestTexture>();
            sortedTextures.Add(texture1);
            sortedTextures.Add(texture2);
            sortedTextures.Add(texture3);
            _tiledMapRenderer.Update(gameTime);

            MoveCamera(gameTime);
            _camera.LookAt(_cameraPosition);

            /*
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _tiledMapRenderer.Update(gameTime);

            //line-by-line analysis of code 
            var kstate = Keyboard.GetState();

            //fetches current keyvoard state, stores variables in kstate 
            if (kstate.IsKeyDown(Keys.Up))
            {
                //checks if up buton is pressed; *gameTime... moving regardesless of framerate
                ballPosition.Y -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                ballPosition.Y += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                ballPosition.X -= ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                ballPosition.X += ballSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            //setting bounds to screen (ball cant go off screen) 

            if (ballPosition.X > _graphics.PreferredBackBufferWidth - cookTexture.Width / 2)
            {
                ballPosition.X = _graphics.PreferredBackBufferWidth - cookTexture.Width / 2;
            }
            else if (ballPosition.X < cookTexture.Width / 2)
            {
                ballPosition.X = cookTexture.Width / 2;
            }

            if (ballPosition.Y > _graphics.PreferredBackBufferHeight - cookTexture.Width / 2)
            {
                ballPosition.Y = _graphics.PreferredBackBufferHeight - cookTexture.Width / 2;
            }
            else if (ballPosition.Y < cookTexture.Height / 2)
            {
                ballPosition.Y = cookTexture.Height / 2;
            }
            */

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);
            _tiledMapRenderer.Draw(_camera.GetViewMatrix());

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //centers correctly (calculating image into center)
            
            foreach(TestTexture texture in sortedTextures)
            {
                texture.draw(_spriteBatch);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

