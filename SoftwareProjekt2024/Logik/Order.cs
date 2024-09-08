using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.StaticObjects;

namespace SoftwareProjekt2024.Logik
{
    public class Order
    {


        public static Texture2D orderSheet;
        public static Texture2D orderStrip;
        public static BitmapFont bmfont;

        public static Texture2D beer;

        public List<Recipe> recipes;
        public List<Recipe> missingRecipes;
        public int drinksCount;
        public int missingDrinksCount;
        public int wrongComponentsCount;

        public bool isFinished;
        int completedComponents; // abgeschlossenen Komponenten je Bestellung
        private Stopwatch timerBestellung;  // Timer für Bestellung
        private const int timeLimitInSeconds = 180; // 3 Minuten Zeitlimit

        public int tableIDtoOrder;

        public Order(int _drinksCount, List<Recipe> _recipes, int tableID)
        {
            recipes = _recipes;
            missingRecipes = new List<Recipe>(recipes);
            drinksCount = _drinksCount;
            missingDrinksCount = drinksCount;
            completedComponents = 0;

            tableIDtoOrder = tableID;

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
            TimeSpan remainingTime = GetRemainingTime();



            int width = orderSheet.Width * 3;
            int height = orderSheet.Height * 3;
            _spriteBatch.Draw(orderSheet, new Rectangle((int)position.X, (int)position.Y, width, height), Color.White);

            if (remainingTime < TimeSpan.FromSeconds(30))
            {

                _spriteBatch.Draw(orderSheet, new Rectangle((int)position.X, (int)position.Y, width, height), Color.Tomato);
            }

            // Draw Timer:
            Vector2 timerPosition = new Vector2(position.X + 10, position.Y + height - 25);                                                        // Scale
            _spriteBatch.DrawString(bmfont, $"{remainingTime.Minutes:D2}:{remainingTime.Seconds:D2}", timerPosition, Color.Black, 0f, Vector2.Zero, 0.85f, SpriteEffects.None, 0f);

            // Starting position to draw the icons 
            Vector2 iconPosition = new Vector2(position.X + 20, position.Y + 20);

            // Iterate over list of recipes and draw their corresponding textures
            foreach (Recipe recipe in recipes)
            {
                if (recipe.currTexture != null)
                {
                    // Define size of icon (for example, scaling down by half)
                    int iconSize = 25;

                    // Draw texture at current icon position
                    _spriteBatch.Draw(recipe.currTexture, new Rectangle((int)iconPosition.X - 10, (int)iconPosition.Y + 30, iconSize, iconSize), Color.White);

                    // Move icon position (x-coords) for next recipe 
                    iconPosition.X += iconSize + 5;  //padding between icons in pixels
                }
            }



            // Define a new position for drinks
            Vector2 drinkIconPosition = new Vector2(position.X + 20, iconPosition.Y + 100);  // Reset position so no overlap happens

            for (int i = 0; i < drinksCount; i++)
            {
                // Draw drink icon at drinkIconPosition
                _spriteBatch.Draw(beer, new Rectangle((int)drinkIconPosition.X - 10, (int)drinkIconPosition.Y - 15, 25, 25), Color.White);

                // Move the drinkIconPosition (x-coords) for next drink
                drinkIconPosition.X += 25 + 5;
            }


            Table.drawNumber(tableIDtoOrder, 76, 15, _spriteBatch, position, 3);
            //draw call for Recipe icons and timer here
        }
    }
}
