using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using RPGGame.Global;

public class JoystickHandle : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    RectTransform rect;
    RectTransform parentRect;
    Joystick joy;
    Vector2 screenVec;

    bool isDrag = false;
    Vector2 dir;
    public Vector2 JoyPos
    {
        get { return dir; }
    }
    public event Vec2CallBackDelegate OnHandleDown, OnHandleUp, OnHandleDrag;

    private void Awake()
    {
        screenVec = new Vector2(GlobalData.ScreenWidth, GlobalData.ScreenHeight) / 2;
        parentRect = transform.parent.GetComponent<RectTransform>();
        joy = GetComponentInParent<Joystick>();
        rect = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos = eventData.position - screenVec;
        dir = Vector2.ClampMagnitude(pos - (Vector2)parentRect.localPosition, joy.range);
        if (OnHandleDrag != null)
        {
            OnHandleDrag(dir / joy.range);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isDrag = true;
        Vector2 pos = eventData.position - screenVec;
        dir = Vector2.ClampMagnitude(pos - (Vector2)parentRect.localPosition, joy.range);
        if (OnHandleDown != null)
        {
            OnHandleDown(dir / joy.range);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isDrag = false;
        dir = Vector2.zero;
        if (OnHandleUp != null)
        {
            OnHandleUp(dir);
        }
    }

    private void Update()
    {
        float speed = isDrag ? joy.damp : joy.gravity;
        rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, dir, Time.deltaTime * speed);
    }
}
