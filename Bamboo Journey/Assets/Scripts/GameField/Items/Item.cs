using UnityEngine;
using UnityEngine.UI;

namespace GameField.Items
{
    public class Item : MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private ItemType _itemType;
        [SerializeField] private RectTransform _rectTransform;
        [SerializeField] private Image _currentLevelImage;
        [SerializeField] private Sprite _levelImage1;
        [SerializeField] private Sprite _levelImage2;
        [SerializeField] private Sprite _levelImage3;
        [SerializeField] private ItemAnimationHandler _itemAnimationHandler;
        [SerializeField] private GameObject _incorrectClickImage;
        [SerializeField] private AudioSource _incorrectClickSound;
        [SerializeField] private AudioSource _combineItemSound;
        private int _levelItem;
        private Points _position;
        private bool _isReadyToDestroy;

        public ItemType ItemType => _itemType;
        public int LevelItem => _levelItem;
        public Points Position => _position;
        public RectTransform RectTransform => _rectTransform;
        public bool IsReadyToDestroy => _isReadyToDestroy;
        public ItemAnimationHandler ItemAnimationHandler => _itemAnimationHandler;
        public GameObject IncorrectClickImage => _incorrectClickImage;
        public Image CurrentLevelImage => _currentLevelImage;

        public void PlayCombineState(Transform targetItemTransform)
        {
            _combineItemSound.Play();
            _itemAnimationHandler.StartCombineAnimation(targetItemTransform);
        }

        public void PlayIncorrectClick()
        {
            _incorrectClickSound.Play();
            _itemAnimationHandler.StartIncorrectClickAnimation(_incorrectClickImage);
        }

        public void SetSize(Vector2 cellSize)
        {
            _rectTransform.sizeDelta = cellSize;
            _collider.size = cellSize;
        }

        public void SetPosition(Points position)
        {
            _position = position;
        }

        public void IncreaseLevel()
        {
            _levelItem++;
            _currentLevelImage.transform.localScale = Vector3.zero;

            switch (_levelItem)
            {
                case 1:
                    _currentLevelImage.color = new Color(1, 1, 1, 1);
                    _currentLevelImage.sprite = _levelImage1;
                    break;
                case 2:
                    _currentLevelImage.sprite = _levelImage2;
                    break;
                case 3:
                    _currentLevelImage.sprite = _levelImage3;
                    break;
            }
            _itemAnimationHandler.StartImproveAnimation(_currentLevelImage);
        }

        public void SetStateDestroy(bool state)
        {
            _isReadyToDestroy = state;
        }
    }
}

