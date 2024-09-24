using System;

namespace GameField.Items
{
    public class Dirt : Item
    {
        public static Action<Points, ItemType> OnImproveDirtToGrass;

        public void ImproveToGrass()
        {
            OnImproveDirtToGrass?.Invoke(Position, ItemType.Grass);
        }
    }
}
