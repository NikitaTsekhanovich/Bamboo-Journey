using System.Collections.Generic;
using Player.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Shop
{
    public class ShopController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _amountCoinsText;
        [SerializeField] private List<ShopItem> _shopItems;
        [SerializeField] private List<TMP_Text> _prices;
        [SerializeField] private List<Image> _icons;
        [SerializeField] private List<TMP_Text> _descriptions;
        [SerializeField] private List<TMP_Text> _amountItems;
        [SerializeField] private List<Button> _buyButtons;

        private int _currentCoins;

        private void OnEnable()
        {
            InitCoins();
            InitShopItems();
        }

        private void InitShopItems()
        {
            foreach (var shopItem in _shopItems)
            {
                _prices[shopItem.Index].text = $"{shopItem.Price}";
                _icons[shopItem.Index].sprite = shopItem.Icon;
                _descriptions[shopItem.Index].text = shopItem.Description;
                UpdateAmountShopItem(shopItem);
                _buyButtons[shopItem.Index].interactable = CheckCanBuyItem(shopItem.Price);
            }
        }

        private void InitCoins()
        {
            _currentCoins = PlayerPrefs.GetInt(PlayerDataKeys.CoinsDataKey);
            UpdateTextCoins();
        }

        private void UpdateTextCoins()
        {
            _amountCoinsText.text = $"{_currentCoins}";
        }

        private bool CheckCanBuyItem(int itemPrice)
        {
            if (_currentCoins - itemPrice >= 0)
                return true;
            return false;
        }

        private void UpdateAmountShopItem(ShopItem shopItem)
        {
            _amountItems[shopItem.Index].text = $"x{PlayerPrefs.GetInt(shopItem.KeyAmount)}";
        }

        private void UpdateStateButtons()
        {
            var index = 0;

            foreach (var button in _buyButtons)
            {
                button.interactable = CheckCanBuyItem(_shopItems[index].Price);
                index++;
            }
        }
        
        public void BuyItem(int index)
        {
            var canBuy = CheckCanBuyItem(_shopItems[index].Price);

            if (canBuy)
            {
                _currentCoins -= _shopItems[index].Price;
                PlayerPrefs.SetInt(PlayerDataKeys.CoinsDataKey, _currentCoins);
                UpdateTextCoins();

                var currentAmount = PlayerPrefs.GetInt(_shopItems[index].KeyAmount);
                PlayerPrefs.SetInt(_shopItems[index].KeyAmount, currentAmount + 1);
                UpdateAmountShopItem(_shopItems[index]);

                UpdateStateButtons();
            }
        }
    }
}

