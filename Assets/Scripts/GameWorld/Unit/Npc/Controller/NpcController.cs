using RPGGame.Enums;
using RPGGame.Table;
using RPGGame.Attr;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPGGame.Controller;
using RPGGame.Tools;
using RPGGame.Manager;
using System;

namespace RPGGame.GameWorld.Unit.Npc
{
    [RequireComponent(typeof(CreativeSpore.PhysicCharBehaviour))]
    [RequireComponent(typeof(CreativeSpore.CharAnimationController))]
    public class NpcController : MonoBehaviour
    {
        private const string SPRITE_PATH = "Textures/Npc/";

        #region 属性字段

        protected NpcStatus _currentStatus = null;
        /// <summary>
        /// Npc的属性
        /// </summary>
        public NpcStatus CurrentStatus { get { return _currentStatus; } }

        private NpcTable _currentTable = null;
        /// <summary>
        /// Npc的表
        /// </summary>
        public NpcTable CurrentTable { get { return _currentTable; } }

        [SerializeField, BField("提示文字")]
        private GameObject _helpText = null;
        /// <summary>
        /// 提示文字
        /// </summary>
        public GameObject HelpText { get { return _helpText; } }

        [SerializeField, BField("地面阴影")]
        private GameObject _shadowSprite = null;
        /// <summary>
        /// 地面阴影
        /// </summary>
        public GameObject ShadowSprite { get { return _shadowSprite; } }

        private bool _isPlayerTouch = false;

        /// <summary>
        /// 玩家是否碰到
        /// </summary>
        public bool IsPlayerTouch { get { return _isPlayerTouch; } }


        private bool _isFirst = false;
        /// <summary>
        /// 第一次进入
        /// </summary>
        public bool IsFirst { get { return _isFirst; } }

        private PlayerController _currentPlayerController = null;
        /// <summary>
        /// 当前玩家
        /// </summary>
        public PlayerController CurrentPlayerController
        {
            get
            {
                if (_currentPlayerController == null)
                {
                    _currentPlayerController = Global.GlobalData.CurrentPlayerController;
                }
                return _currentPlayerController;
            }
        }

        private Dictionary<NpcTypeEnum, NpcActorBase> _npcActorDic = new Dictionary<NpcTypeEnum, NpcActorBase>();
        /// <summary>
        /// Npc角色集合
        /// </summary>
        private Dictionary<NpcTypeEnum, NpcActorBase> NpcActorDic
        {
            get { return _npcActorDic; }
        }

        private Rect _npcRect = new Rect();

        /// <summary>
        /// 碰撞器矩形
        /// </summary>
        public Rect NpcRect { get { return _npcRect; } }

        /// <summary>
        /// 物体控制器
        /// </summary>
        CreativeSpore.PhysicCharBehaviour _phyChar;
        /// <summary>
        /// 物体控制器
        /// </summary>
        public CreativeSpore.PhysicCharBehaviour PhyChar { get { return _phyChar; } }
        /// <summary>
        /// 物体控制器
        /// </summary>
        CreativeSpore.CharAnimationController _animChar;
        /// <summary>
        /// 物体控制器
        /// </summary>
        public CreativeSpore.CharAnimationController AnimChar { get { return _animChar; } }

        #endregion

        #region 公共方法
        /// <summary>
        /// 执行某个Actor
        /// </summary>
        /// <param name="npcEnum"></param>
        /// <returns></returns>
        public bool TakeUpActor(NpcTypeEnum npcEnum)
        {
            if (NpcActorDic.ContainsKey(npcEnum))
            {
                NpcActorDic[npcEnum].TakeUp();
                return true;
            }
            return false;
        }

        #endregion

