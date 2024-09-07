using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.StaticObjects;
using System.Diagnostics;
using System.Linq;

namespace SoftwareProjekt2024.Managers
{
    internal class GameplayLoopManager
    {
        public PerspectiveManager _perspectiveManager;

        Stopwatch _timer;

        public int timebetweenNextGuest = 30;   //in seconds
        public bool newGuestAddedFlag;
        readonly Player _ogerCook;

        int maxGuestPerDifficulty;

        public GameplayLoopManager(PerspectiveManager perspectiveManager, Stopwatch timer, Player ogerCook)
        {
            _perspectiveManager = perspectiveManager;
            _timer = timer;
            newGuestAddedFlag = false;
            _ogerCook = ogerCook;

            maxGuestPerDifficulty = 3;
        }

        public void Update()
        {
            HowManyGuests(_ogerCook.GetDifficulty()); //calculates how many guests are allowed to spawn at the same time, depending on the deifficulty 

            int timeInSeconds = (int)_timer.ElapsedMilliseconds / 1000;
            if (timeInSeconds % timebetweenNextGuest == 0 && !newGuestAddedFlag)
            {
                if (Guest._totalGuestNumber < 8 && tableAvailable() && Guest._totalGuestNumber < maxGuestPerDifficulty) //only as many guests as difficulty allows, but max. 8
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

        public bool tableAvailable()
        {
            foreach (Table table in _perspectiveManager._tables)
            {
                if (!table.isClean() || table.hasGuest())   //pick first clean and free table
                {
                    continue;
                }
                return true;
            }
            return false;
        }

        public void HowManyGuests(int difficultiy)
        {
            switch (difficultiy)
            {
                case 1:
                    maxGuestPerDifficulty = 3;
                    break;
                case 2:
                    maxGuestPerDifficulty = 4;
                    break;
                case 3:
                    maxGuestPerDifficulty = 6;
                    break;
                case 4:
                    maxGuestPerDifficulty = 8;
                    break;
            }
        }

        public void addNewGuest()
        {
            _perspectiveManager._guests.Add(new Guest(Guest.fairyGreen, new Vector2(0, 0), _perspectiveManager, _ogerCook));
            _perspectiveManager._guests.Last().assignTable();
        }
    }
}
