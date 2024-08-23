using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Timers;

namespace SoftwareProjekt2024.Components.StaticObjects;
/* ToDo for Interaction: 
    - only make interaction possible, if you interact with the grill if oger has raw meat in hand 
    - add finished texture, after timer runs out -> dunno how right know, but have to change texture in draw call
            -> Problem: lets complete kessel vanish
    - after grill is done: if you interact with it again, change kessel to empty kessel and dont start animation again
 */
enum GrillStates
{
    EMPTYGRILL,
    ANIMATIONGRILL,
    DONEGRILL
}
internal class Grill : StaticObject
{
    static AnimationManager _grillAnimationManager;

    public static Texture2D _grillTextureFull;
    public static Texture2D _grillTextureEmpty;
    public static Texture2D _grillTextureAnimation;

    public static bool _playGrillAnimation = false;

    private static Timer _grillTimer;
    private static int count = 0;

    public Grill(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        _grillAnimationManager = new AnimationManager(3, 3, new Vector2(64, 96));
        _grillAnimationManager.RowPos = 0;

        _grillTimer = new Timer(1000);
        _grillTimer.Elapsed += Tick;
    }
    public static void HandleInteraction()
    {
        /* what should happen:
            - interaction with meat in grill
            - start Animation 
            - plays Animation for 10 seconds (maybe more?)
            - stops Animation and has texture of grill done
            - interaction with done grill makes it empty again 
        */

        _playGrillAnimation = true;
        _grillTimer.Start();
    }

    public static void Update()
    {
        _grillAnimationManager.Update();

        if (count >= 10)
        {
            _grillTimer.Stop();
            _playGrillAnimation = false;
            _grillAnimationManager.ResetAnimation();
            count = 0; //reset timer to 0, so that animation can start again with next interaction
        }
    }

    // ups the counter every second  
    private static void Tick(object sender, ElapsedEventArgs e)
    {
        count++;
    }

    public override void draw(SpriteBatch spriteBatch)
    {
        if (_playGrillAnimation == false) //empty grill
        {
            spriteBatch.Draw(texture, dest, src, Color.White);
        }
        else //draw call for animation 
        {
            spriteBatch.Draw(
            _grillTextureAnimation,                     //texture 
            dest,                                       //destinationRectangle
            _grillAnimationManager.GetFrame(),        //sourceRectangle (frame) 
            Color.White,                              //color
            0f,                                      //rotation 
            Vector2.Zero,                           //origin
            SpriteEffects.None,                    //effects
            1f);                                  //layer depth
        }
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }
}
