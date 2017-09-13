using RPGGame.Table;
using System.Collections;
using System.Collections.Generic;


namespace RPGGame.Model
{

    /// <summary>
    /// 创建怪物的数据类型
    /// </summary>
    public class CreateMonsterData
    {

        private int _mapId = 0;



        public int MapId
        {
            get { return _mapId; }
            set { _mapId = value; }
        }

        private List<MonsterTable> _monsterInfo = new List<MonsterTable>();

        public List<MonsterTable> MonsterInfo
        {
            get { return MonsterInfo; }
        }
    }
}

