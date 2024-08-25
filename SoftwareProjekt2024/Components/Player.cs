using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Components;

internal class Player : Component
{

    public static Texture2D plain;
    public static Texture2D withPlate;
    public static Texture2D withMeat;
    public static Texture2D withBun;
    public static Texture2D withSalad;
    public static Texture2D withPotato;
    public static Texture2D withPlate_Fries;
    public static Texture2D withPlate_FullBurger;

    public List<Component> inventory;

    public Player(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)
    {
        inventory = new List<Component>();
        state = (int)States.Empty;
        perspectiveManager._sortedComponents.Add(this);
    }

    public override void Update() //Update der Position
    {
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
            default:
                break;
        }
    }



    public void pickUp(Component item)
    {
        inventory.Add(item);
        changeAppearence(item.state);
    }

    public override void draw(SpriteBatch _spriteBatch, AnimationManager _animationManager) // generalisierter Aufruf der Spritedraw Methode
    {
        _spriteBatch.Draw(
        this.texture,                                //texture 
        this.Rect,                                  //destinationRectangle
        _animationManager.GetFrame(),              //sourceRectangle (frame) 
        Color.White,                              //color
        0f,                                      //rotation 
        Vector2.Zero,                           //origin
        SpriteEffects.None,                    //effects
        1f);                                  //layer depth
    }
}