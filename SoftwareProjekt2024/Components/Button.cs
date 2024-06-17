using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoftwareProjekt2024.Components
{
    internal class Button
    {
        private MouseState _currentMouse;
        private MouseState _previousMouse;

        readonly Texture2D _texture;
        readonly Vector2 _position;
        readonly Rectangle _rectangle;

        public Color buttonColor;

        public bool isClicked;

        public bool isHovering;

        public Button(Texture2D texture, Vector2 position)
        {
            buttonColor = Color.White;

            _texture = texture;
            _position = position;
            _rectangle = new Rectangle((int)_position.X - (_texture.Width / 2), (int)_position.Y - (_texture.Height / 2), _texture.Width, _texture.Height);
        }

        public void Update()
        {
            isClicked = false;
            isHovering = false;

            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRect = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRect.Intersects(_rectangle))
            {
                isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Pressed && _previousMouse.LeftButton == ButtonState.Released)
                {
                    isClicked = true;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isHovering)
            {
                spriteBatch.Draw(_texture, _rectangle, Color.Gray);
            }
            else
            {
                spriteBatch.Draw(_texture, _rectangle, Color.White);
            }
        }
    }
}
