﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Managers
{
    public class CollisionManager
    {
        private readonly TileManager _tileManager;
        readonly int halfTile = 16;
        readonly int quarterTile = 8;
        readonly int tileSize = 32;

        public CollisionManager(TileManager tileManager)
        {
            _tileManager = tileManager;
        }

        public bool CheckCollision(Rectangle bounds, PerspectiveManager _perspectiveManager)
        {
            foreach (var tile in _tileManager.collisionLayer)
            {
                Rectangle tileRect;

                /*
                    Calculate the Tiles bounding rectangle
                    Example:
                    tileRect = new Rectangle(
                        (int)tile.Key.X * 32                                             -> adding to this value shifts the left bound to the right
                        , (int)tile.Key.Y * 32                                           -> adding to this value shifts the upper bound downwards
                        , tileSize                                                       -> subtracting from this value shifts the right bound to the left
                        , tileSize );                                                    -> subtracting from this value shifts the lower bound upwards
                 */

                /* Collisions IDs:
                 * upper kitchen equippment: 1
                 * trash can: 2
                 * left Bar: 3
                 * bar: 4
                 * right bar: 5
                 * table: 6 - 9
                 */

                switch ((int)tile.Value)
                {
                    case 1:     //upper kitschen equippment 
                        tileRect = new Rectangle(((int)tile.Key.X * tileSize), ((int)tile.Key.Y * tileSize), tileSize, (tileSize - halfTile));
                        break;
                    case 2:     //trash can 
                        tileRect = new Rectangle(((int)tile.Key.X * tileSize), ((int)tile.Key.Y * tileSize), (tileSize - quarterTile + 2), (tileSize - halfTile));
                        break;
                    case 3:     //left bar
                        tileRect = new Rectangle(((int)tile.Key.X * tileSize) + 16, ((int)tile.Key.Y * tileSize) + (tileSize - quarterTile) + 2, tileSize - 11, quarterTile - 2);
                        break;
                    case 4:     //bar kollision
                        tileRect = new Rectangle(((int)tile.Key.X * tileSize) + 5, ((int)tile.Key.Y * tileSize) + (tileSize - quarterTile) + 2, tileSize - 11, quarterTile - 2);
                        break;
                    case 5:     //right bar
                        tileRect = new Rectangle(((int)tile.Key.X * tileSize) + 5, ((int)tile.Key.Y * tileSize) + (tileSize - quarterTile) + 2, tileSize - 21, quarterTile - 2);
                        break;
                    case 6:     //table left upper -> does collision for whole table
                        tileRect = new Rectangle(((int)tile.Key.X * tileSize) + quarterTile + 3, ((int)tile.Key.Y * tileSize) + tileSize - 2, tileSize + quarterTile, tileSize - quarterTile - 4);
                        break;
                    case 7:
                        tileRect = new Rectangle((int)tile.Key.X * tileSize, (int)tile.Key.Y * tileSize, tileSize - 28, tileSize);
                        break;
                    case 8:
                        tileRect = new Rectangle(((int)tile.Key.X * tileSize) + 28, (int)tile.Key.Y * tileSize, tileSize, tileSize);
                        break;
                    default:    //Generalfall
                        tileRect = new Rectangle((int)tile.Key.X * tileSize, (int)tile.Key.Y * tileSize, tileSize, tileSize);
                        break;
                }

                if (tileRect.Intersects(bounds))
                {
                    return true;
                }
            }

            foreach (Guest guest in _perspectiveManager._guests)
            {
                Rectangle guestRect = new Rectangle((int)guest.position.X + 9, (int)guest.position.Y + 32, guest.Rect.Width - 19, guest.Rect.Height - 15);

                if (guestRect.Intersects(bounds))
                {
                    return true;
                }
            }
            return false;
        }

        internal (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) CalcPlayerBounds(Player player)
        {
            int loweredPlayerBounds = 40;
            int tightenedPlayerBounds = 10;

            // Get the current rectangle representing the player's bounds
            Rectangle playerRect = player.Rect;

            // Calculate the bounding rectangles for collision detection

            // Left bounding rectangle
            Rectangle leftBounds = new Rectangle(
                playerRect.Left + tightenedPlayerBounds,     // Adjusted left side (1 pixel to the left)
                playerRect.Top + loweredPlayerBounds + 1,           // Align with the top of the player
                1,                                             // Width of 1 pixel
                player.height - loweredPlayerBounds - 2           // Height same as player's height
            );

            // Right bounding rectangle
            Rectangle rightBounds = new Rectangle(
                playerRect.Right - tightenedPlayerBounds,       // Right side of the player
                playerRect.Top + loweredPlayerBounds + 1,          // Align with the top of the player
                1,                                            // Width of 1 pixel
                player.height - loweredPlayerBounds - 2          // Height same as player's height
            );

            // Up bounding rectangle
            Rectangle upBounds = new Rectangle(
                playerRect.Left + tightenedPlayerBounds + 2,        // Align with the left side of the player
                playerRect.Top - 1 + loweredPlayerBounds,      // Adjusted up (1 pixel above) + Offset for optics
                player.width - tightenedPlayerBounds * 2 - 3,     // Width same as player's width; oger width != rectangle width
                1                                            // Height of 1 pixel
            );

            // Down bounding rectangle
            Rectangle downBounds = new Rectangle(
                playerRect.Left + tightenedPlayerBounds + 2,        // Align with the left side of the player
                playerRect.Bottom,                             // Bottom side of the player
                player.width - tightenedPlayerBounds * 2 - 3,     // Width same as player's width; oger width != rectangle width
                1                                            // Height of 1 pixel
            );

            // Return all calculated bounding rectangles
            return (leftBounds, rightBounds, upBounds, downBounds);


        }

        public void DrawDebugRect(SpriteBatch spriteBatch, Rectangle rect, int thickness, Texture2D rectangleTexture)
        {
            // upper line of rectangle
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
            // lower line of rectangle
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
            // left line of rectangle
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
            // right line of rectangle
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
