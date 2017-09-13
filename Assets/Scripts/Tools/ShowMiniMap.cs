using CreativeSpore.RpgMapEditor;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RPGGame.Tools
{
    public class ShowMiniMap : MonoBehaviour
    {

        public AutoTileMap CurrentAutoTileMap = null;

        [SerializeField]
        private Image miniMapImage = null;

        void Awake()
        {
            miniMapImage.gameObject.SetActive(false);
        }

        public void Show(AutoTileMap autoTileMap = null)
        {
            AutoTileMap showMap = autoTileMap;
            if (showMap == null)
            {
                showMap = CurrentAutoTileMap;
            }
            if (showMap == null)
            {
                throw new Exception("not show Map");
            }
            showMap.RefreshMinimapTexture();
            miniMapImage.sprite = Sprite.Create(showMap.MinimapTexture, new Rect(0, 0, showMap.MinimapTexture.width, showMap.MinimapTexture.height), Vector2.zero);
            miniMapImage.gameObject.SetActive(true);
        }
    }
}