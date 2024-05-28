using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using SoftwareProjekt2024.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SoftwareProjekt2024.Game1;

namespace SoftwareProjekt2024.Managers
{
    internal class PerspectiveManager
    {
        public List<Component> _sortedComponents;

        public PerspectiveManager() {
            _sortedComponents = new List<Component>();
        }

        public void draw(SpriteBatch spriteBatch, AnimationManager _animationManager)
        {
            _sortedComponents.Sort();

            foreach (var component in _sortedComponents)
            {
                component.draw(spriteBatch, _animationManager);
            }
        }
    }
}
