using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Managers
{
    public class PerspectiveManager
    {
        internal List<Component> _sortedComponents; //Liste aller objekte die in Perspektive relevant sind
        internal List<Component> _staticObjects; //unused for now

        public PerspectiveManager()
        {
            _sortedComponents = new List<Component>(); //erstellt Liste

        }

        public void draw(SpriteBatch spriteBatch, AnimationManager _animationManager)
        {
            _sortedComponents.Sort(); //sortiert Objekte in Liste nach Y-Werten

            foreach (var component in _sortedComponents)
            {
                component.draw(spriteBatch, _animationManager); //drawt Objekte in der sortierten Reihenfolge
            }
        }
    }
}
