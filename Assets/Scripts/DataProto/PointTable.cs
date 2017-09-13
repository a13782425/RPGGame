using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    public class PointTable
    {
        private int _id = 0;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _des = "";

        public string Des
        {
            get { return _des; }
            set { _des = value; }
        }

        private int _mapId = 0;

        public int MapId
        {
            get { return _mapId; }
            set { _mapId = value; }
        }

        private string _point = "";

        public string Point
        {
            get { return _point; }
            set { _point = value; }
        }

        private int _linkId = 0;

        public int LinkId
        {
            get { return _linkId; }
            set { _linkId = value; }
        }

        private int _type = 0;

        public int Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
