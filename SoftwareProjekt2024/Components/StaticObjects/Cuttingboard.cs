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
    public List<Component> cBContents;
    bool hasItemOn;

    public static Texture2D _salad;
    public static Texture2D _saladChopped;
    public static Texture2D _potato;
    public static Texture2D _potatoChopped;

    public CuttingBoardStates _activeCBState;

    private int count;

    public SoundEffectInstance soundInstanceGrill;
    public Cuttingboard(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
    : base(texture, position, _dest, _src, perspectiveManager)
    {
        cBContents = new List<Component>();
        _activeCBState = CuttingBoardStates.EMPTYCUTTINGBOARD;
        hasItemOn = false;

        count = 0;
    }

    public override int getHeight()
    {
        return dest.Height - 10;
    }

    public void HandleInteraction(Player _ogerCook, Vector2 positionWhilePickedUp, InteractionManager interactionManager, InputManager inputManager)
    {
        if (_ogerCook.inventoryIsEmpty() && hasItemOn && (_activeCBState == CuttingBoardStates.POTATO || _activeCBState == CuttingBoardStates.SALAD)) 
        {
            int times = 5;
            interactionManager._interactionTextline = "Press [E] " + (times - count) + " more times until ingredient is chopped";
            interactionManager._allowedInteraction = true;
            if (inputManager.pressedE)
            {
                count++;
            }
        }
        else if (!_ogerCook.inventoryIsEmpty() && !hasItemOn) //Inventory has to have item and cb need to be empty 
        {
            if (_ogerCook.inventory[0] is Potato potato && !potato.chopped) //item in inventory must be potato and potato need to be chopped
            {
                interactionManager._interactionTextline = "Press [E] to put ingredient on cutting board";
                interactionManager._allowedInteraction = true;
                if (inputManager.pressedE)
                {
                    Component item = _ogerCook.inventory[0];
                    _ogerCook.inventory.Clear();
                    _ogerCook.changeAppearence(1);

                    cBContents.Add(item);

                    (item as Potato).chop();

                    hasItemOn = true;

                    _activeCBState = CuttingBoardStates.POTATO;
                }
            }
            else if (_ogerCook.inventory[0] is Salad salad && !salad.chopped) //item in inventory must be salad and salad need to be chopped
            {
                if (inputManager.pressedE)
                {
                    interactionManager._interactionTextline = "Press [E] to put ingredient on cutting board";
                    interactionManager._allowedInteraction = true;
                    Component item = _ogerCook.inventory[0];
                    _ogerCook.inventory.Clear();
                    _ogerCook.changeAppearence(1);

                    cBContents.Add(item);

                    (item as Salad).chop();

                    hasItemOn = true;

                    _activeCBState = CuttingBoardStates.SALAD;
                }
            }
            else
            {
                interactionManager._allowedInteraction = false;
            }
        }
        //for picking up the finished chopping action
        else if (_ogerCook.inventoryIsEmpty() && //oger inventory has to be empty 
            (_activeCBState == CuttingBoardStates.SALADDONE || _activeCBState == CuttingBoardStates.POTATODONE)) //salad or potato need to be finished with chopping 
        {
            interactionManager._interactionTextline = "Press [E] to grab chopped ingredient";
            interactionManager._allowedInteraction = true;
            if (inputManager.pressedE)
            {
                _activeCBState = CuttingBoardStates.EMPTYCUTTINGBOARD; //reset cuttingboard 
                hasItemOn = false;

                Component item = cBContents[0]; //oger inventory is filled with schopped item from board 
                cBContents.Clear();
                _ogerCook.pickUp(item);
                item.position = positionWhilePickedUp;
            }
        }
        else
        {
            interactionManager._allowedInteraction = false;
        }
    }

    public void Update()
    {
        if (count >= 5)
        {
            count = 0;

            if (_activeCBState == CuttingBoardStates.POTATO) //setting right state for texture 
            {
                _activeCBState = CuttingBoardStates.POTATODONE;
            }
            else if (_activeCBState == CuttingBoardStates.SALAD)
            {
                _activeCBState = CuttingBoardStates.SALADDONE;
            }
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
