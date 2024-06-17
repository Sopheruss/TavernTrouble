using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;

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



        public (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) CalcPlayerBounds(Player ogerCook)
        {

            Rectangle playerBounds = ogerCook.Rect;

            // Create and calculate bounds
            // Certain offsets are added to our bounds so that the collision __looks__ okay-ish
            // Collision implementation has certain features/checks missing/bugged so this is a dirty workaround
            // clipping  trough tile still possible, see issue below (DrawDebugRectangle)


            Rectangle leftBounds = playerBounds;
            leftBounds.X -= 34;

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
                // "Does our collisionLayer contain a tile at this position?"
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
                    // adds Tiles that are intersected to a List as a Rectangle
                    // this rectangle has to be in tile coordinates, not in pixel (!)
                    // eg. if player is at pixel-pos (32,32) then he is at tile-pos (1,1)
                    // values truncate down to nearest tile, so if player at pixel-pos (43,37), still at tile-pos (1,1)
                    // maybe there is a discrepancy between ogerCook-rect being in pixel and we checking in tile?

                    intersections.Add(new Rectangle(
                        (target.X + x * TILESIZE) / TILESIZE,       //X-Position in Tilesize 
                        (target.Y + y * TILESIZE - 1) / TILESIZE,   //Y-Position in Tilesize
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
        // for collision, it seems that only the lower half of right side of rectangle is used ???
        // something is NOT right with our collision implementation... I think, the rectangle used for actual collision is drawn to our lower right side
        // to check: remove values substracted from rect.x and rect.y

        public void DrawDebugRect(SpriteBatch spriteBatch, Rectangle rect, int thickness, Texture2D rectangleTexture)
        {
            // upper line of rectangle
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X - 32,
                    rect.Y - 32,
                    rect.Width,
                    thickness
                ),
                Color.White
            );
            // lower line of rectangle
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X - 32,
                    rect.Bottom - thickness - 32,
                    rect.Width,
                    thickness
                ),
                Color.White
            );
            // left line of rectangle
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.X - 32,
                    rect.Y - 32,
                    thickness,
                    rect.Height
                ),
                Color.White
            );
            // right line of rectangle
            spriteBatch.Draw(
                rectangleTexture,
                new Rectangle(
                    rect.Right - thickness - 32,
                    rect.Y - 32,
                    thickness,
                    rect.Height
                ),
                Color.White
            );
        }
    }
}
