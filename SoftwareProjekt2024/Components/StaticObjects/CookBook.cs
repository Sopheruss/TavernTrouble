using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Screens;
using System.Timers;

namespace SoftwareProjekt2024.Components.StaticObjects;

//Animation is only played the first time, the book is opened, otherwise time waste

internal class CookBook : StaticObject
{
    static AnimationManager _cookBookAnimationManager;
    public static Texture2D _cookBookAnimation;
    public static Texture2D _cookBookClose;

    public static bool _playCookBookAnimation = false;

    private static Timer _cookBookTimer;
    private static int count = 0;

    public CookBook(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        _cookBookAnimationManager = new(4, 4, new Vector2(32, 64), 10);
        _cookBookAnimationManager.RowPos = 0;

        _cookBookTimer = new Timer(650); //timer intervall is set to 1000ms -> meaning interval of tick is 1 second 
        _cookBookTimer.Elapsed += Tick; //ticks timer
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    // ups the counter every second  
    private static void Tick(object sender, ElapsedEventArgs e)
    {
        count++;
    }

    public static void HandleInteraction(InteractionManager interactionManager, InputManager inputManager)
    {
        interactionManager._interactionTextline = "Press [E] to interact with cookbook";
        if (inputManager.pressedE)
        {
            _playCookBookAnimation = true;
            _cookBookTimer.Start();
        }
    }

    public static void Update()
    {
        _cookBookAnimationManager.Update();

        //Debug.WriteLine("activeFrame:" + _cookBookAnimationManager.activeFrame);
        //Debug.WriteLine("numFrame:" + _cookBookAnimationManager.numFrames);

        if (count >= 1) //Animation plays for 0.65 sec
        {
            _playCookBookAnimation = false;
            _cookBookAnimationManager.ResetAnimation();
            count = 0;
            Game1.activeScene = Scenes.COOKBOOKSCREEN;
            GamePlay._timer.Stop();
        }
    }

    public override void draw(SpriteBatch spriteBatch)
    {
        if (_playCookBookAnimation == false)
        {
            spriteBatch.Draw(texture, dest, src, Color.White);
        }
        else
        {
            spriteBatch.Draw(
            _cookBookAnimation,                          //texture 
            dest,                                       //destinationRectangle
            _cookBookAnimationManager.GetFrame(),      //sourceRectangle (frame) 
            Color.White,                              //color
            0f,                                      //rotation 
            Vector2.Zero,                           //origin
            SpriteEffects.None,                    //effects
            1f);                                  //layer depth
        }
    }
}
