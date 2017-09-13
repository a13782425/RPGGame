using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using RPGGame.UI;

namespace RPGGame.Utils
{
    public static class ExpandUtils
    {

        #region Image

        public static void FadeIn(this Image owner, float aBegin, float aEnd, float time, float waitTime, Action callBack = null, IEnumerator atorCallBack = null)
        {
            FadeInAndOut fadeInAndOut = owner.GetComponent<FadeInAndOut>();
            if (fadeInAndOut == null)
            {
                fadeInAndOut = owner.gameObject.AddComponent<FadeInAndOut>();
            }
            fadeInAndOut.Begin(aBegin, aEnd, time, waitTime, callBack, atorCallBack);
        }
        public static void FilledIn(this Image owner, float aBegin, float aEnd, float time, float waitTime, Action callBack = null, IEnumerator atorCallBack = null)
        {
            FilledInAndOut filledInAndOut = owner.GetComponent<FilledInAndOut>();
            if (filledInAndOut == null)
            {
                filledInAndOut = owner.gameObject.AddComponent<FilledInAndOut>();
            }
            filledInAndOut.Begin(aBegin, aEnd, time, waitTime, callBack, atorCallBack);
        }

        #endregion

        #region Transform

        public static void ScaleIn(this Transform owner, Vector3 aBegin, Vector3 aEnd, float time, float waitTime, Action callBack = null, IEnumerator atorCallBack = null)
        {
            ScaleInAndOut scaleInAndOut = owner.GetComponent<ScaleInAndOut>();
            if (scaleInAndOut == null)
            {
                scaleInAndOut = owner.gameObject.AddComponent<ScaleInAndOut>();
            }
            scaleInAndOut.Begin(aBegin, aEnd, time, waitTime, callBack, atorCallBack);
        }
        public static void MoveIn(this Transform owner, Vector3 aBegin, Vector3 aEnd, float time, float waitTime, Action callBack = null, IEnumerator atorCallBack = null)
        {
            MoveInAndOut moveInAndOut = owner.GetComponent<MoveInAndOut>();
            if (moveInAndOut == null)
            {
                moveInAndOut = owner.gameObject.AddComponent<MoveInAndOut>();
            }
            moveInAndOut.Begin(aBegin, aEnd, time, waitTime, callBack, atorCallBack);
        }

        #endregion

        #region String

        public static Vector2 ToVector2(this string str, char separator = '|')
        {
            return str.ToVector3(separator);
        }

        public static Vector3 ToVector3(this string str, char separator = '|')
        {
            string[] strs = str.Split(new char[] { separator }, StringSplitOptions.RemoveEmptyEntries);
            Vector3 vec = Vector3.zero;
            int length = strs.Length > 3 ? 3 : strs.Length;
            for (int i = 0; i < length; i++)
            {
                string s = strs[i];
                float f = 0f;
                if (float.TryParse(s, out f))
                {
                    vec[i] = f;
                }
                else
                {
                    vec[i] = 0;
                }
            }
            return vec;
        }

        #endregion

    }
}
