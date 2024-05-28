﻿using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Screens;

namespace SoftwareProjekt2024;

public enum Scenes
{ 
    MAINMENU,
    GAMEPLAY,
    PAUSEMENU, 
    OPTIONMENU
};

public class Game1 : Game

{
    public bool _exit = false;


    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch; 

    int screenWidth = 1080;
    int screenHeight = 720;

    public Scenes activeScene;  
    
    Player _ogerCook;


    AnimationManager _animationManager;
    TileManager _tileManager;
    CameraManager _cameraManager;
    CollisionManager _collisionManager;
    PerspectiveManager _perspectiveManager;
    InputManager _inputManager;


    private MainMenu _mainMenu;
    private GamePlay _gamePlay;
    private PauseMenu _pauseMenu;
    private OptionMenu _optionMenu;


    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        this._graphics.PreferredBackBufferWidth = screenWidth;
        this._graphics.PreferredBackBufferHeight = screenHeight;

        //this._graphics.IsFullScreen = true;

        activeScene = Scenes.MAINMENU;
    }

    protected override void Initialize()
    {


        //calc for middle of screen + hack to spawn into middle of first iteration of map (TEMPORARY)
        midScreenWidth = _graphics.PreferredBackBufferWidth / 2; // higer val => right
        midScreenHeight = _graphics.PreferredBackBufferHeight / 2; // lower val => up


        _cameraManager = new CameraManager(Window, GraphicsDevice, screenWidth, screenHeight);
        _perspectiveManager = new PerspectiveManager();



        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _mainMenu = new MainMenu(Content, screenWidth, screenHeight, Mouse.GetState());
        _gamePlay = new GamePlay(screenWidth, screenHeight, Mouse.GetState());
        _pauseMenu = new PauseMenu(Content, screenWidth, screenHeight, Mouse.GetState());
        _optionMenu = new OptionMenu(Content, screenWidth, screenHeight, Mouse.GetState());


        //local implementation, cuz acces to texture via Sprite class 
        Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");
        
        _ogerCook = new Player(_ogerCookSpritesheet,
                              new Vector2(midScreenWidth, midScreenHeight), _perspectiveManager); //oger Position 


        _tileManager = new TileManager(Content, GraphicsDevice);

        //Debug.WriteLine("Pos Oger X: " + ogerCook.position.X);
        //Debug.WriteLine("Pos Oger Y: " + ogerCook.position.Y);
        
        _collisionManager = new CollisionManager(_tileManager._tiledMap, "collisionlayer", _ogerCook.position.X, _ogerCook.position.Y);
        _inputManager = new InputManager(this, _ogerCook, _collisionManager, _animationManager);

        _gamePlay.LoadContent(Content);

    }

    protected override void Update(GameTime gameTime)
    {


        _animationManager.Update();
        _tileManager.Update(gameTime);
        _cameraManager.Update(gameTime, _ogerCook.position);
        _ogerCook.Update();
        _inputManager.Update();

        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Quit();

        if (_exit)
        {
            Exit();
        }

        switch (activeScene)
        {
            case Scenes.MAINMENU:
                _mainMenu.Update(this);
                break;
            case Scenes.GAMEPLAY:
                _gamePlay.Update(this);
                break;
            case Scenes.PAUSEMENU:
                _pauseMenu.Update(this);
                break;
            case Scenes.OPTIONMENU:
                _optionMenu.Update(this);
                break;
        }


        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {


        GraphicsDevice.Clear(Color.Black);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _tileManager.Draw(_cameraManager.GetViewMatrix());

        _perspectiveManager.draw(_spriteBatch, _animationManager);
        
        switch (activeScene)
        {
            case Scenes.MAINMENU:

                GraphicsDevice.Clear(Color.LightBlue);
                _mainMenu.Draw(_spriteBatch);

                break;
            case Scenes.GAMEPLAY:
                GraphicsDevice.Clear(Color.Beige);

                _gamePlay.Draw(_spriteBatch);

                break;
            case Scenes.PAUSEMENU:
                GraphicsDevice.Clear(Color.LightPink);

                _pauseMenu.Draw(_spriteBatch);

                break;
            case Scenes.OPTIONMENU:
                GraphicsDevice.Clear(Color.LightGreen);

                _optionMenu.Draw(_spriteBatch);

                break;
        }
        
        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void Quit()
    {
        this.Exit();
    }
}

