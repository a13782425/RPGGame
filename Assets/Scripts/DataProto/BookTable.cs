using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    public class BookTable : MonoBehaviour
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
        private string _icon = "";
        public string Icon
        {
            get { return _icon; }
            set { _icon = value; }
        }
    }
}