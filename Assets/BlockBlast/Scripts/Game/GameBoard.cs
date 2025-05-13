using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class GameBoard : MonoBehaviour
    {
        [SerializeField] private GameBoardGridSettings  boardGridSettings;
        [SerializeField] private Vector2Int gridSize;
        
        public int[,] Grid { get; set; }

        private void Awake()
        {
            Grid = new int[boardGridSettings.GridSize.x, boardGridSettings.GridSize.y];
        }
    }
}