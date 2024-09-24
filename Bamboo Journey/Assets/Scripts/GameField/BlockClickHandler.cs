using UnityEngine;
using DG.Tweening;

namespace GameField
{
    public class BlockClickHandler : MonoBehaviour
    {
        [SerializeField] private GameObject _blockClickPanel;
        [SerializeField] private GameObject _loadingAnimationImage;
        private Sequence _loadingProcessAnimation;

        public static BlockClickHandler Instance;

        private void Start()
        {
            if (Instance == null) 
                Instance = this; 
            else 
                Destroy(this);  
        }

        public void OnBlockPanel()
        {
            EndLoadingAnimation();
            _loadingAnimationImage.SetActive(true);
            _loadingProcessAnimation = DOTween.Sequence()
                .Append(_loadingAnimationImage.transform.DORotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.FastBeyond360))
                .SetLoops(-1, LoopType.Yoyo)
                .SetRelative()
                .SetEase(Ease.Linear);

            _blockClickPanel.SetActive(true);
        }

        public void OffBlockPanel()
        {
            EndLoadingAnimation();
            _blockClickPanel.SetActive(false);
        }

        private void EndLoadingAnimation()
        {
            _loadingProcessAnimation.Kill();
            _loadingAnimationImage.SetActive(false);
        }
    }
}

