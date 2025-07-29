using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;

namespace JAS.MediDeci
{
    public class StoryUIManager : MonoBehaviour
    {
        [Header("Managers & Data")]
        public ChoiceLogger choiceLogger;
        private string playerId;

        [Header("UI Elements")]
        public TextMeshProUGUI questionText;
        public RectTransform questionTextRect;
        public ScrollRect questionScrollRect;
        public Image nodeImageDisplay;
        public RectTransform questionContainer;
        public Button returnToMenuButton;
        public RectTransform returnToMenuRect;

        [Header("Option Buttons")]
        public List<Button> optionButtons;

        [Header("Feedback UI")]
        public GameObject feedbackPanel;
        public CanvasGroup feedbackGroup;
        public TextMeshProUGUI feedbackText;
        public Button feedbackNextButton;

        [Header("Initial Node")]
        public StoryNode startingNode;

        // Private state
        private StoryNode _currentNode;
        private Vector2 _feedbackOriginalPos;
        private Vector2 _returnToMenuOriginalPos;

        // Animation constants
        private const float SlideDuration = 0.14f;
        private const float ButtonPopDuration = 0.2f;
        private const float ButtonStagger = 0.05f;

        private void Start()
        {
            _feedbackOriginalPos = feedbackPanel.GetComponent<RectTransform>().anchoredPosition;
            _returnToMenuOriginalPos = returnToMenuRect.anchoredPosition;

            feedbackPanel.SetActive(false);
            returnToMenuRect.gameObject.SetActive(false);

            var _ = ServerManager.Instance;

            playerId = PlayerPrefs.GetString("PlayerID", System.Guid.NewGuid().ToString());
            PlayerPrefs.SetString("PlayerID", playerId);

            if (BGAudioManager.Instance != null)
                BGAudioManager.Instance.PlayMusic(BGAudioManager.Instance.gameMusic);

            returnToMenuButton.onClick.AddListener(ReturnToMenu);

            LoadNode(startingNode);
            StartCoroutine(AnimateInitialButtons());
        }

        /// <summary>Loads and sets up a new node.</summary>
        private void LoadNode(StoryNode node)
        {
            _currentNode = node;
            questionText.text = node.questionText;
            StartCoroutine(ResizeQuestionTextAndResetScroll());

            nodeImageDisplay.gameObject.SetActive(node.nodeImage != null);
            if (node.nodeImage != null)
                nodeImageDisplay.sprite = node.nodeImage;

            returnToMenuRect.gameObject.SetActive(node.showReturnToMenu);
            if (node.showReturnToMenu)
            {
                Vector2 offscreenRight = _returnToMenuOriginalPos + Vector2.right * Screen.width;
                returnToMenuRect.anchoredPosition = offscreenRight;
                StartCoroutine(LerpPosition(returnToMenuRect, offscreenRight, _returnToMenuOriginalPos, SlideDuration));
            }

            // Prepare buttons
            foreach (Button btn in optionButtons)
            {
                btn.gameObject.SetActive(false);
                btn.onClick.RemoveAllListeners();
                btn.transform.localScale = Vector3.zero;
            }

            // Assign visible options
            for (int i = 0; i < Mathf.Min(node.Options.Count, optionButtons.Count); i++)
            {
                StoryNode.StoryOption option = node.Options[i];
                if (option == null || !option.isVisible) continue;

                Button button = optionButtons[i];
                button.GetComponentInChildren<TextMeshProUGUI>().text = option.optionText;

                StoryNode nextNode = option.nextNode;

                button.onClick.AddListener(() =>
                {
                    if (AudioManager.Instance != null)
                        AudioManager.Instance.PlayAudioClip(AudioManager.Instance.clickButtonSound);

                    if (option.isLoggable)
                    {
                        if (choiceLogger == null)
                        {
                            Debug.LogWarning("ChoiceLogger is null!");
                        }
                        else if (ServerManager.Instance == null)
                        {
                            Debug.LogWarning("ServerManager.Instance is null!");
                        }
                        else
                        {
                            choiceLogger.LogChoice(option.optionText, _currentNode.nodeId);
                        }
                    }
                        

                    if (option.isLoggable)
                        StartCoroutine(ShowFeedbackThenLoadNext(option.optionText, nextNode));
                    else if (nextNode != null)
                        StartCoroutine(TransitionToNextNode(nextNode));
                });
            }
        }

        private IEnumerator ShowFeedbackThenLoadNext(string selectedOption, StoryNode nextNode)
        {
            yield return new WaitForSeconds(0.2f);
            yield return StartCoroutine(AnimateFeedbackIn($"Valitsit: {selectedOption}"));

            feedbackNextButton.interactable = true;
            feedbackNextButton.onClick.RemoveAllListeners();
            feedbackNextButton.onClick.AddListener(() =>
            {
                feedbackNextButton.interactable = false;
                SetAllButtonsInteractable(false);

                StartCoroutine(AnimateFeedbackOut(() =>
                {
                    if (nextNode != null)
                        StartCoroutine(TransitionToNextNode(nextNode));
                }));
            });
        }

