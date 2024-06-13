using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.SpriteClasses;
using System.Collections.Generic;

namespace SoftwareProjekt2024
{
    internal class CollisionManager
    {
        TileManager _tileManager;
        List<Rectangle> intersections;


        private const int TILESIZE = 32; // Assuming Tilesize in pixel is 32, adjust as necessary


        // load in map from our tilemanager
        public CollisionManager(TileManager tilemanager)
        {
            _tileManager = tilemanager;
        }


        // Hier potentieller Fehler, da CalcPlayerBounds static?
        public (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) CalcPlayerBounds(Player ogerCook)
        {

            Rectangle playerBounds = ogerCook.Rect;

            // Create and calculate bounds
            Rectangle leftBounds = playerBounds;
            leftBounds.X -= 1;

            Rectangle rightBounds = playerBounds;
            rightBounds.X += 1;

            Rectangle upBounds = playerBounds;
            upBounds.Y -= 1;

            Rectangle downBounds = playerBounds;
            downBounds.Y += 1;

            // Return the single bounds as a tuple
            return (leftBounds, rightBounds, upBounds, downBounds);
        }

        public bool CheckCollision(Rectangle playerBounds)
        {

            // grab intersecting tiles
            intersections = GetIntersectingTilesHorizontal(playerBounds);

            foreach (var rect in intersections)
            {
                // handle collisions if the tile position exists in the tile map layer.
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



        // grabs the intersecting tiles for a Rect. This grabs all tile positions where
        // an intersection is __possible__, not if a tile actually exists there.
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



        // This should be the rectangle used for debugging collision, gets drawn/called in GamePlay.cs

        public void DrawDebugRect(SpriteBatch spriteBatch, Rectangle rect, int thickness, Texture2D rectangleTexture)
        {
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Y,
                    rect.Width,
                    thickness
                ),
                Color.White
            );
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Bottom - thickness,
                    rect.Width,
                    thickness
                ),
                Color.White
            );
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X,
                    rect.Y,
                    thickness,
                    rect.Height
                ),
                Color.White
            );
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.Right - thickness,
                    rect.Y,
                    thickness,
                    rect.Height
                ),
                Color.White
            );
        }
    }
}
