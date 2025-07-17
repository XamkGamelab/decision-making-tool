using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JAS.MediDeci
{
    public class SceneChanger : MonoBehaviour
    {
        public void BackToMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}