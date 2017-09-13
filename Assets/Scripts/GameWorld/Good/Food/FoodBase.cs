using UnityEngine;
using System.Collections;
using RPGGame.GameWorld.Good.Base;

namespace RPGGame.GameWorld.Good.Food
{
    public abstract class FoodBase : GoodBase
    {
        [SerializeField]
        protected int _hunger = 0;
        public int Hunger { get { return _hunger; } }

    }
}
