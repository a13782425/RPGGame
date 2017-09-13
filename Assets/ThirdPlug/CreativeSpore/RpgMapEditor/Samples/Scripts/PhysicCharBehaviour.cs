/*
http://www.cgsoso.com/forum-211-1.html

CG搜搜 Unity3d 每日Unity3d插件免费更新 更有VIP资源！

CGSOSO 主打游戏开发，影视设计等CG资源素材。

插件如若商用，请务必官网购买！

daily assets update for try.

U should buy the asset from home store if u use it in your project!
*/

using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;
using RPGGame.Global;

namespace CreativeSpore
{
    public class PhysicCharBehaviour : MonoBehaviour
    {

        [System.Flags]
        public enum eCollFlags
        {
            NONE = 0,
            DOWN = (1 << 0),
            LEFT = (1 << 1),
            RIGHT = (1 << 2),
            UP = (1 << 3)
        }

        public Vector3 Dir;
        public float MaxSpeed = 1f;
        public bool IsCollEnabled = true;

        private Vector3 m_vPrevPos;
        private float m_speed;

        private bool _isNeedCheck = true;
        /// <summary>
        /// 是否需要检测碰撞
        /// </summary>
        public bool IsNeedCheck { get { return _isNeedCheck; } set { _isNeedCheck = value; } }

        public eCollFlags CollFlags = eCollFlags.NONE;

        public DoorCallBackDelegate DoorCallBack = null;

        public Rect CollRect = new Rect(-0.14f, -0.04f, 0.28f, 0.12f);

        public bool IsMoving
        {
            get { return Dir.sqrMagnitude > 0; }
        }

        void Start()
        {
            m_vPrevPos = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            RpgMapHelper.DebugDrawRect(transform.position, CollRect, Color.white);
            if (!IsNeedCheck)
            {
                return;
            }
            if (Dir.sqrMagnitude > 0f)
            {
                // divide by n per second ( n:2 )
                m_speed += (MaxSpeed - m_speed) / Mathf.Pow(2f, Time.deltaTime);
            }
            else
            {
                m_speed /= Mathf.Pow(2f, Time.deltaTime);
            }
            Dir.z = 0f;
            transform.position += Dir * m_speed * Time.deltaTime;
            if (IsCollEnabled)
            {
                DoCollisions();
            }
        }

        const int k_subDiv = 6; // sub divisions
        /// <summary>
        /// 判断是否碰撞，及碰撞类型
        /// </summary>
        /// <param name="vPos"></param>
        /// <returns></returns>
		public bool IsColliding(Vector3 vPos)
        {
            Vector3 vCheckedPos = Vector3.zero;
            for (int i = 0; i < k_subDiv; ++i)
            {
                for (int j = 0; j < k_subDiv; ++j)
                {
                    vCheckedPos.x = vPos.x + Mathf.Lerp(CollRect.x, CollRect.x + CollRect.width, (float)i / (k_subDiv - 1));
                    vCheckedPos.y = vPos.y + Mathf.Lerp(CollRect.y, CollRect.y + CollRect.height, (float)j / (k_subDiv - 1));
                    int tile_x = 0;
                    int tile_y = 0;
                    eTileCollisionType collType = AutoTileMap.Instance.GetAutotileCollisionAtPosition(vCheckedPos, out tile_x, out tile_y);
                    if (collType != eTileCollisionType.PASSABLE && collType != eTileCollisionType.OVERLAY)
                    {
                        if (collType == eTileCollisionType.DOOR)
                        {
                            if (DoorCallBack != null)
                            {
                                DoorCallBack.Invoke(new Vector2(tile_x, tile_y));
                            }
                            ///根据坐标判断是什么门
                            //Debug.LogError("x===>" + tile_x + ";;;y===>" + tile_y);
                            //Debug.LogError(collType.ToString() + "===>" + vCheckedPos + "====>" + vPos);
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        void DoCollisions()
        {
            Vector3 vTempPos = transform.position;
            Vector3 vCheckedPos = transform.position;
            CollFlags = eCollFlags.NONE;
            if (IsColliding(vCheckedPos))
            {
                //m_speed = 0f;
                vCheckedPos.y = m_vPrevPos.y;
                if (!IsColliding(vCheckedPos))
                {
                    vTempPos.y = m_vPrevPos.y;
                    CollFlags |= m_vPrevPos.y > transform.position.y ? eCollFlags.DOWN : eCollFlags.UP;
                }
                else
                {
                    vCheckedPos = transform.position;
                    vCheckedPos.x = m_vPrevPos.x;
                    if (!IsColliding(vCheckedPos))
                    {
                        vTempPos.x = m_vPrevPos.x;
                        CollFlags |= m_vPrevPos.x > transform.position.x ? eCollFlags.LEFT : eCollFlags.RIGHT;
                    }
                    else
                    {
                        vTempPos = m_vPrevPos;
                        CollFlags |= m_vPrevPos.y > transform.position.y ? eCollFlags.DOWN : eCollFlags.UP;
                        CollFlags |= m_vPrevPos.x > transform.position.x ? eCollFlags.LEFT : eCollFlags.RIGHT;
                    }
                }
                transform.position = vTempPos;
            }
            else
            {
                //image_blend = c_white;
            }
            transform.position = vTempPos;
            m_vPrevPos = transform.position;
        }
    }
}
