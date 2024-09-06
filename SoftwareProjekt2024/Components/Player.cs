using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components;

internal class Player : Component
{
    public static AnimationManager _playerAnimationManager;

    public static Texture2D plain;
    public static Texture2D withPlate;
    public static Texture2D withMeat;
    public static Texture2D withMeatDone;
    public static Texture2D withBun;
    public static Texture2D withSalad;
    public static Texture2D withSaladChopped;
    public static Texture2D withPotato;
    public static Texture2D withFries;
    public static Texture2D withFriesDone;
    public static Texture2D withPlate_Fries;
    public static Texture2D withPlate_FullBurger;
    public static Texture2D withBeerEmpty;
    public static Texture2D withBeerFull;

    public List<Component> inventory;


    // rewardssys
    public int totalPoints;
    public float famePoints;
    private int playerlevel;




    public Player(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)
    {
        perspectiveManager._sortedComponents.Add(this);
        inventory = new List<Component>();
        state = (int)States.Empty;
        totalPoints = 0;
        famePoints = 0.0f;
        playerlevel = 1;
    }


    //Hinzufügen Punkte und Fame
    public void AddPointsAndFame(int points, float fame)
    {
        totalPoints += points;
        Game1._gamePlay.IncreaseScore(points);
        Debug.WriteLine("Player received " + points);
        famePoints += fame;
        Debug.WriteLine("Player received " + fame);

        UpdatePlayerLevel();
    }

    private void UpdatePlayerLevel()
    {
        // Calculate level based on fame points
        int newPlayerLevel = (int)(famePoints / 10);
        if (newPlayerLevel > playerlevel)
        {
            playerlevel = newPlayerLevel;
            Debug.WriteLine("Player leveled up to level " + playerlevel);
        }
    }

    public int GetPlayerLevel()
    {
        return playerlevel;
    }

    public int GetDifficulty()
    {
        if (playerlevel <= 2)
        {
            return 1;
        }
        else if (playerlevel >= 3 && playerlevel <= 4)
        {
            return 2;
        }
        else if (playerlevel >= 5 && playerlevel <= 6)
        {
            return 3;
        }
        else 
        {
            return 4;
        }
    }

    public void Load()
    {
        /* animation */
        //constructing new Animation with 4 Frames in 4 Rows and Frame Size of single Image
        //Vector decides size of the size for the frame (one Oger Frame = 32/32)
        _playerAnimationManager = new(4, 4, new Vector2(32, 32), 10);
    }



    public override void Update() //Update der Position
    {

        _playerAnimationManager.Update();
        base.Update();
    }

    public bool inventoryIsEmpty()
    {
        return inventory.Count == 0;
    }

    public override int getHeight()
    {
        return this.height;
    }

    public void changeAppearence(int appearence)
    {
        switch (appearence)
        {
            case 1:
                texture = Player.plain;
                break;
            case 2:
                texture = Player.withPlate;
                break;
            case 3:
                texture = Player.withPlate_Fries;
                break;
            case 4:
                texture = Player.withPlate_FullBurger;
                break;
            case 5:
                texture = Player.withBun;
                break;
            case 6:
                texture = Player.withMeat;
                break;
            case 7:
                texture = Player.withSalad;
                break;
            case 8:
                texture = Player.withPotato;
                break;
            case 12: //cuz components number
                texture = Player.withMeatDone;
                break;
            case 13:
                texture = Player.withFries;
                break;
            case 14:
                texture = Player.withFriesDone;
                break;
            case 15:
                texture = Player.withSaladChopped;
                break;
            case 16:
                texture = Player.withBeerEmpty;
                break;
            case 17:
                texture = Player.withBeerFull;
                break;
            default:
                break;
        }
    }

    public void pickUp(Component item)
    {
        inventory.Add(item);
        changeAppearence(item.state);
    }

    public override void draw(SpriteBatch _spriteBatch) // generalisierter Aufruf der Spritedraw Methode
    {
        _spriteBatch.Draw(
        this.texture,                                //texture 
        this.Rect,                                  //destinationRectangle
        _playerAnimationManager.GetFrame(),        //sourceRectangle (frame) 
        Color.White,                              //color
        0f,                                      //rotation 
        Vector2.Zero,                           //origin
        SpriteEffects.None,                    //effects
        1f);                                  //layer depth
    }
}