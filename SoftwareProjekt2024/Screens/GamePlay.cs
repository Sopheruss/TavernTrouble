﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Screens;

internal class GamePlay
{
    readonly SpriteBatch _spriteBatch;

    //camera stuff; using Monogame Extended Camera 
    private OrthographicCamera _camera;

    Button _pauseButton;

    PerspectiveManager _perspectiveManager;
    AnimationManager _animationManager;
    TileManager _tileManager;
    CollisionManager _collisionManager;
    InteractionManager _interactionManager;
    InputManager _inputManager;


    Player _ogerCook;

    readonly int _screenWidth;
    readonly int _screenHeight;

    Texture2D rectangleTexture;

    public GamePlay(int screenWidth, int screenHeight, SpriteBatch spriteBatch)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;

        _spriteBatch = spriteBatch;
    }

    public void LoadContent(ContentManager Content, Game1 game, GameWindow window, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
    {
        var viewportAdapter = new BoxingViewportAdapter(window, graphicsDevice, 426, 240); //sets the size of the viewport window 
        _camera = new OrthographicCamera(viewportAdapter);


        _perspectiveManager = new PerspectiveManager();

        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image
        _animationManager = new(4, 4, new Vector2(19, 32));

        //local implementation, cuz acces to texture via Sprite class
        Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");

        _ogerCook = new Player(_ogerCookSpritesheet,
                              new Vector2(250, 200), //PLS ÄNDERN, dafür MapSize 
                              _perspectiveManager);

        _pauseButton = new Button(
            Content.Load<Texture2D>("Buttons/pauseButton"),
            Content.Load<Texture2D>("Buttons/pauseButtonHovering"),
            new Vector2(30, 30));

        _tileManager = new TileManager();
        //_tileManager.textureAtlas = Content.Load<Texture2D>("atlas");
        _tileManager.textureAtlas = Content.Load<Texture2D>("atlasSophie");
        _tileManager.hitboxes = Content.Load<Texture2D>("hitboxes");
        _tileManager.LoadObjectlayer(spriteBatch, 32, 8, 32, _perspectiveManager); //Laden aller Objekte von Tiled

        _collisionManager = new CollisionManager(_tileManager);
        _interactionManager = new InteractionManager(_tileManager);
        _inputManager = new InputManager(game, _ogerCook, _collisionManager, _interactionManager, _animationManager);

        rectangleTexture = new Texture2D(graphicsDevice, 1, 1);         // for player rectangle
        rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });  // ''
    }

    public void Update(Game1 game, GameTime gameTime)
    {
        //uses the move function of monogame extended
        //convert to key -> from input manager, tried to tie the movement to the movement of the oger; doesnt quite work as planed 
        //_camera.Move(_inputManager.ConvertKeyToVector() * movementSpeed * gameTime.GetElapsedSeconds());

        _camera.LookAt(_ogerCook.position + new Vector2(10, 16)); //offset to center oger -> half of the texture width/height

        _pauseButton.Update();

        if (_pauseButton.isClicked || game._escIsPressed)
        {
            game.activeScene = Scenes.PAUSEMENU;
        }

        _ogerCook.Update();
        _animationManager.Update();
        _inputManager.Update();

    }

    public void Draw()
    {
        //two spriteBatch.Begin/End to seperate stuff that is affected by camera and static stuff

        //transformationMatrix is automatically calculated into the draw call 
        var transformMatrix = _camera.GetViewMatrix();

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: transformMatrix); //to make sharp images while scaling 

        _tileManager.Draw(_spriteBatch, 32, 8, 32, _perspectiveManager);

        _perspectiveManager.draw(_spriteBatch, _animationManager);

        _collisionManager.DrawDebugRect(_spriteBatch, _ogerCook.Rect, 1, rectangleTexture); // drawing player rectangle, int value is thickness

        _spriteBatch.End();



        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _pauseButton.Draw(_spriteBatch);

        _spriteBatch.End();
    }
}
