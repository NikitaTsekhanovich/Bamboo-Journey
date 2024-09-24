using GameField.Items;
using Player.Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameInterface;
using System;
using BonusSystem;

namespace GameLogic
{
    public class ScoreController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _coinsText;
        [SerializeField] private TMP_Text _highScoreText;
        [SerializeField] private Image _startFilling1;
        [SerializeField] private Image _startFilling2;
        [SerializeField] private Image _startFilling3;
        [SerializeField] private ParticleSystem _openStarParticle;
        [SerializeField] private AudioSource _openStarSound;
        [SerializeField] private GameObject _checkCollectionText;
        private bool _isOpenStar1;
        private bool _isOpenStar2;
        private bool _isOpenStar3;
        private int _coins;
        private float _currentCoins;
        private int _highScore;

        private const float BorderCoinsStar = 50;

        public static Action<TypeQuest> OnCheckTypeQuest;

        private void Awake()
        {
            _highScore = PlayerPrefs.GetInt(PlayerDataKeys.HighScoreDataKey);
            _coins = PlayerPrefs.GetInt(PlayerDataKeys.CoinsDataKey);
            UpdateMovesText();
            UpdateHighScoreText();
        }

        private void OnEnable()
        {
            Grass.OnIncreaseCoins += IncreaseCoins;
            PauseController.OnRestartScore += RestartScore;
            GameStateController.OnCheckPrizeCollection += GivePrizeCollection;
        }

        private void OnDisable()
        {
            Grass.OnIncreaseCoins -= IncreaseCoins;
            PauseController.OnRestartScore -= RestartScore;
            GameStateController.OnCheckPrizeCollection -= GivePrizeCollection;
        }

        private void IncreaseCoins(int coins)
        {
            DOTween.Sequence()
                .Append(_coinsText.DOColor(Color.green, 0.5f))
                .Append(_coinsText.DOColor(Color.white, 0.5f));

            _currentCoins += coins;
            _coins += coins;
            PlayerPrefs.SetInt(PlayerDataKeys.CoinsDataKey, _coins);
            UpdateMovesText();
            UpdateStars();
            CheckHighScore();
        }

        private void CheckHighScore()
        {
            if (_currentCoins > _highScore)
            {
                _highScore = (int)_currentCoins;
                PlayerPrefs.SetInt(PlayerDataKeys.HighScoreDataKey, _highScore);
                UpdateHighScoreText();
            }
        }

        private void UpdateMovesText()
        {
            _coinsText.text = $"{_coins}";
        }

        private void UpdateHighScoreText()
        {
            _highScoreText.text = $"High score: {_highScore}";
        }

        private void UpdateStars()
        {
            if (_currentCoins <= BorderCoinsStar)
                _startFilling1.fillAmount = _currentCoins / BorderCoinsStar;
            else if (_currentCoins <= BorderCoinsStar * 2)
                _startFilling2.fillAmount = _currentCoins / BorderCoinsStar - 1;
            else if (_currentCoins <= BorderCoinsStar * 3)
                _startFilling3.fillAmount = _currentCoins / BorderCoinsStar - 2;

            if (_currentCoins >= BorderCoinsStar && !_isOpenStar1)
                _isOpenStar1 = CheckOpeningStar(_startFilling1, 1);
            else if (_currentCoins >= BorderCoinsStar * 2 && !_isOpenStar2)
                _isOpenStar2 = CheckOpeningStar(_startFilling2, 2);
            else if (_currentCoins >= BorderCoinsStar * 3 && !_isOpenStar3)
                _isOpenStar3 = CheckOpeningStar(_startFilling3, 3);
        }

        private bool CheckOpeningStar(Image fillAmount, int numberSter)
        {
            switch (numberSter)
            {
                case 1:
                    OnCheckTypeQuest?.Invoke(TypeQuest.Star1);
                    break;
                case 2:
                    OnCheckTypeQuest?.Invoke(TypeQuest.Star2);
                    break;
                case 3:
                    OnCheckTypeQuest?.Invoke(TypeQuest.Star3);
                    break;
            }
            fillAmount.fillAmount = 1;
            _openStarParticle.Play();
            _openStarSound.Play();
            return true;
        }

        private void RestartScore()
        {
            _startFilling1.fillAmount = 0;
            _startFilling2.fillAmount = 0;
            _startFilling3.fillAmount = 0;

            _isOpenStar1 = false;
            _isOpenStar2 = false;
            _isOpenStar3 = false;

            _currentCoins = 0;
        }

        private void GivePrizeCollection()
        {
            if (_isOpenStar3)
            {
                var randomItemIndex = UnityEngine.Random.Range(0, 2);

                if (randomItemIndex == 0)
                    IncreaseItemCollection(PlayerDataKeys.AmountBearCollectonDataKey);
                else
                    IncreaseItemCollection(PlayerDataKeys.AmountMonkeyCollectonDataKey);
            }
            else if (_isOpenStar2)
            {
                var randomItemIndex = UnityEngine.Random.Range(0, 2);

                // if (randomItemIndex == 0)
                //     IncreaseItemCollection(PlayerDataKeys.AmountTigerCollectonDataKey);
                if (randomItemIndex == 0)
                    IncreaseItemCollection(PlayerDataKeys.AmountDolphinCollectonDataKey);
                else
                    IncreaseItemCollection(PlayerDataKeys.AmountSalamanderCollectonDataKey);
            }
            else if (_isOpenStar1)
            {
                var randomItemIndex = UnityEngine.Random.Range(0, 2);

                if (randomItemIndex == 0)
                    IncreaseItemCollection(PlayerDataKeys.AmountPandaCollectonDataKey);
                else
                    IncreaseItemCollection(PlayerDataKeys.AmountAlligatorCollectonDataKey);
            }
        }

        private void IncreaseItemCollection(string key)
        {
            var amountItem = PlayerPrefs.GetInt(key);
            PlayerPrefs.SetInt(key, amountItem + 1);
            _checkCollectionText.SetActive(true);
        }
    }
}

