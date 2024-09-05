using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
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

    public Texture2D _chosenTexture;

    public static Texture2D fairyGreen;
    public static Texture2D fairyRed;
    public static Texture2D fairyBlue;
    public static Texture2D ogerBlue;
    public static Texture2D ogerOrange;
    public static Texture2D ogerPink;
    public static Texture2D wizardRed;
    public static Texture2D wizardYellow;
    public static Texture2D wizardPurple;

    public static Texture2D spawnAnimationTexture;

    public bool hasOrdered;
    public Order order;
    public int assignedTableID;
    public Table assignedTable;

    private bool _drawGuest;
    private bool _drawSpawn;

    public static List<Texture2D> _availableGuests;
    public static int _totalGuestNumber;

    BitmapFont _font;

    public Guest(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)
    {
        perspectiveManager._sortedComponents.Add(this);

        if (_availableGuests == null)
        {
            _availableGuests = new List<Texture2D>
                {
                    fairyGreen,
                    fairyRed,
                    fairyBlue,
                    ogerOrange,
                    ogerBlue,
                    ogerPink,
                    wizardRed,
                    wizardPurple,
                    wizardYellow
                };
        }

        _font = Game1.ContentManager.Load<BitmapFont>("Fonts/font_new"); // load font from content-manager using monogame.ext importer/exporter


        _totalGuestNumber++;

        _guestAnimationManager = new AnimationManager(2, 2, new Vector2(32, 32), 30);
        _guestAnimationManager.RowPos = 0;

        _spawnAnimationManager = new AnimationManager(7, 7, new Vector2(32, 32), 4);
        _spawnAnimationManager.RowPos = 0;

        _perspectiveManager = perspectiveManager;
        hasOrdered = false;
        _drawGuest = false;
        _drawSpawn = true;

        _chosenTexture = ChooseTexture(CreateRandomIntegerTexture());
    }

    public override int getHeight()
    {
        return Rect.Height - 10;
    }

    public static Texture2D ChooseTexture(int wichTexture)
    {
        Texture2D guestTexture = _availableGuests[wichTexture];
        _availableGuests.RemoveAt(wichTexture);
        return guestTexture;
    }

    public static int CreateRandomIntegerTexture()
    {
        Random rnd = new();
        int num = rnd.Next(0, _availableGuests.Count);
        return num; //Generates a number between 0 and 8 -> is number of different textures 
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
        order = new Order(0, new List<Recipe> { new Recipe("Burger"), new Recipe("Fries") });
        _perspectiveManager.activeOrders.Add(order);
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
            //Debug.WriteLine("Table " + table.tableID + " assigned new guest");
            break;
        }
        //negative feedback if no table is clean needed here
    }

    public void eat(Player _ogerCook)
    {
        //feedback for points here?
        //logik um Teller zu leeren und Bestellungszettel zu entfernen hier

        if (order != null)
        {
            // order.CompleteComponent();
            (int rewardPoints, int fame) = judgeOrder();

            Debug.WriteLine($"Debug eat: {rewardPoints}");



            _ogerCook.AddPointsAndFame(rewardPoints, fame);

            Debug.WriteLine($"Der Spieler hat {rewardPoints} Punkte erhalten.");
            Debug.WriteLine($"Der Spieler hat jetzt insgesamt {_ogerCook.totalPoints} Punkte und {_ogerCook.famePoints} Ruhm.");

        }

    }


    public (int points, int fame) judgeOrder()
    {

        int points = 0;
        int fame = 0;

        int completedComponents = (order.recipes.Count - order.missingRecipes.Count) + (order.drinksCount - order.missingDrinksCount);
        points = completedComponents * 10;
        if (order.isFinished && order.wrongComponentsCount == 0) { } //Maybe add bonus points for a perfectly handled order here?
        Debug.WriteLine($"judgeOrderA: {points}");
        
        if (order.wrongComponentsCount > 0)
        {
            points += order.wrongComponentsCount * (-2);
        }

        fame = points / 5;
        fame = Math.Max(0, fame);  // Kein negativer Ruhm

        return (points, fame);
    }


    public void leave()
    {
        _totalGuestNumber--;
        assignedTable.guest = null;
        _perspectiveManager._guests.Remove(this);
        _drawGuest = false;
        _availableGuests.Add(this._chosenTexture);
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

            if (!hasOrdered)
            {
                _spriteBatch.DrawString(_font, "!", new Vector2(this.position.X + 17, this.position.Y - 15), Color.Red);
            }
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