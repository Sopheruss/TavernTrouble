using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components;

internal class NPC : Component
{
    public NPC(Texture2D texture, Vector2 position, PerspectiveManager perspectiveManager) : base(texture, position, perspectiveManager)
    {
    }
}
