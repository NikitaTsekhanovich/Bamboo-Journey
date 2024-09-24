using System;
using System.Collections.Generic;
using GameField.Items;
using GameField.Scrolls;
using UnityEngine;
using System.Collections;
using System.Linq;
using GameField.Scrolls.Properties;
using GameField.Items.Properties;
using GameInterface;
using BonusSystem;

namespace GameField
{
    public class GameFieldUpdater : GameFieldCreator
    {
        [SerializeField] private List<HorizontalScroll> _horizontalScrolls;
        [SerializeField] private List<VerticalScroll> _verticalScrolls;
        private List<Item> _matchItems = new();
        private List<Item> _horizontalItemsScroll = new();
        private List<Item> _verticalItemsScroll = new();
        private HashSet<Grass> _bamboosFruit = new();

        public static Action<int> OnMove;
        public static Action<TypeQuest> OnCheckQuestType;

        private void OnEnable()
        {
            InputHandler.OnClickItem += ClickOnItem;
            ScrollRectEx.OnHorizontalScroll += ChooseHorizontalScroll;
            ScrollRectEx.OnVerticalScroll += ChooseVerticalScroll;
            ScrollRectEx.OffHorizontalScroll += StopHorizontalScroll;
            ScrollRectEx.OffVerticalScroll += StopVerticalScroll;
            Dirt.OnImproveDirtToGrass += RespawnItem;
            PauseController.OnRestartGameField += RestartGameField;
        }

        private void OnDisable()
        {
            InputHandler.OnClickItem -= ClickOnItem;
            ScrollRectEx.OnHorizontalScroll -= ChooseHorizontalScroll;
            ScrollRectEx.OnVerticalScroll -= ChooseVerticalScroll;
            ScrollRectEx.OffHorizontalScroll -= StopHorizontalScroll;
            ScrollRectEx.OffVerticalScroll -= StopVerticalScroll;
            Dirt.OnImproveDirtToGrass -= RespawnItem;
            PauseController.OnRestartGameField -= RestartGameField;
        }

        private void ClickOnItem(Item targetItem)
        {
            if (targetItem.LevelItem >= 3)
            {
                targetItem.PlayIncorrectClick();
                return;
            }

            DFS(targetItem.Position.X, targetItem.Position.Y, targetItem.ItemType, targetItem.LevelItem);

            if (_matchItems.Count >= 3)
            {
                foreach (var item in _matchItems)
                {
                    if (targetItem.Position != item.Position)
                    {
                        StartCoroutine(CombineItems(item, targetItem));
                    }
                }

                var newMoves = (_matchItems.Count - 3) / 2;
                StartCoroutine(ImproveTargetItem(targetItem, newMoves));

                OnMove?.Invoke(newMoves);
            }
            else
            {
                foreach (var item in _matchItems)
                {
                    item.SetStateDestroy(false);
                }
                targetItem.PlayIncorrectClick();
            }

            _matchItems.Clear();
        }

        private void DFS(int x, int y, ItemType typeItem, int levelItem)
        {
            // Move left
            FindItemMatches(x - 1, y, typeItem, levelItem, x - 1 > -1);
            // Move up
            FindItemMatches(x, y + 1, typeItem, levelItem, y + 1 < GameFieldSize);
            // Move down
            FindItemMatches(x, y - 1, typeItem, levelItem, y - 1 > -1);
            // Move right
            FindItemMatches(x + 1, y, typeItem, levelItem, x + 1 < GameFieldSize);
        }
        
        private void FindItemMatches(int x, int y, ItemType typeClickItem, int levelClickItem, bool fieldLimitation)
        {     
            if (fieldLimitation)
            {
                var currentItem = _itemsOnField[x, y];

                if (!currentItem.IsReadyToDestroy &&
                    currentItem.ItemType == typeClickItem &&
                    currentItem.LevelItem == levelClickItem)
                {
                    _matchItems.Add(currentItem);
                    currentItem.SetStateDestroy(true);
                    DFS(x, y, typeClickItem, levelClickItem);
                }
            }
        }

