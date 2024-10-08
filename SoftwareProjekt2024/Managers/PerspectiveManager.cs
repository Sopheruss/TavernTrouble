﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.StaticObjects;
using SoftwareProjekt2024.Logik;
using SoftwareProjekt2024.Screens;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Managers;

public class PerspectiveManager
{
    internal List<Component> _sortedComponents; //Liste aller objekte die in Perspektive relevant sind

    internal List<Table> _tables; //Liste aller Tische
    internal List<Bar> _barFlächen;
    internal List<Workstation> _workstations;
    internal List<Cuttingboard> _cuttingBoards;
    internal List<Component> _nonInteractables;
    internal List<Component> _Interactables;
    internal List<Component> _dekoObjects;

    internal List<Component> _dynamicObjects; //Liste dynamischer Objekte

    internal List<Guest> _guests;

    internal List<Order> activeOrders;

    public PerspectiveManager()
    {
        _sortedComponents = new List<Component>(); //erstellt Liste


        _tables = new List<Table>(); //Zugriff auf den 1. Tisch über _perspectiveManager._tische[0]
        _barFlächen = new List<Bar>();
        _workstations = new List<Workstation>();
        _cuttingBoards = new List<Cuttingboard>();
        _nonInteractables = new List<Component>();
        _Interactables = new List<Component>();
        _dekoObjects = new List<Component>();

        _dynamicObjects = new List<Component>();

        _guests = new List<Guest>();

        activeOrders = new List<Order>();
    }

    public void drawOrders(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Order.orderStrip, GamePlay._orderStripRect, Color.White); //40 * 40
        Vector2 orderPosition = new Vector2(61, 20);    //Startposition
        foreach (Order order in activeOrders)
        {
            order.draw(spriteBatch, orderPosition);
            orderPosition.X += 128;
        }
    }

    public void draw(SpriteBatch spriteBatch)
    {
        _sortedComponents.Sort(); //sortiert Objekte in Liste nach Y-Werten und Levels

        foreach (var component in _sortedComponents)
        {
            component.draw(spriteBatch); //drawt Objekte in der sortierten Reihenfolge
        }
    }
}