using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System;

namespace SoftwareProjekt2024.Components;

internal class Component : SpriteClasses.ScaledSprite, IComparable<Component> //erbt von ScaledSprite, implementiert Comparable
{
    public enum States
    {
        Undefined,
        Empty,
        Plate,
        PlateWFries,
        PlateWBurger,
        Bun,
        Meat,
        Salad,
        Potato,
        PlateWSaladMeat,
        PlateWSaladBun,
        PlateWMeatBun
    }

    public int state;
    public Component(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position)
    {
        //bei Erstellung von Components werden sie in Liste geaddet -> jedoch ab jetzt keine dynamic objects, daher listenaufruf in kindklassen-konstruktoren
    }

    public virtual int getHeight()
    {
        return 0;
    }

    public int CompareTo(Component other) //sortiere nach Y Werten + Höhe -> lower bounds
    {
        if (this.position.Y + this.getHeight() < other.position.Y + other.getHeight()) return -1;
        if (this.position.Y + this.getHeight() == other.position.Y + other.getHeight()) return 0;
        return 1;
    }

    public virtual void draw(SpriteBatch spriteBatch) { }
}