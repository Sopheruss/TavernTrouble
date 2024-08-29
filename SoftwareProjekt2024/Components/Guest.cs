using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components.StaticObjects;
using SoftwareProjekt2024.Logik;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Components;

internal class Guest : Component
{
    static AnimationManager _guestAnimationManager;
    public PerspectiveManager _perspectiveManager;
    public static Texture2D fairy;
    public static Texture2D ogerBlue;
    public static Texture2D ogerGreen;
    public static Texture2D ogerPink;

    public bool hasOrdered;
    public Order guestOrder;
    public int assignedTableID;
    public Table assignedTable;
    public Guest(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)
    {
        _guestAnimationManager = new AnimationManager(2, 2, new Vector2(32, 32), 30);
        _guestAnimationManager.RowPos = 0;

        _perspectiveManager = perspectiveManager;
        hasOrdered = false;
    }

    public override int getHeight()
    {
        return Rect.Height - 10;
    }

    public static void Update()
    {
        _guestAnimationManager.Update();
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
    }

    public override void draw(SpriteBatch _spriteBatch) // generalisierter Aufruf der Spritedraw Methode
    {

        _spriteBatch.Draw(
        this.texture,                                //texture 
        this.Rect,                                  //destinationRectangle
        _guestAnimationManager.GetFrame(),         //sourceRectangle (frame) 
        Color.White,                              //color
        0f,                                      //rotation 
        Vector2.Zero,                           //origin
        SpriteEffects.None,                    //effects
        1f);                                  //layer depth
    }

}