using UnityEngine;
using System.Collections;
using RPGGame.Enums;

namespace RPGGame.Level
{
    public class HomeLevel : BaseLevel
    {
        public override SceneEnum CurrentScene
        {
            get { return SceneEnum.HomeScene; }
        }
    }
}