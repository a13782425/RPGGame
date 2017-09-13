using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    public class LanguageTable
    {

        private int _id = 0;
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _chineseSimplified = "";
        public string ChineseSimplified
        {
            get { return _chineseSimplified; }
            set { _chineseSimplified = value; }
        }
        private string _chineseTraditional = "";
        public string ChineseTraditional
        {
            get { return _chineseTraditional; }
            set { _chineseTraditional = value; }
        }
        private string _english = "";
        public string English
        {
            get { return _english; }
            set { _english = value; }
        }
    }
}