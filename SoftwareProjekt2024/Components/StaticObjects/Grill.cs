using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Timers;

namespace SoftwareProjekt2024.Components.StaticObjects;

enum GrillStates
{
    EMPTYGRILL,
    ANIMATIONGRILL,
    DONEGRILL
}

internal class Grill : StaticObject
{

    public static List<Component> grillContents;
    static bool hasMeatOn;

    static AnimationManager _grillAnimationManager;

    public static Texture2D _grillTextureDone;
    public static Texture2D _grillTextureAnimation;

    public static GrillStates _activeGrillState;

    private static Timer _grillTimer;
    private static int count;

    public static SoundEffectInstance soundInstanceGrill;

    public Grill(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        grillContents = new List<Component>();
        _activeGrillState = GrillStates.EMPTYGRILL;
        hasMeatOn = false;

        _grillAnimationManager = new AnimationManager(3, 3, new Vector2(64, 96), 10);
        _grillAnimationManager.RowPos = 0;

        if (_grillTimer != null)
        {
            _grillTimer.Close();
        }
        count = 0;

        // Load the sound effect and create an instance
        var soundEffect = Game1.ContentManager.Load<SoundEffect>("Sounds/fire-crackling");
        if (soundInstanceGrill != null)
            soundInstanceGrill.Dispose();
        soundInstanceGrill = soundEffect.CreateInstance();
        soundInstanceGrill.IsLooped = false;
        UpdateVolume();
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }


    public static void HandleInteraction(Player _ogerCook, Vector2 positionWhilePickedUp, InteractionManager interactionManager, InputManager inputManager)
    {
        //interaction only possible when carrying raw meat
        if (!_ogerCook.inventoryIsEmpty() && _ogerCook.inventory[0] is Meat meat && !hasMeatOn && !meat.cooked)
        {
            interactionManager._interactionTextline = "Press [E] to roast meat";
            interactionManager._allowedInteraction = true;

            if (inputManager.pressedE)
            {
                Component item = _ogerCook.inventory[0];
                _ogerCook.inventory.Clear();
                _ogerCook.changeAppearence(1);

                grillContents.Add(item);
                (item as Meat).cook();

                hasMeatOn = true;

                _grillTimer = new Timer(1000);
                _grillTimer.Elapsed += Tick;


                _grillTimer.Start(); //starts timer for 10 seconds 
                _activeGrillState = GrillStates.ANIMATIONGRILL; //starts Animation 

                UpdateVolume(); // Update volume before playing
                soundInstanceGrill.Play();
            }
        }
        else if (_activeGrillState == GrillStates.ANIMATIONGRILL)
        {
            int seconds = 10;
            interactionManager._interactionTextline = "Wait " + (seconds - count) + " seconds until patty is done";
            interactionManager._allowedInteraction = true;
        }
        //only with nothing in hands, oger can interact with done grill and pick up done meat
        else if (_ogerCook.inventoryIsEmpty() && _activeGrillState == GrillStates.DONEGRILL)
        {
            interactionManager._interactionTextline = "Press [E] to grab patty";
            interactionManager._allowedInteraction = true;
            if (inputManager.pressedE)
            {
                _activeGrillState = GrillStates.EMPTYGRILL; //meat was picked up -> grill is empty again
                hasMeatOn = false;
                Component item = grillContents[0];
                grillContents.Clear();
                _ogerCook.pickUp(item);
                item.position = positionWhilePickedUp;

                soundInstanceGrill.Stop();
            }
        }
        else
        {
            interactionManager._allowedInteraction = false;
        }
    }

    public static void Update()
    {
        _grillAnimationManager.Update();

        if (count >= 10)
        {
            _grillTimer.Close();
            _grillAnimationManager.ResetAnimation();
            _activeGrillState = GrillStates.DONEGRILL;
            count = 0; //reset timer to 0, so that animation can start again with next interaction

            // Stop the sound effect if it's still playing
            if (soundInstanceGrill.State == SoundState.Playing)
            {
                soundInstanceGrill.Stop();
            }
        }
        UpdateVolume();
    }

    private static void UpdateVolume()
    {
        if (soundInstanceGrill != null)
        {
            soundInstanceGrill.Volume = Game1.VolumeLevel;
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
