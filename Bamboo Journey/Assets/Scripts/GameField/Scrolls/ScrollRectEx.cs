using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using GameField.Items;

namespace GameField.Scrolls
{
    public class ScrollRectEx : ScrollRect 
    {
        private bool routeToParent;
        private ContainerVerticalScrolls _containerVerticalScrolls;
        private List<ScrollRect> _verticalScrolls;
        private int indexScroll;

        public static Action<int> OnHorizontalScroll;
        public static Action<int> OnVerticalScroll;
        public static Action<int> OffHorizontalScroll;
        public static Action<int> OffVerticalScroll;

        protected override void Start()
        {
            _containerVerticalScrolls = GetComponent<ContainerVerticalScrolls>();
            _verticalScrolls = new(_containerVerticalScrolls.VerticalScrolls);
            base.Start();
        }

        private void DoForParents<T>(Action<T> action)
            where T : IEventSystemHandler
        {
            foreach(var component in _verticalScrolls[indexScroll].GetComponents<Component>()) 
            {
                if(component is T)
                {
                    action((T)(IEventSystemHandler)component);
                }
            }
        }

        public override void OnInitializePotentialDrag(PointerEventData eventData)
        {
            DoForParents<IInitializePotentialDragHandler>((parent) => { parent.OnInitializePotentialDrag(eventData); });
            base.OnInitializePotentialDrag (eventData);
        }

        private Item CheckClickOnItem(RaycastHit2D hit)
        {
            if (hit.collider != null && 
                hit.collider.TryGetComponent<Item>(out var item)) 
            {
                return item;
            }
            return null;
        }

        public override void OnBeginDrag (PointerEventData eventData)
        {
            var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if((!horizontal && Math.Abs (eventData.delta.x) > Math.Abs (eventData.delta.y)) ||
                (!vertical && Math.Abs (eventData.delta.x) < Math.Abs (eventData.delta.y)))
            {
                routeToParent = true;

                var item = CheckClickOnItem(hit);
                if (item != null)
                {
                    indexScroll = item.Position.Y;
                    OnVerticalScroll?.Invoke(indexScroll);
                }
            }
            else
            {
                routeToParent = false;

                var item = CheckClickOnItem(hit);
                if (item != null)
                {
                    indexScroll = item.Position.X;
                    OnHorizontalScroll?.Invoke(indexScroll);
                }
            }

            if(routeToParent)
            {
                DoForParents<IBeginDragHandler>((parent) => { parent.OnBeginDrag(eventData); });
            }
            else
            {
                base.OnBeginDrag (eventData);
            }
        }

        public override void OnDrag (PointerEventData eventData)
        {
            if(routeToParent)
            {
                DoForParents<IDragHandler>((parent) => { parent.OnDrag(eventData); });
            }
            else
            {
                base.OnDrag (eventData);
            }
        }

        public override void OnEndDrag (PointerEventData eventData)
        {
            if(routeToParent)
            {
                DoForParents<IEndDragHandler>((parent) => { parent.OnEndDrag(eventData); });
                OffVerticalScroll?.Invoke(indexScroll);
            }
            else
            {
                base.OnEndDrag (eventData);
                OffHorizontalScroll?.Invoke(indexScroll);
            }

            routeToParent = false;
        }
    }
}
