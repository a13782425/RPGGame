using RPGGame.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Enums;
using RPGGame.Manager;

public class Joystick : BaseUI
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

    public override UIEnum CurrentEnum
    {
        get
        {
            return UIEnum.MoveUI;
        }
    }

    public override void Init()
    {
        rect = transform.Find("JoyStickBG").GetComponent<RectTransform>();
        handle = GetComponentInChildren<JoystickHandle>();

        //handle.OnHandleDown += (vec) => { };
        handle.OnHandleDrag += OnDrag;
        handle.OnHandleUp += OnUp;
    }

    private void OnUp(Vector2 vec)
    {
        InputManager.Instance.OnPlayerMoveJoy(vec, true);
    }

    private void OnDrag(Vector2 vec)
    {
        InputManager.Instance.OnPlayerMoveJoy(vec);
    }
}
