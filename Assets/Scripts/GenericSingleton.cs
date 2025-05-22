using UnityEngine;

/// <summary>
/// Generic singleton base class for MonoBehaviours.
/// Ensures only one instance exists and persists across scenes if needed.
/// </summary>
namespace JAS.MediDeci
{
    public class GenericSingleton<T> : MonoBehaviour where T : Component
    {
        public static T Instance { get; private set; }
        protected virtual bool ShouldPersist => true;

        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                Debug.Log($"[Singleton] Instance created: {typeof(T).Name}");

                if (ShouldPersist)
                    DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.LogWarning($"[Singleton] Duplicate {typeof(T).Name} destroyed.");
                Destroy(gameObject);
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}