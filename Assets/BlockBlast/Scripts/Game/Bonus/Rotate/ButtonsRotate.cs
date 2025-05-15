using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace BlockBlast.Scripts.Game.Bonus.Rotate
{
    public class ButtonsRotate : UIBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform uiElement;
        
        private Transform _targetTransform;

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
            _targetTransform = tr;
            Update();
        }
        
        private void Update()
        {
            if(!_targetTransform)
                return;
            
            var screenPoint = RectTransformUtility.WorldToScreenPoint(cam, _targetTransform.position);

            Vector2 localPoint;
            var canvasRect = canvas.GetComponent<RectTransform>();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, 
                    canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam, out localPoint))
            {
                uiElement.localPosition = localPoint;
            }
        }
    }
}