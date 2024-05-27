using System.Diagnostics;
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
            // Debug statements to check the values
            Debug.WriteLine($"Map Width in Tiles: {_tiledMap.Width}");
            Debug.WriteLine($"Map Height in Tiles: {_tiledMap.Height}");
            Debug.WriteLine($"Tile Width in Pixels: {_tiledMap.TileWidth}");
            Debug.WriteLine($"Tile Height in Pixels: {_tiledMap.TileHeight}");

            int mapWidthInPixels = _tiledMap.Width * _tiledMap.TileWidth;
            int mapHeightInPixels = _tiledMap.Height * _tiledMap.TileHeight;

            // Debug statements to check calculated dimensions
            Debug.WriteLine($"Calculated Map Width in Pixels: {mapWidthInPixels}");
            Debug.WriteLine($"Calculated Map Height in Pixels: {mapHeightInPixels}");

            // Calculate the bounds of the map
            MapBounds = new Rectangle(0, 0, mapWidthInPixels, mapHeightInPixels);

            // Debug statement to check the final bounds
            Debug.WriteLine($"Map Bounds: {MapBounds}");
        }

        // Check if a position is within the map bounds
        public bool IsPositionWithinBounds(Vector2 position)
        {
            {
                bool withinBounds = MapBounds.Contains((int)position.X, (int)position.Y);

                // Debugging information
                Debug.WriteLine($"Player Position: {position}");
                Debug.WriteLine($"Map Bounds: {MapBounds}");
                Debug.WriteLine($"Player is {(withinBounds ? "within" : "out of")} bounds.");

                return withinBounds;
            }

        }
    }
}
