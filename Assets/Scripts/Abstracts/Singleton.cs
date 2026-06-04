using UnityEngine;
public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            _instance ??= FindAnyObjectByType<T>();
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this as T;
        }
    }

    protected virtual void Subscribe()
    {
        
    }
    protected virtual void Unsubscribe()
    {
        
    }

    protected virtual void OnDestroy()
    {
        Unsubscribe();
    }

    protected virtual void OnDisable()
    {
        Unsubscribe();
    }

    protected virtual void OnEnable()
    {
        Subscribe();
    }
}