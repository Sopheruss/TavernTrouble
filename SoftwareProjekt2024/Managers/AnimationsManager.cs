using SoftwareProjekt2024.Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareProjekt2024.Managers
{
    public class AnimationsManager
    {
        private readonly Dictionary<object, Animation> _animation = new();
        private object _lastKey; 

        public void AddAnimation(object key, Animation animation)
        {
            _animation.Add(key, animation);
            _lastKey ??= key; 
        }

        public void Update (object key)
        {
            if (_animation.ContainsKey(key))
            {
                _animation[key].Start();
                _animation[key].Update();
                _lastKey = key;
            } 
            else
            {
                _animation[_lastKey].Stopp();
                _animation[_lastKey].Reset();
            }
        }

        public void Draw(Vector2 position)
        {
            _animation[_lastKey].Draw(position);
        }
    }
}
