using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SoftwareProjekt2024.Game1;

namespace SoftwareProjekt2024.Managers
{
    internal class PerspectiveManager
    {
        public SortedSet<Component> sortedComponents;
        public PerspectiveManager() {
            sortedComponents = new SortedSet<Component>();
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (var component in sortedComponents)
            {
                component.draw(spriteBatch);
            }
        }
    }
}
