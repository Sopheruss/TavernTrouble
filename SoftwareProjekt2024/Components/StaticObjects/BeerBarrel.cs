using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;
using System.Diagnostics.Metrics;
using System.Timers;

namespace SoftwareProjekt2024.Components.StaticObjects;


internal class BeerBarrel : StaticObject
{
    private static Timer _beerBarrelTimer;
    private static int count;
    public static bool interactedBarrel;
    public static SoundEffectInstance soundInstanceBeer;
    public BeerBarrel(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
    : base(texture, position, _dest, _src, perspectiveManager)
    {
        interactedBarrel = false;

        if (_beerBarrelTimer != null)
        {
            _beerBarrelTimer.Close();
        }
        
        count = 0;

        // Load the sound effect and create an instance
        var soundEffect = Game1.ContentManager.Load<SoundEffect>("Sounds/pour-beer");
        if (soundInstanceBeer != null)
            soundInstanceBeer.Dispose();
        soundInstanceBeer = soundEffect.CreateInstance();
        soundInstanceBeer.IsLooped = false;
        UpdateVolume();
    }

        public static bool AllowedInteraction(Player ogerCook)
        {
            if (!ogerCook.inventoryIsEmpty() && ogerCook.inventory[0] is Mug && (ogerCook.inventory[0] as Mug).isFilled)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        
    public override int getHeight()
    {
        return dest.Height - 10;
    }

    private static void Tick(object sender, ElapsedEventArgs e)
    {
        count++;
    }

    public static void HandleInteraction(Player _ogerCook, Vector2 positionWhilePickedUp, InteractionManager interactionManager, InputManager inputManager)
    {
        if (!_ogerCook.inventoryIsEmpty() && _ogerCook.inventory[0] is Mug mug && !mug.isFilled)
        {
            interactionManager._interactionTextline = "Press [E] to interact with beer barrel";
            interactionManager._allowedInteraction = true;
            if (inputManager.pressedE)
            {
                Component item = _ogerCook.inventory[0];
                interactedBarrel = true;

                (item as Mug).fill();

                _beerBarrelTimer = new Timer(1000); //timer intervall is set to 1000ms -> meaning interval of tick is 1 second 
                _beerBarrelTimer.Elapsed += Tick; //ticks timer

                _beerBarrelTimer.Start();
                UpdateVolume();
                soundInstanceBeer.Play();
            }
        }
        else if(interactedBarrel && count < 3)
        {
            int seconds = 3;
            interactionManager._interactionTextline = "Wait " + (seconds - count) + " seconds until tankard is full";
            interactionManager._allowedInteraction = true;
        }
        else
        {
            interactionManager._allowedInteraction = false;
        }
    }

    public static void Update(Player _ogerCook)
    {
        if (count >= 3)
        {
            _beerBarrelTimer.Close(); //to fucking dispose the timer, not with dispose apparently 
            count = 0;
            _ogerCook.changeAppearence((int)States.BeerFull);
            interactedBarrel = false;

         // Stop the sound effect if it's still playing
            if (soundInstanceBeer.State == SoundState.Playing)
            {
                soundInstanceBeer.Stop();
            }
        }
        UpdateVolume();
    }

    private static void UpdateVolume()
    {
        if (soundInstanceBeer != null)
        {
            soundInstanceBeer.Volume = Game1.VolumeLevel;
        }
    }


}
