using RPGGame.Manager;
using UnityEngine;

namespace RPGGame.Tools
{
    public class FollowObject : MonoBehaviour
    {

        [SerializeField]
        private float _dampTime = 0.15f;

        [SerializeField]
        public Transform Target = null;

        private Vector3 velocity = Vector3.zero;

        private Camera _camera = null;
        public Camera CurrentCamera
        {
            get
            {
                if (_camera == null)
                {
                    _camera = CameraManager.Instance.CurrentCamera;
                }
                return _camera;
            }
        }

        //void Awake()
        //{
        //    _camera = CameraManager.Instance.CurrentCamera.GetComponent<Camera>();
        //}

        // Update is called once per frame
        void LateUpdate()
        {

            if (Target != null)
            {
                Vector3 point = CurrentCamera.WorldToViewportPoint(Target.position);
                Vector3 delta = Target.position - CurrentCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
                Vector3 destination = this.transform.position + delta;
                this.transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, _dampTime);
            }

        }
    }
}
