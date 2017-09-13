using UnityEngine;
using System.Collections;
using RPGGame.Enums;
using RPGGame.Global;
using System.Collections.Generic;
using System;

namespace RPGGame.UI
{
    public abstract class BaseUI : MonoBehaviour
    {
        public abstract UIEnum CurrentEnum { get; }

        public virtual UITypeEnum CurrentTypeEnum { get { return UITypeEnum.Main; } }

        private bool _isEnable = false;

        public bool IsEnable
        {
            get { return _isEnable; }
            set
            {
                if (_isEnable != value)
                {
                    foreach (Action<bool> item in _uiActive.Values)
                    {
                        item(value);
                    }
                }
                _isEnable = value;
            }
        }

        private Transform _currentTransform = null;

        public Transform CurrentTransform
        {
            get
            {
                if (_currentTransform == null)
                {
                    _currentTransform = this.transform;
                }
                return _currentTransform;
            }
        }

        private GameObject _currentGameObject = null;

        public GameObject CurrentGameObject
        {
            get
            {
                if (_currentGameObject == null)
                {
                    _currentGameObject = this.gameObject;
                }
                return _currentGameObject;
            }
        }

        private Dictionary<int, Action<bool>> _uiActive = new Dictionary<int, Action<bool>>();
        private int _callBackFlag = 0;
        public virtual void Init()
        {

        }

        public virtual void Show()
        {
            this.CurrentGameObject.SetActive(true);
            IsEnable = true;
        }
        public virtual void Hide()
        {
            this.CurrentGameObject.SetActive(false);
            IsEnable = false;
        }

        public int AddListener(Action<bool> func)
        {
            if (func != null)
            {
                _uiActive.Add(_callBackFlag++, func);
                return _callBackFlag;
            }
            return 0;
        }
        public int RemoveListener(int key)
        {
            if (_uiActive.ContainsKey(key))
            {
                _uiActive.Remove(key);
                return key;
            }
            return 0;
        }

    }
}
