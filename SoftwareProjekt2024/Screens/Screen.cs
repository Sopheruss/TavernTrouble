using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SoftwareProjekt2024.Screens;

public interface IScreen
{
    public void Load();
    public void Update(GameTime gameTime);
    public void Draw(SpriteBatch spriteBatch);
};