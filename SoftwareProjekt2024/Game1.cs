using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024;

public class Game1 : Game

{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    Player ogerCook;

    int screenWidth = 720;
    int screenHeight = 480;

    int midScreenWidth;
    int midScreenHeight;

    AnimationManager _animationManager;
    TileManager _tileManager;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        this._graphics.PreferredBackBufferWidth = screenWidth;
        this._graphics.PreferredBackBufferHeight = screenHeight;

        //this._graphics.IsFullScreen = true;
    }

    protected override void Initialize()
    {
        //to make using calc for middle of Screen shorter 
        midScreenWidth = _graphics.PreferredBackBufferWidth / 2;
        midScreenHeight = _graphics.PreferredBackBufferHeight / 2;

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image 
        _animationManager = new(4, 4, new Vector2(19, 32));



        //local implementation, cuz acces to texture via Sprite class 
        Texture2D _ogerCookSpritesheet = Content.Load<Texture2D>("Models/oger_cook_spritesheet");
        ogerCook = new Player(_ogerCookSpritesheet, new Vector2(midScreenWidth, midScreenHeight), _animationManager); //oger Position 

        _tileManager = new TileManager();
        _tileManager.textureAtlas = Content.Load<Texture2D>("atlas");
        _tileManager.hitboxes = Content.Load<Texture2D>("hitboxes");
    }

    protected override void Update(GameTime gameTime)
    {
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        ogerCook.Update();
        _animationManager.Update();

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {

        GraphicsDevice.Clear(Color.Beige);

        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); //to make sharp images while scaling 

        _spriteBatch.Draw(
            ogerCook.texture,               //texture 
            ogerCook.Rect,                  //destinationRectangle
            _animationManager.GetFrame(),   //sourceRectangle (frame) 
            Color.White,                    //color
            0f,                             //rotation 
            new Vector2(                    //origin -> to place center texture correctly
                ogerCook.texture.Width / 4,
                ogerCook.texture.Width / 4),
            SpriteEffects.None,             //effects
            0f);                            //layer depth



        int display_tilesize = 32;
        int num_tiles_per_row = 8;
        int pixel_tilesize = 32;

        foreach (var item in _tileManager.groundworkLayer)
        {
            Rectangle dest = new(
                (int)item.Key.X * display_tilesize,
                (int)item.Key.Y * display_tilesize,
                display_tilesize, display_tilesize); // change to greater value e.g. 64 to create gaps between tiles

            int x = item.Value % num_tiles_per_row;
            int y = item.Value / num_tiles_per_row;

            Rectangle src = new(

                x * pixel_tilesize,
                y * pixel_tilesize,
                pixel_tilesize,
                pixel_tilesize);


            _spriteBatch.Draw(_tileManager.textureAtlas, dest, src, Color.White);


        }
        foreach (var item in _tileManager.objectsLayer)
        {
            Rectangle dest = new(
                (int)item.Key.X * display_tilesize,
                (int)item.Key.Y * display_tilesize,
                display_tilesize, display_tilesize);

            int x = item.Value % num_tiles_per_row;
            int y = item.Value / num_tiles_per_row;

            Rectangle src = new(

                x * pixel_tilesize,
                y * pixel_tilesize,
                pixel_tilesize,
                pixel_tilesize);


            _spriteBatch.Draw(_tileManager.textureAtlas, dest, src, Color.White);


        }

        foreach (var item in _tileManager.collisionLayer) // to visualize hitboxes
        {
            Rectangle dest = new(
                (int)item.Key.X * display_tilesize,
                (int)item.Key.Y * display_tilesize,
                display_tilesize, display_tilesize);

            int x = item.Value % 1;
            int y = item.Value / 1;

            Rectangle src = new(

                x * pixel_tilesize,
                y * pixel_tilesize,
                pixel_tilesize,
                pixel_tilesize);


            _spriteBatch.Draw(_tileManager.hitboxes, dest, src, Color.White);


        }

        _spriteBatch.End();

        base.Draw(gameTime);

    }
}
