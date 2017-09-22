using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;
using RPGGame.Tools;
using RPGGame.Manager;
using RPGGame.Enums;
using System.Collections.Generic;
using RPGGame.Model.DTO;
using RPGGame.GameWorld.Unit.Npc;
using RPGGame.Utils;

namespace RPGGame.Controller
{
    public class PlayerController : MonoBehaviour// CreativeSpore.CharBasicController
    {
        /// <summary>
        /// 动画控制器
        /// </summary>
        public CreativeSpore.CharAnimationController AnimCtrl { get { return m_animCtrl; } }
        /// <summary>
        /// 物理控制器
        /// </summary>
        public CreativeSpore.PhysicCharBehaviour PhyCtrl { get { return m_phyChar; } }

        public bool IsVisible
        {
            get
            {
                return m_animCtrl.TargetSpriteRenderer.enabled;
            }

            set
            {
                SetVisible(value);
            }
        }

        protected CreativeSpore.CharAnimationController m_animCtrl;
        protected CreativeSpore.PhysicCharBehaviour m_phyChar;

        protected float m_timerBlockDir = 0f;
        //public GameObject BulletPrefab;
        public float TimerBlockDirSet = 0.6f;
        public CameraController CurrentCamera;
        public float BulletAngDispersion = 15f;
        public SpriteRenderer ShadowSprite;
        public SpriteRenderer WeaponSprite;
        public int FogSightLength = 5;

        /// <summary>
        /// If player is driving a vehicle, this will be that vehicle
        /// </summary>
        public CreativeSpore.VehicleCharController Vehicle;


        private NpcController _currentNpcController = null;

        /// <summary>
        /// 玩家当前NPC
        /// </summary>
        public NpcController CurrentNpcController
        {
            get { return _currentNpcController; }
            set { _currentNpcController = value; }
        }

        private FollowObject _followObject;
        private int m_lastTileIdx = -1;
        private int m_lastFogSightLength = 0;
        #region 公共方法

        public void SetVisible(bool value)
        {
            m_animCtrl.TargetSpriteRenderer.enabled = value;
            ShadowSprite.enabled = value;
            WeaponSprite.enabled = value;
        }


        #endregion



        #region Unity

        void Awake()
        {
            InputManager.Instance.OnPlayerMove += MoveHandle;

            InputManager.Instance.onJoyStickMove += MoveHandle_Joy;
        }

        void Start()
        {
            m_animCtrl = GetComponent<CreativeSpore.CharAnimationController>();
            m_phyChar = GetComponent<CreativeSpore.PhysicCharBehaviour>();
            if (CurrentCamera == null)
            {
                CurrentCamera = CameraManager.Instance.CurrentCamera.GetComponent<CameraController>();
            }
            m_phyChar.DoorCallBack = DoorCallBack;
            m_animCtrl.IsAnimated = false;
            _followObject = CameraManager.Instance.CurrentCamera.GetComponent<FollowObject>();
            _followObject.Target = transform;

        }

        void Update()
        {
            //m_phyChar.IsNeedCheck = (Vehicle == null && NpcController == null);
            if (Vehicle != null || CurrentNpcController != null)
            {
                m_animCtrl.IsAnimated = false;
            }
            //else
            {
                //DoInputs();

                bool isMoving = (m_phyChar.Dir.sqrMagnitude >= 0.01);
                if (isMoving)
                {
                    //m_phyChar.Dir.Normalize();
                    _followObject.Target = transform;
                }
                else
                {
                    m_phyChar.Dir = Vector3.zero;
                }
            }

            int tileIdx = RpgMapHelper.GetTileIdxByPosition(transform.position);

            if (tileIdx != m_lastTileIdx || m_lastFogSightLength != FogSightLength)
            {
                RpgMapHelper.RemoveFogOfWarWithFade(transform.position, FogSightLength);
            }

            m_lastFogSightLength = FogSightLength;
            m_lastTileIdx = tileIdx;
        }

