using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;

namespace SoftwareProjekt2024
{
    public class CollisionManager
    {
         TiledMap _tiledMap;

        public Rectangle MapBounds { get; private set; }

        public CollisionManager(TiledMap tiledMap)
        {
            _tiledMap = tiledMap;
            Initialize();
        }

        private void Initialize()
        {
            // Calculate the bounds of the map
            MapBounds = new Rectangle(0, 0, _tiledMap.WidthInPixels, _tiledMap.HeightInPixels);
        }

        // Check if a position is within the map bounds
        public bool IsPositionWithinBounds(Vector2 position)
        {
            return MapBounds.Contains(position);
        }
    }
}