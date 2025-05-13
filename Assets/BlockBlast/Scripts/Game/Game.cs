using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private ShapesPanel shapesPanel;
        [SerializeField] private DragHandler dragHandler;

        private bool _isCanSetShape;
        private Shape _dragShape;
        
        private void Awake()
        {
            dragHandler.OnDragShape = OnDragShape;
            dragHandler.OnStopDragShape = OnStopDragShape;
        }

        private void OnStopDragShape()
        {
            if (_isCanSetShape)
            {
                gameBoard.SetShapeFromTemp();
                gameBoard.RemoveTempShape();
                
                shapesPanel.RemoveShape(_dragShape);
            }
            else
            {
                dragHandler.ReturnShapeToDefault();
            }
        }

        private void OnDragShape(Shape shape, Vector3 point)
        {
            _dragShape = shape;
            _isCanSetShape = gameBoard.SetTempShape(shape , point);
        }

        private void Start()
        {
            shapesPanel.CreateShapes();
        }
    }
}