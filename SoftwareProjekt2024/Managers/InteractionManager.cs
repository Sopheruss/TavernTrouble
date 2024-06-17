using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Components;
using Microsoft.Xna.Framework.Input;

namespace SoftwareProjekt2024
{
    /* Folgender Code ist nur Template-Work-In-Progress */

    internal class InteractionManager
    {
        TileManager _tileManager;
        CollisionManager _collisionManager;
        List<Rectangle> intersections;

        public InteractionManager(TileManager tilemanager)
        {
            _tileManager = tilemanager;
        }

    }
}
