using UnityEngine;
using System.Collections;

public class DontDestroyGameObject : MonoBehaviour
{
    void Awake()
    {
        if (Application.isPlaying)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
