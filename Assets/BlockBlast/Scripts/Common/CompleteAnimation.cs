using System;
using UnityEngine;

namespace StickHero.Scripts.Game
{
    public class CompleteAnimation : MonoBehaviour
    {
        public Action OnCompleteAnimation;

        public void CompleteAnimationEvent()
        {
            OnCompleteAnimation?.Invoke();
        }
    }
}