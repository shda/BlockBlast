using System;
using UniRx;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus
{
    public class BonusCountData : MonoBehaviour
    {
        public IntReactiveProperty CountRotateBonus { get; set; } = new();
        public IntReactiveProperty CountDestroyBonus { get; set; } = new();
        public IntReactiveProperty CountChangeShapeBonus { get; set; } = new();

        [SerializeField] private int startCount = 1;
        
        private void Awake()
        {
            CountRotateBonus.Value = startCount;
            CountDestroyBonus.Value = startCount;
            CountChangeShapeBonus.Value = startCount;
        }
    }
}