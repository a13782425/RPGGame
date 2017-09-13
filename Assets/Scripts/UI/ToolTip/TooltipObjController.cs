using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RPGGame.Utils;
using RPGGame.Global;

namespace RPGGame.UI
{
    public class TooltipObjController : MonoBehaviour
    {
        private BelongPool _myPool;
        private Text _showText = null;
        private GameObject _mainObj = null;
        private Transform _mainTran = null;

        private float _beginY = 0;
        private float _endY = 0;
        void Awake()
        {
            _mainObj = this.gameObject;
            _mainTran = this.transform;
            _showText = _mainTran.Find("TooltipContent").GetComponent<Text>();
            _myPool = BelongManager.Pools["Tooltip"];
            _beginY = GlobalData.ScreenHeight / 4 - 50;
            _endY = GlobalData.ScreenHeight / 2 - 50;
            _mainObj.SetActive(false);
        }


        public void ShowContent(string content)
        {
            _showText.text = content;
            _mainObj.SetActive(true);
            _mainTran.MoveIn(new Vector3(0, _beginY), new Vector3(0, _endY), 1f, 0.1f, () =>
            {
                _myPool.PutBack(_mainTran);
                _mainObj.SetActive(false);

            });
            _mainTran.GetComponent<Image>().FadeIn(1, 0, 2, 0);
        }
    }
}