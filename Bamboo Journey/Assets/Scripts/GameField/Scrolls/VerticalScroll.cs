using System;
using GameField.Items;
using UnityEngine;

namespace GameField.Scrolls
{
    public class VerticalScroll : InfiniteScroll
    {
        protected override void InitItemSize()
        {
            _itemSize = _items[0].RectTransform.rect.height;
            _sizeContent = _itemSize * _items.Count;
        }

        protected override void CheckOffsetItems()
        {
            if (ScrollRect.content.localPosition.y < 0)
                DoOffsetItems(new Vector3(0, _sizeContent, 0));

            if (ScrollRect.content.localPosition.y > 2 * _sizeContent)
                DoOffsetItems(-1 * new Vector3(0, _sizeContent, 0));
        }

        protected override void DoOffsetItems(Vector3 offset)
        {
            Canvas.ForceUpdateCanvases();
            ScrollRect.content.localPosition += offset;
            _isUpdated = true;
        }

        protected override void AlignItems()
        {
            var offset = Math.Abs(ScrollRect.content.localPosition.y % _itemSize);
            
            if (offset >= _itemSize / 2)
                ScrollRect.content.localPosition -= new Vector3(0, offset - _itemSize, 0);
            else 
                ScrollRect.content.localPosition -= new Vector3(0, offset, 0);
        }

        protected override void SelectSequenceInHierarchy(bool isFirst, Item item)
        {
            if (isFirst)
                item.RectTransform.SetAsLastSibling();
            else
                item.RectTransform.SetAsFirstSibling();            
        }

        public override int GetOffsetIndex()
        {
            var indexOffset = 0f;

            if (ScrollRect.content.localPosition.y != 0)
                indexOffset = (_sizeContent - ScrollRect.content.localPosition.y) / _itemSize; 

            // The axis is inverted
            return 5 - (int)Math.Round(indexOffset);
        }

        protected override void SetStartPositionContent()
        {
            ScrollRect.content.localPosition = new Vector3(
                ScrollRect.content.localPosition.x,
                _sizeContent, 
                ScrollRect.content.localPosition.z);
        }
    }
}

