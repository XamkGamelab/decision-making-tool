using UnityEngine;

namespace JAS.MediDeci
{
    public class LaunchPlayerProfile : MonoBehaviour
    {
        [Header("PlayerPrefs Keys")]
        public string sliderValueKey = "SavedYear";
        public string inputTextKey = "SavedName";

        [SerializeField] private GameObject panel;

        private void Start()
        {
            if (!PlayerPrefs.HasKey(sliderValueKey) &&
                !PlayerPrefs.HasKey(inputTextKey))
            {
                panel.SetActive(true);
            }
        }
    }
}