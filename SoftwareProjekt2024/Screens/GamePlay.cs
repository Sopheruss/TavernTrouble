using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;
using Penumbra;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.StaticObjects;
using SoftwareProjekt2024.Managers;
using System;
using System.Diagnostics;


namespace SoftwareProjekt2024.Screens;

public class GamePlay
{
    readonly public Game1 _game;
    readonly SpriteBatch _spriteBatch;
    readonly ContentManager _content;

    // Penumbra lighting system
    private PenumbraComponent _penumbra;

    // needed for hull-player-points
    Hull playerHull;
    int topPadding = 32;
    int bottomPadding = 0;
    int leftPadding = 16;
    int rightPadding = 16;

    // Camera stuff; using Monogame Extended Camera
    private OrthographicCamera _camera;

    Button _pauseButton;
    Button _cookBookButton;
    Button _helpButton;

    Texture2D _orderStrip;
    Rectangle _orderStripRect;

    PerspectiveManager _perspectiveManager;
    TileManager _tileManager;
    CollisionManager _collisionManager;
    InteractionManager _interactionManager;
    InputManager _inputManager;
    GameplayLoopManager _gameplayLoopManager;

    BitmapFont bmfont;
    private int score;

    Texture2D _scordeBord;
    Rectangle _scordeBordRect;

    Player _ogerCook;

    readonly int _screenWidth;
    readonly int _screenHeight;

    readonly int _viewPortWidth = 426;
    readonly int _viewPortHeight = 240;

    readonly int _tileSize = 32;

    int _mapWidth = 0;
    int _mapHeight = 0;
    int _mapWidthPx = 0;
    int _mapHeightPx = 0;

    Texture2D rectangleTexture;


    // Stopwatch for tracking elapsed time
    public static Stopwatch _timer;
    private GameTime gameTime;


    public static bool _showLetter = true;
    Letter _letter;
    string _keyPressLetter;
    Vector2 _keyPressLetterSize;

    public static bool _showPossibleInteraction = false;
    public static string _possibleInteractionObject;

    public GamePlay(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _content = Content;
        _game = game;

        _screenWidth = screenWidth;
        _screenHeight = screenHeight;

        _spriteBatch = spriteBatch;

        // Initialize stopwatch
        _timer = new Stopwatch();


        // Initialize Penumbra lighting system
        _penumbra = new PenumbraComponent(_game);

    }

