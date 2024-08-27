using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.BitmapFonts;
using SoftwareProjekt2024.Components;

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

        _header = "Credits:";
        _headerSize = bmfont.MeasureString(_header);
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

        _spriteBatch.DrawString(bmfont, _header, new Vector2(_midScreenWidth - _headerSize.X / 2, 100), Color.Black);

        _spriteBatch.End();
    }
}
