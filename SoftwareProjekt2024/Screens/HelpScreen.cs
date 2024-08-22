using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

internal class HelpScreen
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int _midScreenWidth;
    readonly int _midScreenHeight;

    readonly Texture2D _letter;
    readonly string _letterContent;
    readonly Rectangle _letterRect;
    readonly Vector2 _letterSizeForWidth;
    readonly Vector2 _letterSizeForHeight;

    readonly BitmapFont bmfont;

    readonly Button _returnButton;
    public HelpScreen(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        _midScreenWidth = screenWidth / 2;
        _midScreenHeight = screenHeight / 2;

        _returnButton = new Button(
            Content.Load<Texture2D>("Buttons/returnButton"),
            Content.Load<Texture2D>("Buttons/returnButtonHovering"),
            new Vector2(screenWidth - 70, screenHeight - 70));

        bmfont = Content.Load<BitmapFont>("Fonts/font_new");


        _letterContent = "Hey Kiddo," +
            "\nsince you are reading this, i am probably dead... " +
            "\nAnyway, someone has to take care of the tavern for me." +
            "\nBe a charm and take care of it for me, will you." +
            "\nHere are some ground rules, to get started:" +
            "\n\n - for recipes take a look in the cook book" +
            "\n - don't place raw ingredients on the counter tops, that's unhygienic " +
            "\n - carry only finished food on the plates around" +
            "\n - chop ingredients on the cutting board and prepare them in the cauldron or grill" +
            "\n - if you make a mistake, you can always throw stuff away" +
            "\n - carry the finished food to your customers, don't let them wait!" +
            "\n - be mindful of the time, it quickly runs out before you know it" +
            "\n\nThat should be it, good luck and have fun!" +
            "\nYours truly," +
            "\nGrandma " +
            "\n\nPS: If you forget some rules you can enter the letter in the lower right corner.";
        _letterSizeForWidth = bmfont.MeasureString(" - chop ingredients on the cutting board and prepare them in the cauldron or grill");
        _letterSizeForHeight = bmfont.MeasureString(_letterContent);
        _letter = Content.Load<Texture2D>("Background/letter");
        _letterRect = new Rectangle(_midScreenWidth - 85 - (int)_letterSizeForWidth.X / 2, 10, (int)_letterSizeForWidth.X + 100, (int)_letterSizeForHeight.Y + 50);

    }

    public void Update()
    {
        _returnButton.Update();

        if (_returnButton.isClicked || _returnButton._escIsPressed)
        {
            _game.activeScene = Scenes.GAMEPLAY;
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(_letter, _letterRect, Color.White);

        _spriteBatch.DrawString(bmfont, _letterContent, new Vector2(_letterRect.X + 40, _letterRect.Y + 30), Color.Black);

        _returnButton.Draw(_spriteBatch);

        _spriteBatch.End();
    }
}
