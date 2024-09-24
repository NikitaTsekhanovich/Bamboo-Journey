using System.Collections.Generic;

namespace GameField.Items.Properties
{
    public interface IHaveAbilitiesPositions : IHavePositions
    {
        public List<Points> PositionsAbilities { get; }
    }
}

