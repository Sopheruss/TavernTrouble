using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
using System;
using System.Collections.Generic;
using System.IO;

/* ID-Katalouge: 
 * table: 12
 * trash can: 7, 15
 * grill: 32, 33, 40, 41, 48, 49
 * beer barrel: 34, 42
 * plate stack: 35, 43
 * beer mug: 36, 44
 * salad box: 37, 45
 * meat box: 38, 46
 * bun box: 39, 47
 * potato box: 54, 62
 * bar: 50 - 53
 * counter top: 55, 63
 * cook book: 56, 64
 * boiler: 57, 65
 * cutting board: 58, 66
 * big table: 59, 60, 67, 68
 */

namespace SoftwareProjekt2024
{
    public class TileManager
    {

        public Dictionary<Vector2, int> groundworkLayer;
        public Dictionary<Vector2, int> objectsLayer;
        public Dictionary<Vector2, int> collisionLayer;
        public Dictionary<Vector2, int> interactionLayer;

        public Texture2D textureAtlas;
        public Texture2D hitboxes;

        public int mapWidth = 0;
        public int mapHeight = 0;

        public TileManager()
        {
            string basePath = AppDomain.CurrentDomain.BaseDirectory;

            groundworkLayer = LoadMap(Path.Combine(basePath, "Data", "map_groundLayer.csv"));
            objectsLayer = LoadMap(Path.Combine(basePath, "Data", "map_objectLayer.csv"));
            collisionLayer = LoadMap(Path.Combine(basePath, "Data", "map_collisionLayer.csv"));
            interactionLayer = LoadMap(Path.Combine(basePath, "Data", "map_interactionLayer.csv"));

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

                    if (mapWidth < items.Length)
                    {
                        mapWidth = items.Length;
                    }

                    y++;
                }

                mapHeight = y; //counts lines of csv 

                return result;
            }
        }

        public void Draw(SpriteBatch spriteBatch, int displayTileSize, int numTilesPerRow, int pixelTileSize, PerspectiveManager _perspectiveManager)
        {
            DrawLayer(spriteBatch, groundworkLayer, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
            DrawLayer(spriteBatch, objectsLayer, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
            //DrawLayer(spriteBatch, collisionLayer, hitboxes, displayTileSize, 1, pixelTileSize); // hitboxes only has one tile per row
            //DrawLayer(spriteBatch, interactionLayer, hitboxes, displayTileSize, 1, pixelTileSize); // hitboxes only has one tile per row
        }
        public void LoadObjectlayer(SpriteBatch spriteBatch, int displayTileSize, int numTilesPerRow, int pixelTileSize, PerspectiveManager _perspectiveManager, PenumbraComponent penumbra)
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

                switch (item.Value)
                {
                    case 59 | 60 | 67 | 68:    //Tisch DOES NOT WORK!
                        _perspectiveManager._tische.Add(new Tisch(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));
                        
                        Hull tableHull = new Hull (new Vector2[]
                        {
                            new Vector2(dest.X, dest.Y),
                            new Vector2(dest.X+dest.Width,dest.Y),
                            new Vector2(dest.X+dest.Width, dest.Y+dest.Height),
                            new Vector2(dest.X,dest.Height)
                        }   
                            );

                        penumbra.Hulls.Add(tableHull);
                        
                        break;

                    case 50:    //Bar links
                        _perspectiveManager._nonInteractables.Add(new Bar_Links(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));
                        break;
                    case 51:    //Bar
                        _perspectiveManager._barFlächen.Add(new Bar(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));
                        break;
                    case 52:    //Bar
                        _perspectiveManager._barFlächen.Add(new Bar(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));
                        break;
                    case 53:    //Bar Rechts
                        _perspectiveManager._nonInteractables.Add(new Bar_Rechts(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));
                        break;
                    case 64:    //Kochbuch -> nur untere Hälften, weil für Interaktion nur das wichtig?
                        _perspectiveManager._nonInteractables.Add(new CookBook(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));
                        break;
                    case 65:    //Kessel
                        _perspectiveManager._nonInteractables.Add(new Kessel(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));

                        PointLight staticLightKessel = new PointLight
                        {

                            Position = new Vector2(dest.X + displayTileSize / 2, dest.Y + displayTileSize / 2),
                            Scale = new Vector2(300f),
                            Intensity = 1f,
                            Color = Color.OrangeRed
                        };
                        
                        penumbra.Lights.Add(staticLightKessel);
                        break;
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