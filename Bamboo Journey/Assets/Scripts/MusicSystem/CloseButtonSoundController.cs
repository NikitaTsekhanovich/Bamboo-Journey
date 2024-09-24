using UnityEngine;

namespace MusicSystem
{
    public class CloseButtonSoundController : MonoBehaviour
    {
        [SerializeField] private AudioSource _soundButton;

        public void StartSound()
        {
            _soundButton.Play();
        }
    }
}