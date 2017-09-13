using System;
using System.Collections;
using System.Collections.Generic;
using RPGGame.Enums;
using UnityEngine;
using RPGGame.Manager;

namespace RPGGame.GameWorld.Unit.Npc
{
    public class NormalNpcActor : NpcActorBase
    {

        public override NpcTypeEnum NpcType
        {
            get
            {
                return NpcTypeEnum.Normal;
            }
        }

        private Queue<string> _talkQueue = new Queue<string>();


        public override void Init(NpcController controller)
        {
            base.Init(controller);
            this.CurrentNpcController.PhyChar.IsNeedCheck = false;

        }

        public override void OnPlayerEnter()
        {
            ResolveTalk();
        }

        public override void OnReset()
        {
            base.OnReset();
            ResolveTalk();
        }

        public override bool TakeUp()
        {
            if (_talkQueue.Count > 0)
            {
                string talk = _talkQueue.Dequeue();
                int id = -1;
                if (int.TryParse(talk, out id))
                {
                    UIManager.Instance.ShowDialog(TableManager.Instance.GetLanguage(id), this.CurrentStatus.NpcIcon, DialogEnum.Talk, DialogCallBack);
                }
            }
            else
            {
                _isTakeEnd = true;
                UIManager.Instance.HideDialog();
            }
            return true;
        }

        private void DialogCallBack(bool ok)
        {
            TakeUp();
        }

        /// <summary>
        /// 解析对话
        /// </summary>
        private void ResolveTalk()
        {
            _talkQueue.Clear();
            List<string> talks = this.CurrentStatus.Talks;
            int num = UnityEngine.Random.Range(0, 10000);
            int idx = num % talks.Count;
            string str = talks[idx];
            string[] strs = str.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                _talkQueue.Enqueue(strs[i]);
            }
        }
    }
}