        #region 虚方法

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="table"></param>
        public virtual void Init(NpcTable table)
        {
            _currentTable = table;
            _currentStatus = new NpcStatus(table);
            _phyChar = this.GetComponent<CreativeSpore.PhysicCharBehaviour>();
            _animChar = this.GetComponent<CreativeSpore.CharAnimationController>();
            if (string.IsNullOrEmpty(CurrentTable.NpcTexture))
            {
                Destroy(this.gameObject);
            }
            Sprite sp = Resources.Load<Sprite>(NpcController.SPRITE_PATH + CurrentTable.NpcTexture);
            if (sp == null)
            {
                Destroy(this.gameObject);
            }
            AnimChar.SpriteCharSet = sp;
            AnimChar.CreateSpriteFrames();
            if (this.HelpText != null)
            {
                this.HelpText.GetComponent<InternationalText>().Id = CurrentTable.TipText;
            }
            if (CurrentStatus.NpcType > 0)
            {
                if ((CurrentStatus.NpcType & (int)NpcTypeEnum.Normal) > 0)
                {
                    NpcActorBase npc = this.gameObject.AddComponent<NormalNpcActor>();
                    npc.Init(this);
                    _npcActorDic.Add(NpcTypeEnum.Normal, npc);
                }
                //TODO 添加不同的Actor
            }
            Reset();
        }
        /// <summary>
        /// 重置一些状态
        /// </summary>
        public virtual void Reset()
        {
            this.transform.position = CurrentStatus.NpcPoint;
            this.HelpText.SetActive(false);
        }


        public virtual void Update()
        {
            if (CurrentPlayerController != null)
            {
                Vector3 playerPos = CurrentPlayerController.transform.position;
                if (Vector3.SqrMagnitude(this.transform.position - playerPos) > 1f)
                {
                    return;
                }

                _npcRect = new Rect(this.transform.position.x + _phyChar.CollRect.x, this.transform.position.y + _phyChar.CollRect.y - _phyChar.CollRect.height / 3, _phyChar.CollRect.width, _phyChar.CollRect.height);
#if UNITY_EDITOR
                CreativeSpore.RpgMapEditor.RpgMapHelper.DebugDrawRect(Vector3.zero, NpcRect, Color.blue);
#endif
                Rect playerRect = CurrentPlayerController.PhyCtrl.CollRect;
                Rect rect = new Rect(playerPos.x + playerRect.x, playerPos.y + playerRect.y - playerRect.height / 3, playerRect.width, playerRect.height);
                //Rect rect = new Rect(CurrentPlayerController.transform.position.x + CurrentPlayerController.PhyCtrl.CollRect.x, CurrentPlayerController.transform.position.y + CurrentPlayerController.PhyCtrl.CollRect.y - CurrentPlayerController.PhyCtrl.CollRect.height / 3, CurrentPlayerController.PhyCtrl.CollRect.width, CurrentPlayerController.PhyCtrl.CollRect.height);
                _isPlayerTouch = NpcRect.Overlaps(rect);
                //_isPlayerTouch = CollRect.Contains(CurrentPlayerController.transform.position);
                if (IsPlayerTouch && !IsFirst)
                {
                    //第一次碰到NPC
                    EnterPlayer();
                }
                else if (!IsPlayerTouch)
                {
                    if (CurrentPlayerController.CurrentNpcController != null)
                    {
                        if (CurrentPlayerController.CurrentNpcController == this)
                        {
                            ExitPlayer();
                        }
                    }
                }
            }
            //当npc碰到玩家
            if (IsPlayerTouch)
            {

            }
        }




        #endregion

        #region 私有方法

        private void EnterPlayer()
        {
            InputManager.Instance.OnOkButtonClick = OnButtonClick;
            _isFirst = true;
            CurrentPlayerController.CurrentNpcController = this;
            this.HelpText.SetActive(true);
            foreach (KeyValuePair<NpcTypeEnum, NpcActorBase> item in NpcActorDic)
            {
                item.Value.OnPlayerEnter();
            }
        }

        private void ExitPlayer()
        {
            InputManager.Instance.OnOkButtonClick = null;
            _isFirst = false;
            CurrentPlayerController.CurrentNpcController = null;
            this.HelpText.SetActive(false);
            foreach (KeyValuePair<NpcTypeEnum, NpcActorBase> item in NpcActorDic)
            {
                item.Value.OnPlayerExit();
            }
        }


        private void OnButtonClick(Enums.ButtonEnum buttonEnum, params object[] data)
        {
            int count = 0;
            foreach (KeyValuePair<NpcTypeEnum, NpcActorBase> item in NpcActorDic)
            {
                if (item.Value.IsTakeEnd)
                {
                    count++;
                    continue;
                }
                else
                {
                    TakeUpActor(item.Key);
                    if (item.Value.IsTakeEnd)
                    {
                        count++;
                        continue;
                    }
                    break;
                }
            }
            if (count == NpcActorDic.Count)
            {
                foreach (KeyValuePair<NpcTypeEnum, NpcActorBase> item in NpcActorDic)
                {
                    item.Value.OnReset();
                }
            }
        }

        #endregion
    }
}