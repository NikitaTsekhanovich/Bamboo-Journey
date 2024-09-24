using System;
using System.Collections;
using System.Collections.Generic;
using GameField;
using GameField.Items;
using Player.Data;
using UnityEngine;

namespace Abilities
{
    public class AbilityRain : Ability
    {
        [SerializeField] private GameObject _waterDropAnimation;
        [SerializeField] private Transform _parentForAbilities;
        [SerializeField] private AudioSource _rainSound;

        public static Func<Item[,]> OnStartAbilityRain;

        private void OnEnable()
        {
            Water.OnWaterDrop += StartWaterDrop;
            GameFieldAbilitiesUpdater.OnStartRainAbility += OnClickAbilityRain;
        }

        private void OnDisable()
        {
            Water.OnWaterDrop -= StartWaterDrop;
            GameFieldAbilitiesUpdater.OnStartRainAbility -= OnClickAbilityRain;
        }

        private new void Start()
        {
            _amount = PlayerPrefs.GetInt(PlayerDataKeys.AmountRainDataKey);
            base.Start();
        }

        public void OnClickAbilityRain(List<Item> itemsOnField)
        {
            if (_amount <= 0) return;

            StartWaterDrop(itemsOnField);

            UseAbility();
        }

        private void StartWaterDrop(List<Item> items)
        {            
            BlockClickHandler.Instance.OnBlockPanel();

            foreach (var item in items)
            {
                var waterDrop = Instantiate(_waterDropAnimation, _parentForAbilities);
                waterDrop.transform.localPosition = item.transform.localPosition;

                if (item.ItemType == ItemType.Dirt &&
                    item.LevelItem == 0)
                {
                    item.GetComponent<Dirt>().ImproveToGrass();
                }
                else if (item.ItemType == ItemType.Grass && 
                         item.LevelItem >= 1)
                {
                    item.GetComponent<Grass>().GetFood();
                } 
                _rainSound.Play();
            }  

            StartCoroutine(WaitEndWaterDrop());
        }

        private IEnumerator WaitEndWaterDrop()
        {
            yield return new WaitForSeconds(1f);
            BlockClickHandler.Instance.OffBlockPanel();
        }

        protected override void SetAmountAbilities()
        {
            PlayerPrefs.SetInt(PlayerDataKeys.AmountRainDataKey, _amount);
        }
    }
}

