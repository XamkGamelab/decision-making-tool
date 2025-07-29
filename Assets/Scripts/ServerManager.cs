using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


namespace JAS.MediDeci
{
    public class ServerManager : GenericSingleton<ServerManager>
    {
        [Header("Server Configuration")]
        public string serverBaseURL = "https://ensipaatos.gamelab.fi/api";

        [Header("PlayerPrefs Keys")]
        public string userIdKey = "SavedUserId"; //saving user id from server into PlayerPrefs
        public string sliderValueKey = "SavedYearValue"; //player's year
        public string inputTextKey = "SavedInputName"; //player name

        /// <summary>
        /// Register or get existing user from server
        /// Call this when UserInfoSaver saves the data
        /// </summary>
        public void RegisterUser(string name, int year, System.Action<bool, int> callback = null)
        {
            StartCoroutine(RegisterUserCoroutine(name, year, callback));
        }

        /// <summary>
        /// Log a choice to the server.
        /// Uses the stored user_id from PlayerPrefs
        /// </summary>
        public void LogChoice(string choiceText, string sceneName, System.Action<bool> callback = null)
        {
            if (!PlayerPrefs.HasKey(userIdKey))
            {
                Debug.LogError("No user_id found! Make sure to register user first.");
                callback?.Invoke(false);
                return;
            }

            int userId = PlayerPrefs.GetInt(userIdKey);
            StartCoroutine(LogChoiceCoroutine(userId, choiceText, sceneName, callback));
        }

        /// <summary>
        /// Get the current user ID from PlayerPrefs
        /// </summary>
        public int GetCurrentUserId()
        {
            return PlayerPrefs.GetInt(userIdKey, -1);
        }

        /// <summary>
        /// Check if user is registered
        /// </summary>
        public bool IsUserRegistered()
        {
            return PlayerPrefs.HasKey(userIdKey) && GetCurrentUserId() > 0;
        }

        private IEnumerator RegisterUserCoroutine(string name, int year, System.Action<bool,int> callback)
        {
            string url = serverBaseURL + "/save-user";
            Debug.Log("Full URL:" + url);

            UserData userData = new UserData
            {
                name = name,
                year = year
            };

            string json = JsonUtility.ToJson(userData);
            Debug.Log($"Registering user: {json}");

            using (var request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error registering user: {request.error}");
                    Debug.LogError($"Response: {request.downloadHandler.text}");
                    callback?.Invoke(false, -1);
                }
                else
                {
                    try
                    {
                        UserResponse response = JsonUtility.FromJson<UserResponse>(request.downloadHandler.text);
                        Debug.Log($"User registered successfully! ID: {response.user_id}");

                        // Save user_id to PlayerPrefs
                        PlayerPrefs.SetInt(userIdKey, response.user_id);
                        PlayerPrefs.Save();

                        callback?.Invoke(true, response.user_id);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError($"Error parsing user registration response: {e.Message}");
                        callback?.Invoke(false, -1);
                    }
                }
            }
        }

        private IEnumerator LogChoiceCoroutine(int userId, string choiceText, string sceneName, System.Action<bool> callback)
        {
            string url = serverBaseURL + "/save-choice";

            ChoiceData choiceData = new ChoiceData
            {
                user_id = userId,
                choice_text = choiceText,
                scene_name = sceneName
            };

            string json = JsonUtility.ToJson(choiceData);
            Debug.Log($"Logging choice: {json}");

            using (var request = new UnityWebRequest(url, "POST"))
            {
                byte[] bodyRaw = System.Text.Encoding.UTF8.GetBytes(json);
                request.uploadHandler = new UploadHandlerRaw(bodyRaw);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");

                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Error logging choice: {request.error}");
                    Debug.LogError($"Response: {request.downloadHandler.text}");
                    callback?.Invoke(false);
                }
                else
                {
                    Debug.Log("Choice logged successfully!");
                    Debug.Log($"Server response: {request.downloadHandler.text}");
                    callback?.Invoke(true);
                }
            }
        }

        [System.Serializable]
        public class UserData
        {
            public string name;
            public int year;
        }

        [System.Serializable]
        public class UserResponse
        {
            public int user_id;
            public string message;
            public bool existing;
        }

        [System.Serializable]
        public class ChoiceData
        {
            public int user_id;
            public string choice_text;
            public string scene_name;
        }
    }



}
