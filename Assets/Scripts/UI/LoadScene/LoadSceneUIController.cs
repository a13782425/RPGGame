using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using RPGGame.Manager;
using RPGGame.Enums;
using RPGGame.Utils;
using System.Threading;

namespace RPGGame.UI
{
    public sealed class LoadSceneUIController : MonoBehaviour
    {
        [SerializeField]
        private Image _firstImage = null;
        [SerializeField]
        private Image _secondImage = null;
        [SerializeField]
        private Image _load = null;
        void Awake()
        {
            _firstImage.FadeIn(0, 1, 1, 1, () =>
            {
                _load.transform.localScale = Vector3.one;
            }, FadeCallBack());
        }
        IEnumerator FadeCallBack()
        {
            yield return null;
            //_firstImage.transform.FindChild("Load").ScaleIn(Vector3.one, Vector3.zero, 1, 0);
            _load.FadeIn(1, 0, 1, 0);
            _firstImage.FadeIn(1, 0, 1, 0, () =>
            {
                _secondImage.FadeIn(0, 1, 1, 1, () =>
                {
                    StartCoroutine(LevelManager.Instance.SwitchToLevel(SceneEnum.HomeScene, () =>
                    {
                        _secondImage.FadeIn(1, 0, 1, 0);
                    }));
                });
            });
        }
    }
}