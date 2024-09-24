using System;
using GameField.Items;
using UnityEngine;

namespace GameField.Scrolls
{
    public class HorizontalScroll : InfiniteScroll
    {
        protected override void InitItemSize()
        {
            _itemSize = _items[0].RectTransform.rect.width;
            _sizeContent = _itemSize * _items.Count;
        }

        protected override void CheckOffsetItems()
        {
            if (ScrollRect.content.localPosition.x > 0)
                DoOffsetItems(-1 * new Vector3(_sizeContent, 0, 0));

            if (ScrollRect.content.localPosition.x < 0 - _sizeContent)
                DoOffsetItems(new Vector3(_sizeContent, 0, 0));
        }

        protected override void DoOffsetItems(Vector3 offset)
        {
            Canvas.ForceUpdateCanvases();
            ScrollRect.content.localPosition += offset;
            // _oldVelocity = ScrollRect.velocity;
            _isUpdated = true;
        }

        protected override void AlignItems()
        {
            var offset = Math.Abs(ScrollRect.content.localPosition.x % _itemSize);
 
            if (offset >= _itemSize / 2)
                ScrollRect.content.localPosition += new Vector3(offset - _itemSize, 0, 0);
            else 
                ScrollRect.content.localPosition += new Vector3(offset, 0, 0);
        }

        protected override void SelectSequenceInHierarchy(bool isFirst, Item item)
        {
            if (isFirst)
                item.RectTransform.SetAsFirstSibling();            
            else
                item.RectTransform.SetAsLastSibling();
        }

        public override int GetOffsetIndex()
        {
            var indexOffset = 0f;

            if (ScrollRect.content.localPosition.x != 0)
                indexOffset = (_sizeContent + ScrollRect.content.localPosition.x) / _itemSize; 

            return (int)Math.Round(indexOffset);
        }

        protected override void SetStartPositionContent()
        {
            ScrollRect.content.localPosition = new Vector3(
                0 - _sizeContent, 
                ScrollRect.content.localPosition.y, 
                ScrollRect.content.localPosition.z);
        }
    }
}

