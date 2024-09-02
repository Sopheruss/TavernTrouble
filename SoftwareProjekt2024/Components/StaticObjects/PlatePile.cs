﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components.StaticObjects
{
    internal class PlatePile : StaticObject
    {
        public PlatePile(Texture2D texture, Vector2 position, Rectangle _dest, Rectangle _src, PerspectiveManager perspectiveManager)
        : base(texture, position, _dest, _src, perspectiveManager)
        { }

        public override int getHeight()
        {
            return dest.Height - 10;
        }

        public static bool AllowedInteraction(Player _ogerCook)
        {
            return _ogerCook.inventoryIsEmpty();
        }

        public void HandleInteraction()
        {

        }
    }
}
