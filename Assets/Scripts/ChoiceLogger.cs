using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using JAS.MediDeci;

namespace JAS.MediDeci
{

    public class ChoiceLogger : MonoBehaviour
    {
        /// <summary>
        /// Log a choice to the server
        /// </summary>
        /// <param name="choiceText">The choice that was made</param>
        /// <param name="sceneName">The scene where the choice was made</param>
        public void LogChoice(string choiceText, string sceneName)
        {
            // Check if user is registered
            if (!ServerManager.Instance.IsUserRegistered())
            {
                Debug.LogError("Cannot log choice: User not registered!");
                return;
            }

            Debug.Log($"Logging choice: '{choiceText}' in scene '{sceneName}'");

            ServerManager.Instance.LogChoice(choiceText, sceneName, OnChoiceLogged);
        }

        private void OnChoiceLogged(bool success)
        {
            if (success)
            {
                Debug.Log("Choice successfully logged to server!");
            }
            else
            {
                Debug.LogError("Failed to log choice to server!");
            }
        }

        // Legacy method for backward compatibility - you can remove this if not needed
        public void LogChoice(string playerId, string choiceText, string sceneName)
        {
            Debug.LogWarning("Using legacy LogChoice method. Player ID will be ignored, using registered user instead.");
            LogChoice(choiceText, sceneName);
        }
    }
}