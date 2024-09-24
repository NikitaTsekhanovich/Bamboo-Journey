using System;
using System.Collections.Generic;
using GameField.Items;

namespace GameField
{
    public class GameFieldAbilitiesUpdater : GameFieldUpdater
    {
        public static Action<List<Item>> OnStartRainAbility;
        public static Action<List<Item>> OnStartPandaAbility;
        public static Action<List<Item>> OnStartBambooAbility;

        public void OnClickRainAbility()
        {
            var listItems = ConvertArrayItemsToList();
            OnStartRainAbility?.Invoke(listItems);
            FindBamboo(listItems);
        }

        public void OnClickPandaAbility()
        {
            var listItems = ConvertArrayItemsToList();
            OnStartPandaAbility?.Invoke(listItems);
            CheckFeedingPandas();
        }

        public void OnClickBambooAbility()
        {
            var listItems = ConvertArrayItemsToList();
            OnStartBambooAbility?.Invoke(listItems);
        }

        private List<Item> ConvertArrayItemsToList()
        {
            var listItems = new List<Item>();

            foreach (var itemOnField in _itemsOnField)
                listItems.Add(itemOnField);

            return listItems;
        }
    }
}

