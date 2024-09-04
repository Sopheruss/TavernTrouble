using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.Ingredients;
using SoftwareProjekt2024.Components.StaticObjects;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Screens;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024;

internal class InteractionManager
{
    TileManager _tileManager;
    Player _ogerCook;
    CollisionManager _collisionManager;
    PerspectiveManager _perspectiveManager;
    InputManager _inputManager;

    readonly int quarterTileHeight = 8;
    readonly int tileSize = 32;

    List<Rectangle> intersections;

    Rectangle bounds;
    int interactionState;

    public Vector2 positionWhilePickedUp = new Vector2(-10, -10);  //Position beim Tragen außerhalb der Map

    string _possibleInteractionObject;
    bool _possibleInteraction;
    bool _allowedInteraction;

    public InteractionManager(TileManager tilemanager, Player ogerCook, PerspectiveManager perspectiveManager, InputManager inputManager)
    {
        _tileManager = tilemanager;
        _ogerCook = ogerCook;
        _perspectiveManager = perspectiveManager;
        _inputManager = inputManager;
    }

    public void Update()
    {
        CreateBounds();
        CheckInteraction(bounds);

        if (interactionState == 0)
        {
           _possibleInteraction = false;
           _allowedInteraction = false;
        }
        else
        {
           _possibleInteraction = true;
        }
        //Debug.WriteLine("possible: " + _possibleInteraction);
        //Debug.WriteLine("allowed: " + _allowedInteraction);
    }

    public void Draw(SpriteBatch spriteBatch, BitmapFont bmfont, Vector2 keyPressLetterSize, int screenWidth, int screenHeight)
    {
        if (_possibleInteraction && _allowedInteraction)
        {
            Vector2 textSize = bmfont.MeasureString(_possibleInteractionObject);
            spriteBatch.DrawString(bmfont, _possibleInteractionObject, new Vector2((screenWidth - textSize.X) / 2, screenHeight - 15 - (int)keyPressLetterSize.Y), Color.Beige);
        }
    }

    public int GetInteractionState()
    {
        return interactionState;
    }

    public void CreateBounds()
    {
        //bounds = _ogerCook.Rect;

        int loweredPlayerBounds = 40;
        int tightenedPlayerBounds = 19;

        bounds = new Rectangle(
            _ogerCook.Rect.X + tightenedPlayerBounds,
            _ogerCook.Rect.Y + loweredPlayerBounds,
            _ogerCook.Rect.Width - (2 * tightenedPlayerBounds),
            _ogerCook.Rect.Height - loweredPlayerBounds);
    }

    public void CheckInteraction(Rectangle bounds)
    {
        foreach (var tile in _tileManager.interactionLayer)
        {
            Rectangle tileRect = new Rectangle((int)tile.Key.X * tileSize, (int)tile.Key.Y * tileSize, tileSize, tileSize);

            if (tileRect.Intersects(bounds))
            {
                interactionState = (int)tile.Value; // returns tile ID of intersecting rect to handle interaction for different tile-types later; true
                HandleInteraction(interactionState);
                return;
            }
        }
        interactionState = 0; // 0 means no possible interaction; false
    }

    /*
     * IDs of Interaction Layer: 
     * CookBook: 1
     * Arbeitsfläche 1: 2
     * Arbeitsfläche 2: 3
     * Bierfass: 4
     * Kessel: 5
     * Grill: 6
     * Brett 1: 7
     * Brett 2: 8
     * Brett 3: 9
     * Potato: 10
     * Salad: 11
     * Meat: 12
     * Bun: 13
     * Plates: 14
     * Bierkrug: 15
     * Mülleimer: 16
     * Barflächen 0-12 oben : 20 - 32
     * Barflächen 0-12 unten : 40 - 52
     * Tisch 1: 60
     * Tisch 2: 61
     * Tisch 3: 62
     * Tisch 4: 63
     * Tisch 5: 64
     * Tisch 6: 65
     * Tisch 7: 66
     * Tisch 8: 67
     */


