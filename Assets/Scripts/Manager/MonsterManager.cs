using UnityEngine;
using System.Collections;
using System.Threading;
using System.Collections.Generic;
using RPGGame.Model;
using RPGGame.Model.DTO;
using RPGGame.Table;

namespace RPGGame.Manager
{
    public sealed class MonsterManager : RPGGame.Tools.Singleton<MonsterManager>, IManager
    {
        #region 属性和字段

        private readonly object LOCK_OBJECT = new object();
        /// <summary>
        /// 没有找到怪 睡眠时间
        /// </summary>
        public const int MONSTER_THREAD_SLEEP_TIME = 5;

        private Thread _currentThread = null;

        private List<CreateMonsterData> _waitList = new List<CreateMonsterData>();

        private List<CreateMonsterData> _threadList = new List<CreateMonsterData>();

        private bool _isCanRefreshMonster = true;
        public bool IsCanRefreshMonster
        {
            get { return _isCanRefreshMonster; }
            set { _isCanRefreshMonster = value; }
        }

        private bool _playerIsIdle = false;

        /// <summary>
        /// 玩家是否空闲
        /// </summary>
        public bool PlayerIsIdle { get { return _playerIsIdle; } set { _playerIsIdle = value; } }

        /// <summary>
        /// 检测刷怪线程是否被关闭
        /// </summary>
        private const float CHECK_THREAD_TIME = 5f;
        /// <summary>
        /// 检测刷怪线程是否被关闭(临时变量)
        /// </summary>
        private float _checkThreadTemp = 0f;

        #endregion



        public void StartRefreshMonster()
        {
            if (_currentThread == null)
            {
                _currentThread = new Thread(new ThreadStart(RefreshMonster));
                _currentThread.IsBackground = true;
                _currentThread.Name = "RefreshMonster";
                _currentThread.Start();
            }
            else if (_currentThread.ThreadState != ThreadState.Running)
            {
                _currentThread.Start();
            }
        }




        #region 私有

        /// <summary>
        /// 刷怪线程
        /// </summary>
        private void RefreshMonster()
        {
            while (this.IsCanRefreshMonster)
            {
                if (MapManager.Instance.CurrentScript != null)
                {
                    Scorpio.ScriptFunction sc = MapManager.Instance.CurrentScript.GetValue("Take") as Scorpio.ScriptFunction;
                    if (sc!=null)
                    {
                        sc.Call();
                    }
                }
                Debug.LogError("11");
                if (PlayerIsIdle)
                {
                    //todo : 刷怪条件
                    MapDTO mapDto = MapManager.Instance.CurrentMapDTO;
                    if (mapDto != null && mapDto.IsHasMonster)
                    {
                        //要刷新的怪物数量
                        int monsterCount = Random.Range(0, 10000) % 7;
                        if (monsterCount == 0)
                        {
                            Thread.Sleep(MONSTER_THREAD_SLEEP_TIME * 1000);
                        }
                        else
                        {
                            CreateMonsterData create = new CreateMonsterData();
                            create.MapId = mapDto.Id;
                            for (int i = 0; i < monsterCount; i++)
                            {
                                int monsterIndex = Random.Range(0, 10000) % mapDto.MonsterIds.Count;
                                int monsterId = mapDto.MonsterIds[monsterIndex];
                                MonsterTable monster = TableManager.Instance.GetMonsterById(monsterId);
                                if (monster != null)
                                {
                                    create.MonsterInfo.Add(monster);
                                }
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(MONSTER_THREAD_SLEEP_TIME * 1000);
                    }
                }
                else
                {
                    Thread.Sleep(MONSTER_THREAD_SLEEP_TIME * 1000);
                }
            }
        }

        /// <summary>
        /// 线程和主线程间传递数据
        /// </summary>
        /// <param name="isSwap"></param>
        /// <param name="data"></param>
        private void SwapList(bool isSwap, CreateMonsterData data = null)
        {
            lock (LOCK_OBJECT)
            {
                if (isSwap)
                {
                    _waitList.AddRange(_threadList);
                    _threadList.Clear();
                    if (data != null)
                    {
                        goto Add;
                    }
                }
                Add: _threadList.Add(data);
            }
        }

        private void Stop()
        {
            _isCanRefreshMonster = false;
            if (_currentThread != null)
            {
                if (_currentThread.ThreadState != ThreadState.Stopped)
                {
                    _currentThread.Abort();
                }
            }
        }

        #endregion

        public bool Load()
        {
            StartRefreshMonster();
            return true;
        }

        public bool UnLoad()
        {
            Stop();
            return true;
        }



        public void OnUpdate()
        {
            #region 检测线程是否被杀死

            if (IsCanRefreshMonster)
            {
                if (_checkThreadTemp > CHECK_THREAD_TIME)
                {
                    _checkThreadTemp += Time.deltaTime;
                    if (_currentThread == null)
                    {
                        _currentThread = new Thread(new ThreadStart(RefreshMonster));
                        _currentThread.IsBackground = true;
                        _currentThread.Name = "RefreshMonster";
                    }
                    else if (_currentThread.ThreadState != ThreadState.Running)
                    {
                        _currentThread.Start();
                    }
                    _checkThreadTemp = 0;
                }
            }

            #endregion
        }
    }

}
