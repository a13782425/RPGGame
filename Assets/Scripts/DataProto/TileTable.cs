using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RPGGame.Table
{
    public class TileTable
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

        private string _tileData = "";
        public string TileData
        {
            get { return _tileData; }
            set { _tileData = value; }
        }
    }
}
