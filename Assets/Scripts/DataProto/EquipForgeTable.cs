using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    public class EquipForgeTable
    {
        private int _equipId = 0;
        public int EquipId
        {
            get { return _equipId; }
            set { _equipId = value; }
        }
        private int _npcId = 0;
        public int NpcId
        {
            get { return _npcId; }
            set { _npcId = value; }
        }
        private string _materialInfo = "";
        public string MaterialInfo
        {
            get { return _materialInfo; }
            set { _materialInfo = value; }
        }
    }
}
