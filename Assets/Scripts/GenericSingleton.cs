using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    public static T instance { get; private set; }

    public virtual void Awake()
    {
        if (instance == null)
        {
            instance = this as T;
            Debug.Log("Created an instance of " + this.name);
            // Do we want to do this???
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    protected virtual void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
