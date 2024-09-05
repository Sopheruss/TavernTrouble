using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.StaticObjects;
using SoftwareProjekt2024.Managers;
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
    int _interactionState;

    public Vector2 positionWhilePickedUp = new Vector2(-10, -10);  //Position beim Tragen außerhalb der Map

    public string _interactionTextline;
    bool _possibleInteraction;
    public bool _allowedInteraction;

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

        if (_interactionState == 0)
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
            Vector2 textSize = bmfont.MeasureString(_interactionTextline);
            spriteBatch.DrawString(bmfont, _interactionTextline, new Vector2((screenWidth - textSize.X) / 2, screenHeight - 15 - (int)keyPressLetterSize.Y), Color.Beige);
        }
    }

    public int GetInteractionState()
    {
        return _interactionState;
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
                _interactionState = (int)tile.Value; // returns tile ID of intersecting rect to handle interaction for different tile-types later; true
                HandleInteraction(_interactionState);
                return;
            }
        }
        _interactionState = 0; // 0 means no possible interaction; false
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
                _interactionTextline = null;
                break;
            
            case 1:
                CookBook.HandleInteraction(this, _inputManager);
                break;
            
            case >= 2 and <= 3:
                int workstationID = tileID - 2;
                Workstation workstation = _perspectiveManager._workstations[workstationID];
                workstation.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook, this, _inputManager);
                break;
            
            case 4:
                BeerBarrel.HandleInteraction(_ogerCook, positionWhilePickedUp, this, _inputManager);
                break;

            case 5:
                Kessel.HandleInteraction(_ogerCook, positionWhilePickedUp, this, _inputManager);
                break;

            case 6:
                Grill.HandleInteraction(_ogerCook, positionWhilePickedUp, this, _inputManager);
                break;

            case >= 7 and <= 9:
                int cuttingBoardID = tileID - 7;
                Cuttingboard cuttingBoard = _perspectiveManager._cuttingBoards[cuttingBoardID];
                cuttingBoard.HandleInteraction(_ogerCook, positionWhilePickedUp, this, _inputManager);
                break;
            
            case 10:
                PotatoCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook, this, _inputManager);
                break;

            case 11:
                SaladCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook, this, _inputManager);
                break;

            case 12:
                MeatCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook, this, _inputManager);
                break;

            case 13:
                BunCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook, this, _inputManager);
                break;

            case 14:
                PlatePile.HandleInteraction(_ogerCook, _perspectiveManager, positionWhilePickedUp, this, _inputManager);
                break;

            case 15:
                MugPile.HandleInteraction(_ogerCook, _perspectiveManager, positionWhilePickedUp, this, _inputManager);
                break;

            case 16:
                Trash.HandleInteraction(_ogerCook, _perspectiveManager, positionWhilePickedUp, this, _inputManager);
                break;

            case >= 20 and <= 32:
                int obereBarflächenID = tileID - 20;
                Bar obereBarfläche = _perspectiveManager._barFlächen[obereBarflächenID];
                obereBarfläche.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook, this, _inputManager);
                break;

            case >= 40 and <= 52:
                int untereBarflächenID = tileID - 40;
                Bar untereBarfläche = _perspectiveManager._barFlächen[untereBarflächenID];
                untereBarfläche.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook, this, _inputManager);
                break;

            case >= 60 and <= 67:
                int tableID = tileID - 60;
                Table table = _perspectiveManager._tables[tableID];
                table.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook, this, _inputManager);
                break;

            default:
                _allowedInteraction = false;
                _interactionTextline = null;
                break;
        }
    }
}
