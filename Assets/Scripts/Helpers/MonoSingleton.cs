using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<T>();
                if (instance == null)
                {
                    Debug.LogError($"{typeof(T)} MonoSingleton is null");
                }
            }

            return instance;
        }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }
}