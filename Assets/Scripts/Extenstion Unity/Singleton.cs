using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton <T> : MonoBehaviour where T:Singleton<T>
{
    public static T Instance;

    public virtual void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }
}

public class SingletonBehavior<T> : MonoBehaviour where T : SingletonBehavior<T>
{
    public static T Instance;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = (T)this;
        }
        else
        {
            Destroy(this);
        }
    }

}
