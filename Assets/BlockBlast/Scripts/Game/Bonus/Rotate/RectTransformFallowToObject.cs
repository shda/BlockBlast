using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.Rotate
{
    public class RectTransformFallowToObject : MonoBehaviour
    {
        [SerializeField] private Camera cam;
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform uiElement;
        [SerializeField] private Vector2 offset;
        
        private Transform _targetTransform;
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
            screenPoint += offset;
            var canvasRect = canvas.GetComponent<RectTransform>();
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, 
                    canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : cam, out var localPoint))
            {
                uiElement.localPosition = localPoint;
            }
        }
    }
}