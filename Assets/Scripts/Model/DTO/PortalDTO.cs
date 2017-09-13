using RPGGame.Enums;
using RPGGame.Table;
using RPGGame.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace RPGGame.Model.DTO
{
    public class PortalDTO
    {

        public PortalDTO(PortalTable data)
        {
            this._id = data.Id;
            this._des = data.Des;
            this._mapId = data.MapId;
            this._originPoint = data.OriginPoint.ToVector2();
            this._targetId = data.TargetId;
            this._targetMapId = data.TargetMapId;
            this._targetPoint = data.TargetPoint.ToVector2();
            this._lookDir = (CreativeSpore.CharAnimationController.eDir)data.LookDir;
            this._portalType = (PortalEnum)data.PortalType;

        }
        private int _id = 0;
        public int Id
        {
            get { return _id; }
        }

        private string _des = "";
        public string Des
        {
            get { return _des; }
        }

        private int _mapId = 0;
        public int MapId
        {
            get { return _mapId; }
        }

        private Vector2 _originPoint = Vector2.zero;
        public Vector2 OriginPoint
        {
            get { return _originPoint; }
        }

        private int _targetId = 0;
        public int TargetId
        {
            get { return _targetId; }
        }
        private int _targetMapId = 0;
        public int TargetMapId
        {
            get { return _targetMapId; }
        }

        private Vector2 _targetPoint = Vector2.zero;
        public Vector2 TargetPoint
        {
            get { return _targetPoint; }
        }

        private CreativeSpore.CharAnimationController.eDir _lookDir = CreativeSpore.CharAnimationController.eDir.DOWN;

        public CreativeSpore.CharAnimationController.eDir LookDir
        {
            get { return _lookDir; }
        }


        private PortalEnum _portalType = PortalEnum.None;

        public PortalEnum PortalType
        {
            get { return _portalType; }
        }
    }
}
