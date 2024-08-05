using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Components;

namespace SoftwareProjekt2024.Screens;

internal class CookBookScreen
{
    readonly Game1 _game;
    readonly SpriteBatch _spriteBatch;

    readonly Button _returnButton;

    readonly Texture2D _cookBookRecipes;
    readonly Rectangle _cookBookRecipeRect;
    public CookBookScreen(ContentManager Content, int screenWidth, int screenHeight, Game1 game, SpriteBatch spriteBatch)
    {
        _game = game;
        _spriteBatch = spriteBatch;

        _returnButton = new Button(
            Content.Load<Texture2D>("Buttons/returnButton"),
            Content.Load<Texture2D>("Buttons/returnButtonHovering"),
            new Vector2(screenWidth - 70, screenHeight - 70));

        _cookBookRecipes = Content.Load<Texture2D>("Background/cookBookRecipes");
        _cookBookRecipeRect = new Rectangle(0, 0, screenWidth, screenHeight);
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
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp); // to make sharp images while scaling

        _spriteBatch.Draw(_cookBookRecipes, _cookBookRecipeRect, Color.White);

        _returnButton.Draw(_spriteBatch);

        _spriteBatch.End();

    }
}
