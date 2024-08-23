using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Diagnostics;
using System.Timers;

namespace SoftwareProjekt2024.Components.StaticObjects;

/* ToDo for Interaction: 
    - only make interaction possible, if you interact with the kessel if oger ha a chopped potato in hand 
    - add finished texture, after timer runs out -> dunno how right know, but have to change texture in draw call
            -> Problem: lets complete kessel vanish
    - after kessel is done: if you interact with it again, change kessel to empty kessel and dont start animation again (done -> siehe Interaction Manager)
            -> oger now carrys the finished cooked fries 
 */
public enum KesselStates
{
    EMPTYKESSEL,
    ANIMATIONKESSEL,
    DONEKESEL
}

internal class Kessel : StaticObject
{
    static AnimationManager _kesselAnimationManager;

    public static Texture2D _kesselTextureEmpty;
    public static Texture2D _kesselTextureAnimation;
    public static Texture2D _kesselTextureFull;

    private static Timer _kesselTimer;
    private static int count = 0;

    public static KesselStates _activeKesselState = KesselStates.EMPTYKESSEL;

    public Kessel(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        _kesselAnimationManager = new AnimationManager(3, 3, new Vector2(32, 64)); //Kessel animation has 3 frames in 3 colums, vector is size of one frame 
        _kesselAnimationManager.RowPos = 0; //only one row of animation 
        _kesselTimer = new Timer(1000); //timer intervall is set to 1000ms -> meaning interval of tick is 1 second 
        _kesselTimer.Elapsed += Tick; //ticks timer
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public static void HandleInteraction() //only relevant for Animation
    {
        _kesselTimer.Start();
    }

    // ups the counter every second  
    private static void Tick(object sender, ElapsedEventArgs e)
    {
        count++;
    }

    public static void Update()
    {
        _kesselAnimationManager.Update();

        if (count >= 10) //playes the animation 10 seconds 
        {
            _kesselTimer.Stop();
            _kesselAnimationManager.ResetAnimation();
            _activeKesselState = KesselStates.DONEKESEL;
            count = 0; //reset timer to 0, so that animation can start again with next interaction
        }
    }

    public override void draw(SpriteBatch spriteBatch)
    {
        switch (_activeKesselState)
        {
            case KesselStates.EMPTYKESSEL:
                spriteBatch.Draw(texture, dest, src, Color.White);
                break;

            case KesselStates.ANIMATIONKESSEL:
                spriteBatch.Draw(
                    _kesselTextureAnimation,                     //texture 
                    dest,                                       //destinationRectangle
                    _kesselAnimationManager.GetFrame(),        //sourceRectangle (frame) 
                    Color.White,                              //color
                    0f,                                      //rotation 
                    Vector2.Zero,                           //origin
                    SpriteEffects.None,                    //effects
                    1f);                                  //layer depth
                break;

            case KesselStates.DONEKESEL:
                //TODO: CHANGE TO RIGHT TEXTURE, ALSO FOR OGER -> has to carry finished fries 
                spriteBatch.Draw(texture, dest, src, Color.White);
                Debug.WriteLine("Hier sind die fertigen Pommes!");
                break;
        }
    }
}
