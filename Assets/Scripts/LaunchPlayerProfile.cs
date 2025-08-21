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

        public Button saveButton;
        public Button exitButton;

        private void Start()
        {
            if (!PlayerPrefs.HasKey(sliderValueKey) &&
                !PlayerPrefs.HasKey(inputTextKey))
            {
                panel.SetActive(true);
                exitButton.interactable = false;

                saveButton.onClick.AddListener(() =>
                {
                    exitButton.interactable = true;
                });
            }
            else
            {
                exitButton.interactable = true;
            }
        }
    }
}