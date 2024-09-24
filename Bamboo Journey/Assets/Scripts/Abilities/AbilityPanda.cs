using System.Collections.Generic;
using DG.Tweening;
using GameField;
using GameField.Items;
using Player.Data;
using TMPro;
using UnityEngine;

namespace Abilities
{
    public class AbilityPanda : Ability
    {
        [SerializeField] private TMP_Text _emptyDirtNotFoundText;
        
        private void OnEnable()
        {
            GameFieldAbilitiesUpdater.OnStartPandaAbility += StartDirtyImprove;
        }

        private void OnDisable()
        {
            GameFieldAbilitiesUpdater.OnStartPandaAbility -= StartDirtyImprove;
        }

        private new void Start()
        {
            _amount = PlayerPrefs.GetInt(PlayerDataKeys.AmountPandaDataKey);
            base.Start();
        }

        private void StartDirtyImprove(List<Item> items)
        {
            if (_amount <= 0) return;

            var isUsed = false;

            foreach (var item in items)
            {
                if (item.ItemType == ItemType.Dirt &&
                    item.LevelItem == 0)
                {
                    item.IncreaseLevel();
                    isUsed = true;
                }
            }  

            if (isUsed)
                UseAbility();
            else 
                StartNotFoundDirtAnimation();
        }

        private void StartNotFoundDirtAnimation()
        {
            DOTween.Sequence()
                .Append(_emptyDirtNotFoundText.transform.DOScale(Vector3.one, 1.3f))
                .Append(_emptyDirtNotFoundText.transform.DOScale(Vector3.zero, 1.3f));
        }

        protected override void SetAmountAbilities()
        {
            PlayerPrefs.SetInt(PlayerDataKeys.AmountPandaDataKey, _amount);
        }
    }
}

