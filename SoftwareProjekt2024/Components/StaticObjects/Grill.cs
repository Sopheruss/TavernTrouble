using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;
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

    public static List<Component> grillContents;

    static AnimationManager _grillAnimationManager;

    public static Texture2D _grillTextureDone;
    public static Texture2D _grillTextureAnimation;

    public static GrillStates _activeGrillState = GrillStates.EMPTYGRILL;

    private static Timer _grillTimer;
    private static int count = 0;

    public Grill(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        grillContents = new List<Component>();

        _grillAnimationManager = new AnimationManager(3, 3, new Vector2(64, 96));
        _grillAnimationManager.RowPos = 0;

        _grillTimer = new Timer(1000);
        _grillTimer.Elapsed += Tick;

    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public static void HandleInteraction(Player _ogerCook, Vector2 positionWhilePickedUp)
    {
        //interaction only possible when carrying raw meat
        if (!_ogerCook.inventoryIsEmpty() && _ogerCook.inventory[0] is Meat)
        {
            Component item = _ogerCook.inventory[0];
            _ogerCook.inventory.Clear();
            _ogerCook.changeAppearence(1);

            grillContents.Add(item);
            (item as Meat).cook();

            _grillTimer.Start(); //starts timer for 10 seconds 
            _activeGrillState = GrillStates.ANIMATIONGRILL; //starts Animation 
        }

        //only with nothing in hands, oger can interact with done grill and pick up done meat
        if (_ogerCook.inventoryIsEmpty() && _activeGrillState == GrillStates.DONEGRILL)
        {
            Debug.WriteLine("Meat picked up!");
            _activeGrillState = GrillStates.EMPTYGRILL; //meat was picked up -> grill is empty again

            Component item = grillContents[0];
            grillContents.Clear();
            _ogerCook.pickUp(item);
            item.position = positionWhilePickedUp;
        }

    }

    public static void Update()
    {
        _grillAnimationManager.Update();

        if (count >= 10)
        {
            _grillTimer.Stop();
            _grillAnimationManager.ResetAnimation();
            _activeGrillState = GrillStates.DONEGRILL;
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
        switch (_activeGrillState)
        {
            case GrillStates.EMPTYGRILL:
                spriteBatch.Draw(texture, dest, src, Color.White);
                break;

            case GrillStates.ANIMATIONGRILL: //call for AnimationManager 
                spriteBatch.Draw(
                        _grillTextureAnimation,                      //texture 
                        dest,                                       //destinationRectangle
                        _grillAnimationManager.GetFrame(),         //sourceRectangle (frame) 
                        Color.White,                              //color
                        0f,                                      //rotation 
                        Vector2.Zero,                           //origin
                        SpriteEffects.None,                    //effects
                        1f);                                  //layer depth
                break;

            case GrillStates.DONEGRILL:
                spriteBatch.Draw(_grillTextureDone, dest, new Rectangle(0, 0, _grillTextureDone.Width, _grillTextureDone.Height), Color.White);
                break;
        }
    }

}
