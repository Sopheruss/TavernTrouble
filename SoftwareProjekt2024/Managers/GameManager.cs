using SoftwareProjekt2024.Managers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwareProjekt2024.Managers
{
    public class GameManager
    {
        private Oger_Cook _ogerCook;
        public void Init()
        {
            _ogerCook = new Oger_Cook();
        }

        public void Update()
        {
            InputManager.Update();
            _ogerCook.Update();
        }

        public void Draw()
        {
            _ogerCook.Draw();
        }
    }
}
