using System;
using DG.Tweening;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class DragHandler : MonoBehaviour
    {
        [SerializeField] private Camera camera;
        [SerializeField] private Vector2 offsetScroll = new(5f, 0f);
        public Action<Shape, Vector3> OnDragShape;
        public Action OnStopDragShape;
        
        private DragStatus _dragStatus = DragStatus.None;
        private Shape _selectedShape;
        private Vector2 _offset;
        
        private Vector3 _startShapePosition;
        private Vector3 _startShapeScale;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragStatus = DragStatus.Drag;
                
                Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                var hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

                if (hitCollider && hitCollider is BoxCollider2D dragBoxCollider)
                {
                    _selectedShape = dragBoxCollider.GetComponent<Shape>();
                    _offset = dragBoxCollider.transform.position - new Vector3(mouseWorldPos.x , mouseWorldPos.y, 0);
                    _offset -= offsetScroll;

                    _startShapePosition = _selectedShape.transform.position;
                    _startShapeScale = _selectedShape.transform.localScale;
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (_dragStatus == DragStatus.Drag && _selectedShape)
                {
                    Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                    _selectedShape.transform.position = mouseWorldPos + _offset;
                    _selectedShape.transform.localScale = Vector3.one;
                    OnDragShape?.Invoke(_selectedShape , mouseWorldPos + _offset);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                if (_dragStatus == DragStatus.Drag)
                {
                    _dragStatus = DragStatus.None;
                    OnStopDragShape?.Invoke();
                }
            }
        }

        public void ReturnShapeToDefault()
        {
            if (_selectedShape != null)
            {
                _selectedShape.transform.DOMove(_startShapePosition, 0.3f);
                _selectedShape.transform.DOScale(_startShapeScale, 0.3f);
            }
        }
    }
}