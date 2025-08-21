using UnityEngine;
using System.Collections;

namespace JAS.MediDeci
{
    public class GlobalCoroutineRunner : MonoBehaviour
    {
        public static GlobalCoroutineRunner Instance;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void RunCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }
    }
}