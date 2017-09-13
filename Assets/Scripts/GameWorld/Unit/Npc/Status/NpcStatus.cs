using RPGGame.Enums;
using RPGGame.Manager;
using RPGGame.Table;
using RPGGame.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPGGame.GameWorld.Unit.Npc
{
    public class NpcStatus
    {
        #region 构造函数

        public NpcStatus(NpcTable table)
        {
            _id = table.Id;
            _name = TableManager.Instance.GetLanguage(table.Name);
            _tablePoint = table.Point.ToVector2();
            _npcPoint = MapManager.Instance.CurrentMap == null ? Vector2.zero : MapManager.Instance.CurrentMap.GetPosByTilePos(TablePoint);
            _npcType = table.NpcType;
            _npcIcon = table.NpcIcon;
            string[] str = table.Talks.Split(new char[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            Talks.AddRange(str);
        }

        #endregion

        private int _id = -1;
        /// <summary>
        /// 表中ID
        /// </summary>
        public int Id { get { return _id; } }

        private int _npcType = 0;// NpcTypeEnum.None;
        /// <summary>
        /// Npc类型
        /// </summary>
        public int NpcType { get { return _npcType; } }

        private string _name = "";
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get { return _name; } }

        private string _des = "";
        /// <summary>
        /// 简介
        /// </summary>
        public string Des { get { return _des; } }

        private string _npcIcon = "";
        /// <summary>
        /// 简介
        /// </summary>
        public string NpcIcon { get { return _npcIcon; } }

        private List<string> _talks = new List<string>();
        public List<string> Talks { get { return _talks; } }

        private Vector2 _tablePoint = Vector2.zero;
        /// <summary>
        /// Npc坐标
        /// </summary>
        public Vector2 TablePoint { get { return _tablePoint; } }


        private Vector2 _npcPoint = Vector2.zero;
        /// <summary>
        /// Npc坐标
        /// </summary>
        public Vector2 NpcPoint { get { return _npcPoint; } }
    }
}