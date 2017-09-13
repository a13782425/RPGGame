using UnityEngine;
using System.Collections;
using System;
using UnityEngine.SceneManagement;
using RPGGame.Enums;
using RPGGame.Level;
namespace RPGGame.Manager
{
    public sealed class LevelManager : RPGGame.Tools.Singleton<LevelManager>, IManager
    {
        private BaseLevel _currentLevel = new StartupLevel();
        public BaseLevel CurrentLevel { get { return _currentLevel; } }
        private BaseLevel _nextLevel = null;
        private string _nextLevelName = "";
        private SceneEnum _nextLevelEnum = SceneEnum.None;
        private AsyncOperation _nextAsync = null;

        public SceneEnum CurrentSceneEnum
        {
            get
            {
                if (CurrentLevel != null)
                {
                    return CurrentLevel.CurrentScene;
                }
                return SceneEnum.None;
            }
        }

        public IEnumerator SwitchToLevel(string levelName, Action callBack)
        {
            _nextLevelEnum = (SceneEnum)System.Enum.Parse(typeof(SceneEnum), levelName);
            if (_nextLevelEnum == SceneEnum.None || this.CurrentSceneEnum == _nextLevelEnum || _nextAsync != null)
            {
                Debug.LogError("加载场景：" + levelName + ",场景出错！");
                yield break;
            }
            _nextLevelName = levelName;
            CreateNewScene(_nextLevelEnum);
            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(ChangeLevel(callBack));

            yield return null;
        }

        private IEnumerator ChangeLevel(Action callBack)
        {
            if (_nextAsync != null)
            {
                Debug.LogError("加载场景失败！");
                yield break;
            }
            _nextAsync = SceneManager.LoadSceneAsync(_nextLevelName);
            while (_nextAsync.progress < 0.9f)
            {
                yield return null;
            }
            _nextAsync.allowSceneActivation = false;
            if (callBack != null)
            {
                callBack();
                yield return new WaitForSeconds(1f);
            }
            _nextAsync.allowSceneActivation = true;
            this.CurrentLevel.OnExit();
            _nextLevel.OnGameStart();
            this._currentLevel = _nextLevel;
            _nextLevel = null;
            _nextAsync = null;
            _nextLevelEnum = SceneEnum.None;
            _nextLevelName = "";

        }

        public IEnumerator SwitchToLevel(SceneEnum level, Action callBack)
        {
            _nextLevelEnum = level;
            yield return StartCoroutine(SwitchToLevel(_nextLevelEnum.ToString(), callBack));
        }

        public bool Load()
        {
            this.CurrentLevel.OnGameStart();
            return true;
        }

        public bool UnLoad()
        {
            this.CurrentLevel.OnExit();
            return true;
        }

        void Update()
        {
            if (this.CurrentLevel != null && this.CurrentLevel.IsActived)
            {
                this.CurrentLevel.OnUpdate();
            }
        }

        private void CreateNewScene(SceneEnum sceneEnum)
        {
            switch (sceneEnum)
            {
                case SceneEnum.Startup:
                    _nextLevel = new StartupLevel();
                    break;
                case SceneEnum.LoadScene:
                    _nextLevel = new LoadLevel();
                    break;
                case SceneEnum.HomeScene:
                    _nextLevel = new HomeLevel();
                    break;
                case SceneEnum.MainScene:
                    _nextLevel = new MainLevel();
                    break;
                case SceneEnum.None:
                default:
                    _nextLevel = null;
                    break;
            }
        }

        public void OnUpdate()
        {
        }
    }
}

