using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareProjekt2024.Managers.Models
{
    public class Oger_Cook
    {
        private Vector2 _position;
        private readonly float _speed = 200f;
        private readonly AnimationsManager _animations = new(); 

        public Oger_Cook()
        {
            //TODO: add spritesheet 
            var ogerTexture = Globals.ContentManager.Load<Texture2D>("oger_cook_spritesheet");
            //direction left -> A; might need to change 8 to proper size 
            _animations.AddAnimation(new Vector2(-1, 0), new(ogerTexture, 8, 8, 0.1f, 1));
            //direction right -> D
            _animations.AddAnimation(new Vector2(1, 0), new(ogerTexture, 8, 8, 0.1f, 2));
            //direction up -> W
            _animations.AddAnimation(new Vector2(0, -1), new(ogerTexture, 8, 8, 0.1f, 3));
            //direction down -> S
            _animations.AddAnimation(new Vector2(0, 1), new(ogerTexture, 8, 8, 0.1f, 4));
        }

        public void Update()
        {
            _animations.Update(InputManager.Directions);
        }

        public void Draw()
        {
            _animations.Draw(_position);
        }
    }
}
