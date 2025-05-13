using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StoryUIManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public Button Button1;
    public Button Button2;
    public Button Button3;
    public Button Button4;
    public Button returnToMenuButton;

    public StoryNode startingNode;

    private StoryNode currentNode;

    void Start()
    {
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

        // Set text if the button is shown
        if (node.showButton1)
        {
            Button1.GetComponentInChildren<TextMeshProUGUI>().text = node.Button1Text;
            Button1.onClick.RemoveAllListeners();
            if (node.Node1 != null)
                Button1.onClick.AddListener(() => LoadNode(node.Node1));
        }

        if (node.showButton2)
        {
            Button2.GetComponentInChildren<TextMeshProUGUI>().text = node.Button2Text;
            Button2.onClick.RemoveAllListeners();
            if (node.Node2 != null)
                Button2.onClick.AddListener(() => LoadNode(node.Node2));
        }

        if (node.showButton3)
        {
            Button3.GetComponentInChildren<TextMeshProUGUI>().text = node.Button3Text;
            Button3.onClick.RemoveAllListeners();
            if (node.Node3 != null)
                Button3.onClick.AddListener(() => LoadNode(node.Node3));
        }

        if (node.showButton4)
        {
            Button4.GetComponentInChildren<TextMeshProUGUI>().text = node.Button4Text;
            Button4.onClick.RemoveAllListeners();
            if (node.Node4 != null)
                Button4.onClick.AddListener(() => LoadNode(node.Node4));
        }

        returnToMenuButton.gameObject.SetActive(node.showReturnToMenu);
    }

    void ReturnToMenu()
    {
        Debug.Log("Returning to Menu...");
        // You can load a menu scene like this:
        // SceneManager.LoadScene("MainMenu");
    }
}