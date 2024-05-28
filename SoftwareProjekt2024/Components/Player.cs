using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components;

internal class Player : Component
{
    
    public Player(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)

    {
        
    }

    public override void Update()
    {
        base.Update();
    }
}
