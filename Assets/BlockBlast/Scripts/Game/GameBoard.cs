using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class GameBoardModel
    {
        public bool[,] Grid { get; set; }

        public GameBoardModel(int width, int height)
        {
            Grid = new bool[width, height];
        }
    }
    
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private Vector2Int gridSize;
        
        public bool[,] Grid;

        private void Awake()
        {
            Grid = new bool[gridSize.x, gridSize.y];
        }
    }
}