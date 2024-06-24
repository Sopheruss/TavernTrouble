using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens
{
    internal class OptionMenuMain
    {
        readonly Game1 _game;
        readonly SpriteBatch _spriteBatch;

        readonly int midScreenWidth;
        readonly int midScreenHeight;

        readonly Button _returnButton;

        BitmapFont bmfont;

        public OptionMenuMain(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
        {
            _game = game;
            _spriteBatch = spriteBatch;

            midScreenWidth = screenWidth / 2;
            midScreenHeight = screenHeight / 2;

            _returnButton = new Button(
                Content.Load<Texture2D>("Buttons/returnButton"),
                Content.Load<Texture2D>("Buttons/returnButtonHovering"),
                new Vector2(screenWidth - 70, screenHeight - 70));


            // texture as well as fnt file have to be imported via content-pipline and monogame.extended importer. Beware of filestructure
            bmfont = Content.Load<BitmapFont>("Fonts/font_new");
        }

        public void Update()
        {
            _returnButton.Update();

            if (_returnButton.isClicked || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                _game.activeScene = Scenes.MAINMENU;
            }
        }

        public void Draw()
        {
            _spriteBatch.Begin(samplerState: SamplerState.PointClamp); // to make sharp images while scaling

            _returnButton.Draw(_spriteBatch);

            // Ensure bmfont is not null before drawing
            if (bmfont != null)
            {

                // Add the key bindings display
                string[] keys = { "W", "A", "S", "D", "E", "Esc" };
                string[] descriptions = {
                    "Move Up",
                    "Move Left",
                    "Move Down",
                    "Move Right",
                    "Interact",
                    "Pause/Menu"
                };

                Vector2 position = new Vector2(midScreenWidth - 150, midScreenHeight - 150); // Starting position for the table
                float lineHeight = bmfont.LineHeight + 5; // Line height with a small gap

                for (int i = 0; i < keys.Length; i++)
                {
                    _spriteBatch.DrawString(bmfont, keys[i], position, Color.White);
                    _spriteBatch.DrawString(bmfont, descriptions[i], new Vector2(position.X + 150, position.Y), Color.Black);
                    position.Y += lineHeight; // Move down for the next line
                }
            }

            _spriteBatch.End();
        }
    }
}
