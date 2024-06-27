using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Managers
{
    public class PerspectiveManager
    {
        internal List<Component> _sortedComponents; //Liste aller objekte die in Perspektive relevant sind
        internal List<List<Component>> _staticObjects; //Liste von Listen statischer Objekte
        internal List<Component> _tische; //Liste aller Tische

        public PerspectiveManager()
        {
            _sortedComponents = new List<Component>(); //erstellt Liste
            _staticObjects = new List<List<Component>>();  //Zugriff auf den 1. Tisch über _perspectiveManager._staticObjects[0][0]
            _tische = new List<Component>(); //Zugriff auf den 1. Tisch über _perspectiveManager.

            _staticObjects.Add(_tische);
        }

        public void draw(SpriteBatch spriteBatch, AnimationManager _animationManager)
        {
            _sortedComponents.Sort(); //sortiert Objekte in Liste nach Y-Werten

            foreach (var component in _sortedComponents)
            {
                //Debug.WriteLine(component.position.Y - component.getHeight());
                component.draw(spriteBatch, _animationManager); //drawt Objekte in der sortierten Reihenfolge
            }
        }
    }
}
