using System;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.Rotate
{
    public abstract class BonusBase : MonoBehaviour
    {
        public Action OnComplete;
        
        public abstract void StartBonus();
        public abstract void StopBonus();
    }
}