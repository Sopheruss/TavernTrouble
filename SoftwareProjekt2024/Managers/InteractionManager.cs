using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024
{
    internal class InteractionManager
    {
        TileManager _tileManager;
        List<Rectangle> intersections;
        int TILESIZE = 32;

        public InteractionManager(TileManager tilemanager)
        {
            _tileManager = tilemanager;
        }

        public bool CheckInteraction(Rectangle playerBounds)
        {
            intersections = _tileManager.getIntersectingTilesHorizontal(playerBounds);
            foreach (var rect in intersections)
            {                 //.interactionLayer !!!
                if (_tileManager.collisionLayer.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    return true;
                }
            }

            intersections = _tileManager.getIntersectingTilesVertical(playerBounds);
            foreach (var rect in intersections)
            {                 //.interactionLayer !!!
                if (_tileManager.collisionLayer.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
