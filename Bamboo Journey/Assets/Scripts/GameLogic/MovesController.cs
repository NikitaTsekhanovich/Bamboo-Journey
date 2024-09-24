using UnityEngine;
using TMPro;
using GameField;
using System;
using DG.Tweening;
using GameInterface;

namespace GameLogic
{
    public class MovesController : MonoBehaviour
    {
        [SerializeField] private TMP_Text _movesText;
        [SerializeField] private AudioSource _increaseMovesSound;
        private const int StartMoves = 30;
        private int _currentMoves;

        public static Action OnLose;

        private void Start()
        {
            UpdateMovesText();
        }

        private void UpdateMovesText()
        {
            _currentMoves = StartMoves;
            _movesText.text = $"{_currentMoves}";
        }
        
        private void OnEnable()
        {
            GameFieldUpdater.OnMove += ChangeMoveText;
            PauseController.OnRestartMoves += RestartMoves;
        }

        private void OnDisable()
        {
            GameFieldUpdater.OnMove -= ChangeMoveText;
            PauseController.OnRestartMoves -= RestartMoves;
        }

        private void ChangeMoveText(int moves)
        {
            _currentMoves += moves;
            _movesText.text = $"{_currentMoves}";

            if (moves > 0)
            {
                _increaseMovesSound.Play();
                DOTween.Sequence()
                    .Append(_movesText.DOColor(Color.green, 0.5f))
                    .Append(_movesText.DOColor(Color.white, 0.5f));
            }

            if (_currentMoves <= 0)
                OnLose?.Invoke();
        }

        private void RestartMoves()
        {
            UpdateMovesText();
        }
    }
}

