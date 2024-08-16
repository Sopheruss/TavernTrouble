﻿using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Components;
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
    GamePlay _gamePlay;

    readonly int quarterTileHeight = 8;
    readonly int tileSize = 32;

    List<Rectangle> intersections;

    Rectangle bounds;
    int interactionState;

    public InteractionManager(TileManager tilemanager, Player ogerCook, GamePlay gamePlay)
    {
        _tileManager = tilemanager;
        _ogerCook = ogerCook;
        _gamePlay = gamePlay;
    }

    public void Update()
    {
        CreateBounds();
        CheckInteraction(bounds);

        /*if (interactionState == 0)
        {
            Debug.WriteLine("not possible");
        }
        else
        {
            Debug.WriteLine("possible");
        }*/
    }

    public int GetInteractionState()
    {
        return interactionState;
    }

    public void CreateBounds()
    {
        //bounds = _ogerCook.Rect;

        int loweredPlayerBounds = 40;
        int tightenedPlayerBounds = 3;

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

    public void HandleInteraction(int tileID, PerspectiveManager _perspectiveManager)
    {
        switch (tileID)
        {
            case 1:
                Debug.WriteLine("Kochbuch Interaction");
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
                Debug.WriteLine("Grill Interaction");
                break;

            case 6:
                Debug.WriteLine("Kessel Interaction");
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
                break;

            case 11:
                Debug.WriteLine("Salad Interaction");
                break;

            case 12:
                Debug.WriteLine("Meat Interaction");
                break;

            case 13:
                Debug.WriteLine("Bun Interaction");
                break;

            case 14:
                Debug.WriteLine("Plates Interaction");

                _perspectiveManager._dynamicObjects.Add(new Plate(Plate.plain, new Vector2(-10, -10), _perspectiveManager)); //Position beim Tragen außerhalb der Map
                _ogerCook.pickUp(_perspectiveManager._dynamicObjects.Last());

                break;

            case 15:
                Debug.WriteLine("Bierkrug Interaction");
                break;

            case 16:
                Debug.WriteLine("Trash Interaction");

                _ogerCook.texture = Player.plain;

                break;

            case >= 20 and <= 32:
                Debug.WriteLine("Barfläche oben Interaction");

                int barflächenID = tileID - 20;
                Bar barfläche = _perspectiveManager._barFlächen[barflächenID];
                if (!_ogerCook.inventoryIsEmpty())
                {
                    Component item = _ogerCook.inventory[0];
                    _ogerCook.inventory.Clear();
                    _ogerCook.texture = Player.plain;

                    barfläche.barContents.Add(item);
                    item.position = new Vector2(barfläche.position.X + 9, barfläche.position.Y + 9);
                }
                else if (_ogerCook.inventoryIsEmpty() && !barfläche.isEmpty())
                {
                    Debug.WriteLine("Picking up");
                    Component item = barfläche.barContents[0];
                    barfläche.barContents.Clear();
                    _ogerCook.pickUp(item);
                    item.position = new Vector2(-10, -10);
                }


                break;

            case >= 40 and <= 52:
                Debug.WriteLine("Barfläche unten Interaction");
                break;

            default:
                Debug.WriteLine("INTERACTION");
                _gamePlay.IncreaseScore(5);
                break;
        }
    }
}
