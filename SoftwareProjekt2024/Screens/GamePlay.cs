using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Screens;

internal class GamePlay
{

    Button _pauseButton;

    PerspectiveManager _perspectiveManager;
    AnimationManager _animationManager;
    TileManager _tileManager;
    CollisionManager _collisionManager;
    InteractionManager _interactionManager;
    InputManager _inputManager;

    //it is possible to initialize a List of Sprites!!!
    Player _ogerCook;
    Player testDummy;

    readonly int _screenWidth;
    readonly int _screenHeight;

    Texture2D rectangleTexture;

    public GamePlay(int screenWidth, int screenHeight)
    {
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;
    }

    public void LoadContent(ContentManager Content, Game1 game, GameWindow window, GraphicsDevice graphicsDevice)
    {

        _perspectiveManager = new PerspectiveManager();

        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image
        _animationManager = new(4, 4, new Vector2(19, 32));

        //local implementation, cuz acces to texture via Sprite class
        Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");

        _ogerCook = new Player(_ogerCookSpritesheet,
                              new Vector2(_screenWidth / 2, _screenHeight / 2), //oger Position
                              _perspectiveManager);

        testDummy = new Player(_ogerCookSpritesheet, new Vector2(_screenWidth / 2, _screenHeight / 2), _perspectiveManager);


        Texture2D _pauseButtonTexture = Content.Load<Texture2D>("Buttons/pauseButton");
        _pauseButton = new Button(_pauseButtonTexture, new Vector2(30, 30));

        _tileManager = new TileManager();
        _tileManager.textureAtlas = Content.Load<Texture2D>("atlas");
        _tileManager.hitboxes = Content.Load<Texture2D>("hitboxes");

        _collisionManager = new CollisionManager(_tileManager);
        _interactionManager = new InteractionManager(_tileManager);
        _inputManager = new InputManager(game, _ogerCook, _collisionManager, _interactionManager, _animationManager);

        rectangleTexture = new Texture2D(graphicsDevice, 1, 1);         // for player rectangle
        rectangleTexture.SetData(new Color[] { new(255, 0, 0, 255) });  // ''
    }

    public void Update(Game1 game, GameTime gameTime)
    {
        _pauseButton.Update();

        if (_pauseButton.isClicked || _inputManager._escIsPressed)
        {
            game.activeScene = Scenes.PAUSEMENU;
        }

        _ogerCook.Update();
        _animationManager.Update();
        _inputManager.Update();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _pauseButton.Draw(spriteBatch);

        spriteBatch.Draw(_ogerCook.texture,               //texture
                         _ogerCook.Rect,                  //destinationRectangle
                         _animationManager.GetFrame(),   //sourceRectangle (frame)
                         Color.White,                    //color
                         0f,                             //rotation
                         new Vector2(_ogerCook.texture.Width / 4, //origin -> to place center texture correctly
                                     _ogerCook.texture.Width / 4),
                         SpriteEffects.None,             //effects
                         0f);                            //layer depth

        _tileManager.Draw(spriteBatch, 32, 8, 32, _perspectiveManager);

        _perspectiveManager.draw(spriteBatch, _animationManager);

        _collisionManager.DrawDebugRect(spriteBatch, _ogerCook.Rect, 1, rectangleTexture); // drawing player rectangle, int value is thickness
    }
}
