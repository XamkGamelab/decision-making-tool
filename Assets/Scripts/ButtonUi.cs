using UnityEngine;
using UnityEngine.SceneManagement;

namespace JAS.MediDeci
{
    public class ButtonUI : MonoBehaviour
    {
        public void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void PlayGame1Button()
        {
            PlayClickSound();
            SceneManager.LoadScene("GameTemplate");
        }

        public void QuitGameButton()
        {
            PlayClickSound();
            Debug.Log("You have quit the game");
            Application.Quit();
        }

        private void PlayClickSound()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAudioClip(AudioManager.Instance.clickButtonSound);
            }
        }
    }
}