using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryUIManager : MonoBehaviour
{
    public ChoiceLogger choiceLogger;
    private string playerId;

    public TextMeshProUGUI questionText;
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    public Button returnToMenuButton;

    public StoryNode startingNode;

    private StoryNode currentNode;

    public Image nodeImageDisplay;

    void Start()
    {
        // If you want the same player ID every time the player reopens the game
        if (!PlayerPrefs.HasKey("PlayerID"))
        {
            PlayerPrefs.SetString("PlayerID", System.Guid.NewGuid().ToString());
        }
        playerId = PlayerPrefs.GetString("PlayerID");

        // Ensure the game background music plays
        if (BGAudioManager.instance != null)
        {
            BGAudioManager.instance.PlayMusic(BGAudioManager.instance.gameMusic);
        }

        LoadNode(startingNode);

        returnToMenuButton.onClick.AddListener(ReturnToMenu);
    }

    void LoadNode(StoryNode node)
    {
        currentNode = node;
        questionText.text = node.questionText;

        // Set visibility of each button
        Button1.gameObject.SetActive(node.showButton1);
        Button2.gameObject.SetActive(node.showButton2);
        Button3.gameObject.SetActive(node.showButton3);
        Button4.gameObject.SetActive(node.showButton4);

        // Set image if enabled
        if (node.showImage && node.nodeImage != null)
        {
            nodeImageDisplay.gameObject.SetActive(true);
            nodeImageDisplay.sprite = node.nodeImage;
        }
        else
        {
            nodeImageDisplay.gameObject.SetActive(false);
        }

        // Set text if the button is shown
        if (node.showButton1)
        {
            Button1.GetComponentInChildren<TextMeshProUGUI>().text = node.Button1Text;
            Button1.onClick.RemoveAllListeners();
            if (node.Node1 != null)
            {
                Button1.onClick.AddListener(() =>
                {
                    AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
                    //Log the choice
                    choiceLogger.LogChoice(playerId, node.Button1Text, currentNode.nodeId);
                    LoadNode(node.Node1);
                });
            }
        }

        if (node.showButton2)
        {
            Button2.GetComponentInChildren<TextMeshProUGUI>().text = node.Button2Text;
            Button2.onClick.RemoveAllListeners();
            if (node.Node2 != null)
            {
                Button2.onClick.AddListener(() =>
                {
                    AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
                    choiceLogger.LogChoice(playerId, node.Button2Text, currentNode.nodeId);
                    LoadNode(node.Node2);
                });
            }
        }

        if (node.showButton3)
        {
            Button3.GetComponentInChildren<TextMeshProUGUI>().text = node.Button3Text;
            Button3.onClick.RemoveAllListeners();
            if (node.Node3 != null)
            {
                Button3.onClick.AddListener(() =>
                {
                    AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
                    choiceLogger.LogChoice(playerId, node.Button3Text, currentNode.nodeId);
                    LoadNode(node.Node3);
                });
            }
        }

        if (node.showButton4)
        {
            Button4.GetComponentInChildren<TextMeshProUGUI>().text = node.Button4Text;
            Button4.onClick.RemoveAllListeners();
            if (node.Node4 != null)
            {
                Button4.onClick.AddListener(() =>
                {
                    AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
                    choiceLogger.LogChoice(playerId, node.Button4Text, currentNode.nodeId);
                    LoadNode(node.Node4);
                });
            }
        }

        returnToMenuButton.gameObject.SetActive(node.showReturnToMenu);
    }



    void ReturnToMenu()
    {
        AudioManager.instance.PlayAudioClip(AudioManager.instance.ClickButtonSound);
        Debug.Log("Returning to Menu...");
        SceneManager.LoadScene(0); // Main Menu
    }
}