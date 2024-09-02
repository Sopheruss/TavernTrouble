using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Penumbra;
using SoftwareProjekt2024.Components.DekoObjects;
using SoftwareProjekt2024.Components.StaticObjects;
using SoftwareProjekt2024.Managers;
using System;
using System.Collections.Generic;
using System.IO;

/* ID-Katalouge: 
 * Kitchen Equipment: 
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
 * 
 * big table: 59, 60, 67, 68
 * 
 * Deko Equipment:
 * Candleholder left: 28
 * Candleholder right: 31
 * Candleholder middle: 61
 * window left: 69, 70, 71, 77, 78, 79
 * window middle: 85, 86, 87, 93, 94, 95
 * window right: 101, 102, 103, 109, 110, 111
 * barrel with candles left: 117, 134, 135, 142, 143
 * barrel with candles right: 125, 133, 139, 140, 141
 * canldes on table: 136, 137
 * plant shelve left: 119, 127
 * plant shelve right: 118, 126
 * kitchen stuff on bord: 12
 * rug: 72 - 76, 80 - 84, 88 - 92, 96  100, 104 - 108, 112 - 116, 120 - 124, 128 - 132
 * window nupsi: 138
 */

namespace SoftwareProjekt2024;

public class TileManager
{

    public Dictionary<Vector2, int> groundworkLayer;
    public Dictionary<Vector2, int> objectsLayer;
    public Dictionary<Vector2, int> collisionLayer;
    public Dictionary<Vector2, int> interactionLayer;
    public Dictionary<Vector2, int> dekoLayerObjects;
    public Dictionary<Vector2, int> dekoLayerNonObjects;

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
        dekoLayerObjects = LoadMap(Path.Combine(basePath, "Data", "map_dekoLayerObjects.csv"));
        dekoLayerNonObjects = LoadMap(Path.Combine(basePath, "Data", "map_dekoLayerNonObjects.csv"));

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
        //DrawLayer(spriteBatch, dekoLayerObjects, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
        DrawLayer(spriteBatch, dekoLayerNonObjects, textureAtlas, displayTileSize, numTilesPerRow, pixelTileSize);
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
                    _perspectiveManager._Interactables.Add(new Grill(textureAtlas, new Vector2(dest.X, dest.Y),
                        new Rectangle((int)item.Key.X * displayTileSize, (int)item.Key.Y * displayTileSize, displayTileSize * 2, displayTileSize * 3),  // adjusted dest and src rectangles to initialize whole grill with one case
                        new Rectangle(x * pixelTileSize, y * pixelTileSize, pixelTileSize * 2, pixelTileSize * 3),
                        _perspectiveManager));
                    break;
                case 34:    //Beer Barrel
                    _perspectiveManager._Interactables.Add(new BeerBarrel(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                    break;
                case 35:    //PlatePile
                    _perspectiveManager._Interactables.Add(new PlatePile(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));

                    PointLight BehindPlateWindow = new PointLight
                    {
                        Position = new Vector2(dest.X + displayTileSize / 2 - 25, 15),
                        Scale = new Vector2(505f), // Adjust size 
                        Intensity = 1f, // Adjust intensity 
                        Color = Color.OrangeRed, // Change color 
                        CastsShadows = true, // Ensure shadows are cast
                        ShadowType = ShadowType.Solid, // Set the shadow type


                        /* only for Spotlight
                        0 radians points to the right (positive X direction).
                        π/2 radians points downward (positive Y direction).
                        π radians points to the left (negative X direction).
                        3π/2 radians points upward (negative Y direction).
                        */

                        //Rotation = MathHelper.ToRadians(90f) 

                    };

                    penumbra.Lights.Add(BehindPlateWindow);
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
                case 50:    //Bar left
                    _perspectiveManager._nonInteractables.Add(new Bar_Left(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));

                    Hull BarHull_left = new Hull(new Vector2[]
                    {
                        new Vector2(dest.X+20, dest.Y + 20),
                        new Vector2(dest.X + dest.Width, dest.Y + 20),
                        new Vector2(dest.X + dest.Width, dest.Y + dest.Height),
                        new Vector2(dest.X+20, dest.Y + dest.Height)
                    }
                        );

                    if (!BarHull_left.Valid)
                    {
                        Console.WriteLine("Hull invalid!");
                    }
                    else penumbra.Hulls.Add(BarHull_left);

                    break;
                case 51:    //Bar
                    _perspectiveManager._barFlächen.Add(new Bar(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));

                    Hull BarHull_1 = new Hull(new Vector2[]
                        {
                        new Vector2(dest.X, dest.Y + 20),
                        new Vector2(dest.X + dest.Width, dest.Y + 20),
                        new Vector2(dest.X + dest.Width, dest.Y + dest.Height),
                        new Vector2(dest.X, dest.Y + dest.Height)
                        }
                            );

                    if (!BarHull_1.Valid)
                    {
                        Console.WriteLine("Hull invalid!");
                    }
                    else penumbra.Hulls.Add(BarHull_1);

                    break;
                case 52:    //Bar
                    _perspectiveManager._barFlächen.Add(new Bar(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));

                    Hull BarHull_2 = new Hull(new Vector2[]
                    {
                        new Vector2(dest.X, dest.Y +20), // top left corner (with offset)
                        new Vector2(dest.X + dest.Width, dest.Y +20), // top right corner (with offset)
                        new Vector2(dest.X + dest.Width, dest.Y + dest.Height), //bottom right corner
                        new Vector2(dest.X, dest.Y + dest.Height) // bottom left corner
                    }
                        );

                    if (!BarHull_2.Valid)
                    {
                        Console.WriteLine("Hull invalid!");
                    }
                    else penumbra.Hulls.Add(BarHull_2);

                    break;
                case 53:    //Bar Right
                    _perspectiveManager._nonInteractables.Add(new Bar_Right(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));

                    Hull BarHull_right = new Hull(new Vector2[]
                      {
                        new Vector2(dest.X, dest.Y + 20),
                        new Vector2(dest.X + dest.Width -20, dest.Y + 20),
                        new Vector2(dest.X + dest.Width -20, dest.Y + dest.Height),
                        new Vector2(dest.X, dest.Y + dest.Height)
                      }
                          );

                    if (!BarHull_right.Valid)
                    {
                        Console.WriteLine("Hull invalid!");
                    }
                    else penumbra.Hulls.Add(BarHull_right);

                    break;
                case 54:    //PotatoCrate
                    _perspectiveManager._Interactables.Add(new PotatoCrate(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                    break;
                case 55:    //Workstation
                    _perspectiveManager._Interactables.Add(new Workstation(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                    break;
                case 56:    //CookBook -> kombiniert beide Tiles
                    _perspectiveManager._Interactables.Add(new CookBook(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                    break;
                case 57:    //Boiler
                    _perspectiveManager._Interactables.Add(new Kessel(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));

                    // either SpotLight or PointLight
                    PointLight BehindKesselWindow = new PointLight
                    {
                        Position = new Vector2(dest.X + displayTileSize / 2 - 70, 15),
                        Scale = new Vector2(505f), // Adjust size 
                        Intensity = 1f, // Adjust intensity 
                        Color = Color.OrangeRed, // Change color 
                        CastsShadows = true, // Ensure shadows are cast
                        ShadowType = ShadowType.Solid, // Set the shadow type

                    };

                    penumbra.Lights.Add(BehindKesselWindow);

                    break;
                case 58:    //Cuttingboard
                    _perspectiveManager._cuttingBoards.Add(new Cuttingboard(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));
                    break;
                case 59:    //Table -> one case for initializing whole table
                    _perspectiveManager._tables.Add(new Table(textureAtlas, new Vector2(dest.X, dest.Y),
                        new Rectangle((int)item.Key.X * displayTileSize, (int)item.Key.Y * displayTileSize, displayTileSize * 2, displayTileSize * 2),  // adjusted dest and src rectangles to initialize whole table with one case
                        new Rectangle(x * pixelTileSize, y * pixelTileSize, pixelTileSize * 2, pixelTileSize * 2),
                        _perspectiveManager));

                    break;
                case 62: // potatoBox
                    PointLight BehindKartoffelBox = new PointLight
                    {
                        Position = new Vector2(dest.X + displayTileSize / 2 - 75, 15),
                        Scale = new Vector2(505f),
                        Intensity = 1f,
                        Color = Color.OrangeRed,
                        CastsShadows = true,
                        ShadowType = ShadowType.Solid,

                    };

                    penumbra.Lights.Add(BehindKartoffelBox);
                    break;
            }
        }
    }

    int _counterForLights = 0;

    public void LoadDekoLayer(SpriteBatch spriteBatch, int displayTileSize, int numTilesPerRow, int pixelTileSize, PerspectiveManager _perspectiveManager, PenumbraComponent penumbra)
    {
        foreach (var item in dekoLayerObjects)
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
                case 28:    //Candleholder left
                    _perspectiveManager._dekoObjects.Add(new CandleHolderLeft(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));

                    PointLight staticLightKerzeLeft = new PointLight
                    {

                        Position = new Vector2(dest.X + displayTileSize / 2 + 7, dest.Y + displayTileSize / 2),
                        Scale = new Vector2(27f),
                        Intensity = 1f,
                        Color = Color.LightGoldenrodYellow,
                        CastsShadows = false,


                    };

                    penumbra.Lights.Add(staticLightKerzeLeft);

                    break;
                case 31:    //Candleholder right
                    _perspectiveManager._dekoObjects.Add(new CandleHolderRight(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));

                    PointLight staticLightKerzeRight = new PointLight
                    {

                        Position = new Vector2(dest.X + displayTileSize / 2 - 7, dest.Y + displayTileSize / 2),
                        Scale = new Vector2(27f),
                        Intensity = 1f,
                        Color = Color.LightGoldenrodYellow,
                        CastsShadows = false,

                    };

                    penumbra.Lights.Add(staticLightKerzeRight);

                    break;
                case 61:    //candleholder middle
                    _perspectiveManager._dekoObjects.Add(new CandleHolderMiddle(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));

                    PointLight staticLightKerzeMid = new PointLight
                    {

                        Position = new Vector2(dest.X + displayTileSize / 2, dest.Y + displayTileSize / 2),
                        Scale = new Vector2(30f),
                        Intensity = 1f,
                        Color = Color.LightGoldenrodYellow,
                        CastsShadows = false,


                    };

                    penumbra.Lights.Add(staticLightKerzeMid);

                    break;
                case 69:    //window left 
                    _perspectiveManager._dekoObjects.Add(new Window_Left(textureAtlas, new Vector2(dest.X, dest.Y),
                        new((int)item.Key.X * displayTileSize, (int)item.Key.Y * displayTileSize, displayTileSize * 3, displayTileSize * 2),
                        new(x * pixelTileSize, y * pixelTileSize, pixelTileSize * 3, pixelTileSize * 2),
                        _perspectiveManager));
                    break;
                case 85:    //window middle
                    _perspectiveManager._dekoObjects.Add(new Window_Middle(textureAtlas, new Vector2(dest.X, dest.Y),
                        new((int)item.Key.X * displayTileSize, (int)item.Key.Y * displayTileSize, displayTileSize * 3, displayTileSize * 2),
                        new(x * pixelTileSize, y * pixelTileSize, pixelTileSize * 3, pixelTileSize * 2),
                        _perspectiveManager));
                    break;
                case 101:   //window right
                    _perspectiveManager._dekoObjects.Add(new Window_Right(textureAtlas, new Vector2(dest.X, dest.Y),
                        new((int)item.Key.X * displayTileSize, (int)item.Key.Y * displayTileSize, displayTileSize * 3, displayTileSize * 2),
                        new(x * pixelTileSize, y * pixelTileSize, pixelTileSize * 3, pixelTileSize * 2),
                        _perspectiveManager));
                    break;
                case 133:   //barrel right
                    _perspectiveManager._dekoObjects.Add(new Barrel_Right(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));

                    PointLight staticLightBarrelRight = new PointLight
                    {

                        Position = new Vector2(dest.X + displayTileSize / 2 - 7, dest.Y + displayTileSize / 2 + 15),
                        Scale = new Vector2(42f),
                        Intensity = 1f,
                        Color = Color.LightGoldenrodYellow,
                        CastsShadows = false,

                    };

                    penumbra.Lights.Add(staticLightBarrelRight);

                    break;
                case 134:   //barrel left
                    _perspectiveManager._dekoObjects.Add(new Barrel_Left(textureAtlas, new Vector2(dest.X, dest.Y), doubleHightDestRec, doubleHightSrcRec, _perspectiveManager));


                    PointLight staticLightBarrelLeft = new PointLight
                    {

                        Position = new Vector2(dest.X + displayTileSize / 2 + 7, dest.Y + displayTileSize / 2 + 15),
                        Scale = new Vector2(42f),
                        Intensity = 1f,
                        Color = Color.LightGoldenrodYellow,
                        CastsShadows = false,


                    };

                    penumbra.Lights.Add(staticLightBarrelLeft);


                    break;
                case 136:   //candle on table 
                    _perspectiveManager._dekoObjects.Add(new CandleOnTable(textureAtlas, new Vector2(dest.X, dest.Y),
                        new((int)item.Key.X * displayTileSize, (int)item.Key.Y * displayTileSize, displayTileSize * 2, displayTileSize),
                        new(x * pixelTileSize, y * pixelTileSize, pixelTileSize * 2, pixelTileSize),
                        _perspectiveManager));

                    PointLight staticLightKerzeTable = new PointLight
                    {

                        Position = new Vector2(dest.X + displayTileSize / 2 + 15, dest.Y + displayTileSize / 2 + 10),
                        Scale = new Vector2(40f),
                        Intensity = 1f,
                        Color = Color.LightGoldenrodYellow,
                        CastsShadows = false,
                        // ShadowType = ShadowType.Solid, 

                    };

                    penumbra.Lights.Add(staticLightKerzeTable);

                    break;
                case 138:   //window nupsi
                    _perspectiveManager._dekoObjects.Add(new Window_Nupsi(textureAtlas, new Vector2(dest.X, dest.Y), dest, src, _perspectiveManager));



                    _counterForLights++;

                    // Check if the object counter is even, meaning it's every second object, so light gets created
                    if (_counterForLights % 2 == 0)
                    {
                        PointLight StaticLightWindowNupsi = new PointLight
                        {
                            Position = new Vector2(dest.X + displayTileSize / 2 - 50, dest.Y + displayTileSize / 2 + 100),
                            Scale = new Vector2(350f),
                            Intensity = 1.0f,
                            Color = Color.OrangeRed,
                            CastsShadows = true,
                            ShadowType = ShadowType.Solid,
                        };

                        penumbra.Lights.Add(StaticLightWindowNupsi);
                    }


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