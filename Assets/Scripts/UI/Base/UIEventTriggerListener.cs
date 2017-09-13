using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using RPGGame.Manager;

namespace RPGGame.UI
{
    public class UIEventTriggerListener : EventTrigger
    {

        public delegate void VoidDelegate(GameObject go);
        public VoidDelegate onClick;
        public VoidDelegate onPress;
        public VoidDelegate onEnter;
        public VoidDelegate onExit;
        public VoidDelegate onUp;
        public VoidDelegate onSelect;
        public VoidDelegate onUpdateSelect;

        static public UIEventTriggerListener Get(GameObject go)
        {
            UIEventTriggerListener listener = go.GetComponent<UIEventTriggerListener>();
            if (listener == null) listener = go.AddComponent<UIEventTriggerListener>();
            return listener;
        }
        public override void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null && InputManager.Instance.IsClick) onClick(gameObject);
        }
        public override void OnPointerDown(PointerEventData eventData)
        {
            if (onPress != null && InputManager.Instance.IsClick) onPress(gameObject);
        }
        public override void OnPointerEnter(PointerEventData eventData)
        {
            if (onEnter != null && InputManager.Instance.IsClick) onEnter(gameObject);
        }
        public override void OnPointerExit(PointerEventData eventData)
        {
            if (onExit != null && InputManager.Instance.IsClick) onExit(gameObject);
        }
        public override void OnPointerUp(PointerEventData eventData)
        {
            if (onUp != null && InputManager.Instance.IsClick) onUp(gameObject);
        }
        public override void OnSelect(BaseEventData eventData)
        {
            if (onSelect != null && InputManager.Instance.IsClick) onSelect(gameObject);
        }
        public override void OnUpdateSelected(BaseEventData eventData)
        {
            if (onUpdateSelect != null && InputManager.Instance.IsClick) onUpdateSelect(gameObject);
        }
    }
}