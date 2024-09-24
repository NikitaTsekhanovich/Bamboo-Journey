using GameField.Items;
using UnityEngine;

namespace GameField
{
    public class GameFieldCreator : MonoBehaviour
    {
        [SerializeField] private RectTransform _parentGrid;
        [SerializeField] protected Transform _parentForAbilities;

        private float parentGridWidth;
        private float parentGridHeight;
        private float widhtOffset;
        private float heightOffset;
        private float cellSizeWidth;
        private float cellSizeHeight;

        protected const int GameFieldSize = 5;
        protected Item[,] _itemsOnField = new Item[GameFieldSize, GameFieldSize];

        private void Start()
        {
            InitSizeGameField();
            CreateGameField();
        }

        private void InitSizeGameField()
        {
            parentGridWidth = _parentGrid.rect.width;
            parentGridHeight = _parentGrid.rect.height;
            widhtOffset = parentGridWidth / 2 - parentGridWidth / GameFieldSize / 2;
            heightOffset = parentGridHeight / 2 - parentGridHeight / GameFieldSize / 2;
            cellSizeWidth = parentGridWidth / GameFieldSize;
            cellSizeHeight = parentGridHeight / GameFieldSize;
        }

        private void CreateGameField()
        {
            for (var x = 0; x < GameFieldSize; x++)
            {
                for (var y = 0; y < GameFieldSize; y++)
                {
                    SpawnRandomItem(x, y);
                }
            }
        }

        protected void SpawnRandomItem(int x, int y)
        {
            var randomIndexItem = Random.Range(0, ItemsStorage.Items.Count);
            var randomTypeItem = (ItemType)ItemsDataTypes.TypeStorage.GetValue(randomIndexItem);

            SpawnItem(randomTypeItem, x, y);
        }

        protected void SpawnItem(ItemType randomTypeItem, int x, int y)
        {
            var newItem = Instantiate(ItemsStorage.Items[randomTypeItem]);
            SetPositionItemOnData(x, y, newItem);
            newItem.SetSize(new Vector2(cellSizeWidth, cellSizeHeight));
            SetPositionItemOnGame(x, y, newItem);
            newItem.ItemAnimationHandler.StartSpawnAnimation();
        }

        protected void SetPositionItemOnData(int x, int y, Item newItem)
        {
            newItem.transform.SetParent(_parentGrid.transform);
            newItem.SetPosition(new Points(x, y));
        }

        protected void SetPositionItemOnGame(int x, int y, Item newItem)
        {
            newItem.transform.localPosition = new Vector3((y * cellSizeWidth) - widhtOffset, (x * cellSizeHeight) - heightOffset, 0);
            _itemsOnField[x, y] = newItem;
        }

        protected void DestroyItem(Points position)
        {
            Destroy(_itemsOnField[position.X, position.Y].gameObject);
            _itemsOnField[position.X, position.Y] = null;
        }

        protected void RespawnItem(Points position, ItemType itemType)
        {
            DestroyItem(position);
            SpawnItem(itemType, position.X, position.Y);
        }
    }
}

