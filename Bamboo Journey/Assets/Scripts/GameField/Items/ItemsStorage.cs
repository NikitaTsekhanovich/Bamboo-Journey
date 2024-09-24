using System.Collections.Generic;
using UnityEngine;

namespace GameField.Items
{
    public class ItemsStorage : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _items = new();

        public static Dictionary<ItemType, Item> Items { get; private set; }

        private void Awake()
        {
            Items = new Dictionary<ItemType, Item>();
            InitItems();
        }

        private void InitItems()
        {
            foreach (var item in _items)
            {
                var itemComponent = item.GetComponent<Item>();
                Items[itemComponent.ItemType] = itemComponent; 
            }
        }
    }
}

