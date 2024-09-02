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
using System.Linq;

namespace SoftwareProjekt2024;

internal class InteractionManager
{
    TileManager _tileManager;
    Player _ogerCook;
    CollisionManager _collisionManager;
    PerspectiveManager _perspectiveManager;

    readonly int quarterTileHeight = 8;
    readonly int tileSize = 32;

    List<Rectangle> intersections;

    Rectangle bounds;
    int interactionState;

    public Vector2 positionWhilePickedUp = new Vector2(-10, -10);  //Position beim Tragen außerhalb der Map

    string _possibleInteractionObject;
    bool _possibleInteraction;
    bool _allowedInteraction;

    public InteractionManager(TileManager tilemanager, Player ogerCook, PerspectiveManager perspectiveManager)
    {
        _tileManager = tilemanager;
        _ogerCook = ogerCook;
        _perspectiveManager = perspectiveManager;
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
        Debug.WriteLine("possible: " + _possibleInteraction);
        Debug.WriteLine("allowed: " + _allowedInteraction);
    }

    public void Draw(SpriteBatch spriteBatch, BitmapFont bmfont, Vector2 keyPressLetterSize, int screenWidth, int screenHeight)
    {
        if (_possibleInteraction && _allowedInteraction)
        {
            Vector2 textSize = bmfont.MeasureString("Press [E] to interact with " + _possibleInteractionObject);
            spriteBatch.DrawString(bmfont, "Press [E] to interact with " + _possibleInteractionObject, new Vector2((screenWidth - textSize.X) / 2, screenHeight - 15 - (int)keyPressLetterSize.Y), Color.Beige);
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
                HandleVisualFeedback(interactionState);
                //CheckPermission(interactionState);
                return;
            }
        }
        interactionState = 0; // 0 means no possible interaction; false
    }


    public void HandleVisualFeedback(int tileID)
    {
        switch (tileID)
        {
            case 0:
                _possibleInteractionObject = null;
                _allowedInteraction = false;
                break;
            case 1: 
                _possibleInteractionObject = "cookbook";
                _allowedInteraction = true;
                break;
            case int n when n >= 2 && n <= 3:
                _possibleInteractionObject = "bar space";
                break;
            case 4:
                _possibleInteractionObject = "barrel";
                _allowedInteraction = BeerBarrel.AllowedInteraction(_ogerCook);
                break;
            case 5:
                _possibleInteractionObject = "grate";
                break;
            case 6:
                _possibleInteractionObject = "cauldron";
                break;
            case int n when n >= 7 && n <= 9:
                _possibleInteractionObject = "cutting board";
                break;
            case 10:
                _possibleInteractionObject = "potato box";
                _allowedInteraction = PotatoCrate.AllowedInteraction(_ogerCook);
                break;
            case 11:
                _possibleInteractionObject = "salad box";
                _allowedInteraction = SaladCrate.AllowedInteraction(_ogerCook);
                break;
            case 12:
                _possibleInteractionObject = "meat box";
                _allowedInteraction = MeatCrate.AllowedInteraction(_ogerCook);
                break;
            case 13:
                _possibleInteractionObject = "bun box";
                _allowedInteraction = BunCrate.AllowedInteraction(_ogerCook);
                break;
            case 14:
                _possibleInteractionObject = "plates";
                _allowedInteraction = PlatePile.AllowedInteraction(_ogerCook);
                break;
            case 15:
                _possibleInteractionObject = "tankards";
                _allowedInteraction = MugPile.AllowedInteraction(_ogerCook);
                break;
            case 16:
                _possibleInteractionObject = "trash can";
                _allowedInteraction = Trash.AllowedInteraction(_ogerCook);
                break;
            case int n when n >= 20 && n <= 32:
                _possibleInteractionObject = "bar space";
                int barflächenID = tileID - 20;
                Bar barfläche = _perspectiveManager._barFlächen[barflächenID];
                _allowedInteraction = barfläche.AllowedInteraction(_ogerCook);
                break;
            case int n when n >= 40 && n <= 52:
                _possibleInteractionObject = "bar space";
                break;
            case int n when n >= 60 && n <= 67:
                _possibleInteractionObject = "table";
                break;
            default:
                _possibleInteractionObject = "something";
                break;  
        }
    }

    /*
     * IDs of Interaction Layer: 
     * CookBook: 1
     * Arbeitsfläche 1: 2
     * Arbeitsfläche 2: 3
     * Bierfass: 4
     * Grill: 5
     * Kessel: 6
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
            case 1:
                Debug.WriteLine("Kochbuch Interaction");
                CookBook.HandleInteraction(Game1._gamePlay._game, Game1._gamePlay._timer);
                break;

            case 2:
                Debug.WriteLine("Arbeitsfläche 1 Interaction");
                break;

            case 3:
                Debug.WriteLine("Arbeitsfläche 2 Interaction");
                break;

            case 4:
                Debug.WriteLine("Bierfass Interaction");
                break;

            case 5:
                Debug.WriteLine("Kessel Interaction");
                break;

            case 6:
                Debug.WriteLine("Grill Interaction");
                Grill.HandleInteraction(_ogerCook);
                break;

            case 7:
                Debug.WriteLine("Brett 1 Interaction");
                break;

            case 8:
                Debug.WriteLine("Brett 2 Interaction");
                break;

            case 9:
                Debug.WriteLine("Brett 3 Interaction");
                break;

            case 10:
                Debug.WriteLine("Potato Interaction");
                if (_ogerCook.inventoryIsEmpty())
                {
                    PotatoCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case 11:
                Debug.WriteLine("Salad Interaction");
                if (_ogerCook.inventoryIsEmpty())
                {
                    SaladCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case 12:
                Debug.WriteLine("Meat Interaction");
                if (_ogerCook.inventoryIsEmpty())
                {
                    MeatCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case 13:
                Debug.WriteLine("Bun Interaction");
                if (_ogerCook.inventoryIsEmpty())
                {
                    BunCrate.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                }
                break;

            case 14:
                Debug.WriteLine("Plates Interaction");

                _perspectiveManager._dynamicObjects.Add(new Plate(Plate.plain, positionWhilePickedUp, _perspectiveManager));
                _ogerCook.pickUp(_perspectiveManager._dynamicObjects.Last());

                break;

            case 15:
                Debug.WriteLine("Bierkrug Interaction");
                break;

            case 16:
                Debug.WriteLine("Trash Interaction");
                if (!_ogerCook.inventoryIsEmpty())
                {
                    Trash.HandleInteraction(_ogerCook, _perspectiveManager);
                }
                break;

            case >= 20 and <= 32:
                Debug.WriteLine("Barfläche oben Interaction");

                int barflächenID = tileID - 20;
                Bar barfläche = _perspectiveManager._barFlächen[barflächenID];
                barfläche.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                break;

            case >= 40 and <= 52:
                Debug.WriteLine("Barfläche unten Interaction");
                break;

            case >= 60 and <= 67:
                Debug.WriteLine("Tisch Interaktion");
                int tableID = tileID - 60;
                Table table = _perspectiveManager._tables[tableID];
                table.HandleInteraction(_perspectiveManager, positionWhilePickedUp, _ogerCook);
                break;

            default:
                Debug.WriteLine("INTERACTION");
                Game1._gamePlay.IncreaseScore(5);
                break;
        }
    }
}
