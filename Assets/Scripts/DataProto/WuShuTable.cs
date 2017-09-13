using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    /// <summary>
    /// 招式表
    /// </summary>
    public class WuShuTable
    {
        private int _id = 0;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _des = "";
        public string Des
        {
            get { return _des; }
            set { _des = value; }
        }
        private int _consume = 0;
        public int Consume
        {
            get { return _consume; }
            set { _consume = value; }
        }
        private int _consumeCount = 0;
        public int ConsumeCount
        {
            get { return _consumeCount; }
            set { _consumeCount = value; }
        }
    }
}
