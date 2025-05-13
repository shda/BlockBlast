using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace BlockBlast.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private SelectShapesPanel selectShapesPanel;
        [SerializeField] private DragShapeHandler dragShapeHandler;
        [SerializeField] private GameBoardGridSettings boardGridSettings;
        [SerializeField] private CrystalPool crystalPool;

        private bool _isCanSetShape;
        private Shape _dragShape;

        private void Awake()
        {
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            dragShapeHandler.OnDragShape = OnDragShape;
            dragShapeHandler.OnCompleteDragShape = OnCompleteDragShape;
        }

        private void OnCompleteDragShape()
        {
            if (_isCanSetShape)
            {
                gameBoard.SetShapeFromTemp();
                gameBoard.RemoveTempShape();

                selectShapesPanel.RemoveShape(_dragShape);

                CheckFullLines();
            }
            else
            {
                dragShapeHandler.ReturnShapeToDefault();
            }
        }

        private void CheckFullLines()
        {
            var size = boardGridSettings.GridSize;

            CheckLines(true); // Проверка строк
            CheckLines(false); // Проверка столбцов
            void CheckLines(bool isRow)
            {
                for (var i = 0; i < (isRow ? size.x : size.y); i++)
                {
                    var crystals = new List<Crystal>();
                    for (var j = 0; j < (isRow ? size.y : size.x); j++)
                    {
                        int x = isRow ? i : j;
                        int y = isRow ? j : i;
                        var point = gameBoard.Crystals[x, y];
                        if (point != null)
                        {
                            crystals.Add(gameBoard.Crystals[x, y]);
                        }
                        else
                        {
                            crystals.Clear();
                            break;
                        }
                    }

                    if (crystals.Count > 0)
                    {
                        RemovingCrystalsLine(crystals);
                    }
                }
            }
        }

        private void RemovingCrystalsLine(List<Crystal> crystals)
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

        private void OnDragShape(Shape shape, Vector3 pointShape)
        {
            _dragShape = shape;
            _isCanSetShape = gameBoard.SetTempShape(shape, pointShape);
        }

        private void Start()
        {
            selectShapesPanel.CreateNewShapes();
        }
    }
}