using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Screens;

namespace SoftwareProjekt2024;

public enum Scenes
{
    MAINMENU,
    GAMEPLAY,
    PAUSEMENU,
    OPTIONMENUMAIN,
    OPTIONMENUPAUSE
};

public class Game1 : Game

{
    public bool _exit = false;

    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    readonly int screenWidth = 1080;
    readonly int screenHeight = 720;

    public Scenes activeScene;

    private MainMenu _mainMenu;
    private GamePlay _gamePlay;
    private PauseMenu _pauseMenu;
    private OptionMenuMain _optionMenuMain;
    private OptionMenuPause _optionMenuPause;

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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _mainMenu = new MainMenu(Content, screenWidth, screenHeight, this, _spriteBatch);
        _gamePlay = new GamePlay(screenWidth, screenHeight);
        _pauseMenu = new PauseMenu(Content, screenWidth, screenHeight, this, _spriteBatch);
        _optionMenuMain = new OptionMenuMain(Content, screenWidth, screenHeight, this, _spriteBatch);
        _optionMenuPause = new OptionMenuPause(Content, screenWidth, screenHeight, this, _spriteBatch);

        _gamePlay.LoadContent(Content, this, Window, GraphicsDevice);
    }

    protected override void Update(GameTime gameTime)
    {
        if (_exit)
        {
            Exit();
        }

        switch (activeScene)
        {
            case Scenes.MAINMENU:
                _mainMenu.Update();
                break;
            case Scenes.GAMEPLAY:
                _gamePlay.Update(this, gameTime);
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

        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        switch (activeScene)
        {
            case Scenes.MAINMENU:
                GraphicsDevice.Clear(Color.LightBlue);

                _mainMenu.Draw();

                break;
            case Scenes.GAMEPLAY:
                GraphicsDevice.Clear(Color.Beige);

                _gamePlay.Draw(_spriteBatch);

                break;
            case Scenes.PAUSEMENU:
                GraphicsDevice.Clear(Color.LightPink);

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
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    public void Quit()
    {
        this.Exit();
    }
}

