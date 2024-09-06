using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Components;
using System.Diagnostics;
using System.Linq;

namespace SoftwareProjekt2024.Managers
{
    internal class GameplayLoopManager
    {
        public PerspectiveManager _perspectiveManager;

        Stopwatch _timer;

        public int timebetweenNextGuest = 3;   //in seconds
        public bool newGuestAddedFlag;
        Player _ogerCook;

        public GameplayLoopManager(PerspectiveManager perspectiveManager, Stopwatch timer, Player ogerCook)
        {
            _perspectiveManager = perspectiveManager;
            _timer = timer;
            newGuestAddedFlag = false;
            _ogerCook = ogerCook;
        }

        public void Update()
        {
            int timeInSeconds = (int)_timer.ElapsedMilliseconds / 1000;
            if (timeInSeconds % timebetweenNextGuest == 0 && !newGuestAddedFlag)
            {
                if (Guest._totalGuestNumber < 8) //only as many guests as table 
                {
                    addNewGuest();
                    newGuestAddedFlag = true;
                }
            }
            if (timeInSeconds % timebetweenNextGuest == 1)  // just to prevent adding multiple guests in the same second,
            {                                               // probably better implementation possible
                newGuestAddedFlag = false;
            }
        }


        public void addNewGuest()
        {
            _perspectiveManager._guests.Add(new Guest(Guest.fairyGreen, new Vector2(0, 0), _perspectiveManager, _ogerCook));
            _perspectiveManager._guests.Last().assignTable();
        }
    }
}
