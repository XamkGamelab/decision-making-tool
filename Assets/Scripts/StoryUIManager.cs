using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// Manages the display and interaction of story nodes in the UI.
/// Dynamically loads options based on the current node's data.
/// </summary>
namespace JAS.MediDeci
{
    public class StoryUIManager : MonoBehaviour
    {
        [Header("Choiselogger empty gameobject")]
        public ChoiceLogger choiceLogger;
        private string playerId;

        [Header("UI References")]
        public TextMeshProUGUI questionText;
        public Image nodeImageDisplay;
        public Button returnToMenuButton;

        [Tooltip("Pool of UI buttons used to represent choices. Set size to maximum expected options per node (e.g., 5).")]
        public List<Button> optionButtons;

        [Header("Feedback UI")]
        public GameObject feedbackPanel;
        public TextMeshProUGUI feedbackText;
        public Button feedbackNextButton;

        [Header("Starting Node")]
        public StoryNode startingNode;
        private StoryNode _currentNode;

        private void Start()
        {
            feedbackPanel.SetActive(false);

            // If you want the same player ID every time the player reopens the game
            if (!PlayerPrefs.HasKey("PlayerID"))
            {
                PlayerPrefs.SetString("PlayerID", System.Guid.NewGuid().ToString());
            }

            playerId = PlayerPrefs.GetString("PlayerID");

            // Optional: Start background music
            if (BGAudioManager.Instance != null)
            {
                BGAudioManager.Instance.PlayMusic(BGAudioManager.Instance.gameMusic);
            }

            returnToMenuButton.onClick.AddListener(ReturnToMenu);
            LoadNode(startingNode);
        }

        /// <summary>
        /// Loads a StoryNode and updates all UI elements accordingly.
        /// </summary>
        private void LoadNode(StoryNode node)
        {
            _currentNode = node;
            questionText.text = node.questionText;

            // Handle image display
            bool showImage = node.showImage && node.nodeImage != null;
            nodeImageDisplay.gameObject.SetActive(showImage);
            if (showImage)
                nodeImageDisplay.sprite = node.nodeImage;

            returnToMenuButton.gameObject.SetActive(node.showReturnToMenu);

            // Reset all buttons
            foreach (Button button in optionButtons)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
            }

            // Apply available options
            for (int i = 0; i < Mathf.Min(node.Options.Count, optionButtons.Count); i++)
            {
                var option = node.Options[i];
                if (option == null || !option.isVisible) continue;

                Button button = optionButtons[i];
                TextMeshProUGUI textComponent = button.GetComponentInChildren<TextMeshProUGUI>();

                if (textComponent != null)
                    textComponent.text = option.optionText;

                button.gameObject.SetActive(true);

                // Cache to local variable to avoid closure issues
                StoryNode nextNode = option.nextNode;
                button.onClick.AddListener(() =>
                {
                    if (AudioManager.Instance != null)
                    {
                        AudioManager.Instance.PlayAudioClip(AudioManager.Instance.clickButtonSound);
                    }

                    // Log the choice to backend
                    if (choiceLogger != null)
                    {
                        Debug.Log($"Logging choice: {option.optionText} at node {node.nodeId}");
                        choiceLogger.LogChoice(playerId, option.optionText, node.nodeId);
                    }
                    else
                    {
                        Debug.LogWarning("ChoiceLogger is not assigned!");
                    }

                    // Start delayed feedback + transition coroutine
                    StartCoroutine(ShowFeedbackThenLoadNext(option.optionText, nextNode));
                });
            }
        }

        private IEnumerator ShowFeedbackThenLoadNext(string selectedOption, StoryNode nextNode)
        {
            yield return new WaitForSeconds(0.2f); // Delay before showing feedback

            // Temp text, will be changed down the line
            feedbackText.text = $"Valitsit: {selectedOption}";
            feedbackPanel.SetActive(true);

            // Remove old listeners
            feedbackNextButton.onClick.RemoveAllListeners();

            feedbackNextButton.onClick.AddListener(() =>
            {
                feedbackPanel.SetActive(false);
                if (nextNode != null)
                {
                    LoadNode(nextNode);
                }
            });
        }

        /// <summary>
        /// Returns the player to the main menu scene.
        /// </summary>
        private void ReturnToMenu()
        {
            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayAudioClip(AudioManager.Instance.clickButtonSound);
            }

            SceneManager.LoadScene(0);
        }
    }
}