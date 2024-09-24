using System.Collections.Generic;
using GameField;
using GameField.Items;
using Player.Data;
using TMPro;
using UnityEngine;
using DG.Tweening;

namespace Abilities
{
    public class AbilityBamboo : Ability
    {
        [SerializeField] private TMP_Text _emptyGrassNotFoundText;

        private new void Start()
        {
            _amount = PlayerPrefs.GetInt(PlayerDataKeys.AmountBambooDataKey);
            base.Start();
        }

        private void OnEnable()
        {
            GameFieldAbilitiesUpdater.OnStartBambooAbility += StartGrassImprove;
        }

        private void OnDisable()
        {
            GameFieldAbilitiesUpdater.OnStartBambooAbility -= StartGrassImprove;
        }

        private void StartGrassImprove(List<Item> items)
        {
            if (_amount <= 0) return;

            var isUsed = false;

            foreach (var item in items)
            {
                if (item.ItemType == ItemType.Grass &&
                    item.LevelItem == 0)
                {
                    item.IncreaseLevel();
                    isUsed = true;
                }
            }  

            if (isUsed)
                UseAbility();
            else 
                StartNotFoundGrassAnimation();
        }

        private void StartNotFoundGrassAnimation()
        {
            DOTween.Sequence()
                .Append(_emptyGrassNotFoundText.transform.DOScale(Vector3.one, 1.3f))
                .Append(_emptyGrassNotFoundText.transform.DOScale(Vector3.zero, 1.3f));
        }

        protected override void SetAmountAbilities()
        {
            PlayerPrefs.SetInt(PlayerDataKeys.AmountBambooDataKey, _amount);
        }
    }
}

