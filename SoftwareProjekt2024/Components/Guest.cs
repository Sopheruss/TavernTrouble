﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.StaticObjects;
using SoftwareProjekt2024.Logik;
using SoftwareProjekt2024.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components;

internal class Guest : Component
{
    AnimationManager _guestAnimationManager;
    AnimationManager _spawnAnimationManager;
    public PerspectiveManager _perspectiveManager;

    public static Texture2D _chosenTexture;

    public static Texture2D fairy;
    public static Texture2D ogerBlue;
    public static Texture2D ogerGreen;
    public static Texture2D ogerPink;

    public static Texture2D spawnAnimationTexture;

    public bool hasOrdered;
    public Order guestOrder;
    public int assignedTableID;
    public Table assignedTable;

    private bool _drawGuest;
    private bool _drawSpawn;
    public Guest(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)
    {
        _guestAnimationManager = new AnimationManager(2, 2, new Vector2(32, 32), 30);
        _guestAnimationManager.RowPos = 0;

        _spawnAnimationManager = new AnimationManager(7, 7, new Vector2(32, 32), 10);
        _spawnAnimationManager.RowPos = 0;

        _perspectiveManager = perspectiveManager;
        hasOrdered = false;
        _drawGuest = false;
        _drawSpawn = true;

        chooseTexture(creatRandomIntegerTexture());
    }

    public override int getHeight()
    {
        return Rect.Height - 10;
    }

    public Texture2D chooseTexture(int wichTexture)
    {
        switch (wichTexture)
        {
            case 0:
                return _chosenTexture = fairy;
            case 1:
                return _chosenTexture = ogerGreen;
            case 2:
                return _chosenTexture = ogerBlue;
            case 3:
                return _chosenTexture = ogerPink;
            default:
                return _chosenTexture = fairy;
        }
    }

    public int creatRandomIntegerTexture()
    {
        Random rnd = new Random();
        return rnd.Next(0, 3); //Generates a number between 0 and 3 -> is number of different textures 
    }

    public void Update()
    {
        _guestAnimationManager.Update();

        if (_spawnAnimationManager.activeFrame == 4) { _drawGuest = true; }

        if (_spawnAnimationManager.activeFrame == 6) { _drawSpawn = false; }

        _spawnAnimationManager.Update();
    }

    public void takeOrder() //placeholder
    {
        guestOrder = new Order(false, new List<Recipe> { new Recipe("Burger") });
        hasOrdered = true;
    }

    public void assignTable()
    {
        foreach (Table table in _perspectiveManager._tables)
        {
            if (!table.isClean() || table.hasGuest())   //pick first clean and free table
            {
                continue;
            }
            assignedTableID = table.tableID;
            assignedTable = table;
            table.guest = this;
            position = new Vector2(table.position.X + 12, table.position.Y - 18);
            Debug.WriteLine("Table " + table.tableID + " assigned new guest");
            break;
        }
        //negative feedback if no table is clean needed here
    }

    public void leave()
    {
        assignedTable.guest = null;
        _perspectiveManager._guests.Remove(this);
        _drawGuest = false;
    }

    public override void draw(SpriteBatch _spriteBatch) // generalisierter Aufruf der Spritedraw Methode
    {
        if (_drawGuest)
        {
            _spriteBatch.Draw(
            _chosenTexture,                              //texture 
            this.Rect,                                  //destinationRectangle
            _guestAnimationManager.GetFrame(),         //sourceRectangle (frame) 
            Color.White,                              //color
            0f,                                      //rotation 
            Vector2.Zero,                           //origin
            SpriteEffects.None,                    //effects
            1f);                                  //layer depth
        }

        if (_drawSpawn)
        {
            _spriteBatch.Draw(
            spawnAnimationTexture,                       //texture 
            this.Rect,                                  //destinationRectangle
            _spawnAnimationManager.GetFrame(),         //sourceRectangle (frame) 
            Color.White,                              //color
            0f,                                      //rotation 
            Vector2.Zero,                           //origin
            SpriteEffects.None,                    //effects
            1f);                                  //layer depth
        }
    }

}