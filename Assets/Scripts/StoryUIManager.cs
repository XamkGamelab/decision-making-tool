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
        public RectTransform questionTextRect;
        public ScrollRect questionScrollRect;
        public Image nodeImageDisplay;
        public Button returnToMenuButton;

        [Tooltip("Pool of UI buttons used to represent choices. Set size to maximum expected options per node (e.g., 5).")]
        public List<Button> optionButtons;

        [Header("Feedback UI")]
        public GameObject feedbackPanel;
        public CanvasGroup feedbackGroup;
        public TextMeshProUGUI feedbackText;
        public Button feedbackNextButton;

        [Header("Starting Node")]
        public StoryNode startingNode;
        private StoryNode _currentNode;

        private void Start()
        {
            if (feedbackGroup == null)
                feedbackGroup = feedbackPanel.GetComponent<CanvasGroup>();

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
            questionText.text = _currentNode.questionText;

            StartCoroutine(ResizeQuestionTextAndResetScroll());

            if (_currentNode.nodeImage != null)
            {
                nodeImageDisplay.gameObject.SetActive(true);
                nodeImageDisplay.sprite = _currentNode.nodeImage;
            }
            else
            {
                nodeImageDisplay.gameObject.SetActive(false);
            }

            returnToMenuButton.gameObject.SetActive(_currentNode.showReturnToMenu);

            // Reset all buttons
            foreach (Button button in optionButtons)
            {
                button.gameObject.SetActive(false);
                button.onClick.RemoveAllListeners();
            }

            // Apply available options
            for (int i = 0; i < Mathf.Min(_currentNode.Options.Count, optionButtons.Count); i++)
            {
                StoryNode.StoryOption option = _currentNode.Options[i];
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

                    // If loggable, log and show feedback
                    if (option.isLoggable)
                    {
                        if (choiceLogger != null)
                        {
                            Debug.Log($"Logging choice: {option.optionText} at node {_currentNode.nodeId}");
                            choiceLogger.LogChoice(playerId, option.optionText, _currentNode.nodeId);
                        }
                        else
                        {
                            Debug.LogWarning("ChoiceLogger is not assigned!");
                        }

                        StartCoroutine(ShowFeedbackThenLoadNext(option.optionText, nextNode));
                    }
                    else
                    {
                        // Not loggable: skip feedback and go directly to next node
                        if (nextNode != null)
                        {
                            LoadNode(nextNode);
                        }
                    }
                });
            }
        }

        private IEnumerator ShowFeedbackThenLoadNext(string selectedOption, StoryNode nextNode)
        {
            yield return new WaitForSeconds(0.2f); // Delay before showing feedback

            // Temp text, will be changed down the line
            yield return StartCoroutine(AnimateFeedbackIn($"Valitsit: {selectedOption}"));

            // Remove old listeners
            feedbackNextButton.onClick.RemoveAllListeners();
            feedbackNextButton.onClick.AddListener(() =>
            {
                StartCoroutine(AnimateFeedbackOut(() =>
                {
                    feedbackPanel.SetActive(false);
                    if (nextNode != null)
                    {
                        LoadNode(nextNode);
                    }
                }));
            });
        }

        private IEnumerator ResizeQuestionTextAndResetScroll()
        {
            // Wait one frame for layout to update
            yield return null;

            // Resize the RectTransform height to fit the preferred height of the text
            float preferredHeight = questionText.preferredHeight;
            questionTextRect.sizeDelta = new Vector2(questionTextRect.sizeDelta.x, preferredHeight);

            // Reset scroll to top
            if (questionScrollRect != null)
            {
                questionScrollRect.verticalNormalizedPosition = 1f;
            }
        }

        private IEnumerator AnimateFeedbackIn(string text)
        {
            feedbackText.text = text;
            feedbackPanel.SetActive(true);

            if (feedbackGroup == null)
                feedbackGroup = feedbackPanel.GetComponent<CanvasGroup>();

            feedbackGroup.alpha = 0;
            feedbackPanel.transform.localScale = Vector3.one * 0.8f;

            float duration = 0.2f;
            float t = 0;

            while (t < duration)
            {
                t += Time.deltaTime;
                float p = t / duration;
                feedbackGroup.alpha = Mathf.Lerp(0, 1, p);
                feedbackPanel.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, p);
                yield return null;
            }

            feedbackGroup.alpha = 1;
            feedbackPanel.transform.localScale = Vector3.one;
        }

        private IEnumerator AnimateFeedbackOut(System.Action onComplete)
        {
            float duration = 0.2f;
            float t = 0;

            Vector3 startScale = feedbackPanel.transform.localScale;

            while (t < duration)
            {
                t += Time.deltaTime;
                float p = t / duration;
                feedbackGroup.alpha = Mathf.Lerp(1, 0, p);
                feedbackPanel.transform.localScale = Vector3.Lerp(startScale, Vector3.one * 0.8f, p);
                yield return null;
            }

            feedbackGroup.alpha = 0;
            feedbackPanel.transform.localScale = Vector3.one * 0.8f;

            onComplete?.Invoke();
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