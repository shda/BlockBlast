using System.Linq;
using BlockBlast.Scripts.Game.Bonus;
using Cysharp.Threading.Tasks;
using TriInspector;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace BlockBlast.Scripts.Game
{
    public class Game : MonoBehaviour
    {
        [SerializeField] private GameBoard gameBoard;
        [SerializeField] private PanelDragShapes panelDragShapes;
        [SerializeField] private DragShapeHandler dragShapeHandler;
        [SerializeField] private CrystalPool crystalPool;
        [SerializeField] private GameOverScreen gameOverScreen;
        [SerializeField] private LineExplosionAnimationPool lineExplosionAnimationPool;
        [SerializeField] private ScoreLabes scoreLabes;
        [SerializeField] private BonusCountData bonusCountData;
        [SerializeField] private FullLineProcessor fullLineProcessor;

        private bool _isCanSetShape;
        private Shape _dragShape;

        private void Start()
        {
            gameOverScreen.Hide();
            panelDragShapes.CreateNewShapes();

            ConnectEvents();
            ConnectBonusEvents();
        }

        private void ConnectBonusEvents()
        {
            bonusCountData.CountChangeShapeBonus.Subscribe(OnChangeCountBonuses).AddTo(this);
            bonusCountData.CountDestroyBonus.Subscribe(OnChangeCountBonuses).AddTo(this);
            bonusCountData.CountRotateBonus.Subscribe(OnChangeCountBonuses).AddTo(this);
        }

        private void OnChangeCountBonuses(int count)
        {
            DelayedCheckGameOverAsync();
        }

        [Button]
        private async void DelayedCheckGameOverAsync()
        {
            await UniTask.Delay(100);
            if (!IsFindFreeSpaceForShape() &&
                !IsCanUsingBonus())
            {
                ShowGameOver();
            }
        }

        private void ConnectEvents()
        {
            dragShapeHandler.OnDragShape = OnDragShape;
            dragShapeHandler.OnCompleteDragShape = OnCompleteDragShape;

            gameOverScreen.OnRestartGame = () => { SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); };
        }

        private void OnCompleteDragShape()
        {
            if (_isCanSetShape)
            {
                gameBoard.SetShapeFromTemp();
                gameBoard.RemoveTempShape();

                panelDragShapes.RemoveShape(_dragShape);
                fullLineProcessor.CheckFullLines(_dragShape);

                DelayedCheckGameOverAsync();
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

        private bool IsCanUsingBonus()
        {
            return bonusCountData.CountChangeShapeBonus.Value > 0 ||
                   bonusCountData.CountDestroyBonus.Value > 0;
        }

        private void OnDragShape(Shape shape, Vector3 pointShape)
        {
            _dragShape = shape;
            _isCanSetShape = gameBoard.SetTempShape(shape, pointShape);
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