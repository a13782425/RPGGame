using UnityEngine;
using System.Collections;

public class AutoScale : MonoBehaviour
{

    public const float MAX_ROTE = 1.1f;
    public const float MIN_ROTE = 0.9f;

    public enum ScaleType
    {
        Both, Small, Big, Custom, Confine1fBig
    }


    public ScaleType scleType = ScaleType.Both;

    //Transform mTransform;
    void Awake()
    {
        //mTransform = transform;
    }



    void Start()
    {
        float rote = 960f * Screen.width / (640f * Screen.height);
        switch (scleType)
        {
            case ScaleType.Both:
                if (rote != 1.0f)
                {
                    transform.localScale = new Vector3(rote, rote, rote);
                }
                break;
            case ScaleType.Small:
                if (rote > 1.0f)
                {
                    transform.localScale = new Vector3(rote, rote, rote);
                }
                break;
            case ScaleType.Big:
                if (rote < 1.0f)
                {
                    transform.localScale = new Vector3(rote, rote, rote);
                }
                break;
            case ScaleType.Custom:
                rote = rote < MIN_ROTE ? MIN_ROTE : (rote > MAX_ROTE ? MAX_ROTE : rote);
                transform.localScale = new Vector3(rote, rote, rote);
                break;
            case ScaleType.Confine1fBig:
                rote = (rote > 1f ? (1f - (rote - 1f)) : 1f);
                transform.localScale = new Vector3(rote, rote, rote);
                break;

        }

    }



}