    public void LoadContent(GameWindow window, GraphicsDevice graphicsDevice)
    {
        /* camera */
        var viewportAdapter = new BoxingViewportAdapter(window, graphicsDevice, _viewPortWidth, _viewPortHeight); //sets the size of the viewport window
        _camera = new OrthographicCamera(viewportAdapter);

        /* perspective */
        _perspectiveManager = new PerspectiveManager();

        /* button */
        _pauseButton = new Button(
            _content.Load<Texture2D>("Buttons/pauseButton"),
            _content.Load<Texture2D>("Buttons/pauseButtonHovering"),
            new Vector2(30, 30));

        _cookBookButton = new Button(
            _content.Load<Texture2D>("Buttons/cookBookButton"),
            _content.Load<Texture2D>("Buttons/cookBookButtonHovering"),
            new Vector2(30, _screenHeight - 30));

        _helpButton = new Button(
            _content.Load<Texture2D>("Buttons/helpButton"),
            _content.Load<Texture2D>("Buttons/helpButtonHovering"),
            new Vector2(_screenWidth - 30, _screenHeight - 30));

        /* plate types */
        Plate.plain = _content.Load<Texture2D>("Food/plate");
        Plate.withMeat = _content.Load<Texture2D>("Food/meat_plate");
        Plate.withMeat_Bun = _content.Load<Texture2D>("Food/meat_bun_plate");
        Plate.withFullBurger = _content.Load<Texture2D>("Food/full_burger_on_plate");
        Plate.withBun = _content.Load<Texture2D>("Food/bun_plate");
        Plate.withBun_Salad = _content.Load<Texture2D>("Food/salad_bun_plate");
        Plate.withSalad = _content.Load<Texture2D>("Food/salad_plate");
        Plate.withMeat_Salad = _content.Load<Texture2D>("Food/meat_salad_plate");
        Plate.withFries = _content.Load<Texture2D>("Food/fries_plate");

        /* other dynamic objects */
        //Bun.bunTexture = _content.Load<Texture2D>("")

        /* map */
        _tileManager = new TileManager();
        _tileManager.textureAtlas = _content.Load<Texture2D>("Map/atlas");
        _tileManager.hitboxes = _content.Load<Texture2D>("Map/hitboxes");
        _tileManager.LoadObjectlayer(_spriteBatch, _tileSize, 8, _tileSize, _perspectiveManager, _penumbra); //Laden aller Objekte von Tiled
        _tileManager.LoadDekoLayer(_spriteBatch, _tileSize, 8, _tileSize, _perspectiveManager, _penumbra); //Laden aller Objekte von Deko Ebene

        _mapHeight = _tileManager.mapHeight;
        _mapWidth = _tileManager.mapWidth;
        _mapHeightPx = _tileManager.mapHeight * _tileSize;
        _mapWidthPx = _tileManager.mapWidth * _tileSize;

        /* player */
        //local implementation, cuz acces to texture via Sprite class
        Player.plain = _content.Load<Texture2D>("Models/oger_cook_spritesheet");
        Player.withPlate = _content.Load<Texture2D>("Models/Oger_Plate");
        Player.withMeat = _content.Load<Texture2D>("Models/Oger_Meat");
        Player.withBun = _content.Load<Texture2D>("Models/Oger_Bun");
        Player.withSalad = _content.Load<Texture2D>("Models/Oger_Salad");
        Player.withPotato = _content.Load<Texture2D>("Models/Oger_Potato");
        Player.withPlate_Fries = _content.Load<Texture2D>("Models/Oger_Plate_Fries");
        Player.withPlate_FullBurger = _content.Load<Texture2D>("Models/Oger_Plate_Full_Burger");

        _ogerCook = new Player(Player.plain,
                              new Vector2(_mapWidthPx / 2, _mapHeightPx / 4),
                              _perspectiveManager);

        rectangleTexture = new Texture2D(graphicsDevice, 1, 1);         // For player rectangle
        rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });  // ''

        _ogerCook.Load();

        /* guests */
        Guest.fairy = _content.Load<Texture2D>("Npc/Fairy_Npc");
        Guest.ogerBlue = _content.Load<Texture2D>("Npc/Oger_Npc_Blue");
        Guest.ogerGreen = _content.Load<Texture2D>("Npc/Oger_Npc_Green");
        Guest.ogerPink = _content.Load<Texture2D>("Npc/Oger_Npc_Pink");

        /* kessel */
        Kessel._kesselTextureFull = _content.Load<Texture2D>("Kessel/Kessel_Done");
        Kessel._kesselTextureAnimation = _content.Load<Texture2D>("Kessel/Kessel_Spritesheet");

        /* grill */
        Grill._grillTextureDone = _content.Load<Texture2D>("Grill/Grill_Done");
        Grill._grillTextureAnimation = _content.Load<Texture2D>("Grill/Grill_Spritesheet");

        /* cookBook */
        CookBook._cookBookClose = _content.Load<Texture2D>("CookBook/cookBook_Closed");
        CookBook._cookBookAnimation = _content.Load<Texture2D>("CookBook/cookBook_Spritesheet");

        /* collision, interaction, input */
        _collisionManager = new CollisionManager(_tileManager);
        _interactionManager = new InteractionManager(_tileManager, _ogerCook);
        _inputManager = new InputManager(_game, _ogerCook, _collisionManager, _interactionManager, _perspectiveManager);
        _gameplayLoopManager = new GameplayLoopManager(_perspectiveManager, _timer);

        /* font */
        bmfont = _content.Load<BitmapFont>("Fonts/font_new"); // load font from content-manager using monogame.ext importer/exporter

        /* sounds */
        // grill, bar, usw... soonTM

        /* score Bord*/
        _scordeBord = _content.Load<Texture2D>("OrderBar/scoreBord");
        _scordeBordRect = new Rectangle(_screenWidth - 110, _pauseButton.Height - bmfont.LineHeight, _scordeBord.Width, _scordeBord.Height);

        /* order */
        _orderStrip = _content.Load<Texture2D>("OrderBar/orderStrip");
        _orderStripRect = new Rectangle(0, 0, _screenWidth, 30 + _pauseButton.Height);

        /* Letter */
        _letter = new Letter(_content, _spriteBatch, _screenWidth, _screenHeight, new Vector2(_screenWidth / 2 - 553, _screenHeight / 2 - 329 - 20 - (int)_keyPressLetterSize.Y)); //numbers hard coded on size of letter Rect
        _keyPressLetter = "Press [any key] to continue";
        _keyPressLetterSize = bmfont.MeasureString(_keyPressLetter);

        /* lights, hulls */
        _penumbra.Initialize();
        _penumbra.AmbientColor = new Color(150, 150, 150); // RGB Values, control surrounding lights. (0-255) 

        playerHull = CreateHullAroundPlayer(_ogerCook.Rect, topPadding, bottomPadding, leftPadding, rightPadding);
        _penumbra.Hulls.Add(playerHull);
    }


    Hull CreateHullAroundPlayer(Rectangle playerBounds, int topPadding, int bottomPadding, int leftPadding, int rightPadding)
    {
        // Calculate the new rectangle with specified padding
        Rectangle adjustedBounds = new Rectangle(
            playerBounds.Left + leftPadding,
            playerBounds.Top + topPadding,
            playerBounds.Width - leftPadding - rightPadding,
            playerBounds.Height - topPadding - bottomPadding
        );

        // Ensure the rectangle dimensions are positive
        adjustedBounds.Width = Math.Max(adjustedBounds.Width, 1);
        adjustedBounds.Height = Math.Max(adjustedBounds.Height, 1);

        // Define the hull points based on the adjusted rectangle
        Vector2 topLeft = new Vector2(adjustedBounds.Left, adjustedBounds.Top);
        Vector2 topRight = new Vector2(adjustedBounds.Right, adjustedBounds.Top);
        Vector2 bottomRight = new Vector2(adjustedBounds.Right, adjustedBounds.Bottom);
        Vector2 bottomLeft = new Vector2(adjustedBounds.Left, adjustedBounds.Bottom);

        return new Hull(new Vector2[] { topLeft, topRight, bottomRight, bottomLeft });
    }

    private void CalculateCameraLookAt()
    {
        //Begrenzt ogerPosition Werte auf Intervall

        var x = MathHelper.Clamp(_ogerCook.position.X, _viewPortWidth / 2, _mapWidthPx - _viewPortWidth / 2);    //min: Hälfte der angezeigten Bildschirmweite
                                                                                                                 //max: Tilemap-Weite - Hälfte der angezeigten Bildschirmweite
        var y = MathHelper.Clamp(_ogerCook.position.Y, _viewPortHeight / 2, _mapHeightPx - _viewPortHeight / 2); //min: Hälfte der angezeigten Bildschirmhöhe
                                                                                                                 //max: Tilemap-Höhe - Hälfte der angezeigten Bildschirmhöhe
        Vector2 ClampedPosition = new Vector2(x, y);

        _camera.LookAt(ClampedPosition + new Vector2(10, 16)); //offset to center oger -> half of the texture width/height
    }

    public void Update()
    {
        if (_showLetter)
        {
            _letter.Update();
        }
        else
        {
            CalculateCameraLookAt(); //Berechne neue Camera-Zentrierung

            _pauseButton.Update();
            _cookBookButton.Update();
            _helpButton.Update();

            if (_pauseButton.isClicked || _pauseButton._escIsPressed)
            {
                Game1.activeScene = Scenes.PAUSEMENU;
                _timer.Stop(); // Stop the stopwatch when paused
            }
            else if (_cookBookButton.isClicked)
            {
                Game1.activeScene = Scenes.COOKBOOKSCREEN;
                _timer.Stop();
            }
            else if (_helpButton.isClicked)
            {
                Game1.activeScene = Scenes.HELPSCREEN;
                _timer.Stop();
            }
            else
            {
                if (Game1.activeScene == Scenes.GAMEPLAY)
                {
                    _timer.Start(); // Resume the stopwatch if not paused and wait until gameplay is actually called
                }
            }

            _ogerCook.Update();
            _inputManager.Update();
            _interactionManager.Update();
            _gameplayLoopManager.Update();

            Guest.Update();

            // only Update Kessel/Grill/CookBook when Animation is supposed to play
            if (CookBook._playCookBookAnimation) { CookBook.Update(); }
            if (Kessel._activeKesselState == KesselStates.ANIMATIONKESSEL) { Kessel.Update(); }
            if (Grill._activeGrillState == GrillStates.ANIMATIONGRILL) { Grill.Update(); }

            // Ensure the player hull is updated correctly
            UpdatePlayerHull();

            // Update the Penumbra component
            _penumbra.Update(gameTime);
        }
    }

    // Ensure the player's hull is updated separately, because otherwise the init pos of player keeps hull points stuck
    private void UpdatePlayerHull()
    {
        // Remove the old hull from the Penumbra component
        _penumbra.Hulls.Remove(playerHull);

        // Create a new hull with updated points
        playerHull = CreateHullAroundPlayer(_ogerCook.Rect, topPadding, bottomPadding, leftPadding, rightPadding);

        // Add the new hull to the Penumbra component
        _penumbra.Hulls.Add(playerHull);
    }



    // call this method to add to current displayed score
    // (currently only in interactionManager)

    public void IncreaseScore(int increment)
    {
        score += increment;
    }

    public void Draw()
    {
        // Two spriteBatch.Begin/End to separate stuff that is affected by camera and static stuff
        // TransformationMatrix is automatically calculated into the draw call, added to penumbra so the light does not move

        _penumbra.Transform = _camera.GetViewMatrix();
        _penumbra.BeginDraw();

        var transformMatrix = _camera.GetViewMatrix();

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix); // To make sharp images while scaling

        _tileManager.Draw(_spriteBatch, _tileSize, 8, _tileSize, _perspectiveManager);

        _perspectiveManager.draw(_spriteBatch);

        (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) = _collisionManager.CalcPlayerBounds(_ogerCook);
        _collisionManager.DrawDebugRect(_spriteBatch, leftBounds, 1, rectangleTexture); // Drawing player rectangle, int value is thickness
        _collisionManager.DrawDebugRect(_spriteBatch, rightBounds, 1, rectangleTexture);
        _collisionManager.DrawDebugRect(_spriteBatch, upBounds, 1, rectangleTexture);
        _collisionManager.DrawDebugRect(_spriteBatch, downBounds, 1, rectangleTexture);

        _spriteBatch.End();
        _penumbra.Draw(gameTime); // draw everything NOT affected by light (UI, Menu)



        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
        _spriteBatch.Draw(_orderStrip, _orderStripRect, Color.White);
        _spriteBatch.Draw(_scordeBord, _scordeBordRect, Color.White);
        _spriteBatch.DrawString(bmfont, "Score: \n" + score, new Vector2(_screenWidth - 100, _pauseButton.Height - bmfont.LineHeight + 10), Color.White);
        _pauseButton.Draw(_spriteBatch);
        _cookBookButton.Draw(_spriteBatch);
        _helpButton.Draw(_spriteBatch);

        // Display the elapsed time
        string elapsedTime = _timer.Elapsed.ToString(@"mm\:ss");
        _spriteBatch.DrawString(bmfont, "Time: \n" + elapsedTime, new Vector2(_screenWidth - 100, _pauseButton.Height + bmfont.LineHeight + 10), Color.White);

        if (_showLetter)
        {
            _spriteBatch.DrawString(bmfont, _keyPressLetter, new Vector2(_screenWidth / 2 - (int)_keyPressLetterSize.X / 2, _screenHeight - 15 - (int)_keyPressLetterSize.Y), Color.Beige);
            _letter.Draw();
        }

        if (_showPossibleInteraction)
        {
            Vector2 textSize = bmfont.MeasureString("Press [E] to interact with " + _possibleInteractionObject);
            _spriteBatch.DrawString(bmfont, "Press [E] to interact with " + _possibleInteractionObject, new Vector2((_screenWidth - textSize.X) / 2, _screenHeight - 15 - (int)_keyPressLetterSize.Y), Color.Beige);
        }

        _spriteBatch.End();
    }
}
