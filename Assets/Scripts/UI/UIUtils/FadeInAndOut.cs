using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

namespace RPGGame.UI
{
    [RequireComponent(typeof(Image))]
    public sealed class FadeInAndOut : MonoBehaviour
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
            Color _color = CurrentImage.color;
            _color.a = aBegin;
            CurrentImage.color = _color;
            StartCoroutine(FadeBegin(aBegin, aEnd, time, waitTime, callBack, atorCallBack));
        }

        private IEnumerator FadeBegin(float aBegin, float aEnd, float time, float waitTime, Action callBack, IEnumerator atorCallBack)
        {
            //while (Mathf.Abs(CurrentImage.color.a - aEnd) > 0.001f)
            //{
            //    _intervalTime = _intervalTime / time;
            //    float a = Mathf.Lerp(aBegin, aEnd, _intervalTime);
            //    Color _color = CurrentImage.color;
            //    _color.a = a;
            //    CurrentImage.color = _color;
            //    _intervalTime += Time.deltaTime;
            //    yield return null;
            //}
            yield return null;
            while (_intervalTime < 1f)
            {
                float a = Mathf.Lerp(aBegin, aEnd, _intervalTime);
                Color _color = CurrentImage.color;
                _color.a = a;
                CurrentImage.color = _color;
                _intervalTime += Time.deltaTime / time;
                yield return null;
            }

            Color color = CurrentImage.color;
            color.a = aEnd;
            CurrentImage.color = color;

            yield return new WaitForSeconds(waitTime);

            if (atorCallBack != null)
                yield return StartCoroutine(atorCallBack);
            if (callBack != null)
                callBack();

            //if (callBack != null)
            //{
            //    yield return new WaitForSeconds(waitTime);
            //    callBack();
            //}
            //if (atorCallBack != null)
            //    yield return StartCoroutine(atorCallBack);
        }
    }
}