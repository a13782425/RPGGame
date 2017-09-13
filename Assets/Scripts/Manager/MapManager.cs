using UnityEngine;
using System.Collections;
using CreativeSpore.RpgMapEditor;
using RPGGame.Table;
using System.Collections.Generic;
using RPGGame.Model.DTO;
using RPGGame.Enums;
using System;
using RPGGame.GameWorld.Unit.Npc;
using RPGGame.Utils;
using Scorpio;

namespace RPGGame.Manager
{
    public sealed class MapManager : RPGGame.Tools.Singleton<MapManager>, IManager
    {

        #region 属性和字段

        private GameObject _currentMapRoot = null;

        public GameObject CurrentMapRoot { get { return _currentMapRoot; } }

        private const string MapPath = "DataAssets/Maps/";
        private const string TilePath = "DataAssets/Tiles/";

        private Dictionary<int, AutoTileMapData> _cacheMapDataDic = new Dictionary<int, AutoTileMapData>();
        private Dictionary<int, AutoTileset> _cacheTilesetDic = new Dictionary<int, AutoTileset>();

        private Dictionary<int, List<GameObject>> _cacheNpcDic = new Dictionary<int, List<GameObject>>();

        private ScriptObject _currentScript = null;
        /// <summary>
        /// 当前地图的脚本
        /// </summary>
        public ScriptObject CurrentScript { get { return _currentScript; } }

        private MapDTO _lastMapDTO = null;
        public MapDTO LastMapDTO
        {
            get
            {
                return _lastMapDTO;
            }
        }

        private MapDTO _currentMapDTO = null;
        public MapDTO CurrentMapDTO
        {
            get
            {
                return _currentMapDTO;
            }
        }
        private MapDTO _nextMapDTO = null;
        public MapDTO NextMapDTO
        {
            get
            {
                return _nextMapDTO;
            }
        }

        private AutoTileset _lastTileset = null;
        private AutoTileset _currentTileset = null;
        private AutoTileset _nextTileset = null;

        private AutoTileMapData _lastMapData = null;
        private AutoTileMapData _currentMapData = null;
        private AutoTileMapData _nextMapData = null;

        public AutoTileMapData CurrentMapData { get { return _currentMapData; } }

        private AutoTileMap _currentMap = null;
        /// <summary>
        /// 当前地图
        /// </summary>
        public AutoTileMap CurrentMap
        {
            get
            {
                if (_currentMap == null)
                {
                    _currentMap = AutoTileMap.Instance;
                }
                return _currentMap;
            }
        }
        /// <summary>
        /// 是否加载完成
        /// </summary>
        public bool IsInit { get { return this.CurrentMap == null ? false : this.CurrentMap.IsInitialized; } }