        #endregion

        //void CreateBullet(Vector3 vPos, Vector3 vDir)
        //{
        //    GameFactory.CreateBullet(gameObject, BulletPrefab, vPos, vDir, 4f);
        //}

        //void DoInputs()
        //{
        //    Vector3 vBulletDir = Vector3.zero;
        //    Vector3 vBulletPos = Vector3.zero;
        //    if (Input.GetKeyDown("j")) //down
        //    {
        //        vBulletPos = new Vector3(-0.08f, -0.02f, 0f);
        //        vBulletPos += transform.position;
        //        vBulletDir = Vector3.down;
        //        m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.DOWN;
        //        m_timerBlockDir = TimerBlockDirSet;
        //    }
        //    else if (Input.GetKeyDown("h")) // left
        //    {
        //        vBulletPos = new Vector3(-0.10f, 0.10f, 0f);
        //        vBulletPos += transform.position;
        //        vBulletDir = -Vector3.right;
        //        m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.LEFT;
        //        m_timerBlockDir = TimerBlockDirSet;
        //    }
        //    else if (Input.GetKeyDown("k")) // right
        //    {
        //        vBulletPos = new Vector3(0.10f, 0.10f, 0f);
        //        vBulletPos += transform.position;
        //        vBulletDir = Vector3.right;
        //        m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.RIGHT;
        //        m_timerBlockDir = TimerBlockDirSet;
        //    }
        //    else if (Input.GetKeyDown("u")) // up
        //    {
        //        vBulletPos = new Vector3(0.08f, 0.32f, 0f);
        //        vBulletPos += transform.position;
        //        vBulletDir = -Vector3.down;
        //        m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.UP;
        //        m_timerBlockDir = TimerBlockDirSet;
        //    }

        //    //if (vBulletDir != Vector3.zero)
        //    //{
        //    //    float fRand = Random.Range(-1f, 1f);
        //    //    fRand = Mathf.Pow(fRand, 5f);
        //    //    vBulletDir = Quaternion.AngleAxis(BulletAngDispersion * fRand, Vector3.forward) * vBulletDir;
        //    //    CreateBullet(vBulletPos, vBulletDir);
        //    //}
        //}



        #region 私有方法

        private void MoveHandle(ButtonEnum buttonEnum, float val)
        {
            m_timerBlockDir -= Time.deltaTime;
            switch (buttonEnum)
            {
                case ButtonEnum.X:
                    m_phyChar.Dir = new Vector3(val, 0, 0);
                    break;
                case ButtonEnum.Y:
                    m_phyChar.Dir = new Vector3(0, val, 0);
                    break;
                default:
                    m_phyChar.Dir = new Vector3(0, 0, 0);
                    break;
            }
            if (m_phyChar.IsMoving)
            {
                m_animCtrl.IsAnimated = true;

                if (m_timerBlockDir <= 0f)
                {
                    switch (buttonEnum)
                    {
                        case ButtonEnum.X:
                            if (val > 0)
                            {
                                m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.RIGHT;
                            }
                            else
                            {
                                m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.LEFT;
                            }
                            break;
                        case ButtonEnum.Y:
                            if (val > 0)
                            {
                                m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.UP;
                            }
                            else
                            {
                                m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.DOWN;
                            }
                            break;
                        default:
                            break;
                    }
                    if (CurrentNpcController != null)
                    {
                        Vector2 vec = CurrentNpcController.transform.position - this.transform.position;
                        CreativeSpore.CharAnimationController.eDir dir = MathUtils.Instance.GetAxisDirection(vec.normalized);
                        Debug.LogError(dir.ToString());
                        if (dir == m_animCtrl.CurrentDir)
                        {
                            m_phyChar.Dir = Vector3.zero;
                            //m_phyChar.IsNeedCheck = true;
                        }
                    }
                }
            }
            else
            {
                m_animCtrl.IsAnimated = false;
            }
        }

