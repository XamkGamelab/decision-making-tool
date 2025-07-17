using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace JAS.MediDeci
{
    public class UserInfoSaver : MonoBehaviour
    {
        [Header("UI References")]
        public Slider valueSlider;
        public TextMeshProUGUI yearDisplayText;
        public TMP_InputField nameInputField;
        public Button saveButton;

        [Header("PlayerPrefs Keys")]
        public string sliderValueKey = "SavedYearValue";
        public string inputTextKey = "SavedInputName";

        private void Start()
        {
            // Load previously saved values, if any
            if (PlayerPrefs.HasKey(sliderValueKey))
            {
                float savedValue = PlayerPrefs.GetFloat(sliderValueKey);
                valueSlider.value = savedValue;
                UpdateValueDisplay(savedValue);
            }

            if (PlayerPrefs.HasKey(inputTextKey))
            {
                nameInputField.text = PlayerPrefs.GetString(inputTextKey);
            }

            // Hook up events
            valueSlider.onValueChanged.AddListener(UpdateValueDisplay);
            saveButton.onClick.AddListener(SaveValues);
        }

        private void UpdateValueDisplay(float value)
        {
            yearDisplayText.text = value.ToString();
        }

        private void SaveValues()
        {
            float sliderValue = valueSlider.value;
            string inputText = nameInputField.text;

            PlayerPrefs.SetFloat(sliderValueKey, sliderValue);
            PlayerPrefs.SetString(inputTextKey, inputText);
            PlayerPrefs.Save();

            Debug.Log($"Saved: Slider={sliderValue}, Input='{inputText}'");
        }
    }
}