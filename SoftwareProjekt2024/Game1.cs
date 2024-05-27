using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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

    int screenWidth = 720;
    int screenHeight = 480;

    public Scenes activeScene; 

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
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _mainMenu = new MainMenu(Content, screenWidth, screenHeight, Mouse.GetState());
        _gamePlay = new GamePlay(screenWidth, screenHeight, Mouse.GetState());
        _pauseMenu = new PauseMenu(Content, screenWidth, screenHeight, Mouse.GetState());
        _optionMenu = new OptionMenu(Content, screenWidth, screenHeight, Mouse.GetState());

        _gamePlay.LoadContent(Content);
    }

    protected override void Update(GameTime gameTime)
    {
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

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

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
