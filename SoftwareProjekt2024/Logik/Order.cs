using System.Collections.Generic;

namespace SoftwareProjekt2024.Logik
{
    public class Order
    {
        public List<Recipe> recipes;
        public bool hasDrink;
        int completedComponents; // abgeschlossenen Komponenten je Bestellung
        public Order(bool _hasDrink, List<Recipe> _recipes)
        {
            recipes = _recipes;
            hasDrink = _hasDrink;
            completedComponents = 0;
        }



        // Gibt Gesamtanzahl Komponenten (Gerichte + Getränk) zurück
        public int TotalComponents()
        {
            return recipes.Count + (hasDrink ? 1 : 0);  // Getränk wird als eine Komponente gezählt
        }



        // Aktualisiert Fortschritt
        public void CompleteComponent()
        {
            if (completedComponents < TotalComponents())
            {
                completedComponents++;
            }
        }

        public int GetRewardPoints()
        {
            if (completedComponents == 0)
                return 0;

            // Belohnung basierend Verhältnis abgeschlossene Komponenten zur Gesamtzahl
            int rewardPerComponent = 10;
            return completedComponents * rewardPerComponent;

        }

        // Überprüfen, ob  Bestellung abgeschlossen 
        public bool IsCompleted()
        {
            return completedComponents == TotalComponents();
        }

        // Ruhmpunkte berechnen basierend auf Punkten
        public float GetFamePoints(int totalPoints)
        {
            return totalPoints / 4.0f;
        }

    }
}
