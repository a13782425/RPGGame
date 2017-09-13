using UnityEngine;
using System.Collections;
using System;


namespace RPGGame.Attr
{
    /// <summary>
    /// Inspector时候显示
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public class BFieldAttribute : Attribute
    {
        public BFieldAttribute()
        {

        }

        public BFieldAttribute(string name)
        {
            _showName = name;
        }

        private string _showName = "";
        public string ShowName { get { return _showName; } set { _showName = value; } }
    }

    /// <summary>
    /// 游戏编辑时显示
    /// </summary>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = false)]
    public class BGameEditorAttribute : Attribute
    {
        public BGameEditorAttribute()
        {

        }

        public BGameEditorAttribute(string name)
        {
            _showName = name;
        }

        private string _showName = "";
        public string ShowName { get { return _showName; } set { _showName = value; } }
    }

}