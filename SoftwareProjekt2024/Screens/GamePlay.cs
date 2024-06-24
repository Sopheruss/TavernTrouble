using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
using System.Diagnostics;

namespace SoftwareProjekt2024.Screens
{
    internal class GamePlay
    {
        readonly SpriteBatch _spriteBatch;

        // Camera stuff; using Monogame Extended Camera 
        private OrthographicCamera _camera;

        Button _pauseButton;

        PerspectiveManager _perspectiveManager;
        AnimationManager _animationManager;
        TileManager _tileManager;
        CollisionManager _collisionManager;
        InteractionManager _interactionManager;
        InputManager _inputManager;

        BitmapFont bmfont;
        private int score;

        Player _ogerCook;

        readonly int _screenWidth;
        readonly int _screenHeight;

        Texture2D rectangleTexture;

        // Stopwatch for tracking elapsed time
        private Stopwatch _timer;

        public GamePlay(int screenWidth, int screenHeight, SpriteBatch spriteBatch)
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;

            _spriteBatch = spriteBatch;

            // Initialize the stopwatch
            _timer = new Stopwatch();
        }

        public void LoadContent(ContentManager Content, Game1 game, GameWindow window, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            var viewportAdapter = new BoxingViewportAdapter(window, graphicsDevice, 426, 240); // Sets the size of the viewport window 
            _camera = new OrthographicCamera(viewportAdapter);

            _perspectiveManager = new PerspectiveManager();

            // Constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image
            _animationManager = new(4, 4, new Vector2(19, 32));

            // Local implementation, because access to texture via Sprite class
            Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");

            _ogerCook = new Player(_ogerCookSpritesheet,
                                  new Vector2(250, 200), // Change if necessary for map size 
                                  _perspectiveManager);

            _pauseButton = new Button(
                Content.Load<Texture2D>("Buttons/pauseButton"),
                Content.Load<Texture2D>("Buttons/pauseButtonHovering"),
                new Vector2(30, 30));

            _tileManager = new TileManager();
            _tileManager.textureAtlas = Content.Load<Texture2D>("atlas");
            _tileManager.hitboxes = Content.Load<Texture2D>("hitboxes");
            _tileManager.LoadObjectlayer(spriteBatch, 32, 8, 32, _perspectiveManager); // Load all objects from Tiled

            _collisionManager = new CollisionManager(_tileManager);
            _interactionManager = new InteractionManager(_tileManager);
            _inputManager = new InputManager(game, _ogerCook, _collisionManager, _interactionManager, _animationManager);

            rectangleTexture = new Texture2D(graphicsDevice, 1, 1);         // For player rectangle
            rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });  // ''

            bmfont = Content.Load<BitmapFont>("Fonts/font_new");

            // Start the stopwatch
            _timer.Start();
        }

        public void Update(Game1 game, GameTime gameTime)
        {
            _camera.LookAt(_ogerCook.position + new Vector2(10, 16)); // Offset to center ogre -> half of the texture width/height

            _pauseButton.Update();

            if (_pauseButton.isClicked || _inputManager._escIsPressed)
            {
                game.activeScene = Scenes.PAUSEMENU;
                _timer.Stop(); // Stop the stopwatch when paused
            }
            else
            {
                _timer.Start(); // Resume the stopwatch if not paused
            }

            _ogerCook.Update();
            _animationManager.Update();
            _inputManager.Update();
            score++;
        }

        public void Draw()
        {
            // Two spriteBatch.Begin/End to separate stuff that is affected by camera and static stuff

            // TransformationMatrix is automatically calculated into the draw call 
            var transformMatrix = _camera.GetViewMatrix();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix); // To make sharp images while scaling 

            _tileManager.Draw(_spriteBatch, 32, 8, 32, _perspectiveManager);

            _perspectiveManager.draw(_spriteBatch, _animationManager);

            _collisionManager.DrawDebugRect(_spriteBatch, _ogerCook.Rect, 1, rectangleTexture); // Drawing player rectangle, int value is thickness

            _spriteBatch.End();

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

            _spriteBatch.DrawString(bmfont, "Placeholder: " + score, new Vector2(50, 50), Color.White);
            _pauseButton.Draw(_spriteBatch);

            // Display the elapsed time
            string elapsedTime = _timer.Elapsed.ToString(@"mm\:ss");
            _spriteBatch.DrawString(bmfont, "Time: " + elapsedTime, new Vector2(50, 70), Color.LightGreen);

            _spriteBatch.End();
        }
    }
}
