using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JAS.MediDeci
{
    public class Scenechanger : MonoBehaviour
    {
        public void BackToMenu()
        {
            SceneManager.LoadScene("Main Menu");
        }
    }
}
