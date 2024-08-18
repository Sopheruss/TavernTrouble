using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.StaticObjects;
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
            //DrawLayer(spriteBatch, objectsLayer, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
            //DrawLayer(spriteBatch, collisionLayer, hitboxes, displayTileSize, 1, pixelTileSize); // hitboxes only has one tile per row
            //DrawLayer(spriteBatch, interactionLayer, hitboxes, displayTileSize, 1, pixelTileSize); // hitboxes only has one tile per row
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

                Rectangle doubleHightDestRec = new(     // adjusted dest and src for objects with two tileID's
                    (int)item.Key.X * displayTileSize,
                    (int)item.Key.Y * displayTileSize,
                    displayTileSize, displayTileSize * 2);

                Rectangle doubleHightSrcRec = new(
                    x * pixelTileSize,
                    y * pixelTileSize,
                    pixelTileSize, pixelTileSize * 2);

                switch (item.Value)
                {
                    case 7:    //Trash
                        _perspectiveManager._Interactables.Add(new Trash(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 32:    //Grill
                        _perspectiveManager._tische.Add(new Grill(textureAtlas, new Vector2(dest.X, dest.Y),
                            new Rectangle((int)item.Key.X * displayTileSize, (int)item.Key.Y * displayTileSize, displayTileSize * 2, displayTileSize * 3),  // adjusted dest and src rectangles to initialize whole grill with one case
                            new Rectangle(x * pixelTileSize, y * pixelTileSize, pixelTileSize * 2, pixelTileSize * 3),
                            _perspectiveManager));
                        break;
                    case 34:    //Bierfass
                        _perspectiveManager._Interactables.Add(new Bierfass(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 35:    //PlatePile
                        _perspectiveManager._Interactables.Add(new PlatePile(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 36:    //MugPile
                        _perspectiveManager._Interactables.Add(new MugPile(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 37:    //SaladCrate
                        _perspectiveManager._Interactables.Add(new SaladCrate(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 38:    //MeatCrate
                        _perspectiveManager._Interactables.Add(new MeatCrate(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 39:    //BunCrate
                        _perspectiveManager._Interactables.Add(new BunCrate(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
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
                    case 54:    //Cuttingboard
                        _perspectiveManager._Interactables.Add(new PotatoCrate(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 55:    //Workstation
                        _perspectiveManager._Interactables.Add(new Workstation(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 56:    //Kochbuch -> kombiniert beide Tiles
                        _perspectiveManager._Interactables.Add(new CookBook(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 57:    //Kessel
                        _perspectiveManager._Interactables.Add(new Kessel(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 58:    //Cuttingboard
                        _perspectiveManager._Interactables.Add(new Cuttingboard(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                        break;
                    case 59:    //Tisch -> one case for initializing whole table
                        _perspectiveManager._tische.Add(new Tisch(textureAtlas, new Vector2(dest.X, dest.Y),
                            new Rectangle((int)item.Key.X * displayTileSize, (int)item.Key.Y * displayTileSize, displayTileSize * 2, displayTileSize * 2),  // adjusted dest and src rectangles to initialize whole table with one case
                            new Rectangle(x * pixelTileSize, y * pixelTileSize, pixelTileSize * 2, pixelTileSize * 2),
                            _perspectiveManager));
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