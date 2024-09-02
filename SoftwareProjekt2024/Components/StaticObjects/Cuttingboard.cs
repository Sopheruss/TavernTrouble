using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Components.StaticObjects;

enum CuttingBoardStates
{
    EMPTYCUTTINGBOARD,
    POTATO,
    POTATODONE,
    SALAD,
    SALADDONE
}

internal class Cuttingboard : StaticObject
{
    public static List<Component> cBContents;
    static bool hasItemOn;

    public static Texture2D _salad;
    public static Texture2D _saladChopped;
    public static Texture2D _potato;
    public static Texture2D _potatoChopped;

    public CuttingBoardStates _activeCBState;

    //private static Timer _cBTimer;
    private static int count;

    public static SoundEffectInstance soundInstanceGrill;
    public Cuttingboard(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
    : base(texture, position, _dest, _src, perspectiveManager)
    {
        cBContents = new List<Component>();
        _activeCBState = CuttingBoardStates.EMPTYCUTTINGBOARD;
        hasItemOn = false;

        count = -1;
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public void HandleInteraction(Player _ogerCook, Vector2 positionWhilePickedUp)
    {
        if (_ogerCook.inventoryIsEmpty()) { count++; }

        if (!_ogerCook.inventoryIsEmpty() && !hasItemOn) //Inventory has to have item and cb need to be empty 
        {
            if (_ogerCook.inventory[0] is Potato potato && !potato.chopped) //item in inventory must be potato and potato need to be chopped
            {

                Component item = _ogerCook.inventory[0];
                _ogerCook.inventory.Clear();
                _ogerCook.changeAppearence(1);

                cBContents.Add(item);

                (item as Potato).chop();

                hasItemOn = true;

                _activeCBState = CuttingBoardStates.POTATO;

            }
            else if (_ogerCook.inventory[0] is Salad salad && !salad.chopped) //item in inventory must be salad and salad need to be chopped
            {

                Component item = _ogerCook.inventory[0];
                _ogerCook.inventory.Clear();
                _ogerCook.changeAppearence(1);

                cBContents.Add(item);

                (item as Salad).chop();

                hasItemOn = true;

                _activeCBState = CuttingBoardStates.SALAD;
            }
        }

        //for picking up the finished chopping action
        if (_ogerCook.inventoryIsEmpty() && //oger inventory has to be empty 
            (_activeCBState == CuttingBoardStates.SALADDONE || _activeCBState == CuttingBoardStates.POTATODONE)) //salad or potato need to be finished with chopping 
        {
            _activeCBState = CuttingBoardStates.EMPTYCUTTINGBOARD; //reset cuttingboard 
            hasItemOn = false;

            Component item = cBContents[0]; //oger inventory is filled with schopped item from board 
            cBContents.Clear();
            _ogerCook.pickUp(item);
            item.position = positionWhilePickedUp;
        }
    }

    public void Update()
    {

        if (count >= 5)
        {
            if (_activeCBState == CuttingBoardStates.POTATO) //setting right state for texture 
            {
                _activeCBState = CuttingBoardStates.POTATODONE;
            }
            else if (_activeCBState == CuttingBoardStates.SALAD)
            {
                _activeCBState = CuttingBoardStates.SALADDONE;
            }

            count = 0;
        }
    }

    public override void draw(SpriteBatch _spriteBatch)
    {
        switch (_activeCBState)
        {
            case CuttingBoardStates.EMPTYCUTTINGBOARD:
                _spriteBatch.Draw(texture, dest, src, Color.White);
                break;
            case CuttingBoardStates.POTATO:
                _spriteBatch.Draw(_potato, dest, new Rectangle(0, 0, _potato.Width, _potato.Height), Color.White);
                break;
            case CuttingBoardStates.POTATODONE:
                _spriteBatch.Draw(_potatoChopped, dest, new Rectangle(0, 0, _potatoChopped.Width, _potato.Height), Color.White);
                break;
            case CuttingBoardStates.SALAD:
                _spriteBatch.Draw(_salad, dest, new Rectangle(0, 0, _salad.Width, _potato.Height), Color.White);
                break;
            case CuttingBoardStates.SALADDONE:
                _spriteBatch.Draw(_saladChopped, dest, new Rectangle(0, 0, _saladChopped.Width, _potato.Height), Color.White);
                break;
        }
    }
}
