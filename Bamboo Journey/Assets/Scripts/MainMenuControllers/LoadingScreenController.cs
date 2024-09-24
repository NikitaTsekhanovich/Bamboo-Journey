using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

namespace MainMenuControllers
{
    public class LoadingScreenController : MonoBehaviour
    {
        [SerializeField] private Canvas _loadingScreenCanvas;
        [SerializeField] private Image _background;
        [SerializeField] private TMP_Text _loadingText;
        [SerializeField] private Image _logo;
        private Coroutine _loadingTextAnimation;
        private GraphicRaycaster _loadingScreenBlockClick;

        public static LoadingScreenController Instance;

        private void Start() 
        {             
            if (Instance == null) 
            { 
                _loadingScreenBlockClick = _loadingScreenCanvas.GetComponent<GraphicRaycaster>();
                Instance = this; 
                DontDestroyOnLoad(gameObject);
            } 
            else 
            { 
                Destroy(this);  
            } 
        }

        public void ChangeScene(string nameScene)
        {
            _loadingScreenBlockClick.enabled = true;
           StartAnimationFade(nameScene);
        }

        private void StartAnimationFade(string nameScene)
        {
            _loadingTextAnimation = StartCoroutine(StartLoadingTextAnimation());
            _loadingText.DOFade(1f, 0.7f);
            _logo.DOFade(1f, 0.7f);

            DOTween.Sequence()
                .Append(_background.DOFade(1f, 0.7f))
                .AppendInterval(1.5f)
                .AppendCallback(() => LoadScene(nameScene))
                .AppendInterval(1.5f)
                .OnComplete(() => EndAnimationFade());
        }

        private void LoadScene(string nameScene)
        {
            SceneManager.LoadSceneAsync(nameScene);
        }

        private void EndAnimationFade()
        {
            _logo.DOFade(0f, 0.7f);
            _loadingText.DOFade(0f, 0.7f);

            DOTween.Sequence()
                .Append(_background.DOFade(0f, 0.7f))
                .AppendCallback(() => StopCoroutine(_loadingTextAnimation))
                .AppendCallback(() => _loadingScreenBlockClick.enabled = false);
        }

        private IEnumerator StartLoadingTextAnimation()
        {
            while (true)
            {
                _loadingText.text = $"Loading.";
                yield return new WaitForSeconds(0.3f);

                _loadingText.text = $"Loading..";
                yield return new WaitForSeconds(0.3f);

                _loadingText.text = $"Loading...";
                yield return new WaitForSeconds(0.3f);
            }
        }    
    }
}