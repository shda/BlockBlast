using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlockBlast.Scripts.Game.Bonus.Rotate
{
    public class ButtonsRotate : UIBehaviour
    {
        [SerializeField] private RectTransformFallowToObject fallowToObject;

        public Action<ButtonsRotate> PressRotate;
        public Action<ButtonsRotate> PressApply;

        public void OnPressRotate()
        {
            PressRotate?.Invoke(this);
        }

        public void OnPressApply()
        {
            PressApply?.Invoke(this);
        }
        
        public void SetFallowTransform(Transform tr)
        {
            fallowToObject.SetFallowTransform(tr);
        }
    }
}