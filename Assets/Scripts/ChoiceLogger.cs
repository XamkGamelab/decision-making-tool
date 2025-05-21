using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class ChoiceLogger : MonoBehaviour
{
    public void LogChoice(string playerId, string choiceText, string sceneName)
    {
        StartCoroutine(SendChoice(playerId, choiceText, sceneName));
    }

    IEnumerator SendChoice(string playerId, string choiceText, string sceneName)
    {
        string url = "http://localhost:3000/save-choice";

        string json = JsonUtility.ToJson(new ChoiceData
        {
            player_id = playerId,
            choice_text = choiceText,
            scene_name = sceneName
        });

        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
            Debug.Log("Error: " + request.error);
        else
            Debug.Log("Choice saved!");
    }

    [System.Serializable]
    public class ChoiceData
    {
        public string player_id;
        public string choice_text;
        public string scene_name;
    }
}