using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoftwareProjekt2024
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D _ogerCookSpritesheet;

        int counter;
        int activeFrame;
        int numFrames; 

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet_lowRes");

            activeFrame = 0;
            numFrames = 4;
            counter = 0;

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            counter++;
            if(counter > 29) //animation changes every 30 frames 
            {
                counter = 0; //counter reset 
                activeFrame++; //active frame to next Frame 

                if (activeFrame == numFrames) //reset active frame
                {
                    activeFrame = 0;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(
                _ogerCookSpritesheet,
                new Rectangle(100, 100, 100, 200),
                new Rectangle(activeFrame * 19, 0, 19, 32), //frame rectangle -> must adjust 
                Color.White);
            

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
