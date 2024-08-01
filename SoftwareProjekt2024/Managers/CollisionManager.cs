using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Managers
{
    public class CollisionManager
    {
        private readonly TileManager _tileManager;
        readonly int halfTileHeight = 16;
        readonly int quarterTileHeight = 8;
        readonly int tileSize = 32;


        public CollisionManager(TileManager tileManager)
        {
            _tileManager = tileManager;
        }

        public bool CheckCollision(Rectangle bounds)
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
                        , tileSize );                                           -> subtracting from this value shifts the lower bound upwards
                 */

                switch ((int)tile.Value)
                {
                    case 4:     //Fall für den Tisch: kleineres Rectangle, um Kollision erst ab Hälfte des Tisches beginnen zu lassen
                        tileRect = new Rectangle(((int)tile.Key.X * 32) + 5, ((int)tile.Key.Y * 32) + (tileSize - quarterTileHeight) + 2, tileSize - 11, quarterTileHeight - 2);
                        break;
                    case 3:     //linker Barrand
                        tileRect = new Rectangle(((int)tile.Key.X * 32) + 16, ((int)tile.Key.Y * 32) + (tileSize - quarterTileHeight) + 2, tileSize - 11, quarterTileHeight - 2);
                        break;
                    case 5:     //rechter Barrand
                        tileRect = new Rectangle(((int)tile.Key.X * 32) + 5, ((int)tile.Key.Y * 32) + (tileSize - quarterTileHeight) + 2, tileSize - 21, quarterTileHeight - 2);
                        break;
                    default:    //Generalfall
                        tileRect = new Rectangle((int)tile.Key.X * 32, (int)tile.Key.Y * 32, 32, 32);
                        break;
                }

                if (tileRect.Intersects(bounds))
                {
                    return true;
                }
            }
            return false;
        }

        internal (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) CalcPlayerBounds(Player player)
        {
            int loweredPlayerBounds = 40;
            int tightenedPlayerBounds = 3;

            // Get the current rectangle representing the player's bounds
            Rectangle playerRect = player.Rect;

            // Calculate the bounding rectangles for collision detection

            // Left bounding rectangle
            Rectangle leftBounds = new Rectangle(
                playerRect.Left - 1 + tightenedPlayerBounds,     // Adjusted left side (1 pixel to the left)
                playerRect.Top + loweredPlayerBounds,           // Align with the top of the player
                1,                                             // Width of 1 pixel
                player.height - loweredPlayerBounds           // Height same as player's height
            );

            // Right bounding rectangle
            Rectangle rightBounds = new Rectangle(
                playerRect.Right - tightenedPlayerBounds,       // Right side of the player
                playerRect.Top + loweredPlayerBounds,          // Align with the top of the player
                1,                                            // Width of 1 pixel
                player.height - loweredPlayerBounds          // Height same as player's height
            );

            // Up bounding rectangle
            Rectangle upBounds = new Rectangle(
                playerRect.Left + tightenedPlayerBounds,        // Align with the left side of the player
                playerRect.Top - 1 + loweredPlayerBounds,      // Adjusted up (1 pixel above) + Offset for optics
                player.width - tightenedPlayerBounds * 2,     // Width same as player's width; oger width != rectangle width
                1                                            // Height of 1 pixel
            );

            // Down bounding rectangle
            Rectangle downBounds = new Rectangle(
                playerRect.Left + tightenedPlayerBounds,        // Align with the left side of the player
                playerRect.Bottom,                             // Bottom side of the player
                player.width - tightenedPlayerBounds * 2,     // Width same as player's width; oger width != rectangle width
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