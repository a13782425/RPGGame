using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace RPGGame.UI
{
    [RequireComponent(typeof(Image))]
    public sealed class FilledInAndOut : MonoBehaviour
    {
        private float _intervalTime = 0;

        private Image _currentImage = null;

        private Image CurrentImage
        {
            get
            {
                if (_currentImage == null)
                {
                    _currentImage = this.GetComponent<Image>();
                }
                return _currentImage;
            }
        }
        public void Begin(float aBegin, float aEnd, float time, float waitTime, Action callBack = null, IEnumerator atorCallBack = null)
        {
            _intervalTime = 0;
            CurrentImage.fillAmount = aBegin;
            StartCoroutine(FilledBegin(aBegin, aEnd, time, waitTime, callBack, atorCallBack));
        }

        private IEnumerator FilledBegin(float aBegin, float aEnd, float time, float waitTime, Action callBack, IEnumerator atorCallBack)
        {
            //while (Mathf.Abs(CurrentImage.fillAmount - aEnd) > 0.001f)
            //{
            //    _intervalTime = _intervalTime / time;
            //    float f = Mathf.Lerp(aBegin, aEnd, _intervalTime);
            //    CurrentImage.fillAmount = f;
            //    _intervalTime += Time.deltaTime;
            //    yield return null;
            //}
            yield return null;
            while (_intervalTime < 1f)
            {
                float f = Mathf.Lerp(aBegin, aEnd, _intervalTime);
                CurrentImage.fillAmount = f;
                _intervalTime += Time.deltaTime / time;
                yield return null;
            }

            CurrentImage.fillAmount = aEnd;
            yield return new WaitForSeconds(waitTime);

            if (atorCallBack != null)
                yield return StartCoroutine(atorCallBack);
            if (callBack != null)
                callBack();
        }
    }
}
