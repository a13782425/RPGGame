using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIScale : MonoBehaviour
{
    public bool IsPos = false;
    public bool IsSmallScale = false;

    //[SerializeField]
    //private float _height = 640f;
    //[SerializeField]
    //private float _width = 960f;

    [SerializeField]
    private float _height = 768;

    [SerializeField]
    private float _width = 1280;
    private float _nowHeight;
    private float _nowWidth;

    void Awake()
    {
        _nowWidth = Screen.width;
        _nowHeight = Screen.height;
    }

    // Use this for initialization
    void Start()
    {
        float x = _nowWidth / _width;
        float y = _nowHeight / _height;
        if (IsSmallScale)
        {
            float f = x > y ? y : x;
            (this.transform as RectTransform).localScale = new Vector3(f, f, 0);
        }
        else
        {
            (this.transform as RectTransform).localScale = new Vector3(x, y, 0);
        }




        if (IsPos)
        {
            Vector2 vec = (this.transform as RectTransform).anchoredPosition;
            (this.transform as RectTransform).anchoredPosition = new Vector2(vec.x * x, vec.y * y);
        }

    }
}
