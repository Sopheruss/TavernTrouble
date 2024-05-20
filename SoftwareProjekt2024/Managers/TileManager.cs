﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace SoftwareProjekt2024
{
    public class TileManager
    {
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

        public TileManager(ContentManager content, GraphicsDevice graphicsDevice)
        {
            _tiledMap = content.Load<TiledMap>("Maps/map1");
            _tiledMapRenderer = new TiledMapRenderer(graphicsDevice, _tiledMap);
        }

        public void Update(GameTime gameTime)
        {
            _tiledMapRenderer.Update(gameTime);
        }

        // Draw method now accepts the view matrix
        public void Draw(Matrix viewMatrix)
        {
            _tiledMapRenderer.Draw(viewMatrix);
        }
    }
}