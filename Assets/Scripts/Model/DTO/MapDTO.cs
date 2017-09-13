using UnityEngine;
using System.Collections;
using RPGGame.Table;
using System.Collections.Generic;
using System;
using RPGGame.Manager;
using RPGGame.Enums;
using RPGGame.Utils;

namespace RPGGame.Model.DTO
{

    /// <summary>
    /// 地图数据的DTO
    /// </summary>
    public class MapDTO
    {

        public MapDTO(MapTable data)
        {
            this._id = data.Id;
            this._name = TableManager.Instance.GetLanguage(data.Name);
            this._mapData = data.MapData;
            this._isHasMonster = data.IsHasMonster;
            this._tileId = data.TileId;
            this._isBattleMap = data.IsBattleMap;
            this._scriptPath = data.ScriptPath;
            //分割NPC
            string[] npcs = data.NpcIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < npcs.Length; i++)
            {
                int num = -1;
                if (int.TryParse(npcs[i], out num))
                {
                    this._npcIds.Add(num);
                }
            }
            //分割传送门
            string[] strs = data.PortalIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < strs.Length; i++)
            {
                int num = 0;
                if (int.TryParse(strs[i], out num))
                {
                    PortalDTO por = TableManager.Instance.GetPortalById(num);
                    if (por != null && !_portalIdDic.ContainsKey(num))
                    {
                        _portalIdDic.Add(num, por);
                    }
                }
            }
            //分割战斗场景
            string[] battles = data.BattleMapIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < battles.Length; i++)
            {
                int num = 0;
                if (int.TryParse(battles[i], out num))
                {
                    _battleMapIdList.Add(num);
                }
            }
            //分割怪物
            string[] monsters = data.MonsterIds.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < monsters.Length; i++)
            {
                int num = 0;
                if (int.TryParse(monsters[i], out num))
                {
                    _monsterIds.Add(num);
                }

            }
            this._mapWeather = (WeatherEnum)data.MapWeather;
        }

        private int _id = 0;
        public int Id
        {
            get { return _id; }
        }

        private string _name = "";
        public string Name
        {
            get { return _name; }
        }

        private string _mapData = "";

        public string MapData
        {
            get { return _mapData; }
        }

        private int _tileId = 0;

        public int TileId
        {
            get { return _tileId; }
        }

        private List<int> _npcIds = new List<int>();

        public List<int> NpcIds
        {
            get { return _npcIds; }
        }

        private Dictionary<int, PortalDTO> _portalIdDic = new Dictionary<int, PortalDTO>();
        public Dictionary<int, PortalDTO> PortalIdDic
        {
            get { return _portalIdDic; }
        }

        public List<int> _battleMapIdList = new List<int>();
        /// <summary>
        /// 战斗场景ID
        /// </summary>
        public List<int> BattleMapIdList { get { return _battleMapIdList; } }

        private List<int> _monsterIds = new List<int>();

        public List<int> MonsterIds
        {
            get { return _monsterIds; }
        }

        private bool _isHasMonster = false;

        public bool IsHasMonster
        {
            get { return _isHasMonster; }
        }
        private bool _isBattleMap = false;

        public bool IsBattleMap
        {
            get { return _isBattleMap; }
        }

        private WeatherEnum _mapWeather = WeatherEnum.None;

        public WeatherEnum MapWeather
        {
            get { return _mapWeather; }
        }


        private string _scriptPath = "";

        public string ScriptPath
        {
            get { return _scriptPath; }
        }


        private CreativeSpore.RpgMapEditor.AutoTileMapData _currentMapData = null;
        public CreativeSpore.RpgMapEditor.AutoTileMapData CurrentMapData
        {
            get
            {
                if (_currentMapData == null)
                {
                    _currentMapData = new CreativeSpore.RpgMapEditor.AutoTileMapData();
                    _currentMapData.Data = FileUtils.Instance.ReadMap(Global.GlobalPath.MapPath + MapData);
                    //_currentMapData = Resources.Load<CreativeSpore.RpgMapEditor.AutoTileMapData>("DataAssets/Maps/" + MapData);
                }
                return _currentMapData;
            }
        }
    }

}
