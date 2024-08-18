using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.StaticObjects;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Managers
{
    public class PerspectiveManager
    {
        internal List<Component> _sortedComponents; //Liste aller objekte die in Perspektive relevant sind


        internal List<Component> _tische; //Liste aller Tische
        internal List<Bar> _barFlächen;
        internal List<Component> _nonInteractables;
        internal List<Component> _Interactables;

        internal List<Component> _dynamicObjects; //Liste dynamischer Objekte


        public PerspectiveManager()
        {
            _sortedComponents = new List<Component>(); //erstellt Liste


            _tische = new List<Component>(); //Zugriff auf den 1. Tisch über _perspectiveManager._tische[0]
            _barFlächen = new List<Bar>();
            _nonInteractables = new List<Component>();
            _Interactables = new List<Component>();

            _dynamicObjects = new List<Component>();

        }

        public void draw(SpriteBatch spriteBatch, AnimationManager _animationManager)
        {
            _sortedComponents.Sort(); //sortiert Objekte in Liste nach Y-Werten und Levels

            foreach (var component in _sortedComponents)
            {
                //Debug.WriteLine(component.position.Y - component.getHeight());
                component.draw(spriteBatch, _animationManager); //drawt Objekte in der sortierten Reihenfolge
            }
        }
    }
}