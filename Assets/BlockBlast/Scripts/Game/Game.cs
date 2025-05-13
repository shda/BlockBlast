using System;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private GameBoardGridSettings  boardGridSettings;
        [SerializeField] private ShapesPanel shapesPanel;
        [SerializeField] private ShapeCollection shapeCollection;
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private DragHandler dragHandler;

        private bool _isCanSetShape;
        
        private void Awake()
        {
            dragHandler.OnDragShape = OnDragShape;
            dragHandler.OnStopDragShape = OnStopDragShape;
        }

        private void OnStopDragShape()
        {
            if (_isCanSetShape)
            {
                
            }
            else
            {
                gameBoard.RemoveTempShape();
            }
        }

        private void OnDragShape(Shape shape, Vector3 point)
        {
            _isCanSetShape = gameBoard.SetTempShape(shape , point);
        }

        private void Start()
        {
            for (int i = 0; i < 3; i++)
            {
                var newShape = shapeCollection.CrateRandomShape();
                newShape.CreateCrystals(crystalPool , boardGridSettings);
                shapesPanel.AddShape(newShape);
            }
        }
    }
}