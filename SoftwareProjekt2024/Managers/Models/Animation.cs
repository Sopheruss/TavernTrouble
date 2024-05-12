using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareProjekt2024.Managers.Models
{
    public class Animation
    {
        private readonly Texture2D _texture; //texture of model 
        private readonly List<Rectangle> _sourceRectangle = new(); //bounding rectangles for each frame 
        private readonly int _frames; //count of frames in animation 
        private int _frame; //index of current frame 
        private readonly float _frameTime; //time taken to change frames 
        private float _frameTimeLeft;
        private bool _active = true; //switch to turn animation on/off 

        public Animation (Texture2D texture, int framesX, int framesY, float frameTime, int row = 1)
        {
            _texture = texture;
            _frameTime = frameTime;
            _frameTimeLeft = _frameTime;
            var frameWidth = _texture.Width / framesX;
            var frameHeight = _texture.Height / framesY; 

            for (int i = 0; i < _frames; i++)
            {
                _sourceRectangle.Add(new(i*frameWidth, (row-1) * frameHeight, frameWidth, frameHeight));
            }
        }
        public void Stopp()
        {
            _active = false;
        }
        public void Start ()
        {
            _active = true;
        }
        
        public void Reset()
        {
            _frame = 0;
            _frameTimeLeft = _frameTime; 
        }

        public void Update()
        {
            if (!_active) return;

            _frameTimeLeft -= Globals.TotalSeconds; 

            if (_active)
            {
                _frameTimeLeft += _frameTime;
                _frame = (_frame + 1) % _frames; 
            }
        }

        public void Draw(Vector2 position)
        {
            Globals.SpriteBatch.Draw(_texture, position, _sourceRectangle[_frame], Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, 1);
        }
    }
}
