using System;
using BlockBlast.Scripts.Game.Bonus.Rotate;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.ChangeShape
{
    public class ButtonsChangeShape : MonoBehaviour
    {
        [SerializeField] private RectTransformFallowToObject  fallowToObject;
        
        public Action PressChangeShape;

        public void OnPressChangeShape()
        {
            PressChangeShape?.Invoke();
        }

        public void SetFallowTransform(Transform tr)
        {
            fallowToObject.SetFallowTransform(tr);
        }
    }
}