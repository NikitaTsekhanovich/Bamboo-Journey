using System;
using System.Collections.Generic;
using GameField.Items.Properties;
using UnityEngine;

namespace GameField.Items
{
    public class Grass : Item, IHaveAbilitiesPositions
    {
        [SerializeField] private List<Points> _positionsAbilities = new ();
        [SerializeField] private Fruit _fruit;
        [SerializeField] private AudioSource _eatingFruitSound;
        private bool _hasFruit;
        private Fruit fruitClone;

        private const float OffsetSpawnFruit = 28f;

        public List<Points> PositionsAbilities => _positionsAbilities;
        public bool HasFruit => _hasFruit;

        public static Action<int> OnIncreaseCoins;

        public void GetFood()
        {
            if (!_hasFruit)
            {
                _hasFruit = true;
                InstantiateFruit();
            }
        }

        public void DoFeed(Item neighbor, Transform parentForAbilities)
        {   
            if (fruitClone == null)
                InstantiateFruit();

            _eatingFruitSound.Play();
            OnIncreaseCoins?.Invoke(LevelItem + neighbor.LevelItem);
            fruitClone.transform.SetParent(parentForAbilities);
            fruitClone.StartFeed(neighbor);
            fruitClone = null;
            _hasFruit = false;
        }

        private void InstantiateFruit()
        {
            fruitClone = Instantiate(_fruit, transform);
            fruitClone.transform.localPosition = new Vector3(
                OffsetSpawnFruit,
                -OffsetSpawnFruit,
                transform.localPosition.z);
            ItemAnimationHandler.StartSpawnFruitAnimation(fruitClone);
        }
    }
}
