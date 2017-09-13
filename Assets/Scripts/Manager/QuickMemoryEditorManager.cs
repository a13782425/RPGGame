using UnityEngine;
using System.Collections;

namespace RPGGame.Manager
{
    public sealed class QuickMemoryEditorManager : RPGGame.Tools.Singleton<QuickMemoryEditorManager>, IManager
    {
        private KeyCode[] keys = new KeyCode[] {
            KeyCode.F1,
            KeyCode.F2,
            KeyCode.F3,
            KeyCode.F4,
            KeyCode.F5,
            KeyCode.F6,
            KeyCode.F7,
            KeyCode.F8,
            KeyCode.F9,
            KeyCode.F10 };

        public bool Load()
        {
            return true;
        }

        public bool UnLoad()
        {
            return true;
        }

        private bool GetKeyDown(int index)
        {
            return Input.GetKeyDown(keys[index]);
        }

        public void OnUpdate()
        {
            if (GetKeyDown(0))
            {
                UIManager.Instance.ShowMask();
            }
            if (GetKeyDown(1))
            {
                UIManager.Instance.HideMask();
            }
            if (GetKeyDown(2))
            {
                UIManager.Instance.ShowDialog("今天天气不错", "", Enums.DialogEnum.Tip);
            }
            if (GetKeyDown(3))
            {
                UIManager.Instance.ShowMask(() => { UIManager.Instance.HideMask(); }, MapManager.Instance.LoadBattleMap(10000));
            }
            if (GetKeyDown(4))
            {
                InputManager.Instance.IsClick = !InputManager.Instance.IsClick;
            }
            if (GetKeyDown(5))
            {

            }
            if (GetKeyDown(6))
            {

            }
            if (GetKeyDown(7))
            {

            }
            if (GetKeyDown(8))
            {

            }
            if (GetKeyDown(9))
            {

            }

        }
    }
}