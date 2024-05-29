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
        public List<Rectangle> textureStore;
        public Texture2D textureAtlas;


        public TileManager()
        {
            groundworkLayer = LoadMap("../../../Data/tavern_groundworkLayer.csv");
            objectsLayer = LoadMap("../../../Data/tavern_objectsLayer.csv");
            collisionLayer = LoadMap("../../../Data/tavern_collisionLayer.csv");

            textureStore = new()
            {
                new Rectangle(0,0,32,32),
                new Rectangle(0,32,32,32)


            };


            Dictionary<Vector2, int> LoadMap(string filepath)
            {
                Dictionary<Vector2, int> result = new();

                using StreamReader reader = new(filepath);

                int y = 0;
                string line;

                // loop until file is null, when null, end of file is reached
                while ((line = reader.ReadLine()) != null)
                {
                    string[] items = line.Split(',');

                    for (int x = 0; x < items.Length; x++)
                    {
                        if (int.TryParse(items[x], out int value)) // if we parse our value successfully into an integer, cont
                        {
                            if (value > -1)
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
    }
}
