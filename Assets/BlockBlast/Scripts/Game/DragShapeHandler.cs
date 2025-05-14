using System;
using DG.Tweening;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class DragShapeHandler : MonoBehaviour
    {
        [SerializeField] private Camera mainCamera;
        [SerializeField] private Vector2 offsetScroll = new(5f, 0f);
        
        public Action<Shape, Vector3> OnDragShape;
        public Action OnCompleteDragShape;

        private bool _isDrag;
        private Shape _selectedShape;
        private Vector2 _offsetStartDrag;
        
        private Vector3 _saveStartShapePosition;
        private Vector3 _saveStartShapeScale;

        private bool _isShapeMovingToStartPositon;
        
        private void Update()
        {
            if(_isShapeMovingToStartPositon)
                return;
            
            StartDragHandler();
            DragHandler();
            CompleteDragHandler();
        }
        
        private void StartDragHandler()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                var hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

                if (hitCollider && hitCollider is BoxCollider2D dragBoxCollider)
                {
                    _isDrag = true;
                    
                    _selectedShape = dragBoxCollider.GetComponent<Shape>();
                    _offsetStartDrag = dragBoxCollider.transform.position - new Vector3(mouseWorldPos.x , mouseWorldPos.y, 0);
                    _offsetStartDrag -= offsetScroll;

                    _saveStartShapePosition = _selectedShape.transform.position;
                    _saveStartShapeScale = _selectedShape.transform.localScale;
                }
                else
                {
                    _selectedShape = null;
                }
            }
        }
        
        private void DragHandler()
        {
            if (Input.GetMouseButton(0))
            {
                if (_isDrag && _selectedShape)
                {
                    Vector2 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    _selectedShape.transform.position = mouseWorldPos + _offsetStartDrag;
                    _selectedShape.transform.localScale = Vector3.one;
                    OnDragShape?.Invoke(_selectedShape , mouseWorldPos + _offsetStartDrag);
                }
            }
        }
        
        private void CompleteDragHandler()
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (_isDrag)
                {
                    _isDrag = false;
                    OnCompleteDragShape?.Invoke();
                }
            }
        }

        public void ReturnShapeToDefault()
        {
            if (_selectedShape)
            {
                _isShapeMovingToStartPositon = true;
                
                _selectedShape.transform.DOMove(_saveStartShapePosition, 0.3f);
                _selectedShape.transform.DOScale(_saveStartShapeScale, 0.3f).onComplete = () =>
                {
                    _isShapeMovingToStartPositon = false;
                };
            }
        }
    }
}