using SoftwareProjekt2024.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareProjekt2024.Managers
{
    public class GameManager
    {
        private Coin _coin;
        private Hero _hero;

        public void Init()
        {
            _coin = new(new(300, 300));
            _hero = new();
        }

        public void Update()
        {
            InputManager.Update();
            _coin.Update();
            _hero.Update();
        }

        public void Draw()
        {
            _coin.Draw();
            _hero.Draw();
        }
    }
}