        private IEnumerator TransitionToNextNode(StoryNode nextNode)
        {
            Vector2 startPos = questionContainer.anchoredPosition;
            Vector2 offscreenLeft = startPos + Vector2.left * Screen.width;
            Vector2 offscreenRight = startPos + Vector2.right * Screen.width;

            SetAllButtonsInteractable(false);
            feedbackNextButton.interactable = false;

            // Optional: Slide out return to menu
            if (returnToMenuRect.gameObject.activeSelf)
            {
                Vector2 offscreenReturn = _returnToMenuOriginalPos + Vector2.right * Screen.width;
                yield return LerpPosition(returnToMenuRect, _returnToMenuOriginalPos, offscreenReturn, SlideDuration);
                returnToMenuRect.gameObject.SetActive(false);
            }

            // Slide out question container
            yield return LerpPosition(questionContainer, startPos, offscreenLeft, SlideDuration);

            // Clean current
            feedbackPanel.SetActive(false);
            foreach (var btn in optionButtons) btn.gameObject.SetActive(false);

            // Slide in new node
            questionContainer.anchoredPosition = offscreenRight;
            LoadNode(nextNode);
            yield return LerpPosition(questionContainer, offscreenRight, startPos, SlideDuration);

            // Pop-in buttons
            for (int i = 0; i < Mathf.Min(_currentNode.Options.Count, optionButtons.Count); i++)
            {
                StoryNode.StoryOption option = _currentNode.Options[i];
                if (option == null || !option.isVisible) continue;

                StartCoroutine(AnimateButtonPop(optionButtons[i].gameObject, i * ButtonStagger));
            }
        }

        private IEnumerator AnimateButtonPop(GameObject obj, float delay)
        {
            yield return new WaitForSeconds(delay);

            obj.SetActive(true);
            Button btn = obj.GetComponent<Button>();
            btn.interactable = false;

            Transform tf = obj.transform;
            Vector3 overshoot = Vector3.one * 1.1f;

            float t = 0;
            while (t < ButtonPopDuration)
            {
                t += Time.deltaTime;
                tf.localScale = Vector3.Lerp(Vector3.zero, overshoot, t / ButtonPopDuration);
                yield return null;
            }

            t = 0;
            while (t < ButtonPopDuration * 0.5f)
            {
                t += Time.deltaTime;
                tf.localScale = Vector3.Lerp(overshoot, Vector3.one, t / (ButtonPopDuration * 0.5f));
                yield return null;
            }

            tf.localScale = Vector3.one;
            btn.interactable = true;
        }

        private IEnumerator ResizeQuestionTextAndResetScroll()
        {
            yield return null;
            float height = questionText.preferredHeight;
            questionTextRect.sizeDelta = new Vector2(questionTextRect.sizeDelta.x, height);
            questionScrollRect.verticalNormalizedPosition = 1f;
        }

        private IEnumerator AnimateFeedbackIn(string text)
        {
            feedbackText.text = text;
            RectTransform rect = feedbackPanel.GetComponent<RectTransform>();

            Vector2 from = _feedbackOriginalPos + Vector2.down * rect.rect.height;
            rect.anchoredPosition = from;
            feedbackPanel.SetActive(true);

            yield return LerpPosition(rect, from, _feedbackOriginalPos, 0.15f);
        }

        private IEnumerator AnimateFeedbackOut(System.Action onComplete)
        {
            onComplete?.Invoke();
            yield return null;
        }

        private IEnumerator AnimateInitialButtons()
        {
            yield return null;

            for (int i = 0; i < Mathf.Min(_currentNode.Options.Count, optionButtons.Count); i++)
            {
                StoryNode.StoryOption option = _currentNode.Options[i];
                if (option == null || !option.isVisible) continue;

                StartCoroutine(AnimateButtonPop(optionButtons[i].gameObject, i * ButtonStagger));
            }
        }

        private IEnumerator LerpPosition(RectTransform rect, Vector2 from, Vector2 to, float duration)
        {
            float t = 0;
            while (t < duration)
            {
                t += Time.deltaTime;
                rect.anchoredPosition = Vector2.Lerp(from, to, t / duration);
                yield return null;
            }

            rect.anchoredPosition = to;
        }

        private void SetAllButtonsInteractable(bool interactable)
        {
            foreach (var btn in optionButtons)
                btn.interactable = interactable;
        }

        private void ReturnToMenu()
        {
            if (AudioManager.Instance != null)
                AudioManager.Instance.PlayAudioClip(AudioManager.Instance.clickButtonSound);

            SceneManager.LoadScene(2); //Tässä vain QA:ta varten 2 on vanha menu, 0 uusi
        }
    }
}