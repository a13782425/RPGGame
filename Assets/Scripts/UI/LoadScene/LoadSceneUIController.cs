using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RPGGame.Manager;
using RPGGame.Enums;
using RPGGame.Utils;
using System.Threading;
using RPGGame.Global;

namespace RPGGame.UI
{
    /// <summary>
    /// 此脚本中做对游戏资源的校对
    /// </summary>
    public sealed class LoadSceneUIController : MonoBehaviour
    {
        [SerializeField]
        private Image _firstImage = null;
        [SerializeField]
        private Image _secondImage = null;
        [SerializeField]
        private Image _load = null;
        /// <summary>
        /// 校验游戏数据
        /// </summary>
        private bool _isVerify = false;
        void Awake()
        {
            _firstImage.FadeIn(0, 1, 1, 1, () =>
            {
                _load.transform.localScale = Vector3.one;
            }, FadeCallBack());

            ZipUtils.UnzipFile(GlobalPath.StreamingAssetsPath + "/GlobalData", Application.persistentDataPath);
            _isVerify = true;
        }
        IEnumerator FadeCallBack()
        {
            yield return null;
            //_firstImage.transform.FindChild("Load").ScaleIn(Vector3.one, Vector3.zero, 1, 0);
            _load.FadeIn(1, 0, 1, 0);
            _firstImage.FadeIn(1, 0, 1, 0, () =>
            {
                _secondImage.FadeIn(0, 1, 1, 1, null, SwitchLevel());
            });
        }


        IEnumerator SwitchLevel()
        {
            while (!_isVerify)
            {
                yield return null;
            }
            StartCoroutine(LevelManager.Instance.SwitchToLevel(SceneEnum.HomeScene, () =>
            {
                _secondImage.FadeIn(1, 0, 1, 0);
            }));
        }
    }
}