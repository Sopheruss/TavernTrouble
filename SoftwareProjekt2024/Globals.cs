using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareProjekt2024
{
    public static class Globals
    {
        public static float TotalSeconds { get; set; }
        public static ContentManager ContentManager { get; set;}
        public static SpriteBatch SpriteBatch { get; set;}

        public static void Update(GameTime gameTime)
        {
            TotalSeconds = (float) gameTime.ElapsedGameTime.TotalSeconds;
        }
    }
}
