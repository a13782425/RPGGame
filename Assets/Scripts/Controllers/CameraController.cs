using UnityEngine;
using System.Collections;
using RPGGame.Manager;

namespace RPGGame.Controller
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        public Camera Camera { get; private set; }

        public float Zoom = 1f;
        public float PixelToUnits = 100f;

        private Rect m_boundingBox;

        private bool _beginCal = false;

        // Use this for initialization
        void Start()
        {
            Camera = GetComponent<Camera>();
        }

        Vector3 m_vCamRealPos;
        void LateUpdate()
        {
            if (!_beginCal)
            {
                return;
            }
            //Note: ViewCamera.orthographicSize is not a real zoom based on pixels. This is the formula to calculate the real zoom.
            Camera.orthographicSize = (Screen.height) / (2f * Zoom * PixelToUnits);
            Vector3 vOri = Camera.ScreenPointToRay(Vector3.zero).origin;

            m_vCamRealPos = Camera.transform.position;
            Vector3 vPos = Camera.transform.position;
            float mod = (1f / (Zoom * PixelToUnits));
            vPos.x -= vOri.x % mod;
            vPos.y -= vOri.y % mod;
            Camera.transform.position = vPos;

            DoKeepInsideMapBounds();

        }

        // Update is called once per frame
        void DoKeepInsideMapBounds()
        {
            Rect rCamera = new Rect();
            rCamera.width = Screen.width / (PixelToUnits * Zoom);
            rCamera.height = Screen.height / (PixelToUnits * Zoom);
            rCamera.center = Camera.transform.position;

            Rect rMap = new Rect();
            rMap.width = MapManager.Instance.CurrentMap.MapTileWidth * MapManager.Instance.CurrentMap.Tileset.TileWorldWidth;
            rMap.height = MapManager.Instance.CurrentMap.MapTileHeight * MapManager.Instance.CurrentMap.Tileset.TileWorldHeight;
            rMap.x = MapManager.Instance.CurrentMap.transform.position.x;
            rMap.y = MapManager.Instance.CurrentMap.transform.position.y;

            rMap.y -= rMap.height;

            Vector3 vOffset = Vector3.zero;

            //CreativeSpore.RpgMapEditor.RpgMapHelper.DebugDrawRect(Vector3.zero, rCamera, Color.cyan);
            //CreativeSpore.RpgMapEditor.RpgMapHelper.DebugDrawRect(Vector3.zero, rMap, Color.green);

            float right = (rCamera.x < rMap.x) ? rMap.x - rCamera.x : 0f;
            float left = (rCamera.xMax > rMap.xMax) ? rMap.xMax - rCamera.xMax : 0f;
            float down = (rCamera.y < rMap.y) ? rMap.y - rCamera.y : 0f;
            float up = (rCamera.yMax > rMap.yMax) ? rMap.yMax - rCamera.yMax : 0f;

            vOffset.x = (right != 0f && left != 0f) ? rMap.center.x - Camera.transform.position.x : right + left;
            vOffset.y = (down != 0f && up != 0f) ? rMap.center.y - Camera.transform.position.y : up + down;

            Camera.transform.position += vOffset;
            m_vCamRealPos += vOffset;
        }

        public void Load()
        {
            m_boundingBox = new Rect();
            m_boundingBox.width = MapManager.Instance.CurrentMap.MapTileWidth * MapManager.Instance.CurrentMap.Tileset.TileWorldWidth;
            m_boundingBox.height = MapManager.Instance.CurrentMap.MapTileHeight * MapManager.Instance.CurrentMap.Tileset.TileWorldHeight;
            m_boundingBox.x = MapManager.Instance.CurrentMap.transform.position.x;
            m_boundingBox.y = MapManager.Instance.CurrentMap.transform.position.y;
            _beginCal = true;
        }

        public void UnLoad()
        {
            _beginCal = false;
        }



        //void DoKeepInsideBounds()
        //{
        //    Rect rCamera = new Rect();
        //    rCamera.width = Screen.width / (PixelToUnits * Zoom);
        //    rCamera.height = Screen.height / (PixelToUnits * Zoom);
        //    rCamera.center = Camera.transform.position;

        //    Vector3 vOffset = Vector3.zero;
        //    Rect rBoundingBox = m_boundingBox;
        //    rBoundingBox.y -= rBoundingBox.height;

        //    float right = (rCamera.x < rBoundingBox.x) ? rBoundingBox.x - rCamera.x : 0f;
        //    float left = (rCamera.xMax > rBoundingBox.xMax) ? rBoundingBox.xMax - rCamera.xMax : 0f;
        //    float down = (rCamera.y < rBoundingBox.y) ? rBoundingBox.y - rCamera.y : 0f;
        //    float up = (rCamera.yMax > rBoundingBox.yMax) ? rBoundingBox.yMax - rCamera.yMax : 0f;

        //    vOffset.x = (right != 0f && left != 0f) ? rBoundingBox.center.x - Camera.transform.position.x : right + left;
        //    vOffset.y = (down != 0f && up != 0f) ? rBoundingBox.center.y - Camera.transform.position.y : up + down;

        //    Camera.transform.position += vOffset;
        //}

        //void OnPostRender()
        //{
        //    Camera.transform.position = m_vCamRealPos;
        //}
    }
}
