using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;

namespace SoftwareProjekt2024
{
    public class CameraManager
    {
        private OrthographicCamera _camera;
        private Vector2 _cameraPosition;

        public CameraManager(GameWindow window, GraphicsDevice graphicsDevice, int virtualWidth, int virtualHeight)
        {
            var viewportAdapter = new BoxingViewportAdapter(window, graphicsDevice, virtualWidth, virtualHeight);
            _camera = new OrthographicCamera(viewportAdapter);
        }

        public void Update(GameTime gameTime)
        {
            MoveCamera(gameTime);
            _camera.LookAt(_cameraPosition);
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
            var seconds = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var movementDirection = GetMovementDirection();
            _cameraPosition += speed * movementDirection * seconds;
        }

        public Matrix GetViewMatrix()
        {
            return _camera.GetViewMatrix();
        }
    }
}
