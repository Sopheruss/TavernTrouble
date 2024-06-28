using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024
{
    /* Folgender Code ist nur Template-Work-In-Progress */

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
            CheckInteraction(bounds);

            if (interactionState == 0)
            {
                Debug.WriteLine("interaction not possible");
            }

            if (interactionState != 0)
            {
                Debug.WriteLine("INTERACTION POSSIBLE");
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
                    Debug.WriteLine("INTERACTION");
                    break;
            }
        }
    }
}
