using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoftwareProjekt2024
{
    public class Game1 : Game
    {
        Texture2D cookTexture;
        Vector2 ballPosition;
        float ballSpeed;

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

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
            ballPosition = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            ballSpeed = 300f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

           
            cookTexture = Content.Load<Texture2D>("Oger_Koch");
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

           
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

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            _spriteBatch.Begin();
            //centers correctly (calculating image into center)
            _spriteBatch.Draw(cookTexture,
                ballPosition,
                null,
                Color.White,
                0f,
                new Vector2(cookTexture.Width / 2, cookTexture.Height / 2),
                0.3f,
                SpriteEffects.None,
                0f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
