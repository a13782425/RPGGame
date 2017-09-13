using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    public class MonsterTable
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

        private string _point = "";

        public string Point
        {
            get { return _point; }
            set { _point = value; }
        }

        private bool _isBoss = false;

        public bool IsBoss
        {
            get { return _isBoss; }
            set { _isBoss = value; }
        }

        private string _dropList = "";

        public string DropList
        {
            get { return _dropList; }
            set { _dropList = value; }
        }

        }
}