        /// <summary>
        /// 是否在加载地图
        /// </summary>
        private bool _isLoadMap = false;
        /// <summary>
        /// 是否在加载地图
        /// </summary>
        public bool IsLoadMap
        {
            get { return _isLoadMap; }
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 加载正常场景ID
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public IEnumerator LoadMap(int mapId)
        {
            _isLoadMap = true;
            //AutoTileMapData currentMapData = null;
            #region 加载地图数据

            if (_cacheMapDataDic.ContainsKey(mapId))
            {
                _nextMapData = _cacheMapDataDic[mapId];
            }
            else
            {
                _nextMapDTO = TableManager.Instance.GetMapById(mapId);
                if (NextMapDTO == null)
                {
                    Debug.LogError("ID:" + mapId + "，的地图没有找到！");
                    yield break;
                }
                _nextMapData = NextMapDTO.CurrentMapData;// Resources.Load<CreativeSpore.RpgMapEditor.AutoTileMapData>("DataAssets/Maps/" + CurrentMapDTO.MapData);
                _cacheMapDataDic.Add(mapId, _nextMapData);
            }

            HideNpc();

            _lastMapDTO = CurrentMapDTO;
            _currentMapDTO = NextMapDTO;
            _nextMapDTO = null;
            if (_nextMapData == null)
            {
                Debug.LogError("ID:" + mapId + "，的地图读取失败！");
                yield break;
            }
            //if (this.CurrentMapData != null)
            {
                this._lastMapData = this.CurrentMapData;
                this._currentMapData = _nextMapData;
                _nextMapData = null;
            }

            #endregion

            #region 加载瓷砖数据

            if (_cacheTilesetDic.ContainsKey(_currentMapDTO.TileId))
            {
                _nextTileset = _cacheTilesetDic[_currentMapDTO.TileId];
            }
            else
            {
                TileTable tile = TableManager.Instance.GetTileById(CurrentMapDTO.TileId);
                if (tile == null)
                {
                    Debug.LogError("ID:" + CurrentMapDTO.TileId + "，的地图数据没有找到！");
                    yield break;
                }
                _nextTileset = Resources.Load<CreativeSpore.RpgMapEditor.AutoTileset>(TilePath + tile.TileData);
                //FileUtils.Instance.ReadFile<CreativeSpore.RpgMapEditor.AutoTileset>(Global.GlobalData.TilePath + tile.TileData);
                _cacheTilesetDic.Add(tile.Id, _nextTileset);
            }
            {
                this._lastTileset = this._currentTileset;
                this._currentTileset = _nextTileset;
                _nextTileset = null;
            }
            #endregion
            this.CurrentMap.Tileset = this._currentTileset;
            this.CurrentMap.MapData = this.CurrentMapData;
            if (!string.IsNullOrEmpty(this.CurrentMapDTO.ScriptPath))
            {
                ScorpioManager.Instance.LoadFile(Global.GlobalPath.MapScriptPath + this.CurrentMapDTO.ScriptPath);
                //_currentScript = ScorpioManager.Instance.LoadFile(Global.GlobalPath.MapScriptPath + this.CurrentMapDTO.ScriptPath);
                //_currentScript.SetValue("MainPlayer", ScorpioManager.Instance.ScriptEngine.CreateObject(ScoUtils.GetMainPlayer()));
            }
            else
            {
                _currentScript = null;
            }

            while (!this.IsInit)
            {
                yield return null;
            }
            InitNpc();
            yield return null;
        }

        /// <summary>
        /// 加载战斗场景ID
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public IEnumerator LoadBattleMap(int mapId)
        {
            _isLoadMap = true;
            yield return StartCoroutine(LoadMap(mapId));

            LevelManager.Instance.CurrentLevel.MainPlayer.transform.position = this.CurrentMap.GetPosByTilePos(25, 25);
            CreativeSpore.CharAnimationController anim = LevelManager.Instance.CurrentLevel.MainPlayer.GetComponent<CreativeSpore.CharAnimationController>();
            anim.IsAnimated = false;
            yield return null;
            //UIManager.Instance.HideMask();
        }

        public IEnumerator LoadPortal(PortalDTO portal)
        {
            yield return null;
            Vector2 targetPos = portal.TargetPoint;
            Vector2 pos = this.CurrentMap.GetPosByTilePos(targetPos);
            switch (portal.PortalType)
            {
                case PortalEnum.ThisScene:
                    break;
                case PortalEnum.OtherScene:
                    yield return StartCoroutine(LoadMap(portal.TargetMapId));
                    break;
                case PortalEnum.None:
                default:
                    Debug.LogError("传送门回调出问题了！！！");
                    goto End;
            }
            yield return null;
            LevelManager.Instance.CurrentLevel.MainPlayer.transform.position = pos;
            CreativeSpore.CharAnimationController anim = LevelManager.Instance.CurrentLevel.MainPlayer.GetComponent<CreativeSpore.CharAnimationController>();
            anim.CurrentDir = portal.LookDir;

            End: yield return null;
        }

        public void Register()
        {
            CurrentMap.OnLoadBefore += OnLoadBefore;
            CurrentMap.OnLoadEnd += OnLoadEnd;
        }
        public void UnRegister()
        {
            CurrentMap.OnLoadBefore = null;
            CurrentMap.OnLoadEnd = null;
        }
        /// <summary>
        /// 地图数据转XML
        /// </summary>
        /// <param name="autoTileMap"></param>
        /// <returns></returns>
        public string MapDataToXml()
        {
            if (this.CurrentMapData == null)
            {
                throw new Exception("CurrentMapData is null");
            }
            return MapDataToXml(this.CurrentMapData);
        }
        /// <summary>
        /// 地图数据转XML
        /// </summary>
        /// <param name="autoTileMap"></param>
        /// <returns></returns>
        public string MapDataToXml(AutoTileMapData autoTileMapData)
        {
            return autoTileMapData.Data.GetXmlString();
        }
        /// <summary>
        /// 保存地图
        /// </summary>
        public void SaveMapData(string path)
        {
            if (this.CurrentMapData == null)
            {
                throw new Exception("CurrentMapData is null");
            }
            SaveMapData(path, this.CurrentMapData);
        }

        /// <summary>
        /// 保存地图
        /// </summary>
        public void SaveMapData(string path, AutoTileMapData mapData)
        {
            mapData.Data.SaveToFile(path);
        }


        #endregion

        #region 重载

        public bool Load()
        {
            if (this.CurrentMapRoot == null)
            {
                GameObject tileMapRoot = Resources.Load<GameObject>("Prefabs/Map/TileMapRoot");
                _currentMapRoot = GameObject.Instantiate(tileMapRoot);
                _currentMapRoot.name = "TileMapRoot";
                _currentMapRoot.transform.rotation = Quaternion.identity;
                _currentMapRoot.transform.position = Vector3.zero;
                _currentMapRoot.transform.SetParent(this.transform);
            }
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

        #region 私有

        private void OnLoadEnd(params object[] args)
        {
            UIManager.Instance.HideMask();
            _isLoadMap = false;
        }

        private void OnLoadBefore(System.Action act)
        {
            if (UIManager.Instance.IsShowMask)
            {
                act();
            }
            else
            {
                UIManager.Instance.ShowMask(act);
            }
        }

        /// <summary>
        /// 加载NPC
        /// </summary>
        private void InitNpc()
        {
            if (_cacheNpcDic.ContainsKey(this.CurrentMapDTO.Id))
            {
                List<GameObject> npcList = _cacheNpcDic[this.CurrentMapDTO.Id];

                for (int i = 0; i < npcList.Count; i++)
                {
                    GameObject npc = npcList[i];
                    npc.SetActive(true);
                    npc.GetComponent<NpcController>().Reset();
                }
            }
            else
            {
                _cacheNpcDic.Add(this.CurrentMapDTO.Id, new List<GameObject>());
                if (this.CurrentMapDTO.NpcIds.Count > 0)
                {
                    for (int i = 0; i < this.CurrentMapDTO.NpcIds.Count; i++)
                    {
                        NpcTable npc = TableManager.Instance.GetNpcById(this.CurrentMapDTO.NpcIds[i]);
                        if (npc != null)
                        {
                            GameObject npcObj = Resources.Load<GameObject>("Prefabs/Npc/" + npc.PrefabName);
                            if (npcObj != null)
                            {
                                GameObject game = Instantiate<GameObject>(npcObj);
                                //todo 这里加载不同的NPC
                                NpcController controller = game.GetComponent<NpcController>();
                                controller.Init(npc);

                                _cacheNpcDic[this.CurrentMapDTO.Id].Add(game);
                            }

                        }
                    }
                }
            }
        }

        /// <summary>
        /// 隐藏上一个场景的NPC
        /// </summary>
        private void HideNpc()
        {
            if (this.CurrentMapDTO == null)
            {
                return;
            }
            if (_cacheNpcDic.ContainsKey(this.CurrentMapDTO.Id))
            {
                List<GameObject> npcList = _cacheNpcDic[this.CurrentMapDTO.Id];

                for (int i = 0; i < npcList.Count; i++)
                {
                    GameObject npc = npcList[i];
                    npc.SetActive(false);
                }
            }
        }

        #endregion


    }
}