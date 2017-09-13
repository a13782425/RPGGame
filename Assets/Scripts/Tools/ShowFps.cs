using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.Tools
{
    public class ShowFps : MonoBehaviour
    {

        public float f_UpdateInterval = 0.5F;

        private float f_LastInterval;

        private int i_Frames = 0;

        private float f_Fps;
        GUIStyle textStyle = new GUIStyle();
        void Start()
        {
            //Application.targetFrameRate=60;

            f_LastInterval = Time.realtimeSinceStartup;
            textStyle.normal.background = null; //这是设置背景填充的  
            textStyle.normal.textColor = new Color(1, 1, 0);   //设置字体颜色的  
            textStyle.fontSize = 40; //当然，这是字体颜色  
            i_Frames = 0;
        }

        void OnGUI()
        {

            GUI.Label(new Rect(0, Screen.height / 2, 200, 200), "FPS:" + f_Fps.ToString("f2"), textStyle);
        }

        void Update()
        {
            ++i_Frames;

            if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
            {
                f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

                i_Frames = 0;

                f_LastInterval = Time.realtimeSinceStartup;
            }
        }
    }
}