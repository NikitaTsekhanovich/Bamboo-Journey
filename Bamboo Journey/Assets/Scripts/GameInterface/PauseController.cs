using System;
using MainMenuControllers;
using MusicSystem;
using Player.Data;
using UnityEngine;
using UnityEngine.UI;

namespace GameInterface
{
    public class PauseController : MonoBehaviour
    {
        [SerializeField] private Image _currentMusicImage;
        [SerializeField] private Image _currentEffectsImage;
        [SerializeField] private Sprite _musicOffImage;
        [SerializeField] private Sprite _effectsOffImage;
        [SerializeField] private GameObject _education;
        
        public static Action OnRestartGameField;
        public static Action OnRestartScore;
        public static Action OnRestartMoves;
        public static Action OnBlockClick;
        public static Action OffBlockClick;

        private void Start()
        {
            if (PlayerPrefs.GetInt("MusicIsOn") == 1)
                _currentMusicImage.sprite = _musicOffImage;
            if (PlayerPrefs.GetInt("EffectsIsOn") == 1)
                _currentEffectsImage.sprite = _effectsOffImage;

            if (PlayerPrefs.GetInt(PlayerDataKeys.EducationCompletedDataKey) == 0)
            {
                _education.SetActive(true);
                PlayerPrefs.SetInt(PlayerDataKeys.EducationCompletedDataKey, 1);
            }
        }

        public void BackToMenu()
        {
            LoadingScreenController.Instance.ChangeScene("Menu");
        }

        public void RestartGame()
        {
            OnRestartMoves?.Invoke();
            OnRestartGameField?.Invoke();
            OnRestartScore?.Invoke();
        }

        public void OpenMenu()
        {
            OnBlockClick?.Invoke();
        }

        public void CloseMenu()
        {
            OffBlockClick?.Invoke();
        }

        public void ChangeMusic()
        {
            MusicController.Instance.ChangeMusicState(_currentMusicImage);
        }

        public void ChangeEffects()
        {
            MusicController.Instance.ChangeEffectsState(_currentEffectsImage);
        }
    }
}

