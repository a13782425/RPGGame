using UnityEngine;
using System.Collections;

namespace RPGGame.Table
{
    public class FoodBuffTable
    {
        private int _foodId = 0;
        public int FoodId
        {
            get { return _foodId; }
            set { _foodId = value; }
        }
        private int _buffId = 0;
        public int BuffId
        {
            get { return _buffId; }
            set { _buffId = value; }
        }
        private int _buffVariationNum = 0;
        /// <summary>
        /// 概率
        /// </summary>

        public int BuffVariationNum
        {
            get { return _buffVariationNum; }
            set { _buffVariationNum = value; }
        }
        private int _buffProbability = 0;
        /// <summary>
        /// 概率
        /// </summary>
        public int BuffProbability
        {
            get { return _buffProbability; }
            set { _buffProbability = value; }
        }
    }
}