using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Timers;

namespace SoftwareProjekt2024.Components.StaticObjects;

internal class BeerBarrel : StaticObject
{
    private static Timer _beerBarrelTimer;
    private static int count;
    public static bool interactedBarrel;
    public BeerBarrel(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
    : base(texture, position, _dest, _src, perspectiveManager)
    {
        interactedBarrel = false;

        if (_beerBarrelTimer != null)
        {
            _beerBarrelTimer.Close();
        }

        count = 0;
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    private static void Tick(object sender, ElapsedEventArgs e)
    {
        count++;
    }

    public static void HandleInteraction(Player _ogerCook, Vector2 positionWhilePickedUp)
    {
        if (!_ogerCook.inventoryIsEmpty() && _ogerCook.inventory[0] is Mug mug && !mug.isFilled)
        {
            Component item = _ogerCook.inventory[0];
            interactedBarrel = true;

            (item as Mug).fill();

            _beerBarrelTimer = new Timer(1000); //timer intervall is set to 1000ms -> meaning interval of tick is 1 second 
            _beerBarrelTimer.Elapsed += Tick; //ticks timer

            _beerBarrelTimer.Start();
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
        }
    }
}
