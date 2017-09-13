using UnityEngine;
using System.Collections;
using RPGGame.Attr;
using UnityEngine.UI;
using RPGGame.Manager;

namespace RPGGame.Tools
{
    [RequireComponent(typeof(Text))]
    public class InternationalText : MonoBehaviour
    {
        [SerializeField, BField("文字ID")]
        private int _id = 0;

        /// <summary>
        /// 文字ID
        /// </summary>
        public int Id { get { return _id; } set { _id = value; SetText(); } }

        private Text _currentText = null;

        public Text CurrentText
        {
            get
            {
                if (_currentText == null)
                {
                    _currentText = this.GetComponent<Text>();
                }
                return _currentText;
            }
        }

        void OnEnable()
        {
            SetText();
            Global.GlobalSetting.LanguageCallBack += SetText;
        }

        public void SetText()
        {
            CurrentText.text = TableManager.Instance.GetLanguage(Id);
        }

        private void OnDisable()
        {
            Global.GlobalSetting.LanguageCallBack -= SetText;
        }
    }
}