        private void MoveHandle_Joy(Vector2 vec)
        {
            m_timerBlockDir -= Time.deltaTime;
            //switch (buttonEnum)
            //{
            //    case ButtonEnum.X:
            //        m_phyChar.Dir = new Vector3(val, 0, 0);
            //        break;
            //    case ButtonEnum.Y:
            //        m_phyChar.Dir = new Vector3(0, val, 0);
            //        break;
            //    default:
            //        m_phyChar.Dir = new Vector3(0, 0, 0);
            //        break;
            //}
            m_phyChar.Dir = vec;

            if (m_phyChar.IsMoving)
            {
                m_animCtrl.IsAnimated = true;

                if (m_timerBlockDir <= 0f)
                {
                    float x, y;
                    x = vec.x;
                    y = vec.y;

                    if(x > 0)
                    {
                        if(x > Mathf.Abs(y))
                        {
                            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.RIGHT;
                        }
                        else if(x > y)
                        {
                            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.DOWN;
                        }
                        else
                        {
                            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.UP;
                        }
                    }
                    else
                    {
                        if (-x > Mathf.Abs(y))
                        {
                            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.LEFT;
                        }
                        else if (x > y)
                        {
                            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.DOWN;
                        }
                        else
                        {
                            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.UP;
                        }
                    }
                    //switch (buttonEnum)
                    //{
                    //    case ButtonEnum.X:
                    //        if (val > 0)
                    //        {
                    //            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.RIGHT;
                    //        }
                    //        else
                    //        {
                    //            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.LEFT;
                    //        }
                    //        break;
                    //    case ButtonEnum.Y:
                    //        if (val > 0)
                    //        {
                    //            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.UP;
                    //        }
                    //        else
                    //        {
                    //            m_animCtrl.CurrentDir = CreativeSpore.CharAnimationController.eDir.DOWN;
                    //        }
                    //        break;
                    //    default:
                    //        break;
                    //}
                    if (CurrentNpcController != null)
                    {
                        Vector2 vect = CurrentNpcController.transform.position - this.transform.position;
                        CreativeSpore.CharAnimationController.eDir dir = MathUtils.Instance.GetAxisDirection(vect.normalized);
                        Debug.LogError(dir.ToString());
                        if (dir == m_animCtrl.CurrentDir)
                        {
                            m_phyChar.Dir = Vector3.zero;
                            //m_phyChar.IsNeedCheck = true;
                        }
                    }
                }
            }
            else
            {
                m_animCtrl.IsAnimated = false;
            }
        }

        #region 回调

        /// <summary>
        /// 跳转场景回调
        /// </summary>
        /// <param name="targetPos"></param>
        private void DoorCallBack(Vector2 originPos)
        {
            PortalDTO portal = null;
            foreach (KeyValuePair<int, PortalDTO> item in MapManager.Instance.CurrentMapDTO.PortalIdDic)
            {
                if (item.Value.OriginPoint == originPos)
                {
                    portal = item.Value;
                    break;
                }
            }
            if (portal != null)
            {
                UIManager.Instance.ShowMask(() => { UIManager.Instance.HideMask(); }, MapManager.Instance.LoadPortal(portal));
                //UIManager.Instance.ShowMask(() =>
                //{
                //    switch (protal.PortalType)
                //    {
                //        case PortalEnum.ThisScene:
                //            Vector2 targetPos = protal.TargetPoint;
                //            Vector2 pos = MapManager.Instance.CurrentMap.GetPosByTilePos(targetPos);
                //            this.transform.position = pos;
                //            this.AnimCtrl.CurrentDir = protal.LookDir;
                //            break;
                //        case PortalEnum.OtherScene:
                //            break;
                //        case PortalEnum.None:
                //        default:
                //            Debug.LogError("传送门回调出问题了！！！");
                //            break;
                //    }
                //    UIManager.Instance.HideMask();
                //});
            }
        }

        #endregion

        #endregion


    }
}