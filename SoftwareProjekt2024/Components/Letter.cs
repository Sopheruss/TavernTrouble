using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Screens;

namespace SoftwareProjekt2024.Components;

internal class Letter
{
    readonly SpriteBatch _spriteBatch;

    readonly int _midScreenWidth;
    readonly int _midScreenHeight;

    readonly Texture2D _letter;
    readonly string _letterContent;
    readonly Rectangle _letterRect;
    readonly Vector2 _letterSizeForWidth;
    readonly Vector2 _letterSizeForHeight;
    readonly Vector2 _letterSize;

    readonly BitmapFont bmfont;

    public Letter(ContentManager Content, SpriteBatch spriteBatch, int screenWidth, int screenHeight, Vector2 position)
    {
        _spriteBatch = spriteBatch;

        _midScreenWidth = screenWidth / 2;
        _midScreenHeight = screenHeight / 2;

        bmfont = Content.Load<BitmapFont>("Fonts/font_new");


        _letterContent = "Hey Kiddo," +
            "\nsince you are reading this, i am probably dead... " +
            "\nAnyway, someone has to take care of the tavern for me. " +
            "\n So be a charm and take care of it for me, will you." +
            "\nHere are some ground rules, to get started:" +
            "\n\n - speak with guests to take their order" +
            "\n - for recipes take a look in the cook book" +
            "\n - carry only finished food on the plates around" +
            "\n - chop ingredients on the cutting board and prepare them in the cauldron or grill" +
            "\n - if you make a mistake, you can always throw stuff away" +
            "\n - carry the finished food to your customers, don't let them wait!" +
            "\n - be mindful of the time, it quickly runs out before you know it" +
            "\n - let your guests eat in peace, but dont forget to collect what they owe" +
            "\n - and dont forget to clean the tables, otherwise no new guests will come" +
            "\n\nThat should be it, good luck and have fun!" +
            "\nYours truly," +
            "\nGrandma " +
            "\nPS: If you forget some rules you can access the letter in the lower right corner.";

        _letter = Content.Load<Texture2D>("Background/letter");

        _letterSizeForWidth = bmfont.MeasureString(" - chop ingredients on the cutting board and prepare them in the cauldron or grill");
        _letterSizeForHeight = bmfont.MeasureString(_letterContent);

        _letterSize = new Vector2(_letterSizeForWidth.X + 100, _letterSizeForHeight.Y + 50);

        _letterRect = new Rectangle((int)position.X, (int)position.Y, (int)_letterSize.X, (int)_letterSize.Y);
    }

    public void Update()
    {
        var keys = Keyboard.GetState().GetPressedKeys();

        if (keys.Length > 0)
        {
            GamePlay._showLetter = false;
        }
    }

    public void Draw()
    {
        _spriteBatch.Draw(_letter, _letterRect, Color.White);

        _spriteBatch.DrawString(bmfont, _letterContent, new Vector2(_letterRect.X + 40, _letterRect.Y + 30), Color.Black);
    }
}
