using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using SoftwareProjekt2024.Managers;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024
{   
    internal class CollisionManager
    {
        Player _ogerCook;
        TileManager _tileManager;
        List<Rectangle> intersections;
        int TILESIZE = 32;
        
        public CollisionManager(Player ogerCook, TileManager tilemanager) 
        {
            _ogerCook = ogerCook;
            _tileManager = tilemanager;
            intersections = new();
        }

        public List<Rectangle> getIntersectingTilesHorizontal(Rectangle target)
        {
            List<Rectangle> intersections = new();
            int widthInTiles = (target.Width - (target.Width % TILESIZE)) / TILESIZE;
            int heightInTiles = (target.Height - (target.Height % TILESIZE)) / TILESIZE;
            
            for (int x = 0; x <= widthInTiles; x++)
            {
                for(int y = 0; y <= heightInTiles; y++)
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

        public List<Rectangle> getIntersectingTilesVertical(Rectangle target)
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

        public bool checkCollision(Rectangle playerBounds)
        {
            intersections = getIntersectingTilesHorizontal(playerBounds);
            foreach(var rect in intersections)
            {
                if (_tileManager.collisionLayer.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    return true;
                }
            }

            intersections = getIntersectingTilesVertical(playerBounds);
            foreach (var rect in intersections)
            {
                if (_tileManager.collisionLayer.TryGetValue(new Vector2(rect.X, rect.Y), out int _val))
                {
                    return true;
                }
            }
            return false;
        }
    }
}

