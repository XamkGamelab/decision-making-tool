using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace JAS.MediDeci
{
    public class ButtonUi : MonoBehaviour
    {
        [Header("Timing")]
        private float startTime;
        private float clickTime;
        private float resultTime;

        [Header("UI References")]
        public TextMeshProUGUI resultText;
        public GameObject AnswerButtonsVisible;
        public GameObject BackToMenuButtonVisible;

        private int randomScene;

        private const int MainMenuSceneIndex = 0;
        private const int MaxRandomSceneIndex = 4; // Random scenes are 1ñ3

        private void Start()
        {
            startTime = Time.time;
            AnswerButtonsVisible.SetActive(true);
            BackToMenuButtonVisible.SetActive(false);
            randomScene = Random.Range(1, MaxRandomSceneIndex);
            Debug.Log("Random scene: " + randomScene);
        }

        public void BackToMenu()
        {
            SceneManager.LoadScene(MainMenuSceneIndex);
        }

        public void HandleAnswerClick(int buttonIndex)
        {
            Debug.Log($"You clicked button {buttonIndex}");
            clickTime = Time.time;
            resultTime = clickTime - startTime;
            Debug.Log("Your reaction time is: " + resultTime);

            AnswerButtonsVisible.SetActive(false);
            BackToMenuButtonVisible.SetActive(true);

            string resultMessage = buttonIndex == 1
                ? $"Olit oikeassa. Reaktioaikasi oli: {resultTime:F2} sekuntia"
                : $"Olit v‰‰r‰ss‰. Reaktioaikasi oli: {resultTime:F2} sekuntia";

            resultText.text = resultMessage;
        }

        public void PlayGame1Button()
        {
            PlayClickSound();
            SceneManager.LoadScene(randomScene);
        }

        public void PlayGame2Button()
        {
            PlayClickSound();
            SceneManager.LoadScene(4);
        }

        public void PlayGame3Button()
        {
            PlayClickSound();
            SceneManager.LoadScene(5);
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