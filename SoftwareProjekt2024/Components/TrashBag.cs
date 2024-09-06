using Microsoft.Xna.Framework;
using SoftwareProjekt2024.Managers;

namespace SoftwareProjekt2024.Components
{
    internal class TrashBag : DynamicObject
    {
        public TrashBag(PerspectiveManager perspectiveManager)
            : base(Plate.plain, new Vector2(0, 0), perspectiveManager)
        {
            state = (int)States.TrashBag;
        }
    }
}
