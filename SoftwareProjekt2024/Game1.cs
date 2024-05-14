using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoftwareProjekt2024
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        int screenWidth = 1920;
        int screenHeight = 1080;

        Texture2D _ogerCookSpritesheet;
        Vector2 _ogerPosition;
        float _ogerSpeed; 

        AnimationManager _animationManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            /*this._graphics.PreferredBackBufferWidth = screenWidth;
            this._graphics.PreferredBackBufferHeight = screenHeight;

            this._graphics.IsFullScreen = true;*/
        }

        protected override void Initialize()
        {
            _ogerPosition = new Vector2(_graphics.PreferredBackBufferWidth/2, _graphics.PreferredBackBufferHeight/2);
            _ogerSpeed = 300f; 
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet_lowRes");

            _animationManager = new(16, 4, new Vector2(19, 32));
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _animationManager.Update();
            

            //Movement in Porgess 

            /*//line-by-line analysis of code 
            var kstate = Keyboard.GetState();

            //fetches current keyvoard state, stores variables in kstate 
            if (kstate.IsKeyDown(Keys.Up))
            {
                //checks if up buton is pressed; *gameTime... moving regardesless of framerate
                _ogerPosition.Y -= _ogerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Down))
            {
                _ogerPosition.Y += _ogerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Left))
            {
                _ogerPosition.X -= _ogerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.Right))
            {
                _ogerPosition.X += _ogerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }*/ 

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(
                _ogerCookSpritesheet, //Texture 
                _ogerPosition, //Position
                _animationManager.GetFrame(), //source rectangle (frame); HARD CODED!!!!
                Color.White, //color
                0f, //rotation 
                new Vector2(19/2,32/2), //HARD CODED!!!,  ist da, dass Textur richtig liegt 
                8f, //scale
                SpriteEffects.None, //effects 
                0f); //layer depth 

            /*_spriteBatch.Draw(
                _ogerCookSpritesheet,
                new Rectangle(100, 100, 100, 200),
                _animationManager.GetFrame(),
                Color.White);*/
            
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
