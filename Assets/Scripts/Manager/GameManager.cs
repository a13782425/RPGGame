using UnityEngine;
using System.Collections;

namespace RPGGame.Manager
{
    public sealed class GameManager : RPGGame.Tools.Singleton<GameManager>, IManager
    {

        public bool Load()
        {
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
