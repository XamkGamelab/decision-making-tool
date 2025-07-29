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

        [Header("UI Feedback")]
        public GameObject loadingIndicator; // Optional loading spinner
        public TextMeshProUGUI statusText; // Optional status text

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

            // Check if user is already registered
            if (ServerManager.Instance.IsUserRegistered())
            {
                int userId = ServerManager.Instance.GetCurrentUserId();
                SetStatus($"Already registered with ID: {userId}", Color.green);
            }
        }

        private void UpdateValueDisplay(float value)
        {
            yearDisplayText.text = value.ToString();
        }

        private void SaveValues()
        {
            float sliderValue = valueSlider.value;
            string inputText = nameInputField.text.Trim();

            // Validate input
            if (string.IsNullOrEmpty(inputText))
            {
                SetStatus("Please enter your name!", Color.red);
                return;
            }

            // Save locally first
            PlayerPrefs.SetFloat(sliderValueKey, sliderValue);
            PlayerPrefs.SetString(inputTextKey, inputText);
            PlayerPrefs.Save();

            Debug.Log($"Saved locally: Slider={sliderValue}, Input='{inputText}'");

            // Show loading state
            SetLoadingState(true);
            SetStatus("Registering with server...", Color.yellow);

            // Register with server
            ServerManager.Instance.RegisterUser(inputText, (int)sliderValue, OnUserRegistered);
        }

        private void OnUserRegistered(bool success, int userId)
        {
            SetLoadingState(false);

            if (success)
            {
                SetStatus($"Successfully registered! User ID: {userId}", Color.green);
                Debug.Log($"User registered with server. ID: {userId}");
            }
            else
            {
                SetStatus("Failed to register with server!", Color.red);
                Debug.LogError("Failed to register user with server");
            }
        }

        private void SetLoadingState(bool loading)
        {
            if (loadingIndicator != null)
                loadingIndicator.SetActive(loading);

            if (saveButton != null)
                saveButton.interactable = !loading;
        }

        private void SetStatus(string message, Color color)
        {
            if (statusText != null)
            {
                statusText.text = message;
                statusText.color = color;
            }
        }
    }
}