﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.DekoObjects;

internal class Window_Middle : StaticObject
{
    public Window_Middle(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
       : base(texture, position, _dest, _src, perspectiveManager)
    { }
}