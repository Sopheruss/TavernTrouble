using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;

namespace SoftwareProjekt2024
{
    internal class InteractionManager
    {
        TileManager _tileManager;
        Player _ogerCook;
        CollisionManager _collisionManager;

        readonly int quarterTileHeight = 8;
        readonly int tileSize = 32;

        List<Rectangle> intersections;

        Rectangle bounds;
        int interactionState;

        public InteractionManager(TileManager tilemanager, Player ogerCook)
        {
            _tileManager = tilemanager;
            _ogerCook = ogerCook;
        }

        public void Update()
        {
            CreateBounds();
            CheckInteraction(bounds);

            if (interactionState == 0)
            {
                //Debug.WriteLine("interaction not possible");
            }
            else
            {
                // Debug.WriteLine("INTERACTION POSSIBLE");
            }
        }

        public int GetInteractionState()
        {
            return interactionState;
        }

        public void CreateBounds()
        {
            bounds = _ogerCook.Rect;
        }

        public void CheckInteraction(Rectangle bounds)
        {
            foreach (var tile in _tileManager.interactionLayer)
            {
                Rectangle tileRect = new Rectangle((int)tile.Key.X * tileSize, (int)tile.Key.Y * tileSize, tileSize, tileSize);

                if (tileRect.Intersects(bounds))
                {
                    interactionState = (int)tile.Value; // returns tile ID of intersecting rect to handle interaction for different tile-types later; true
                    return;
                }
            }
            interactionState = 0; // 0 means no possible interaction; false
        }

        public void HandleInteraction(int tileID)
        {
            switch (tileID)
            {
                default:
                    //Debug.WriteLine("INTERACTION");
                    break;
            }
        }
    }
}
