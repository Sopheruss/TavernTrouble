﻿using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Components;
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
        bounds = _ogerCook.Rect;
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
     * Default: 3
     * Kessel: 1
     * Grill: 5
     * Cook Book: 4
     */
    public void HandleInteraction(int tileID)
    {
        switch (tileID)
        {
            case 1:
                Debug.WriteLine("Kessel Interaction");
                break;
            case 2:
                Debug.WriteLine("green Interaction");
                break;
            case 3:
                Debug.WriteLine("blue Interaction");
                break;
            case 4:
                Debug.WriteLine("CookBook Interaction");
                break;
            case 5:
                Debug.WriteLine("Grill Interaction");
                break;
            default:
                Debug.WriteLine("INTERACTION");
                _gamePlay.IncreaseScore(5);
                break;
        }
    }
}
