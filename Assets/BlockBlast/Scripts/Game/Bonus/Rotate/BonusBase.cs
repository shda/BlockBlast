using System;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.Rotate
{
    public abstract class BonusBase : MonoBehaviour
    {
        [SerializeField] protected BonusCountData bonusCountData;
        
        public Action OnComplete;
        
        public abstract void StartBonus();
        public abstract void StopBonus();
        public abstract bool IsCanUsingBonus();
    }
}