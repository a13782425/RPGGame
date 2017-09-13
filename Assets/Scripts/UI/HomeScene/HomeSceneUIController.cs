using UnityEngine;
using System.Collections;
using RPGGame.Manager;
using RPGGame.Enums;
using RPGGame.Utils;
using UnityEngine.UI;

namespace RPGGame.UI
{
    public sealed class HomeSceneUIController : MonoBehaviour
    {
        //[SerializeField]
        //private Image _backGround = null;

        public void BeginGame()
        {
            Global.GlobalData.SelectGame = "Default";
            UIManager.Instance.ShowMask();
            ArchivedUtils.Instance.LoadArchived();
            //yield return null;
            ScorpioManager.Instance.Load();
            StartCoroutine(LoadLevel());
        }

        private IEnumerator LoadLevel()
        {
            yield return StartCoroutine(TableManager.Instance.LoadTables());
            yield return StartCoroutine(LevelManager.Instance.SwitchToLevel(SceneEnum.MainScene, null));
        }

    }
}