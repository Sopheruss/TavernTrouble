using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;
using System.Collections.Generic;

namespace SoftwareProjekt2024.Components;

internal class Bar : StaticObject
{
    public List<Component> barContents;
    public Bar(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
    {
        barContents = new List<Component>();
        state = (int)States.Empty;
    }

    public bool isEmpty()
    {
        return barContents.Count == 0;
    }
}
