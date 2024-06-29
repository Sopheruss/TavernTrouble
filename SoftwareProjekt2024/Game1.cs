using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Screens;

namespace SoftwareProjekt2024;

public enum Scenes
{
    SPLASHSCREEN,
    MAINMENU,
    GAMEPLAY,
    PAUSEMENU,
    OPTIONMENUMAIN,
    OPTIONMENUPAUSE,
    COOKBOOKSCREEN
};

public class Game1 : Game

{
    public bool _exit = false;

    public bool fullScreen = false;

    readonly private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    readonly int screenWidth = 1280;
    readonly int screenHeight = 720;

    public Scenes activeScene;

    private SplashScreen _splashScreen;
    private MainMenu _mainMenu;
    private GamePlay _gamePlay;
    private PauseMenu _pauseMenu;
    private OptionMenuMain _optionMenuMain;
    private OptionMenuPause _optionMenuPause;
    private CookBookScreen _cookBookScreen;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        this._graphics.PreferredBackBufferWidth = screenWidth;
        this._graphics.PreferredBackBufferHeight = screenHeight;

        if (fullScreen)
        {
            this._graphics.IsFullScreen = true;
        }
        else
        {
            this._graphics.IsFullScreen = false;
        }

        activeScene = Scenes.MAINMENU;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _splashScreen = new SplashScreen(Content, screenWidth, screenHeight, this, _spriteBatch);
        _mainMenu = new MainMenu(Content, screenWidth, screenHeight, this, _spriteBatch);
        _gamePlay = new GamePlay(Content, screenWidth, screenHeight, this, _spriteBatch);
        _pauseMenu = new PauseMenu(Content, screenWidth, screenHeight, this, _spriteBatch);
        _optionMenuMain = new OptionMenuMain(Content, screenWidth, screenHeight, this, _spriteBatch);
        _optionMenuPause = new OptionMenuPause(Content, screenWidth, screenHeight, this, _spriteBatch);
        _cookBookScreen = new CookBookScreen(Content, screenWidth, screenHeight, this, _spriteBatch);

        _gamePlay.LoadContent(Window, GraphicsDevice);
    }



    protected override void Update(GameTime gameTime)
    {
        if (_exit)
        {
            Exit();
        }

        switch (activeScene)
        {
            case Scenes.SPLASHSCREEN:
                _splashScreen.Update();
                break;
            case Scenes.MAINMENU:
                _mainMenu.Update();
                break;
            case Scenes.GAMEPLAY:
                _gamePlay.Update();
                break;
            case Scenes.PAUSEMENU:
                _pauseMenu.Update();
                break;
            case Scenes.OPTIONMENUMAIN:
                _optionMenuMain.Update();
                break;
            case Scenes.OPTIONMENUPAUSE:
                _optionMenuPause.Update();
                break;
            case Scenes.COOKBOOKSCREEN:
                _cookBookScreen.Update();
                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        switch (activeScene)
        {
            case Scenes.SPLASHSCREEN:
                GraphicsDevice.Clear(Color.AliceBlue);

                _splashScreen.Draw();
                break;
            case Scenes.MAINMENU:
                _mainMenu.Draw();
                break;
            case Scenes.GAMEPLAY:
                GraphicsDevice.Clear(Color.Black);

                _gamePlay.Draw();
                break;
            case Scenes.PAUSEMENU:
                GraphicsDevice.Clear(Color.LightGreen);

                _pauseMenu.Draw();
                break;
            case Scenes.OPTIONMENUMAIN:
                GraphicsDevice.Clear(Color.LightGreen);

                _optionMenuMain.Draw();
                break;
            case Scenes.OPTIONMENUPAUSE:
                GraphicsDevice.Clear(Color.LightBlue);

                _optionMenuPause.Draw();
                break;

            case Scenes.COOKBOOKSCREEN:
                GraphicsDevice.Clear(Color.Beige);

                _cookBookScreen.Draw();
                break;
        }

        base.Draw(gameTime);
    }

    public void Quit()
    {
        this.Exit();
    }
}

