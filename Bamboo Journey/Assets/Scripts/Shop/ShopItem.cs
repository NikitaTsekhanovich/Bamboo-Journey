using Player.Data;
using UnityEngine;

namespace Shop
{
    [CreateAssetMenu(fileName = "ShopItem", menuName = "Shop item/ Item")]
    public class ShopItem : ScriptableObject
    {
        [SerializeField] private int _index;
        [SerializeField] private int _price;
        [SerializeField] private Sprite _icon;
        [SerializeField] private string _description;
        [SerializeField] private string _keyAmount;
        
        public int Index => _index;
        public int Price => _price;
        public Sprite Icon => _icon;
        public string Description => _description;
        public string KeyAmount => _keyAmount;
    }
}
