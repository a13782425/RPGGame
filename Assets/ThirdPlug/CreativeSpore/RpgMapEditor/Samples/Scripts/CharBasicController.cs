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

namespace CreativeSpore
{
    [RequireComponent(typeof(CharAnimationController))]
    [RequireComponent(typeof(PhysicCharBehaviour))]
    public class CharBasicController : MonoBehaviour
    {
        public CharAnimationController AnimCtrl { get { return m_animCtrl; } }
        public PhysicCharBehaviour PhyCtrl { get { return m_phyChar; } }

        public bool IsVisible
        {
            get
            {
                return m_animCtrl.TargetSpriteRenderer.enabled;
            }

            set
            {
                SetVisible( value );
            }
        }

        protected CharAnimationController m_animCtrl;
        protected PhysicCharBehaviour m_phyChar;

        protected float m_timerBlockDir = 0f;

        protected virtual void Start()
        {
            m_animCtrl = GetComponent<CharAnimationController>();
            m_phyChar = GetComponent<PhysicCharBehaviour>();
        }

        protected virtual void Update()
        {
            float fAxisX = Input.GetAxis("Horizontal");
            float fAxisY = Input.GetAxis("Vertical");
            UpdateMovement(fAxisX, fAxisY);
        }


        protected void UpdateMovement( float fAxisX, float fAxisY )
        {
            m_timerBlockDir -= Time.deltaTime;
            m_phyChar.Dir = new Vector3(fAxisX, fAxisY, 0);

            if (m_phyChar.IsMoving)
            {
                m_animCtrl.IsAnimated = true;

                if (m_timerBlockDir <= 0f)
                {
                    if (Mathf.Abs(fAxisX) > Mathf.Abs(fAxisY))
                    {
                        if (fAxisX > 0)
                            m_animCtrl.CurrentDir = CharAnimationController.eDir.RIGHT;
                        else if (fAxisX < 0)
                            m_animCtrl.CurrentDir = CharAnimationController.eDir.LEFT;
                    }
                    else
                    {
                        if (fAxisY > 0)
                            m_animCtrl.CurrentDir = CharAnimationController.eDir.UP;
                        else if (fAxisY < 0)
                            m_animCtrl.CurrentDir = CharAnimationController.eDir.DOWN;
                    }
                }
            }
            else
            {
                m_animCtrl.IsAnimated = false;
            }
        }

        public virtual void SetVisible( bool value )
        {
            m_animCtrl.TargetSpriteRenderer.enabled = value;
        }
    }
}
