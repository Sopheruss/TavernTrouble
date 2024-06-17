using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Managers
{
    public class CollisionManager
    {
        private readonly TileManager _tileManager;

        public CollisionManager(TileManager tileManager)
        {
            _tileManager = tileManager;
        }

        public bool CheckCollision(Rectangle bounds)
        {
            foreach (var tile in _tileManager.collisionLayer)
            {
                Rectangle tileRect = new Rectangle((int)tile.Key.X * 32, (int)tile.Key.Y * 32, 32, 32);

                if (tileRect.Intersects(bounds))
                {
                    return true;
                }
            }
            return false;
        }

        internal (Rectangle leftBounds, Rectangle rightBounds, Rectangle upBounds, Rectangle downBounds) CalcPlayerBounds(Player player)
        {
            int playerWidth = 30;
            int playerHeight = 60;

            Rectangle playerRect = player.Rect;

            Rectangle leftBounds = new Rectangle(playerRect.Left - 1, playerRect.Top, 1, playerHeight);
            Rectangle rightBounds = new Rectangle(playerRect.Right, playerRect.Top, 1, playerHeight);
            Rectangle upBounds = new Rectangle(playerRect.Left, playerRect.Top - 1, playerWidth, 1);
            Rectangle downBounds = new Rectangle(playerRect.Left, playerRect.Bottom, playerWidth, 1);

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
                    rect.X ,
                    rect.Y ,
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

