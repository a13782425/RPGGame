using UnityEngine;
using System.Collections;
using RPGGame.Enums;
using System.Collections.Generic;

namespace RPGGame.GameWorld.Buff
{
    public abstract class BuffBase
    {
        public abstract BuffEnum CurrentEnum { get; }

        private int _roundNum = 0;
        public int RoundNum { get { return _roundNum; } set { _roundNum = value; } }

        private int _variation = 0;
        /// <summary>
        /// 变化量
        /// </summary>
        public int Variation { get { return _variation; } set { _variation = value; } }

        private List<GameObject> _targetObjs = new List<GameObject>();

        public List<GameObject> TargetObjs { get { return _targetObjs; } }

        public virtual void Init(IEnumerable<GameObject> targets)
        {
            this.TargetObjs.AddRange(targets);
        }

        public virtual bool Freed()
        {
            return Freed(this.TargetObjs);
        }

        public virtual bool Freed(IEnumerable<GameObject> targets)
        {
            this.TargetObjs.Clear();
            this.TargetObjs.AddRange(targets);
            return true;
        }

    }
}
