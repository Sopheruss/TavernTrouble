using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
using System;
using System.Collections.Generic;
using System.IO;


namespace SoftwareProjekt2024
{
    public class TileManager
    {

        public Dictionary<Vector2, int> groundworkLayer;
        public Dictionary<Vector2, int> objectsLayer;
        public Dictionary<Vector2, int> collisionLayer;
        //public Dictionary<Vector2, int> interactionLayer;

        public Texture2D textureAtlas;
        public Texture2D hitboxes;

        //implementing for camera? 
        readonly int mapWidth;
        readonly int mapHeight;

        public TileManager()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            groundworkLayer = LoadMap(Path.Combine(basePath, "Data", "tavern_groundworkLayer.csv"));
            objectsLayer = LoadMap(Path.Combine(basePath, "Data", "tavern_objectsLayer.csv"));
            collisionLayer = LoadMap(Path.Combine(basePath, "Data", "tavern_collisionLayer.csv"));
            //interactionLayer = LoadMap("../../../Data/tavern_interactionLayer.csv");




            // Opens a CSV file, reads it line by line, splits the line into
            // an array of integers. Converts data into a Dictionary where the
            // keys is the physical position of the number in the file
            // (e.g. line 3, column 2) => (2, 1)).
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

        public void Draw(SpriteBatch spriteBatch, int displayTileSize, int numTilesPerRow, int pixelTileSize, PerspectiveManager _perspectiveManager)
        {
            DrawLayer(spriteBatch, groundworkLayer, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
            //DrawLayer(spriteBatch, objectsLayer, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
            //LoadLayer(spriteBatch, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize, _perspectiveManager);
            //DrawLayer(spriteBatch, collisionLayer, hitboxes, displayTileSize, 1, pixelTileSize); // hitboxes only has one tile per row
        }
        public void LoadObjectlayer(SpriteBatch spriteBatch, int displayTileSize, int numTilesPerRow, int pixelTileSize, PerspectiveManager _perspectiveManager)
        {
            foreach (var item in objectsLayer)
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

                if (item.Value == 21)
                {
                    _perspectiveManager._staticObjects[0].Add(new Tisch(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));
                }
            }
        }



        private void DrawLayer(SpriteBatch spriteBatch, Dictionary<Vector2, int> layer, Texture2D texture, int displayTileSize, int numTilesPerRow, int pixelTileSize)
        {
            foreach (var item in layer)
            {
                Rectangle dest = new(
                    (int)item.Key.X * displayTileSize, 
                    (int)item.Key.Y * displayTileSize,
                    displayTileSize, displayTileSize);


                // get the src rect (the part of the image drawn) from the value
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
