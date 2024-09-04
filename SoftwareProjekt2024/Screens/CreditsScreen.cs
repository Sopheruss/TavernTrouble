using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;
using SoftwareProjekt2024.Components.StaticObjects;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Screens;

internal class CreditsScreen
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly int _midScreenWidth;
    readonly int _midScreenHeight;

    readonly BitmapFont bmfont;

    readonly string _header;
    readonly Vector2 _headerSize;
    private List<string> _credits;
    private List<Vector2> _creditSizes;


    Button _returnButton;
    public CreditsScreen(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
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

        _header = "Credits";
        _headerSize = bmfont.MeasureString(_header);

        _credits = new List<string>
        {
            "Developed by TeamNowak",
            "Produced for Acagamics e.V. - 3D Game Project 2024",
            " ",
            "Music ",
            "'Woodland Fantasy' by Matthew Pablo",
            "'Track 22', from 'The Nine Lives of Er The Cat' by tcarisland",



        };

        // Measure the size of each credit text line
        _creditSizes = new List<Vector2>();
        foreach (var credit in _credits)
        {
            _creditSizes.Add(bmfont.MeasureString(credit));
        }

    }

    public void Update()
    {
        _returnButton.Update();

        if (_returnButton.isClicked || _returnButton._escIsPressed)
        {
            Game1.activeScene = Scenes.MAINMENU;
        }
    }

    public void Draw()
    {
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);


        _returnButton.Draw(_spriteBatch);

        // Draw the header centered
        _spriteBatch.DrawString(bmfont, _header,
            new Vector2(_midScreenWidth - _headerSize.X / 2, 100), Color.Black);

        // Draw the credits, aligned under the header
        float yOffset = 150; // Starting Y position below the header
        const float lineSpacing = 40; // Adjust for spacing between lines

        for (int i = 0; i < _credits.Count; i++)
        {
            string credit = _credits[i];
            Vector2 creditSize = _creditSizes[i];

            // Center the credit line based on the screen width
            _spriteBatch.DrawString(bmfont, credit,
                new Vector2(_midScreenWidth - creditSize.X / 2, yOffset), Color.Black);

            // Move to the next line
            yOffset += lineSpacing;
        }

        _spriteBatch.End();
    }
}
