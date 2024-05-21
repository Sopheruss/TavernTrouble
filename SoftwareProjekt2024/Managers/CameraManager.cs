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
            _camera.LookAt(_cameraPosition);
        }

        public Matrix GetViewMatrix()
        {
            return _camera.GetViewMatrix();
        }
    }
}