    public void HandleInteraction(int tileID)
    {
        switch (tileID)
        {
            case 0:
                _allowedInteraction = false;
                _possibleInteractionObject = null;
                break;
            
            case 1:
                _possibleInteractionObject = "Press [E] to interact with cookbook";
                _allowedInteraction = true;
                if (_inputManager.pressedE)
                {
                    CookBook.HandleInteraction();
                }
                break;
            
            case >= 2 and <= 3:
                _possibleInteractionObject = "Press [E] to interact with bar space";
                int workstationID = tileID - 2;
                Workstation workstation = _perspectiveManager._workstations[workstationID];
                _allowedInteraction = workstation.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    workstation.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;
            
            case 4:
                _possibleInteractionObject = "Press [E] to interact with beer barrel";
                _allowedInteraction = BeerBarrel.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    BeerBarrel.HandleInteraction(_ogerCook, positionWhilePickedUp);
                }
                break;

            case 5:
                _possibleInteractionObject = "Press [E] to interact with cauldron";
                //_allowedInteraction = Kessel.AllowedInteraction();
                if (_inputManager.pressedE)
                {
                    Kessel.HandleInteraction(_ogerCook, positionWhilePickedUp);
                }
                break;

            case 6:
                _possibleInteractionObject = "Press [E] to interact with grate";
                //_allowedInteraction = Grill.AllowedInteraction();
                if (_inputManager.pressedE)
                {
                    Grill.HandleInteraction(_ogerCook, positionWhilePickedUp);
                }
                break;

            case >= 7 and <= 9:
                _possibleInteractionObject = "Press [E] to interact with cutting board";
                int cuttingBoardID = tileID - 7;
                Cuttingboard cuttingBoard = _perspectiveManager._cuttingBoards[cuttingBoardID];
                //_allowedInteraction = cuttingBoard.AllowedInteraction();
                if (_inputManager.pressedE)
                {
                    cuttingBoard.HandleInteraction(_ogerCook, positionWhilePickedUp);
                }
                break;
            
            case 10:
                _possibleInteractionObject = "Press [E] to interact with potato box";
                _allowedInteraction = PotatoCrate.AllowedInteraction(_ogerCook);
                if(_inputManager.pressedE)
                {
                    PotatoCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case 11:
                _possibleInteractionObject = "Press [E] to interact with salad box";
                _allowedInteraction = SaladCrate.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    SaladCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case 12:
                _possibleInteractionObject = "Press [E] to interact with meat box";
                _allowedInteraction = MeatCrate.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    MeatCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case 13:
                _possibleInteractionObject = "Press [E] to interact with bun box";
                _allowedInteraction = BunCrate.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    BunCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case 14:
                _possibleInteractionObject = "Press [E] to interact with plates";
                _allowedInteraction = PlatePile.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    PlatePile.HandleInteraction(_ogerCook, _perspectiveManager, positionWhilePickedUp);
                }
                break;

            case 15:
                _possibleInteractionObject = "Press [E] to interact with tankards";
                _allowedInteraction = MugPile.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    MugPile.HandleInteraction(_ogerCook, _perspectiveManager, positionWhilePickedUp);
                }
                break;

            case 16:
                _possibleInteractionObject = "Press [E] to interact with trash can";
                _allowedInteraction = Trash.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    Trash.HandleInteraction(_ogerCook, _perspectiveManager);
                }
                break;

            case >= 20 and <= 32:
                _possibleInteractionObject = "Press [E] to interact with bar space";
                int obereBarflächenID = tileID - 20;
                Bar obereBarfläche = _perspectiveManager._barFlächen[obereBarflächenID];
                _allowedInteraction = obereBarfläche.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    obereBarfläche.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case >= 40 and <= 52:
                _possibleInteractionObject = "Press [E] to interact with bar space";
                int untereBarflächenID = tileID - 40;
                Bar untereBarfläche = _perspectiveManager._barFlächen[untereBarflächenID];
                _allowedInteraction = untereBarfläche.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    untereBarfläche.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case >= 60 and <= 67:
                _possibleInteractionObject = "Press [E] to interact with table";
                int tableID = tileID - 60;
                Table table = _perspectiveManager._tables[tableID];
                //_allowedInteraction = table.AllowedInteraction(_ogerCook);
                if (_inputManager.pressedE)
                {
                    table.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            default:
                _possibleInteractionObject = "Press [E] to interact with something";
                break;
        }
    }
}
