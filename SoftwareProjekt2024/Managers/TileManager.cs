using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoftwareProjekt2024
{
    public class TileManager
    {
        public Dictionary<Vector2, int> tilemap;
        public List<Rectangle> textureStore;
        public Texture2D textureAtlas;


        public TileManager()
        {
            tilemap = LoadMap("../../../Data/a1.csv");

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
                            if (value >= 0)  // = to include 0, otherwise > 
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
