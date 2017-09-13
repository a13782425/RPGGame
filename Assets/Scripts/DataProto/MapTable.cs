using UnityEngine;
using System.Collections;


namespace RPGGame.Table
{
    public class MapTable
    {
        private int _id = 0;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _name = -1;
        public int Name
        {
            get { return _name; }
            set { _name = value; }
        }

        private string _mapData = "";

        public string MapData
        {
            get { return _mapData; }
            set { _mapData = value; }
        }

        private int _tileId = 0;

        public int TileId
        {
            get { return _tileId; }
            set { _tileId = value; }
        }

        private string _battleMapIds = "";

        public string BattleMapIds
        {
            get { return _battleMapIds; }
            set { _battleMapIds = value; }
        }

        private string _npcIds = "";

        public string NpcIds
        {
            get { return _npcIds; }
            set { _npcIds = value; }
        }

        private string _monsterIds = "";

        public string MonsterIds
        {
            get { return _monsterIds; }
            set { _monsterIds = value; }
        }

        private string _portalIds = "";
        public string PortalIds
        {
            get { return _portalIds; }
            set { _portalIds = value; }
        }



        private bool _isHasMonster = false;

        public bool IsHasMonster
        {
            get { return _isHasMonster; }
            set { _isHasMonster = value; }
        }

        private bool _isBattleMap = false;
        public bool IsBattleMap
        {
            get { return _isBattleMap; }
            set { _isBattleMap = value; }
        }


        private int _mapWeather = 0;
        public int MapWeather
        {
            get { return _mapWeather; }
            set { _mapWeather = value; }
        }

        private string _scriptPath = "";
        public string ScriptPath
        {
            get { return _scriptPath; }
            set { _scriptPath = value; }
        }
        //private CreativeSpore.RpgMapEditor.AutoTileMapData _currentMapData = null;
        //public CreativeSpore.RpgMapEditor.AutoTileMapData CurrentMapData
        //{
        //    get
        //    {
        //        if (_currentMapData == null)
        //        {
        //            _currentMapData = Resources.Load<CreativeSpore.RpgMapEditor.AutoTileMapData>("DataAssets/Maps/" + MapData);
        //        }
        //        return _currentMapData;
        //    }
        //}
    }
}
