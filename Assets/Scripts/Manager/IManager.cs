using UnityEngine;
using System.Collections;


namespace RPGGame.Manager
{
    public interface IManager
    {
        bool Load();
        bool UnLoad();

        void OnUpdate();
    }
}