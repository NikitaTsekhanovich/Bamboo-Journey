using MusicSystem;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuControllers
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private Image _currentMusicImage;
        [SerializeField] private Image _currentEffectsImage;
        
        public void StartGame()
        {
            LoadingScreenController.Instance.ChangeScene("Game");
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
