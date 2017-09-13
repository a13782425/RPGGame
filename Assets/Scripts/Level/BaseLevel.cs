using UnityEngine;
using System.Collections;
using RPGGame.Enums;
using RPGGame.Manager;

namespace RPGGame.Level
{
    public abstract class BaseLevel
    {
        public abstract SceneEnum CurrentScene { get; }

        protected GameObject _mainPlayer = null;

        public GameObject MainPlayer { get { return _mainPlayer; } }

        protected bool _isActived = false;
        public bool IsActived { get { return _isActived; } }

        public virtual void OnGameStart()
        {
            this._isActived = true;
        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnExit()
        {
        }


    }
}

