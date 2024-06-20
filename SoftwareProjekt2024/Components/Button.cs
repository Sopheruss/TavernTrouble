using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SoftwareProjekt2024.Components
{
    internal class Button
    {
        private MouseState _currentMouse;
        private MouseState _previousMouse;

        readonly Texture2D _textureNotHovering;
        readonly Texture2D _textureHovering;
        readonly Vector2 _position;
        readonly Rectangle _rectangle;

        public Color buttonColor;

        public bool isClicked;

        public bool isHovering;

        public Button(Texture2D textureNotHovering, Texture2D textureHovering, Vector2 position)
        {
            buttonColor = Color.White;

            _textureNotHovering = textureNotHovering;
            _textureHovering = textureHovering;
            _position = position;
            _rectangle = new Rectangle((int)_position.X - (_textureNotHovering.Width / 2), (int)_position.Y - (_textureNotHovering.Height / 2), _textureNotHovering.Width, _textureNotHovering.Height);
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
                spriteBatch.Draw(_textureHovering, _rectangle, Color.White);
            }
            else
            {
                spriteBatch.Draw(_textureNotHovering, _rectangle, Color.White);
            }
        }
    }
}
