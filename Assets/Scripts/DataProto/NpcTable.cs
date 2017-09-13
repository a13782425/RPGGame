using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    /// <summary>
    /// Npc表
    /// </summary>
    public class NpcTable
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
        private string _prefabName = "";
        public string PrefabName
        {
            get { return _prefabName; }
            set { _prefabName = value; }
        }
        private string _npcTexture = "";
        public string NpcTexture
        {
            get { return _npcTexture; }
            set { _npcTexture = value; }
        }
        private string _npcIcon = "";
        public string NpcIcon
        {
            get { return _npcIcon; }
            set { _npcIcon = value; }
        }

        private int _tipText = -1;
        public int TipText
        {
            get { return _tipText; }
            set { _tipText = value; }
        }
        private string _point = "";
        public string Point
        {
            get { return _point; }
            set { _point = value; }
        }
        private string _talks = "";
        public string Talks
        {
            get { return _talks; }
            set { _talks = value; }
        }

        private int _npcType = -1;
        public int NpcType
        {
            get { return _npcType; }
            set { _npcType = value; }
        }
    }
}
