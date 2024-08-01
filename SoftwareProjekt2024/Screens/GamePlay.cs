﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.BitmapFonts;
using MonoGame.Extended.ViewportAdapters;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
using System.Diagnostics;

namespace SoftwareProjekt2024.Screens;

internal class GamePlay
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;
    readonly ContentManager _content;

    // Camera stuff; using Monogame Extended Camera
    private OrthographicCamera _camera;

    Button _pauseButton;
    Button _cookBookButton;

    Texture2D _orderStrip;
    Rectangle _orderStripRect;

    PerspectiveManager _perspectiveManager;
    AnimationManager _animationManager;
    TileManager _tileManager;
    CollisionManager _collisionManager;
    InteractionManager _interactionManager;
    InputManager _inputManager;

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
    readonly private Stopwatch _timer;

    public GamePlay(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _content = Content;
        _game = game;

        _screenWidth = screenWidth;
        _screenHeight = screenHeight;

        _spriteBatch = spriteBatch;

        // Initialize stopwatch
        _timer = new Stopwatch();
    }

    public void LoadContent(GameWindow window, GraphicsDevice graphicsDevice)
    {
        /* camera */
        var viewportAdapter = new BoxingViewportAdapter(window, graphicsDevice, _viewPortWidth, _viewPortHeight); //sets the size of the viewport window
        _camera = new OrthographicCamera(viewportAdapter);

        /* perspective */
        _perspectiveManager = new PerspectiveManager();

        /* animation */
        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image
        //Vector decides size of the size for the frame (one Oger Frame = 19/32)
        _animationManager = new(4, 4, new Vector2(19, 32));

        /* button */
        _pauseButton = new Button(
            _content.Load<Texture2D>("Buttons/pauseButton"),
            _content.Load<Texture2D>("Buttons/pauseButtonHovering"),
            new Vector2(30, 30));

        _cookBookButton = new Button(
            _content.Load<Texture2D>("Buttons/cookBookButton"),
            _content.Load<Texture2D>("Buttons/cookBookButtonHovering"),
            new Vector2(30, _screenHeight - 30));

        /* map */
        _tileManager = new TileManager();
        //_tileManager.textureAtlas = Content.Load<Texture2D>("atlas");
        _tileManager.textureAtlas = _content.Load<Texture2D>("Map/atlas");
        _tileManager.hitboxes = _content.Load<Texture2D>("Map/hitboxes");
        _tileManager.LoadObjectlayer(_spriteBatch, _tileSize, 8, _tileSize, _perspectiveManager); //Laden aller Objekte von Tiled

        _mapHeight = _tileManager.mapHeight;
        _mapWidth = _tileManager.mapWidth;
        _mapHeightPx = _tileManager.mapHeight * _tileSize;
        _mapWidthPx = _tileManager.mapWidth * _tileSize;

        /* player */
        //local implementation, cuz acces to texture via Sprite class
        Texture2D _ogerCookSpritesheet = _content.Load<Texture2D>("Models/oger_cook_spritesheet");

        _ogerCook = new Player(_ogerCookSpritesheet,
                              new Vector2(_mapWidthPx / 2, _mapHeightPx / 6),
                              _perspectiveManager);

        rectangleTexture = new Texture2D(graphicsDevice, 1, 1);         // For player rectangle
        rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });  // ''

        /* collision, interaction, input */
        _collisionManager = new CollisionManager(_tileManager);
        _interactionManager = new InteractionManager(_tileManager, _ogerCook, this);
        _inputManager = new InputManager(_game, _ogerCook, _collisionManager, _interactionManager, _animationManager);

        /* font */
        bmfont = _content.Load<BitmapFont>("Fonts/font_new"); // load font from content-manager using monogame.ext importer/exporter

        /* timer */
        _timer.Start();

        /* score Bord*/
        _scordeBord = _content.Load<Texture2D>("OrderBar/scoreBord");
        _scordeBordRect = new Rectangle(_screenWidth - 110, _pauseButton.Height - 20/*bmfont.LineHeight*/, _scordeBord.Width, _scordeBord.Height);

        /* order */
        _orderStrip = _content.Load<Texture2D>("OrderBar/orderStrip");
        _orderStripRect = new Rectangle(0, 0, _screenWidth, 30 + _pauseButton.Height);
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
        CalculateCameraLookAt(); //Berechne neue Camera-Zentrierung

        _pauseButton.Update();
        _cookBookButton.Update();

        if (_pauseButton.isClicked || _pauseButton._escIsPressed)
        {
            _game.activeScene = Scenes.PAUSEMENU;
            _timer.Stop(); // Stop the stopwatch when paused
        }
        else if (_cookBookButton.isClicked)
        {
            _game.activeScene = Scenes.COOKBOOKSCREEN;
            _timer.Stop();
        }
        else
        {
            _timer.Start(); // Resume the stopwatch if not paused
        }

        _ogerCook.Update();
        _animationManager.Update();
        _inputManager.Update();
        _interactionManager.Update();
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

        // TransformationMatrix is automatically calculated into the draw call
        var transformMatrix = _camera.GetViewMatrix();

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix); // To make sharp images while scaling

        _tileManager.Draw(_spriteBatch, _tileSize, 8, _tileSize, _perspectiveManager);

        _perspectiveManager.draw(_spriteBatch, _animationManager);

        //_collisionManager.DrawDebugRect(_spriteBatch, _ogerCook.Rect, 1, rectangleTexture); // Drawing player rectangle, int value is thickness

        _spriteBatch.End();



        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_orderStrip, _orderStripRect, Color.White);

        _spriteBatch.Draw(_scordeBord, _scordeBordRect, Color.White);

        _spriteBatch.DrawString(bmfont, "Score: \n" + score, new Vector2(_screenWidth - 100, _pauseButton.Height - bmfont.LineHeight + 10), Color.White);

        _pauseButton.Draw(_spriteBatch);

        _cookBookButton.Draw(_spriteBatch);

        // Display the elapsed time
        string elapsedTime = _timer.Elapsed.ToString(@"mm\:ss");
        _spriteBatch.DrawString(bmfont, "Time: \n" + elapsedTime, new Vector2(_screenWidth - 100, _pauseButton.Height + bmfont.LineHeight + 10), Color.White);

        _spriteBatch.End();
    }
}
