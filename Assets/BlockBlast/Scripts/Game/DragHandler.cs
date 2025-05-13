using System;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class DragHandler : MonoBehaviour
    {
        [SerializeField] private Camera camera;

        public Action<Shape, Vector3> OnDragShape;
        public Action OnStopDragShape;
        
        private DragStatus _dragStatus = DragStatus.None;
        private BoxCollider2D _dragBoxCollider;
        private Shape _selectedShape;
        private Vector2 _offset;
        
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _dragStatus = DragStatus.Drag;
                
                Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                Collider2D hitCollider = Physics2D.OverlapPoint(mouseWorldPos);

                if (hitCollider != null && hitCollider is BoxCollider2D)
                {
                    _dragBoxCollider = hitCollider as BoxCollider2D;
                    _selectedShape = _dragBoxCollider.GetComponent<Shape>();
                    _offset = _dragBoxCollider.transform.position - new Vector3(mouseWorldPos.x , mouseWorldPos.y, 0);
                }
            }

            if (Input.GetMouseButton(0))
            {
                if (_dragStatus == DragStatus.Drag && _dragBoxCollider)
                {
                    Vector2 mouseWorldPos = camera.ScreenToWorldPoint(Input.mousePosition);
                    _dragBoxCollider.transform.position = mouseWorldPos + _offset;
                    
                    OnDragShape?.Invoke(_selectedShape , mouseWorldPos);
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                _dragBoxCollider = null;
                if (_dragStatus == DragStatus.Drag)
                {
                    _dragStatus = DragStatus.Complete;
                    OnStopDragShape?.Invoke();
                }
            }
        }
    }
}