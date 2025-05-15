using System.Collections.Generic;
using System.Linq;
using BlockBlast.Scripts.Game.Bonus;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BlockBlast.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private PanelDragShapes panelDragShapes;
        [SerializeField] private DragShapeHandler dragShapeHandler;
        [SerializeField] private GameBoardGridSettings boardGridSettings;
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameOverScreen  gameOverScreen;
        [SerializeField] private LineExplosionAnimationPool lineExplosionAnimationPool;
        [SerializeField] private ScoreLabes scoreLabes;
        [SerializeField] private BonusHandler bonusHandler;

        private bool _isCanSetShape;
        private Shape _dragShape;
        
        private void Awake()
        {
            gameOverScreen.Hide();
            ConnectEvents();
        }

        private void ConnectEvents()
        {
            dragShapeHandler.OnDragShape = OnDragShape;
            dragShapeHandler.OnCompleteDragShape = OnCompleteDragShape;
            
            gameOverScreen.OnRestartGame = () =>
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
                
                if (!IsFindFreeSpaceForShape())
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
            scoreLabes.UpdateBestScore();
            gameOverScreen.Show();
            
            Debug.Log("ShowGameOver");
        }
        
        private bool IsFindFreeSpaceForShape()
        {
            return panelDragShapes.Shapes.Any(shape => gameBoard.FindFreeSpaceOnBoard(shape) != null);
        }

        private void CheckFullLines()
        {
            var size = boardGridSettings.GridSize;
            
            var crystalsHide = new HashSet<Crystal>();
            
            var lines = CheckLines(true); // Проверка строк
            lines += CheckLines(false); // Проверка столбцов
            bonusHandler.FullLineRemove(lines);
            RemovingCrystalsLine(crystalsHide);
            
            
            int  CheckLines(bool isRow)
            { 
                int countLines = 0;
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

                        countLines++;
                        PlayExplosionLine(crystals);
                    }
                }

                return countLines;
            }
        }

        private void RemovingCrystalsLine(IEnumerable<Crystal> crystals)
        {
            foreach (var crystal in crystals)
            {
                scoreLabes.AddScore(1);

                crystal.PlayAnimationHide(() =>
                {
                    crystal.transform.localScale = Vector3.one;
                    crystalPool.Release(crystal);
                });
                
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

            var explosion = lineExplosionAnimationPool.Get();
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
                lineExplosionAnimationPool.Release(explosion); 
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