        private void ChooseHorizontalScroll(int x)
        {
            for (var y = 0; y < GameFieldSize; y++)
            {
                ChangeParentScrollItem(_horizontalItemsScroll, x, y, _horizontalScrolls[x]);
            }
            _horizontalScrolls[x].InitInfiniteScroll(_horizontalItemsScroll);
        }

        private void ChooseVerticalScroll(int y)
        {
            for (var x = 0; x < GameFieldSize; x++)
            {
                ChangeParentScrollItem(_verticalItemsScroll, x, y, _verticalScrolls[y]);
                _itemsOnField[x, y].RectTransform.SetAsFirstSibling();
            }
            _verticalScrolls[y].InitInfiniteScroll(_verticalItemsScroll);
        }

        private void ChangeParentScrollItem<TScroll>(List<Item> itemsScroll, int x, int y, TScroll scroll)
            where TScroll : IHaveScrollRect
        {
            _itemsOnField[x, y].transform.SetParent(scroll.ScrollRect.content);
            itemsScroll.Add(_itemsOnField[x, y]);
        }

        private void StopHorizontalScroll(int x) // refactoring
        {
            _horizontalScrolls[x].StopScroll();
            var offsetIndex = _horizontalScrolls[x].GetOffsetIndex();

            if (offsetIndex != 0)
                OnMove?.Invoke(-1);

            for (var y = 0; y < GameFieldSize; y++)
                SetPositionItemOnData(x, (y + offsetIndex) % GameFieldSize, _horizontalItemsScroll[y]);

            var sortedItems = _horizontalItemsScroll.OrderBy(item => item.Position.Y).ToList();

            for (var y = 0; y < GameFieldSize; y++)
                SetPositionItemOnGame(x, y, sortedItems[y]);

            _horizontalItemsScroll.Clear();
            CheckFeedingPandas();
        }

        private void StopVerticalScroll(int y) // refactoring
        {
            _verticalScrolls[y].StopScroll();
            var offsetIndex = _verticalScrolls[y].GetOffsetIndex();

            if (offsetIndex - GameFieldSize != 0)
                OnMove?.Invoke(-1);

            for (var x = 0; x < GameFieldSize; x++)
                SetPositionItemOnData((x + offsetIndex) % GameFieldSize, y, _verticalItemsScroll[x]);

            var sortedItems = _verticalItemsScroll.OrderBy(item => item.Position.X).ToList();

            for (var x = 0; x < GameFieldSize; x++)
                SetPositionItemOnGame(x, y, sortedItems[x]);

            _verticalItemsScroll.Clear();
            CheckFeedingPandas();
        }

        private IEnumerator ImproveTargetItem(Item targetItem, int newMoves)
        {
            yield return new WaitForSeconds(0.5f);

            targetItem.SetStateDestroy(false);
            targetItem.IncreaseLevel();
            if (newMoves > 0)
                targetItem.ItemAnimationHandler.StartIncreaseMovesAnimation(targetItem, newMoves);

            CheckQuestType(targetItem);

            yield return new WaitForSeconds(0.25f);
            BlockClickHandler.Instance.OffBlockPanel();
            CheckTargetItemAbility(targetItem);
        }

        private void CheckQuestType(Item targetItem)
        {
            // refactoring
            switch (targetItem.LevelItem)
            {
                case 1:
                    switch (targetItem.ItemType)
                    {
                        case ItemType.Grass:
                            OnCheckQuestType?.Invoke(TypeQuest.Bamboo1);
                            break;
                        case ItemType.Dirt:
                            OnCheckQuestType?.Invoke(TypeQuest.Panda1);
                            break;
                        case ItemType.Water:
                            OnCheckQuestType?.Invoke(TypeQuest.Drop1);
                            break;
                    }
                    break;
                case 2:
                    switch (targetItem.ItemType)
                    {
                        case ItemType.Grass:
                            OnCheckQuestType?.Invoke(TypeQuest.Bamboo2);
                            break;
                        case ItemType.Dirt:
                            OnCheckQuestType?.Invoke(TypeQuest.Panda2);
                            break;
                        case ItemType.Water:
                            OnCheckQuestType?.Invoke(TypeQuest.Drop2);
                            break;
                    }
                    break;
                case 3:
                    switch (targetItem.ItemType)
                    {
                        case ItemType.Grass:
                            OnCheckQuestType?.Invoke(TypeQuest.Bamboo3);
                            break;
                        case ItemType.Dirt:
                            OnCheckQuestType?.Invoke(TypeQuest.Panda3);
                            break;
                        case ItemType.Water:
                            OnCheckQuestType?.Invoke(TypeQuest.Drop3);
                            break;
                    }
                    break;
            }
        }

