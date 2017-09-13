using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

namespace RPGGame.UI
{
    public sealed class ScaleInAndOut : MonoBehaviour
    {
        private float _intervalTime = 0;

        public void Begin(Vector3 aBegin, Vector3 aEnd, float time, float waitTime, Action callBack = null, IEnumerator atorCallBack = null)
        {
            _intervalTime = 0;
            this.transform.localScale = aBegin;
            StartCoroutine(ScaleBegin(aBegin, aEnd, time, waitTime, callBack, atorCallBack));
        }

        private IEnumerator ScaleBegin(Vector3 aBegin, Vector3 aEnd, float time, float waitTime, Action callBack, IEnumerator atorCallBack)
        {
            //while ((this.transform.localScale - aEnd).magnitude > 0.001f)
            //{
            //    _intervalTime = _intervalTime / time;
            //    float x = Mathf.Lerp(aBegin.x, aEnd.x, _intervalTime);
            //    float y = Mathf.Lerp(aBegin.y, aEnd.y, _intervalTime);
            //    float z = Mathf.Lerp(aBegin.z, aEnd.z, _intervalTime);
            //    this.transform.localScale = new Vector3(x, y, z);
            //    _intervalTime += Time.deltaTime;
            //    yield return null;
            //}
            yield return null;
            while (_intervalTime < 1f)
            {
                float x = Mathf.Lerp(aBegin.x, aEnd.x, _intervalTime);
                float y = Mathf.Lerp(aBegin.y, aEnd.y, _intervalTime);
                float z = Mathf.Lerp(aBegin.z, aEnd.z, _intervalTime);
                this.transform.localScale = new Vector3(x, y, z);
                _intervalTime += Time.deltaTime / time;
                yield return null;
            }

            this.transform.localScale = aEnd;

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
