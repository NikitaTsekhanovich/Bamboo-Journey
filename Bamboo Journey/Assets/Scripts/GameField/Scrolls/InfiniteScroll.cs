using System.Collections;
using System.Collections.Generic;
using GameField.Items;
using GameField.Scrolls.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace GameField.Scrolls
{
    public abstract class InfiniteScroll : MonoBehaviour, IHaveScrollRect, ICanStopScroll
    {
        public ScrollRect ScrollRect { get; private set; }

        protected List<Item> _items = new();
        protected List<Item> _itemsClones = new();
        protected Vector2 _oldVelocity;
        protected bool _isUpdated;
        protected float _itemSize;
        protected float _sizeContent;

        private bool _onScroll;

        private void Start()
        {
            ScrollRect = GetComponent<ScrollRect>();
        }

        public void InitInfiniteScroll(List<Item> items)
        {
            SetListItems(items);
            InitItemSize();
            SpawnCloneItems();

            _onScroll = true;
        }

        private void Update()
        {
            if (_onScroll)
            {
                ChangeVelocity();
                CheckOffsetItems();
            }
        }

        private void SetListItems(List<Item> items)
        {
            _items = new (items);
        }

        private void SpawnCloneItems()
        {
            _isUpdated = false;
            _oldVelocity = Vector2.zero;

            for (var i = 0; i < _items.Count; i++)
            {
                var item = Instantiate(_items[i % _items.Count], ScrollRect.content);
                _itemsClones.Add(item);
                SelectSequenceInHierarchy(false, item);
            }

            for (var i = 0; i < _items.Count; i++)
            {
                var num = _items.Count - i - 1;

                while (num < 0)
                {
                    num += _items.Count;
                }

                var item = Instantiate(_items[num], ScrollRect.content);
                _itemsClones.Add(item);
                SelectSequenceInHierarchy(true, item);
            }

            SetStartPositionContent();
        }

        private void ChangeVelocity()
        {
            if (_isUpdated)
            {
                _isUpdated = false;
                ScrollRect.velocity = _oldVelocity;
            }
        } 

        public void StopScroll()
        {
            _onScroll = false;
            AlignItems();
            ClearCloneItems();
        }

        private void ClearCloneItems()
        {
            foreach (var itemClone in _itemsClones)
                Destroy(itemClone.gameObject);
            _itemsClones.Clear();
        }

        public abstract int GetOffsetIndex();
        protected abstract void CheckOffsetItems();
        protected abstract void DoOffsetItems(Vector3 offset);
        protected abstract void InitItemSize();
        protected abstract void AlignItems();
        protected abstract void SelectSequenceInHierarchy(bool isFirst, Item item);
        protected abstract void SetStartPositionContent();
    }
}

