using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components.StaticObjects;
using SoftwareProjekt2024.Logik;
using SoftwareProjekt2024.Managers;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SoftwareProjekt2024.Components;

internal class Guest : Component
{
    readonly Player _ogerCook;
    readonly AnimationManager _guestAnimationManager;
    readonly AnimationManager _spawnAnimationManager;
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

    public static Texture2D exclamationPoint;
    public static Texture2D exclamationPointGreen;

    public static Texture2D spawnAnimationTexture;

    public bool hasOrdered;
    public bool hasFinishedEating;
    public Order order;
    public int assignedTableID;
    public Table assignedTable;
    public bool markForRemovel;
    private bool _drawGuest;
    private bool _drawSpawn;
    private bool _drawDespawn;

    public static List<Texture2D> _availableGuests;
    public static int _totalGuestNumber;

    BitmapFont _font;

    readonly int maxMeals = 3;
    readonly int maxBurgers = 2;

    public Guest(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager, Player ogerCook) : base(texture, position, perspectiveManager)
    {
        _ogerCook = ogerCook;
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
        hasFinishedEating = false;

        _drawGuest = false;
        _drawSpawn = true;
        _drawDespawn = false;
        markForRemovel = false;

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
        if (!assignedTable.tableOrderfinished && order != null && order.IsTimeUp())
        {
            assignedTable.tableOrderfinished = true;
            eat();
            //add visual feedback for not completing order on time here, e.g. texture = angryGuest
        }

        _guestAnimationManager.Update();

        if (_spawnAnimationManager.activeFrame == 4)
        {
            _drawGuest = true;
        }

        if (_spawnAnimationManager.activeFrame == 6 && _drawSpawn)
        {
            _drawSpawn = false;
            _spawnAnimationManager.ResetAnimation();
        }

        //STARTED DESPAWN ANIMATION BUT STUCK 
        if (_spawnAnimationManager.activeFrame == 6 && _drawDespawn)
        {
            _drawDespawn = false;
            leave();
        }

        _spawnAnimationManager.Update();
    }

    public void takeOrder()
    {
        int nNeededComponents = _ogerCook.GetDifficulty();
        int nExistingComponents = 0;
        int nDrinks = 0;
        int nMeals = 0;
        int nRecipes = Recipe.recipes.Count;
        int nBurgers = 0;

        List<Recipe> meals = new List<Recipe>();
        List<string> recipeNames = Recipe.recipes.Keys.ToList<string>();

        while (nExistingComponents < nNeededComponents)
        {
            if (nMeals < maxMeals)
            {
                Random random = new Random();
                int componentResult = random.Next(2);

                if (componentResult == 0)
                {
                    int recipeResult = random.Next(nRecipes);
                    if (recipeNames[recipeResult] == "burger")
                    {
                        if (nBurgers < maxBurgers)
                        {
                            nBurgers++;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    nMeals++;
                    meals.Add(new Recipe(recipeNames[recipeResult]));
                }
                else
                {
                    nDrinks++;
                }
                nExistingComponents++;
            }
            else
            {
                nDrinks++;
                nExistingComponents++;
            }
        }
        order = new Order(nDrinks, meals, assignedTableID);
        _perspectiveManager.activeOrders.Add(order);
        hasOrdered = true;
    }

    public void assignTable()
    {
        List<Table> _availableTables = new List<Table>();

        foreach (Table table in _perspectiveManager._tables)
        {
            if (!table.isClean() || table.hasGuest())   //pick first clean and free table
            {
                continue;
            }

            _availableTables.Add(table);
        }

        int rndNum = CreateRandomIntegerTable(_availableTables.Count);

        assignedTableID = _availableTables[rndNum].tableID;
        assignedTable = _availableTables[rndNum];
        _availableTables[rndNum].guest = this;
        position = new Vector2(_availableTables[rndNum].position.X + 12, _availableTables[rndNum].position.Y - 18);


        //assignedTableID = table.tableID;
        //assignedTable = table;
        //table.guest = this;
        //position = new Vector2(table.position.X + 12, table.position.Y - 18);

        //negative feedback if no table is clean needed here
    }

    public static int CreateRandomIntegerTable(int _availableTables)
    {
        Random rnd = new();
        int num = rnd.Next(0, _availableTables);
        return num; //Generates a number between 0 and free tables  
    }

    public void eat()
    {
        if (assignedTable.isClean())
        {
            //no eating animation in this case, maybe just leave?
        }

        //Animation and timer for eating here
        assignedTable.emptyPlatesMugs();
        hasFinishedEating = true;
    }


    public (int points, int fame) judgeOrder()
    {

        int points = 0;
        int fame = 0;

        int completedComponents = (order.recipes.Count - order.missingRecipes.Count) + (order.drinksCount - order.missingDrinksCount);
        points = completedComponents * 10;          //10 points for each completed component
        points -= order.missingRecipes.Count * 2;   //-2 for each missing drink or recipe
        points -= order.missingDrinksCount * 2;

        if (order.isFinished && order.wrongComponentsCount == 0) { } //Maybe add bonus points for a perfectly handled order here?
        Debug.WriteLine($"judgeOrderA: {points}");

        if (order.wrongComponentsCount > 0)
        {
            points += order.wrongComponentsCount * (-2);
        }

        if (order.IsTimeUp())
        {
            points -= 10;   //negative feedback for not finishing on time here
        }

        fame = points / 5; //
        fame = Math.Max(0, fame);  // Kein negativer Ruhm

        return (points, fame);
    }

    public void giveFeedback(Player _ogerCook)
    {
        if (order != null)
        {
            (int rewardPoints, int fame) = judgeOrder();

            Debug.WriteLine($"Debug eat: {rewardPoints}");

            _ogerCook.AddPointsAndFame(rewardPoints, fame);

            Debug.WriteLine($"Der Spieler hat {rewardPoints} Punkte erhalten.");
            Debug.WriteLine($"Der Spieler hat jetzt insgesamt {_ogerCook.totalPoints} Punkte und {_ogerCook.famePoints} Ruhm.");

            _drawDespawn = true;

        }
        //add visual guest point feedback here

    }

    public void leave()
    {
        _perspectiveManager.activeOrders.Remove(order);
        _totalGuestNumber--;
        assignedTable.guest = null;
        //_perspectiveManager._guests.Remove(this);
        this.markForRemovel = true;
        _drawGuest = false;
        _availableGuests.Add(this._chosenTexture);

        if (assignedTable.isClean())
        {
            assignedTable.tableOrderfinished = false;
        }
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
                //_spriteBatch.DrawString(_font, "!", new Vector2(this.position.X + 17, this.position.Y - 15), Color.Red);
                _spriteBatch.Draw(exclamationPoint, new Rectangle((int)this.position.X + 17, (int)this.position.Y - 5, exclamationPoint.Width, exclamationPoint.Height), Color.White);
            }
            else if (hasFinishedEating)
            {
                _spriteBatch.Draw(exclamationPointGreen, new Rectangle((int)this.position.X + 17, (int)this.position.Y - 5, exclamationPoint.Width, exclamationPoint.Height), Color.White);
            }
        }

        if (_drawSpawn || _drawDespawn)
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