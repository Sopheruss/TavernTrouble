using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;

namespace SoftwareProjekt2024
{
    public class CollisionManager
    {
        private List<Rectangle> _collisionObjects;
        public int _offsetX;                    
        public int _offsetY;                    

        public CollisionManager(TiledMap map, string collisionLayerName)
        {
            _collisionObjects = new List<Rectangle>();
            LoadCollisionObjects(map, collisionLayerName);
            _offsetX = 605 - 47;                                            //hardcoded for the beginning
            _offsetY = 449 - 110;                                           //hardcoded for the beginning
        }

        private void LoadCollisionObjects(TiledMap map, string collisionLayerName)
        {
            var collisionLayer = map.GetLayer<TiledMapObjectLayer>(collisionLayerName);

            if (collisionLayer != null)
            {
                foreach (var obj in collisionLayer.Objects)
                {
                    var rect = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.Size.Width, (int)obj.Size.Height);
                    _collisionObjects.Add(rect);
                }
            }
        }

        public bool CheckCollision(Rectangle playerBounds)
        {
            foreach (var rect in _collisionObjects)
            {
                if (playerBounds.Intersects(rect))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

