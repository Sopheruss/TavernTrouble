
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace SoftwareProjekt2024
{
    public class TileManager
    {

        public Dictionary<Vector2, int> groundworkLayer;
        public Dictionary<Vector2, int> objectsLayer;
        public Dictionary<Vector2, int> collisionLayer;
        public Texture2D textureAtlas;
        public Texture2D hitboxes;

        public TileManager()
        {
            groundworkLayer = LoadMap("../../../Data/tavern_groundworkLayer.csv");
            objectsLayer = LoadMap("../../../Data/tavern_objectsLayer.csv");
            collisionLayer = LoadMap("../../../Data/tavern_collisionLayer.csv");

            Dictionary<Vector2, int> LoadMap(string filepath)
            {
                Dictionary<Vector2, int> result = new();

                using StreamReader reader = new(filepath);

                int y = 0;
                string line;

                while ((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split(',');

                    for (int x = 0; x < items.Length; x++)
                    {
                        if (int.TryParse(items[x], out int value))
                        {
                            if (value > -1) // -1 is "nothing", tile id starts with 0. 
                            {
                                result[new Vector2(x, y)] = value;
                            }
                        }
                    }

                    y++;
                }

                return result;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int displayTileSize, int numTilesPerRow, int pixelTileSize)
        {
            DrawLayer(spriteBatch, groundworkLayer, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
            DrawLayer(spriteBatch, objectsLayer, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
            DrawLayer(spriteBatch, collisionLayer, hitboxes, displayTileSize, 1, pixelTileSize); // hitboxes only has one tile per row
        }

        private void DrawLayer(SpriteBatch spriteBatch, Dictionary<Vector2, int> layer, Texture2D texture, int displayTileSize, int numTilesPerRow, int pixelTileSize)
        {
            foreach (var item in layer)
            {
                Rectangle dest = new(
                    (int)item.Key.X * displayTileSize,
                    (int)item.Key.Y * displayTileSize,
                    displayTileSize, displayTileSize);

                int x = item.Value % numTilesPerRow;
                int y = item.Value / numTilesPerRow;

                Rectangle src = new(
                    x * pixelTileSize,
                    y * pixelTileSize,
                    pixelTileSize, pixelTileSize);

                spriteBatch.Draw(texture, dest, src, Color.White);
            }
        }
    }
}

