using UnityEngine;
using UnityEngine.UI;

namespace JAS.MediDeci
{
    public class LaunchPlayerProfile : MonoBehaviour
    {
        [Header("PlayerPrefs Keys")]
        public string sliderValueKey = "SavedYear";
        public string inputTextKey = "SavedName";

        [SerializeField] private GameObject panel;

        public Button button;
        public GameObject exitButton;

        private void Start()
        {
            if (!PlayerPrefs.HasKey(sliderValueKey) &&
                !PlayerPrefs.HasKey(inputTextKey))
            {
                panel.SetActive(true);
                exitButton.SetActive(false);

                button.onClick.AddListener(() =>
                {
                    exitButton.SetActive(true);
                });
            }
            else
            {
                exitButton.SetActive(true);
            }
        }
    }
}