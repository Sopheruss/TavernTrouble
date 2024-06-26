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

        readonly int quarterTileHeight = 8;
        readonly int tileSize = 32;

        List<Rectangle> intersections;

        public InteractionManager(TileManager tilemanager)
        {
            _tileManager = tilemanager;
        }

        public int CheckInteraction(Rectangle bounds)
            // uses collision logic, could also return Strings if preferred
        {
            foreach (var tile in _tileManager.interactionLayer)
            {
                Rectangle tileRect;

                switch ((int)tile.Value) 
                {
                    // cases: if we want to draw rects in different sizes for specific tiles
                    // default: normal rects in tile-size

                    default:
                        tileRect = new Rectangle((int)tile.Key.X * 32, (int)tile.Key.Y * 32, 32, 32);
                        break;
                }

                if (tileRect.Intersects(bounds))
                {
                    return (int)tile.Value; // returns tile ID of intersecting rect to handle interaction for different tile-types later; true
                }
            }
            return 0; // 0 means no possible interaction; false
        }

        public void HandleInteraction(int tileID)
        {
            switch(tileID)
            {
                default:
                    Debug.WriteLine("INTERACTION");
                    break;
            }
        }
    }
}
