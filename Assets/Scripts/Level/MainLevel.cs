using UnityEngine;
using System.Collections;
using RPGGame.Enums;
using RPGGame.Manager;
using RPGGame.Global;
using RPGGame.Tools;
using RPGGame.Controller;

namespace RPGGame.Level
{
    public class MainLevel : BaseLevel
    {
        public override SceneEnum CurrentScene
        {
            get { return SceneEnum.MainScene; }
        }
        public override void OnGameStart()
        {
            base.OnGameStart();
            LevelManager.Instance.StartCoroutine(InitGameWorld());
        }

        IEnumerator InitGameWorld()
        {
            yield return new WaitForSeconds(0.5f);

            GameObject obj = Resources.Load<GameObject>("Prefabs/Role/Player");
            _mainPlayer = GameObject.Instantiate(obj);
            _mainPlayer.transform.position = MapManager.Instance.CurrentMap.GetPosByTilePos(GlobalData.CurrentPlayer.X, GlobalData.CurrentPlayer.Y);
            _mainPlayer.transform.rotation = Quaternion.identity;
            CameraManager.Instance.CurrentCamera.GetComponent<CameraController>().Load();
            CameraManager.Instance.CurrentCamera.GetComponent<CameraController>().PixelToUnits = GlobalSetting.CurrentCameraRange;
            CameraManager.Instance.CurrentCamera.GetComponent<FollowObject>().Target = _mainPlayer.transform;
            GlobalData.CurrentPlayerController = _mainPlayer.GetComponent<PlayerController>();
            yield return MapManager.Instance.StartCoroutine(MapManager.Instance.LoadMap(1));
            Vector3 point = CameraManager.Instance.CurrentCamera.WorldToViewportPoint(_mainPlayer.transform.position);
            Vector3 delta = _mainPlayer.transform.position - CameraManager.Instance.CurrentCamera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            Vector3 destination = CameraManager.Instance.CurrentCamera.transform.position + delta;
            CameraManager.Instance.CurrentCamera.transform.position = destination;
            yield return null;
            UIManager.Instance.LoadUI(UIEnum.MoveUI, UITypeEnum.ToolTip);
            UIManager.Instance.LoadUI(UIEnum.OperationUI, UITypeEnum.ToolTip);
            yield return new WaitForSeconds(0.5f);
            MapManager.Instance.Register();
            yield return null;
            UIManager.Instance.MaskUI.HideMask();
        }
    }
}
