using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components
{
    internal class Button
    {
        private MouseState _currentMouse;

        Texture2D _texture;
        Vector2 _position;
        Rectangle _rectangle;

        public bool isClicked;

        public Button(Texture2D texture, int ScreenWidth, int ScreenHeight, Vector2 position, MouseState mouse)
        {
            _texture = texture;
            _position = position;
            _rectangle = new Rectangle((int)_position.X - (_texture.Width/2), (int)_position.Y - (_texture.Height/2), _texture.Width, _texture.Height);
        }


        public void Update(MouseState mouse)
        {
            _currentMouse = Mouse.GetState();

            var mouseRect = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRect.Intersects(_rectangle))
            {
                if (_currentMouse.LeftButton == ButtonState.Pressed)
                {
                    isClicked = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_texture, _rectangle, Color.White);
        }
    }
}
