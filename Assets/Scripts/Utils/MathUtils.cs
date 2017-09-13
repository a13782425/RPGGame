using UnityEngine;
using System.Collections;

namespace RPGGame.Utils
{
    public sealed class MathUtils : Singleton<MathUtils>
    {
        /// <summary>
        /// 正数在左负数在右
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public float AngleLRAtan2(Transform self, Transform target)
        {
            return AngleLRAtan2(self, target.position);
        }
        /// <summary>
        /// 正数在左负数在右
        /// </summary>
        /// <param name="self"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public float AngleLRAtan2(Transform self, Vector3 targetPos)
        {
            Vector3 v3 = self.InverseTransformPoint(targetPos);
            return Mathf.Atan2(v3.y, v3.z) * Mathf.Deg2Rad;
        }
        /// <summary>
        /// 正数在下负数在上
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public float AngleUDAtan2(Transform self, Transform target)
        {
            return AngleUDAtan2(self, target.position);
        }
        /// <summary>
        /// 正数在下负数在上
        /// </summary>
        /// <param name="self"></param>
        /// <param name="targetPos"></param>
        /// <returns></returns>
        public float AngleUDAtan2(Transform self, Vector3 targetPos)
        {
            Vector3 v3 = self.InverseTransformPoint(targetPos);
            return Mathf.Atan2(v3.z, v3.z) * Mathf.Deg2Rad;
        }

        /// <summary>
        /// 上负 下正
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public float CalDot(Vector3 self, Vector3 target, Vector3 up)
        {
            Vector3 vectorTarget = target - self;
            vectorTarget = new Vector3(vectorTarget.x, vectorTarget.y, 0);
            return Vector3.Dot(up.normalized, vectorTarget.normalized);
        }

        /// <summary>
        /// 下0-上180
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public float AngleByDot(Vector3 self, Vector3 target, Vector3 up)
        {
            float dotValue = CalDot(self, target, up);
            return Mathf.Acos(dotValue) * Mathf.Rad2Deg;
        }

        /// <summary>
        /// 负数在左，正数在右
        /// </summary>
        /// <param name="self"></param>
        /// <param name="target"></param>
        /// <param name="up"></param>
        /// <returns></returns>
        public float CalCross(Vector3 self, Vector3 target, Vector3 up)
        {
            Vector3 vectorTarget = target - self;
            vectorTarget = new Vector3(vectorTarget.x, vectorTarget.y, 0);
            return Vector3.Cross(up, vectorTarget).z;
        }


        public CreativeSpore.CharAnimationController.eDir GetAxisDirection(Vector2 vector2)
        {
            if (vector2 == Vector2.zero)
            {
                return CreativeSpore.CharAnimationController.eDir.NONE;
            }
            float angle = VectorAngle(new Vector2(0, 1), vector2);
            if (angle > -45 && angle < 45)
            {
                return CreativeSpore.CharAnimationController.eDir.UP;
            }
            if (angle > 45 && angle < 135)
            {
                return CreativeSpore.CharAnimationController.eDir.RIGHT;
            }
            if (angle > 135 || angle < -135)
            {
                return CreativeSpore.CharAnimationController.eDir.DOWN;
            }
            if (angle < -45 && angle > -135)
            {
                return CreativeSpore.CharAnimationController.eDir.LEFT;
            }
            return CreativeSpore.CharAnimationController.eDir.NONE;
        }


        /// <summary>
        /// 算夹角
        /// </summary>
        /// <returns></returns>
        private float VectorAngle(Vector2 from, Vector2 to)
        {
            float angle;
            Vector3 cross = Vector3.Cross(from, to);
            angle = Vector2.Angle(from, to);

            return cross.z > 0 ? -angle : angle;
        }
    }
}