using System.Linq;
﻿using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace SoftwareProjekt2024.Logik
{
    public class Order
    {

        // for future reference:
        // public static Order CurrentOrder { get; set; }

        public List<Recipe> recipes;
        public bool hasDrink;
        public bool isFinished;
        int completedComponents; // abgeschlossenen Komponenten je Bestellung
        private Stopwatch timerBestellung;  // Timer für Bestellung
        private const int timeLimitInSeconds = 120; // 2 Minuten Zeitlimit

        public Order(bool _hasDrink, List<Recipe> _recipes)
        {
            recipes = _recipes;
            hasDrink = _hasDrink;
            completedComponents = 0;


            timerBestellung = new Stopwatch();
            timerBestellung.Start();
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
            if (completedComponents == 0 || IsTimeUp())
                return 0;

            // Verhältnis abgeschlossene Komponenten zur Gesamtzahl
            int rewardPerComponent = 10;
            return completedComponents * rewardPerComponent;
        }



        // Überprüfen, ob  Bestellung abgeschlossen 
        public bool IsCompleted()
        {
            return completedComponents == TotalComponents() || IsTimeUp();
        }



        // Ruhmpunkte berechnen basierend auf Punkten
        public float GetFamePoints(int totalPoints)
        {
            if (IsTimeUp())
            {
                return 0.0f;  // Keine Ruhmpunkte bei abgelaufener Zeit
            }

            return totalPoints / 2.0f;
        }

        public bool Equals(Order other)
        {
            List<Recipe> myRecipes = recipes;
            List<Recipe> otherRecipes = other.recipes;
            foreach (Recipe recipe in myRecipes)
            {
                int matchingRecipeIndex = otherRecipes.FindIndex(t => t.name == recipe.name);
                if (matchingRecipeIndex == -1) return false;
                else
                {

                    otherRecipes.RemoveAt(matchingRecipeIndex);
                }
            }
            return !otherRecipes.Any() && hasDrink == other.hasDrink;
            //does not work for now
        }

        //Überprüfen ob Zeitlimit abgelaufen ist
        public bool IsTimeUp()
        {
            // Zeitlimit von 2 Minuten erreicht?
            return timerBestellung.Elapsed.TotalSeconds >= timeLimitInSeconds;
        }

        // Timer stoppen (falls nötig)
        public void StopTimer()
        {
            timerBestellung.Stop();
        }

        // Somehow need to draw it on OrderSheet... in Gameplay...
        public TimeSpan GetRemainingTime()
        {
            if (IsTimeUp())
                return TimeSpan.Zero;

            return TimeSpan.FromSeconds(timeLimitInSeconds) - timerBestellung.Elapsed;
        }
    }
}
