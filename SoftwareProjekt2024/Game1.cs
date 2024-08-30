using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
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
    COOKBOOKSCREEN,
    HELPSCREEN,
    CREDITSSCREEN
};

public class Game1 : Game

{
    public bool _exit = false;

    public bool fullScreen = false;

    public GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    readonly int screenWidth = 1280;
    readonly int screenHeight = 720;

    public static Scenes activeScene;

    private SplashScreen _splashScreen;
    private MainMenu _mainMenu;
    public static GamePlay _gamePlay;

    private PauseMenu _pauseMenu;
    private OptionMenuMain _optionMenuMain;
    private OptionMenuPause _optionMenuPause;
    private CookBookScreen _cookBookScreen;
    private CreditsScreen _creditsScreen;
    private HelpScreen _helpScreen;

    private Song _currentSong;
    private Song _introMenuSoundtrack;
    private Song _gameplaySoundtrackCozy;

    public static float VolumeLevel { get; set; } = 0.0f; // Shared volume level

    // global for AnimSounds, so signature doesnt have to be changed
    public static ContentManager ContentManager { get; private set; } 

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

        activeScene = Scenes.SPLASHSCREEN;
    }

    protected override void Initialize()
    {
        base.Initialize();
    }


    protected override void LoadContent()
    {
        ContentManager = Content;

        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _splashScreen = new SplashScreen(Content, screenWidth, screenHeight, this, _spriteBatch);
        _mainMenu = new MainMenu(Content, screenWidth, screenHeight, this, _spriteBatch);
        _pauseMenu = new PauseMenu(Content, screenWidth, screenHeight, this, _spriteBatch);
        _optionMenuMain = new OptionMenuMain(Content, screenWidth, screenHeight, this, _spriteBatch);
        _optionMenuPause = new OptionMenuPause(Content, screenWidth, screenHeight, this, _spriteBatch);
        _cookBookScreen = new CookBookScreen(Content, screenWidth, screenHeight, this, _spriteBatch);
        _creditsScreen = new CreditsScreen(Content, screenWidth, screenHeight, this, _spriteBatch);
        _helpScreen = new HelpScreen(Content, screenWidth, screenHeight, this, _spriteBatch);

        CreateGamePlay();

        _introMenuSoundtrack = Content.Load<Song>("Sounds/woodland_fantasy");
        _gameplaySoundtrackCozy = Content.Load<Song>("Sounds/inn_music");
    }

    public void CreateGamePlay()
    {
        _gamePlay = new GamePlay(Content, screenWidth, screenHeight, this, _spriteBatch);
        _gamePlay.LoadContent(Window, GraphicsDevice);
    }

    private void PlaySong(Song song)
    {
        // needed, so that the soundtrack works properly across different scenes
        // called in update, so updated every frame if check is not set
        if (_currentSong != song)
        {

            MediaPlayer.Stop(); // stop everything that played before
            MediaPlayer.IsRepeating = true; // toggle in order to loop (or not)
            MediaPlayer.Play(song);
            _currentSong = song;
        }
        MediaPlayer.Volume = VolumeLevel;
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
                PlaySong(_introMenuSoundtrack);
                break;
            case Scenes.MAINMENU:
                _mainMenu.Update();
                break;
            case Scenes.GAMEPLAY:
                _gamePlay.Update();
                PlaySong(_gameplaySoundtrackCozy);
                break;
            case Scenes.PAUSEMENU:
                _pauseMenu.Update();
                PlaySong(_gameplaySoundtrackCozy);
                break;
            case Scenes.OPTIONMENUMAIN:
                _optionMenuMain.Update();
                break;
            case Scenes.OPTIONMENUPAUSE:
                _optionMenuPause.Update();
                PlaySong(_gameplaySoundtrackCozy);
                break;
            case Scenes.COOKBOOKSCREEN:
                _cookBookScreen.Update();
                break;
            case Scenes.CREDITSSCREEN:
                _creditsScreen.Update();
                break;
            case Scenes.HELPSCREEN:
                _helpScreen.Update();
                break;
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        switch (activeScene)
        {
            case Scenes.SPLASHSCREEN:
                GraphicsDevice.Clear(Color.LightGreen);

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
                GraphicsDevice.Clear(Color.LightYellow);

                _cookBookScreen.Draw();
                break;
            case Scenes.CREDITSSCREEN:
                GraphicsDevice.Clear(Color.LightYellow);

                _creditsScreen.Draw();
                break;
            case Scenes.HELPSCREEN:
                GraphicsDevice.Clear(Color.LightBlue);

                _helpScreen.Draw();
                break;
        }

        base.Draw(gameTime);
    }

    public void Quit()
    {
        this.Exit();
    }
}
