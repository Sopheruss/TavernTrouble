using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Components;
using System.Collections.Generic;

namespace SoftwareProjekt2024
{
    internal class CollisionManager
    {
        TileManager _tileManager;
        List<Rectangle> intersections;

        private const int TILESIZE = 32; // Assuming TILESIZE is 32, adjust as necessary

        public CollisionManager(TileManager tilemanager)
        {
            _tileManager = tilemanager;
        }

        public (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) CalcPlayerBounds(Player ogerCook)
        {
            int halftileOffset = 16;              //half a tile -> 32px / 2 = 16px
            int halfOgerOffsetX = 19 / 2;        //player rectangle draws in the middle, offset to left edge
            int halfOgerOffsetY = 32 / 2;        //player rectangle draws in the middle, offset to left top
            int cosmeticOffsetX = 5;            //(hardcoding)
            int cosmeticOffsetY = 4;           //(hardcoding)

            Rectangle playerBounds = ogerCook.Rect;
            playerBounds.X -= (halftileOffset + halfOgerOffsetX + cosmeticOffsetX);
            playerBounds.Y -= (halftileOffset + halfOgerOffsetY + cosmeticOffsetY);

            // Create and calculate bounds
            Rectangle leftBounds = playerBounds;
            leftBounds.X -= 1;

            Rectangle rightBounds = playerBounds;
            rightBounds.X += playerBounds.Width; // +width because manager tracks left side of rectangle (dunno why)

            Rectangle upBounds = playerBounds;
            upBounds.Y -= 1;

            Rectangle downBounds = playerBounds;
            downBounds.Y += halftileOffset;

            // Return the single bounds as a tuple
            return (leftBounds, rightBounds, upBounds, downBounds);
        }

        public bool CheckCollision(Rectangle playerBounds)
        {
            intersections = GetIntersectingTilesHorizontal(playerBounds);
            foreach (var rect in intersections)
            {
                if (_tileManager.collisionLayer.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    return true;
                }
            }

            intersections = GetIntersectingTilesVertical(playerBounds);
            foreach (var rect in intersections)
            {
                if (_tileManager.collisionLayer.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    return true;
                }
            }
            return false;
        }

        public List<Rectangle> GetIntersectingTilesHorizontal(Rectangle target)
        {
            List<Rectangle> intersections = new();
            int widthInTiles = (target.Width - (target.Width % TILESIZE)) / TILESIZE;
            int heightInTiles = (target.Height - (target.Height % TILESIZE)) / TILESIZE;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersections.Add(new Rectangle(
                        (target.X + x * TILESIZE) / TILESIZE,
                        (target.Y + y * TILESIZE - 1) / TILESIZE,
                        TILESIZE,
                        TILESIZE
                    ));
                }
            }
            return intersections;
        }

        public List<Rectangle> GetIntersectingTilesVertical(Rectangle target)
        {
            List<Rectangle> intersections = new();
            int widthInTiles = (target.Width - (target.Width % TILESIZE)) / TILESIZE;
            int heightInTiles = (target.Height - (target.Height % TILESIZE)) / TILESIZE;

            for (int x = 0; x <= widthInTiles; x++)
            {
                for (int y = 0; y <= heightInTiles; y++)
                {
                    intersections.Add(new Rectangle(
                        (target.X + x * TILESIZE - 1) / TILESIZE,
                        (target.Y + y * TILESIZE) / TILESIZE,
                        TILESIZE,
                        TILESIZE
                    ));
                }
            }
            return intersections;
        }
    }
}
