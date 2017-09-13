using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    public class PortalTable
    {
        //Id Des MapId OriginPoint LinkId TargetPoint Type
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

        private string _originPoint = "";
        public string OriginPoint
        {
            get { return _originPoint; }
            set { _originPoint = value; }
        }

        private int _targetId = 0;
        public int TargetId
        {
            get { return _targetId; }
            set { _targetId = value; }
        }

        private int _targetMapId = 0;
        public int TargetMapId
        {
            get { return _targetMapId; }
            set { _targetMapId = value; }
        }

        private string _targetPoint = "";
        public string TargetPoint
        {
            get { return _targetPoint; }
            set { _targetPoint = value; }
        }

        private int _lookDir = 0;
        public int LookDir
        {
            get { return _lookDir; }
            set { _lookDir = value; }
        }

        private int _portalType = 0;

        public int PortalType
        {
            get { return _portalType; }
            set { _portalType = value; }
        }
    }

}