using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using LitJson;
using RPGGame.Utils;
using RPGGame.Table;
using RPGGame.Model.DTO;
using System;

namespace RPGGame.Manager
{
    public sealed class TableManager : RPGGame.Tools.Singleton<TableManager>, IManager
    {

        private const string TABLE_PATH = "Data/Table/";

        #region 字段
        private List<MapTable> _mapList = new List<MapTable>();
        private Dictionary<int, MapDTO> _mapDic = new Dictionary<int, MapDTO>();
        private List<BookTable> _bookList = new List<BookTable>();
        //private List<BuffTable> _buffList = new List<BuffTable>();
        private List<EquipTable> _equipList = new List<EquipTable>();
        private List<EquipForgeTable> _equipForgeList = new List<EquipForgeTable>();
        private List<FoodTable> _foodList = new List<FoodTable>();
        private List<FoodBuffTable> _foodBuffList = new List<FoodBuffTable>();
        private List<LanguageTable> _languageList = new List<LanguageTable>();
        private Dictionary<int, LanguageTable> _languageDic = new Dictionary<int, LanguageTable>();
        private List<MaterialTable> _materialList = new List<MaterialTable>();
        private List<NpcTable> _npcList = new List<NpcTable>();
        private List<WuShuTable> _wuShuList = new List<WuShuTable>();
        private List<TileTable> _tileList = new List<TileTable>();
        private List<PortalTable> _portalList = new List<PortalTable>();
        private List<MonsterTable> _monsterList = new List<MonsterTable>();
        private Dictionary<int, PortalDTO> _portalDic = new Dictionary<int, PortalDTO>();
        #endregion

        #region 公共

        public IEnumerator LoadTables()
        {
            _languageList = LoadTable<LanguageTable>("LanguageTable");
            yield return null;
            LanguageResolver();
            yield return null;
            _mapList = LoadTable<MapTable>("MapTable");
            yield return null;
            _bookList = LoadTable<BookTable>("BookTable");
            yield return null;
            //_buffList = LoadTable<BuffTable>("Table/BuffTable");
            //yield return null;
            _equipList = LoadTable<EquipTable>("EquipTable");
            yield return null;
            _equipForgeList = LoadTable<EquipForgeTable>("EquipForgeTable");
            yield return null;
            _foodList = LoadTable<FoodTable>("FoodTable");
            yield return null;
            _foodBuffList = LoadTable<FoodBuffTable>("FoodBuffTable");
            yield return null;
            _materialList = LoadTable<MaterialTable>("MaterialTable");
            yield return null;
            _npcList = LoadTable<NpcTable>("NpcTable");
            yield return null;
            _wuShuList = LoadTable<WuShuTable>("WuShuTable");
            yield return null;
            _tileList = LoadTable<TileTable>("TileTable");
            yield return null;
            _portalList = LoadTable<PortalTable>("PortalTable");
            yield return null;
            _monsterList = LoadTable<MonsterTable>("MonsterTable");
            yield return null;
            PortalResolver();
            yield return null;

            MapResolver();
        }



        /// <summary>
        /// 根据系统语言获取文字
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetLanguage(int id)
        {
            switch (Global.GlobalSetting.CurrentLanguage)
            {
                case SystemLanguage.Chinese:
                case SystemLanguage.ChineseSimplified:
                    return _languageDic[id].ChineseSimplified;
                case SystemLanguage.ChineseTraditional:
                    return _languageDic[id].ChineseTraditional;
                case SystemLanguage.English:
                default:
                    return _languageDic[id].English;
            }
        }

        public MapDTO GetMapById(int id)
        {
            if (_mapDic.ContainsKey(id))
            {
                return _mapDic[id];
            }
            return null;
        }

        public MonsterTable GetMonsterById(int id)
        {
            MonsterTable monster = null;

            for (int i = 0; i < _monsterList.Count; i++)
            {
                if (_monsterList[i].Id == id)
                {
                    monster = _monsterList[i];
                    break;
                }
            }
            return monster;
        }

        public PortalDTO GetPortalById(int id)
        {
            if (_portalDic.ContainsKey(id))
            {
                return _portalDic[id];
            }
            return null;
        }

        public NpcTable GetNpcById(int id)
        {
            NpcTable npc = null;

            for (int i = 0; i < _npcList.Count; i++)
            {
                if (_npcList[i].Id == id)
                {
                    npc = _npcList[i];
                    break;
                }
            }
            return npc;
        }

        public TileTable GetTileById(int id)
        {
            TileTable tile = null;

            for (int i = 0; i < _tileList.Count; i++)
            {
                if (_tileList[i].Id == id)
                {
                    tile = _tileList[i];
                    break;
                }
            }
            return tile;
        }

        #endregion

        #region 私有

        private void PortalResolver()
        {
            for (int i = 0; i < _portalList.Count; i++)
            {
                if (_portalDic.ContainsKey(_portalList[i].Id))
                {
                    continue;
                }
                _portalDic.Add(_portalList[i].Id, new PortalDTO(_portalList[i]));
            }
        }

        private void LanguageResolver()
        {
            for (int i = 0; i < _languageList.Count; i++)
            {
                if (_languageDic.ContainsKey(_languageList[i].Id))
                {
                    continue;
                }
                _languageDic.Add(_languageList[i].Id, _languageList[i]);
            }
        }
        private void MapResolver()
        {
            for (int i = 0; i < _mapList.Count; i++)
            {
                if (_mapDic.ContainsKey(_mapList[i].Id))
                {
                    continue;
                }
                _mapDic.Add(_mapList[i].Id, new MapDTO(_mapList[i]));
            }
        }

        private List<T> LoadTable<T>(string name) where T : new()
        {
            string content = FileUtils.Instance.ReadFile(Global.GlobalPath.TablePath + name);
            //TextAsset text = Resources.Load<TextAsset>(TABLE_PATH + name);
            return JsonMapper.ToObject<List<T>>(EncryptUtils.Instance.Decipher(content, EncryptUtils.GlobalEncryptKey));
        }

        #endregion

        #region 重写

        public bool Load()
        {
            return true;
        }

        public bool UnLoad()
        {
            return true;
        }
        public void OnUpdate()
        {
        }
        #endregion
    }
}

