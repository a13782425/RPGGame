using System.Collections;
using UnityEngine;

namespace RPGGame.Tools
{
    public class Singleton<T> where T : Singleton<T>, new()
    {
        private GameObject _currentGameObject = null;
        public GameObject gameObject
        {
            get { return _currentGameObject; }
        }

        private Transform _currentTransform = null;
        public Transform transform
        {
            get { return _currentTransform; }
        }
        private MonoBehaviour _mono = null;

        private static T _instance = null;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new T();

                    GameObject game = new GameObject(typeof(T).Name);
                    _instance._currentGameObject = game;
                    _instance._currentTransform = game.transform;
                    _instance._mono = _instance._currentGameObject.AddComponent<DontDestroyGameObject>();
                    //if (Application.isPlaying)
                    //{
                    //    UnityEngine.Object.DontDestroyOnLoad(game);
                    //}
                }
                return _instance;
            }
        }


        #region 公共方法

        public Object Instantiate(Object original)
        {
            return GameObject.Instantiate(original);
        }

        public T Instantiate<T>(T original) where T : UnityEngine.Object
        {
            return GameObject.Instantiate<T>(original);
        }

        public Coroutine StartCoroutine(IEnumerator routine)
        {
            return _mono.StartCoroutine(routine);
        }

        public void StopAllCoroutines()
        {
            _mono.StopAllCoroutines();
        }
        public void StopCoroutine(Coroutine routine)
        {
            _mono.StopCoroutine(routine);
        }
        public void StopCoroutine(IEnumerator routine)
        {
            _mono.StopCoroutine(routine);
        }

        #endregion

        #region 虚方法


        #endregion
    }
}
