﻿using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Components;
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

    readonly int quarterTileHeight = 8;
    readonly int tileSize = 32;

    List<Rectangle> intersections;

    Rectangle bounds;
    int interactionState;

    public Vector2 positionWhilePickedUp = new Vector2(-10, -10);  //Position beim Tragen außerhalb der Map

    string possibleInteractionObject;

    public InteractionManager(TileManager tilemanager, Player ogerCook)
    {
        _tileManager = tilemanager;
        _ogerCook = ogerCook;

    }

    public void Update()
    {
        CreateBounds();
        CheckInteraction(bounds);

        if (interactionState == 0)
        {
            GamePlay._showPossibleInteraction = false;
        }
        else
        {
            GamePlay._showPossibleInteraction = true;
            GamePlay._possibleInteractionObject = possibleInteractionObject;
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
                ChangeInteractionString(interactionState);
                return;
            }
        }
        interactionState = 0; // 0 means no possible interaction; false
    }

    public void ChangeInteractionString(int tileID)
    {
        switch (tileID)
        {
            case 0:
                possibleInteractionObject = null;
                break;
            case 1:
                possibleInteractionObject = "cookbook";
                break;
            case int n when n >= 2 && n <= 3:
                possibleInteractionObject = "bar space";
                break;
            case 4:
                possibleInteractionObject = "barrel";
                break;
            case 5:
                possibleInteractionObject = "grate";
                break;
            case 6:
                possibleInteractionObject = "cauldron";
                break;
            case int n when n >= 7 && n <= 9:
                possibleInteractionObject = "cutting board";
                break;
            case 10:
                possibleInteractionObject = "potato box";
                break;
            case 11:
                possibleInteractionObject = "salad box";
                break;
            case 12:
                possibleInteractionObject = "meat box";
                break;
            case 13:
                possibleInteractionObject = "bun box";
                break;
            case 14:
                possibleInteractionObject = "plates";
                break;
            case 15:
                possibleInteractionObject = "tankards";
                break;
            case 16:
                possibleInteractionObject = "trash can";
                break;
            case int n when n >= 20 && n <= 32:
                possibleInteractionObject = "bar space";
                break;
            case int n when n >= 40 && n <= 52:
                possibleInteractionObject = "bar space";
                break;
            case int n when n >= 60 && n <= 67:
                possibleInteractionObject = "table";
                break;
            default:
                possibleInteractionObject = "something";
                break;
        }
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


    public void HandleInteraction(int tileID, PerspectiveManager _perspectiveManager)
    {
        switch (tileID)
        {
            case 1:
                Debug.WriteLine("Kochbuch Interaction");
                CookBook.HandleInteraction();
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

                if (Kessel._activeKesselState == KesselStates.DONEKESSEL && _ogerCook.inventoryIsEmpty())
                {
                    //TODO: Oger inventory and spritesheet have to be altered acording to pick up of done Fries 
                    Debug.WriteLine("Oger hat jetzt Pommes in der Hand!");
                    Kessel._activeKesselState = KesselStates.EMPTYKESSEL;
                }
                else //HERE IF CASE -> only interaction if oger carrys chopped potato 
                {
                    Kessel._activeKesselState = KesselStates.ANIMATIONKESSEL;
                    Kessel.HandleInteraction();
                }
                break;

            case 6:
                Debug.WriteLine("Grill Interaction");

                Grill.HandleInteraction(positionWhilePickedUp, _ogerCook);

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
