using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RPGGame.Enums;
using RPGGame.Manager;

namespace RPGGame.UI
{
    public class MoveUIController : BaseUI
    {

        private Button _upButton;
        private Button _downButton;
        private Button _leftButton;
        private Button _rightButton;

        public override void Init()
        {
            base.Init();
            _upButton = this.CurrentTransform.Find("MovePanel/Up").GetComponent<Button>();
            _downButton = this.CurrentTransform.Find("MovePanel/Down").GetComponent<Button>();
            _leftButton = this.CurrentTransform.Find("MovePanel/Left").GetComponent<Button>();
            _rightButton = this.CurrentTransform.Find("MovePanel/Right").GetComponent<Button>();
            UIEventTriggerListener.Get(_upButton.gameObject).onPress = OnButtonClick;
            UIEventTriggerListener.Get(_downButton.gameObject).onPress = OnButtonClick;
            UIEventTriggerListener.Get(_leftButton.gameObject).onPress = OnButtonClick;
            UIEventTriggerListener.Get(_rightButton.gameObject).onPress = OnButtonClick;
            UIEventTriggerListener.Get(_upButton.gameObject).onUp = OnButtonUpClick;
            UIEventTriggerListener.Get(_downButton.gameObject).onUp = OnButtonUpClick;
            UIEventTriggerListener.Get(_leftButton.gameObject).onUp = OnButtonUpClick;
            UIEventTriggerListener.Get(_rightButton.gameObject).onUp = OnButtonUpClick;
        }

        private void OnButtonUpClick(GameObject go)
        {
            string name = go.name;

            switch (name)
            {
                case "Up":
                case "Down":
                    InputManager.Instance.OnPlayerMoveClick(ButtonEnum.Y, 0);
                    break;
                case "Left":
                case "Right":
                    InputManager.Instance.OnPlayerMoveClick(ButtonEnum.X, 0);
                    break;
                default:
                    break;
            }
        }
        private void OnButtonClick(GameObject go)
        {
            string name = go.name;
            switch (name)
            {
                case "Up":
                    InputManager.Instance.OnPlayerMoveClick(ButtonEnum.Y, 1);
                    break;
                case "Down":
                    InputManager.Instance.OnPlayerMoveClick(ButtonEnum.Y, -1);
                    break;
                case "Left":
                    InputManager.Instance.OnPlayerMoveClick(ButtonEnum.X, -1);
                    break;
                case "Right":
                    InputManager.Instance.OnPlayerMoveClick(ButtonEnum.X, 1);
                    break;
                default:
                    break;
            }
        }

#if UNITY_EDITOR||UNITY_STANDALONE
        void Update()
        {
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            {
                InputManager.Instance.OnPlayerMoveClick(ButtonEnum.X, -1);
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            {
                InputManager.Instance.OnPlayerMoveClick(ButtonEnum.X, 1);
            }
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                InputManager.Instance.OnPlayerMoveClick(ButtonEnum.Y, 1);
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                InputManager.Instance.OnPlayerMoveClick(ButtonEnum.Y, -1);
            }
            if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                InputManager.Instance.OnPlayerMoveClick(ButtonEnum.X, 0);
            }
            if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.DownArrow))
            {
                InputManager.Instance.OnPlayerMoveClick(ButtonEnum.Y, 0);
            }
        }
#endif


        public override UIEnum CurrentEnum
        {
            get { return UIEnum.MoveUI; }
        }
    }
}
