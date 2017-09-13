using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public float range = 30f;//摇杆范围(像素)
    public float damp = 50f;//拖拽缓冲
    public float gravity = 30f;//回落缓冲

    RectTransform rect;
    JoystickHandle handle;

    public Vector2 Dir
    {
        get { return handle.JoyPos / range; }
    }

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        handle = GetComponentInChildren<JoystickHandle>();
    }    
}
