using System;
using System.Collections.Generic;
using System.Linq;
using BlockBlast.Scripts.Common.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace BlockBlast.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private PanelDragShapes panelDragShapes;
        [SerializeField] private DragShapeHandler dragShapeHandler;
        [SerializeField] private GameBoardGridSettings boardGridSettings;
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameOverPresenter  gameOver;
        [SerializeField] private ExplosionBlockPool explosionBlockPool;
        [SerializeField] private Score score;

        private bool _isCanSetShape;
        private Shape _dragShape;
        
        private void Awake()
        {
            gameOver.Hide();
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            dragShapeHandler.OnDragShape = OnDragShape;
            dragShapeHandler.OnCompleteDragShape = OnCompleteDragShape;
            
            gameOver.OnRestartGame = () =>
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            };
        }

        private void OnCompleteDragShape()
        {
            if (_isCanSetShape)
            {
                gameBoard.SetShapeFromTemp();
                gameBoard.RemoveTempShape();

                panelDragShapes.RemoveShape(_dragShape);
                CheckFullLines();
                
                if (!IsFindPalaceToShape())
                {
                    ShowGameOver();
                    return;
                }
            }
            else
            {
                dragShapeHandler.ReturnShapeToDefault();
            }
        }

        private void ShowGameOver()
        {
            score.UpdateBestScore();
            gameOver.Show();
            
            Debug.Log("ShowGameOver");
        }
        
        private bool IsFindPalaceToShape()
        {
            bool isCanSetShape = false;
            
            foreach (var shape in panelDragShapes.Shapes)
            {
                if (gameBoard.CheckIsCanSetShape(shape))
                {
                    isCanSetShape = true;
                    break;
                }
            }

            return isCanSetShape;
        }

        private void CheckFullLines()
        {
            var size = boardGridSettings.GridSize;
            
            var crystalsHide = new HashSet<Crystal>();
            
            CheckLines(true); // Проверка строк
            CheckLines(false); // Проверка столбцов

            RemovingCrystalsLine(crystalsHide);
            
            
            void CheckLines(bool isRow)
            {
                for (var i = 0; i < (isRow ? size.x : size.y); i++)
                {
                    var crystals = new List<Crystal>();
                    for (var j = 0; j < (isRow ? size.y : size.x); j++)
                    {
                        var x = isRow ? i : j;
                        var y = isRow ? j : i;
                        var point = gameBoard.Crystals[x, y];
                        if (point)
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
                        foreach (var crystal in crystals)
                        {
                            crystalsHide.Add(crystal);
                        }
                        
                        PlayExplosionLine(crystals);
                    }
                }
            }
        }

        private void RemovingCrystalsLine(IEnumerable<Crystal> crystals)
        {
            foreach (var crystal in crystals)
            {
                score.AddScore(1);
                
                crystal.transform.DOScale(Vector3.zero, 0.3f).onComplete = () =>
                {
                    crystal.transform.localScale = Vector3.one;
                    crystalPool.Release(crystal);
                };
                gameBoard.ClearPoint(crystal.Coordinates);
            }
        }

        private void PlayExplosionLine(List<Crystal> crystals)
        {
            var isVertical = !Mathf.Approximately(crystals[0].transform.position.y, crystals[1].transform.position.y);
            var firstPosition = crystals[0].transform.position;

            foreach (var crystal in crystals)
            {
                if (crystal.transform.position.x < firstPosition.x || 
                    crystal.transform.position.y < firstPosition.y)
                {
                    firstPosition = crystal.transform.position;
                }
            }

            var explosion = explosionBlockPool.Get();
            explosion.transform.position = firstPosition;
            explosion.SetColor(_dragShape.CrystalColor);
            if (isVertical)
            {
                explosion.PlayVerticalAnimation();
            }
            else
            {
                explosion.PlayHorizontalAnimation();
            }

            explosion.OnComplete = () =>
            {
                explosionBlockPool.Release(explosion); 
            };
        }

        private void OnDragShape(Shape shape, Vector3 pointShape)
        {
            _dragShape = shape;
            _isCanSetShape = gameBoard.SetTempShape(shape, pointShape);
        }

        private void Start()
        {
            panelDragShapes.CreateNewShapes();
        }
        
        public void DebugInitLevel(IntGrid grid)
        {
            gameBoard.DebugInitLevel(grid);
        }

        public void DebugInitShapes(Shape[] setShapes)
        {
            panelDragShapes.DebugInitShapes(setShapes);
        }
    }
}