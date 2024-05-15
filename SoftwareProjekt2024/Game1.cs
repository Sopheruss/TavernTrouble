using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.SpriteClasses;
using System.Diagnostics;

namespace SoftwareProjekt2024
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        //it is possible to initialize a List of Sprites!!!
        ScaledSprite ogerSprite; 

        int screenWidth = 1920;
        int screenHeight = 1080;

        int midScreenWidth;
        int midScreenHeight;

        AnimationManager _animationManager;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            this._graphics.PreferredBackBufferWidth = screenWidth;
            this._graphics.PreferredBackBufferHeight = screenHeight;

            this._graphics.IsFullScreen = true;
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

            //local implementation, cuz accec to texture via Sprite class 
            Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet_lowRes");
            ogerSprite = new ScaledSprite(_ogerCookSpritesheet,
                new Vector2(midScreenWidth, midScreenHeight)); //oger Position 
                //1f); //ogerSpeed    just for MovingSprite

            //constructing new Animation with 16 Frames in 4 Rows 
            _animationManager = new(16, 4, new Vector2(19, 32));
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            ogerSprite.Update();
            _animationManager.Update();
            

            /*//Movement in Porgess 

            //line-by-line analysis of code 
            var kstate = Keyboard.GetState();

            //fetches current keyvoard state, stores variables in kstate 
            if (kstate.IsKeyDown(Keys.A))
            {
                //checks if up buton is pressed; *gameTime... moving regardesless of framerate
                //_ogerPosition.Y -= _ogerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.D))
            {
                //_ogerPosition.Y += _ogerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.W))
            {
                //_ogerPosition.X -= _ogerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (kstate.IsKeyDown(Keys.S))
            {
                //_ogerPosition.X += _ogerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            } */

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

            _spriteBatch.Draw(
                ogerSprite.texture,             //texture 
                ogerSprite.Rect,                //destinationRectangle
                _animationManager.GetFrame(),   //sourceRectangle (frame) 
                Color.White,                    //color
                0f,                             //rotation 
                new Vector2(                    //origin -> to place center texture correctly
                    ogerSprite.texture.Width/4, 
                    ogerSprite.texture.Width/4),         
                SpriteEffects.None,             //effects
                0f);                            //layer depth
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
