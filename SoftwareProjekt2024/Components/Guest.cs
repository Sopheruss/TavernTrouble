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
        _perspectiveManager = perspectiveManager;
        hasOrdered = false;
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
            position = new Vector2(table.position.X, table.position.Y);
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