        private void CheckTargetItemAbility(Item targetItem)
        {
            if (targetItem.TryGetComponent<Water>(out var water))
            {                
                var neighbors = GetNeighbors(water);
                water.DoWatering(neighbors);
                FindBamboo(neighbors);
            }
            else 
            {
                CheckFeedingPandas();
            }
        }

        private List<Item> GetNeighbors<TItem>(TItem targetItem)
            where TItem : IHaveAbilitiesPositions
        {
            var neighboringItems = new List<Item>();

            foreach (var positionNeighbor in targetItem.PositionsAbilities)
            {
                var positionItemX = targetItem.Position.X + positionNeighbor.X;
                var positionItemY = targetItem.Position.Y + positionNeighbor.Y;

                if (positionItemX >= 0 && positionItemX < GameFieldSize &&
                    positionItemY >= 0 && positionItemY < GameFieldSize)
                {
                    neighboringItems.Add(_itemsOnField[positionItemX, positionItemY]);
                }
            }

            return neighboringItems;
        }

        protected void FindBamboo(List<Item> neighborsWatering)
        {
            foreach (var neighborWatering in neighborsWatering)
            {
                if (neighborWatering.LevelItem >= 1 && 
                    neighborWatering.TryGetComponent<Grass>(out var grass) &&
                    grass.HasFruit)
                {
                    var neighbors = GetNeighbors(grass);
                    var wasFeeding = false;

                    foreach (var neighbor in neighbors)
                    {
                        if (neighbor.ItemType == ItemType.Dirt && 
                            neighbor.LevelItem >= 1)
                        {   
                            grass.DoFeed(neighbor, _parentForAbilities);
                            wasFeeding = true;
                        }
                    }

                    if (!wasFeeding)
                        _bamboosFruit.Add(grass);
                }
            }
        }

        protected void CheckFeedingPandas()
        {
            var tempBamboosFruit = new HashSet<Grass>(_bamboosFruit);

            foreach (var bambooFruit in tempBamboosFruit)
            {
                var neighbors = GetNeighbors(bambooFruit);

                foreach (var neighbor in neighbors)
                {
                    if (neighbor.ItemType == ItemType.Dirt && 
                        neighbor.LevelItem >= 1)
                    {
                        bambooFruit.DoFeed(neighbor, _parentForAbilities);
                        _bamboosFruit.Remove(bambooFruit);
                    }
                }
            }
        }

        private IEnumerator CombineItems(Item item, Item targetItem)
        {
            BlockClickHandler.Instance.OnBlockPanel();

            item.PlayCombineState(targetItem.transform);

            yield return new WaitForSeconds(0.4f);

            item.ItemAnimationHandler.EndCombineAnimation();
            item.ItemAnimationHandler.EndIdleAnimation();

            if (item.TryGetComponent<Grass>(out var grass) && _bamboosFruit.Contains(grass))
                _bamboosFruit.Remove(grass);
            DestroyItem(item.Position);
            SpawnRandomItem(item.Position.X, item.Position.Y);

            yield return new WaitForSeconds(0.1f);
        }

        private void RestartGameField()
        {
            for (var x = 0; x < GameFieldSize; x++)
            {
                for (var y = 0; y < GameFieldSize; y++)
                {
                    var item = _itemsOnField[x, y];

                    item.ItemAnimationHandler.EndIncreaseMovesAnimation();
                    item.ItemAnimationHandler.EndImproveAnimation();

                    if (item.LevelItem >= 1)
                    {
                        item.ItemAnimationHandler.EndIdleAnimation();
                    }

                    DestroyItem(new Points(x, y));
                    SpawnRandomItem(x, y);
                }
            }
        }
    }
}

