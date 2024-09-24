using System;
using System.Collections.Generic;
using GameField.Items.Properties;
using UnityEngine;

namespace GameField.Items
{
    public class Water : Item, IHaveAbilitiesPositions
    {
        [SerializeField] private List<Points> _positionsAbilities = new();
        [SerializeField] private GameObject _waterDropAnimation;

        public List<Points> PositionsAbilities => _positionsAbilities;

        public static Action<List<Item>> OnWaterDrop;

        public void DoWatering(List<Item> neighboringItems)
        {
            OnWaterDrop?.Invoke(neighboringItems);
        }
    }
}

