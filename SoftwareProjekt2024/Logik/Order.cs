using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;

namespace SoftwareProjekt2024.Logik
{
    public class Order
    {


        public static Texture2D orderSheet;
        public static Texture2D orderStrip;
        public static BitmapFont bmfont;

        public List<Recipe> recipes;
        public List<Recipe> missingRecipes;
        public int drinksCount;
        public int missingDrinksCount;
        public int wrongComponentsCount;

        public bool isFinished;
        int completedComponents; // abgeschlossenen Komponenten je Bestellung
        private Stopwatch timerBestellung;  // Timer für Bestellung
        private const int timeLimitInSeconds = 120; // 2 Minuten Zeitlimit

        public Order(int _drinksCount, List<Recipe> _recipes)
        {
            recipes = _recipes;
            missingRecipes = new List<Recipe>(recipes);
            drinksCount = _drinksCount;
            missingDrinksCount = drinksCount;
            completedComponents = 0;


            timerBestellung = new Stopwatch();
            timerBestellung.Start();
        }

        public void addRecipe(string recipeName)
        {
            Recipe matchingRecipe = null;
            foreach (Recipe recipe in missingRecipes)
            {
                if (recipe.name == recipeName)
                {
                    matchingRecipe = recipe;
                    missingRecipes.Remove(recipe);
                    if (IsComplete()) isFinished = true;
                    return;
                }
            }
            wrongComponentsCount++;
        }

        internal void addDrink(Mug mug)
        {
            if (mug.isFilled && missingDrinksCount > 0) missingDrinksCount--;
            else if (!mug.isFilled || missingDrinksCount == 0) wrongComponentsCount++;
            if (IsComplete()) isFinished = true;
        }

        public bool IsComplete()
        {
            return missingRecipes.Count == 0 && missingDrinksCount == 0;
        }

        // Gibt Gesamtanzahl Komponenten (Gerichte + Getränk) zurück
        public int TotalComponents()
        {
            return recipes.Count + drinksCount;  // Getränk wird als eine Komponente gezählt
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

        public void draw(SpriteBatch _spriteBatch, Vector2 position)
        {
            int width = orderSheet.Width * 3;
            int height = orderSheet.Height * 3;
            _spriteBatch.Draw(orderSheet, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);

            // Draw Timer:

            TimeSpan remainingTime = GetRemainingTime();
            Vector2 timerPosition = new Vector2(position.X+10, position.Y + height-25);                                                        // Scale
            _spriteBatch.DrawString(bmfont, $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}", timerPosition, Color.Black,0f,Vector2.Zero,0.85f,SpriteEffects.None,0f);


        }
    }
}
