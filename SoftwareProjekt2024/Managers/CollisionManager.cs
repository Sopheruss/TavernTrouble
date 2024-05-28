using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;

namespace SoftwareProjekt2024
{
    public class CollisionManager
    {
        private List<Rectangle> _collisionObjects;
        public float _offsetX;                    
        public float _offsetY;                    

        public CollisionManager(TiledMap map, string collisionLayerName, float ogerX, float ogerY)
        {
            _collisionObjects = new List<Rectangle>();
            _offsetX = ogerX;                                            
            _offsetY = ogerY;                                            
            LoadCollisionObjects(map, collisionLayerName);
        }


        private void LoadCollisionObjects(TiledMap map, string collisionLayerName)
        {
            var collisionLayer = map.GetLayer<TiledMapObjectLayer>(collisionLayerName);
            if (collisionLayer != null)
            {
                foreach (var obj in collisionLayer.Objects)
                {
                    if (obj.Name == "spawn")
                    {                                                     
                        //Debug.WriteLine("Obj Pos X: " + obj.Position.X);
                        _offsetX -= obj.Position.X;                     
                        //Debug.WriteLine("Obj Pos Y: " + obj.Position.Y);
                        _offsetY -= obj.Position.Y;
                    }
                    else {
                        var rect = new Rectangle((int)obj.Position.X, (int)obj.Position.Y, (int)obj.Size.Width, (int)obj.Size.Height);
                        _collisionObjects.Add(rect);
                    }
                }
                Debug.WriteLine(_offsetX);
                Debug.WriteLine(_offsetY);
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

