using System;
using System.Collections.Generic;
using UnityEngine;

namespace SnakeWorks
{
    public class ShopManager : MonoBehaviour
    {
        public static ShopManager Instance { get; private set; }

        [SerializeField] private ShopItem[] _items;
        [SerializeField] private ShopItemUI _shopItemUIPrefab;
        [SerializeField] private Transform _itemCreationParent;

        // Dictionary for fast lookups
        private Dictionary<string, ShopItem> _itemDict = new();

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            foreach (var item in _items)
            {
                ShopItemUI itemUI = Instantiate(_shopItemUIPrefab, _itemCreationParent);
                _itemDict.Add(item.Id, item);
                itemUI.Init(item);
            }
        }

        public bool Purchase(ShopItem item)
        {
            if (PlayerManager.Instance.Points < item.Price)
            {
                return false;
            }

            item.Amount++;
            PlayerManager.Instance.AddPoints(-item.Price);
            return true;
        }

        public int GetItemAmount(string id)
        {
            return _itemDict[id].Amount;
        }
    }

    [Serializable]
    public class ShopItem
    {
        [SerializeField] public string Id;
        [SerializeField] public string Name;
        [SerializeField] public Sprite Icon; 
        [SerializeField, TextArea] public string Description;
        [SerializeField] public int Price;
        [SerializeField] public int Amount;
    }
}
