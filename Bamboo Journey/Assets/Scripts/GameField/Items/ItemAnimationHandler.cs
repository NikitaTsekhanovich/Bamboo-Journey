using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

namespace GameField.Items
{
    public class ItemAnimationHandler : MonoBehaviour
    {
        [SerializeField] private TMP_Text _increaseMovesText;
        private Sequence _combineAnimation;
        private Sequence _idleAnimation;
        private Sequence _increaseMovesAnimation;
        private Sequence _improveAnimation;

        public void StartCombineAnimation(Transform transformTargetItem)
        {   
            _combineAnimation = DOTween.Sequence()
                .Append(transform.DOMove(transformTargetItem.position, 0.2f));
        }

        public void EndCombineAnimation()
        {
            _combineAnimation.Kill();
        }

        public void StartSpawnAnimation()
        {
            transform.DOScale(Vector3.one, 0.2f);
        }

        public void StartImproveAnimation(Image currentLevelImage)
        {
            _improveAnimation = DOTween.Sequence()
                .Append(currentLevelImage.transform.DOScale(Vector3.one, 0.2f))
                .AppendCallback(() => StartIdleAnimation(currentLevelImage));
        }

        public void StartSpawnFruitAnimation(Fruit fruit)
        {
            fruit.transform.DOScale(Vector3.one, 0.2f);
        }

        public void StartIncorrectClickAnimation(GameObject incorrectClickImage)
        {
            BlockClickHandler.Instance.OnBlockPanel();
            var firstPosition = incorrectClickImage.transform.localPosition;

            DOTween.Sequence()
                .Append(transform.DOShakePosition(1f, new Vector3(5, 5, 0)))
                .AppendCallback(() => firstPosition = incorrectClickImage.transform.localPosition)
                .AppendCallback(() => BlockClickHandler.Instance.OffBlockPanel());

            DOTween.Sequence()
                .Append(incorrectClickImage.transform.DOScale(Vector3.one, 0.4f))
                .Append(incorrectClickImage.transform.DOScale(Vector3.zero, 0.4f));
        }

        public void StartIdleAnimation(Image currentLevelImage)
        {
            EndIdleAnimation();
            _idleAnimation = DOTween.Sequence()
                .Append(currentLevelImage.transform.DOScale(new Vector3(0.85f, 1.15f, 1f), 1.5f))
                .Append(currentLevelImage.transform.DOScale(Vector3.one, 1.5f));

            _idleAnimation.SetLoops(-1, LoopType.Yoyo);
        }

        public void EndIdleAnimation()
        {
            if (_idleAnimation != null)
                _idleAnimation.Kill();
        }

        public void StartIncreaseMovesAnimation(Item targetItem, int newMoves)
        {
            var newIncreaseMovesText = Instantiate(_increaseMovesText, targetItem.transform);
            newIncreaseMovesText.text = $"+{newMoves} moves!";

            _increaseMovesAnimation = DOTween.Sequence()
                .Append(newIncreaseMovesText.transform.DOScale(Vector3.one, 1f))
                .AppendInterval(0.5f)
                .Append(newIncreaseMovesText.transform.DOScale(Vector3.zero, 1f))
                .AppendCallback(() => Destroy(newIncreaseMovesText.gameObject));
        }

        public void EndIncreaseMovesAnimation()
        {
            if (_increaseMovesAnimation != null)
                _increaseMovesAnimation.Kill();
        }

        public void EndImproveAnimation()
        {
            if (_improveAnimation != null)
                _improveAnimation.Kill();
        }
    }
}

