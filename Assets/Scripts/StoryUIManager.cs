using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Manages the display and interaction of story nodes in the UI.
/// Dynamically loads options based on the current node's data.
/// </summary>
public class StoryUIManager : MonoBehaviour
{
    [Header("Choicelogger Object")]
    public ChoiceLogger choiceLogger;
    private string playerId;

    [Header("UI References")]
    public TextMeshProUGUI questionText;
    public Image nodeImageDisplay;
    public Button returnToMenuButton;

    [Tooltip("Pool of UI buttons used to represent choices. Set size to maximum expected options per node (e.g., 5).")]
    public List<Button> optionButtons;

    [Header("Starting Node")]
    public StoryNode startingNode;
    //private StoryNode currentNode;

    private void Start()
    {
        // If you want the same player ID every time the player reopens the game
        if (!PlayerPrefs.HasKey("PlayerID"))
        {
            PlayerPrefs.SetString("PlayerID", System.Guid.NewGuid().ToString());
        }

        playerId = PlayerPrefs.GetString("PlayerID");

        // Optional: Start background music
        if (BGAudioManager.instance != null)
        {
            BGAudioManager.instance.PlayMusic(BGAudioManager.instance.gameMusic);
        }

        returnToMenuButton.onClick.AddListener(ReturnToMenu);
        LoadNode(startingNode);
    }

    /// <summary>
    /// Loads a StoryNode and updates all UI elements accordingly.
    /// </summary>
    private void LoadNode(StoryNode node)
    {
        //currentNode = node;

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
            string selectedChoiceText = option.optionText;
            button.onClick.AddListener(() =>
            {
                if (AudioManager.instance != null)
                {
                    AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
                }

                if (choiceLogger != null)
                {
                    Debug.Log($"Logging choice: {selectedChoiceText} at node {node.nodeId}");
                    choiceLogger.LogChoice(playerId, selectedChoiceText, node.nodeId);
                }
                else
                {
                    Debug.LogWarning("ChoiceLogger is not assigned!");
                }

                if (nextNode != null)
                    LoadNode(nextNode);
            });
        }
    }

    /// <summary>
    /// Returns the player to the main menu scene.
    /// </summary>
    private void ReturnToMenu()
    {
        if (AudioManager.instance != null)
        {
            AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
        }

        SceneManager.LoadScene(0);
    }
}