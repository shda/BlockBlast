using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private ShapesPanel shapesPanel;
        [SerializeField] private DragHandler dragHandler;
        [SerializeField] private GameBoardGridSettings boardGridSettings;
        [SerializeField] private CrystalPool crystalPool;

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

                CheckFullLines();
            }
            else
            {
                dragHandler.ReturnShapeToDefault();
            }
        }

        private void CheckFullLines()
        {
            var size = boardGridSettings.GridSize;

            var crystals = new List<Crystal>();

            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    var point = gameBoard.Grid[x, y];
                    if (point > 0)
                    {
                        crystals.Add(gameBoard.Crystals[x , y]);
                    }
                    else
                    {
                        crystals.Clear();
                        break;
                    }
                }

                if (crystals.Count > 0)
                {
                    HideCrystals(crystals);
                }
                
                crystals.Clear();
            }
            
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    var point = gameBoard.Grid[x, y];
                    if (point > 0)
                    {
                        crystals.Add(gameBoard.Crystals[x , y]);
                    }
                    else
                    {
                        crystals.Clear();
                        break;
                    }
                }

                if (crystals.Count > 0)
                {
                    HideCrystals(crystals);
                }
                
                crystals.Clear();
            }
        }

        private void HideCrystals(List<Crystal> crystals)
        {
            foreach (var crystal in crystals)
            {
                crystal.transform.DOScale(Vector3.zero, 0.3f).onComplete = () =>
                {
                    crystal.transform.localScale = Vector3.one;
                    crystalPool.Release(crystal);
                };
                gameBoard.ClearPoint(crystal.Coordinates);
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