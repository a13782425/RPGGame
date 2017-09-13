using UnityEngine;
using System.Collections;

namespace RPGGame.Manager
{
    public sealed class CameraManager : RPGGame.Tools.Singleton<CameraManager>, IManager
    {
        public Camera CurrentCamera { get; private set; }

        public bool Load()
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/Camera/GameCamera");
            GameObject game = GameObject.Instantiate(obj);
            game.name = "GameCamera";
            CurrentCamera = game.GetComponent<Camera>();
            game.transform.rotation = Quaternion.identity;
            game.transform.SetParent(this.transform);
            //DontDestroyOnLoad(game);
            return true;
        }

        public bool UnLoad()
        {
            return true;
        }


        public void OnUpdate()
        {

        }
    }
}