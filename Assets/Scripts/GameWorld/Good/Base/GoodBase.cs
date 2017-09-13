using UnityEngine;
using System.Collections;

namespace RPGGame.GameWorld.Good.Base
{
    public abstract class GoodBase : MonoBehaviour
    {
        public bool IsCanUse = false;
        public virtual bool UseGood() { return true; }

        public GameObject Owner = null;

        public virtual void Init() { }

        void Awake()
        {
            Init();
        }
    }
}
