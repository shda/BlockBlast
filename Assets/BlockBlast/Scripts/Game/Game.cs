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

            void CheckLines(bool isRow)
            {
                for (int i = 0; i < (isRow ? size.x : size.y); i++)
                {
                    var crystals = new List<Crystal>();
                    for (int j = 0; j < (isRow ? size.y : size.x); j++)
                    {
                        int x = isRow ? i : j;
                        int y = isRow ? j : i;
                        var point = gameBoard.Grid[x, y];
                        if (point > 0)
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
                        HideCrystals(crystals);
                    }
                }
            }

            CheckLines(true); // Проверка строк
            CheckLines(false); // Проверка столбцов
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
            _isCanSetShape = gameBoard.SetTempShape(shape, point);
        }

        private void Start()
        {
            shapesPanel.CreateShapes();
        }
    }
}