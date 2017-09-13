using UnityEngine;
using System.Collections;
using RPGGame.Enums;

namespace RPGGame.Level
{
    public class LoadLevel : BaseLevel
    {
        public override SceneEnum CurrentScene
        {
            get { return SceneEnum.LoadScene; }
        }
    }
}