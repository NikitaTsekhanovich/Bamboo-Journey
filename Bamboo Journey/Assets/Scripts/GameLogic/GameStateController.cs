using System;
using UnityEngine;

namespace GameLogic
{
    public class GameStateController : MonoBehaviour
    {
        [SerializeField] private GameObject _loseScreen;
        [SerializeField] private AudioSource _loseSound;

        public static Action OnBlockClick;
        public static Action OnCheckPrizeCollection;

        private void OnEnable()
        {
            MovesController.OnLose += DoLose;
        }

        private void OnDisable()
        {
            MovesController.OnLose -= DoLose;
        }

        private void DoLose()
        {
            OnCheckPrizeCollection?.Invoke();
            OnBlockClick?.Invoke();
            _loseSound.Play();
            _loseScreen.SetActive(true);
        }
    }
}

