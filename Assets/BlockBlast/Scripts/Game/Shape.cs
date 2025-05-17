using System.Collections.Generic;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class Shape : MonoBehaviour
    {
        [SerializeField] private IntGrid grid;
        [SerializeField] private BoxCollider2D collider2d;

        private readonly List<Crystal> _crystals = new ();
        public IntGrid Grid => grid;
        public Vector2 Size => new(grid.width , grid.height);
        public Vector3 ColliderSize => collider2d.size;
        
        public int CrystalColor { get; private set; }

        private CrystalPool _crystalPool; 
        private GameBoardGridSettings _boardGridSettings;
        private int _indexColor;
        
        public void CreateCrystals(
            CrystalPool crystalPool,
            GameBoardGridSettings boardGridSettings , int indexColor = 0)
        {
            _crystalPool = crystalPool;
            _boardGridSettings = boardGridSettings;
            _indexColor = indexColor;
            
            ReleaseCrystals(crystalPool);
            
            var step = boardGridSettings.GridStep;

            CrystalColor = indexColor;
            
            for (int y = 0; y < grid.height; y++)
                for (int x = 0; x < grid.width; x++)
                {
                    var point = grid[x , y];
                    if (point > 0)
                    {
                        var newCristal = crystalPool.Get();
                        newCristal.SetSprite(indexColor);
                        newCristal.transform.parent = transform;
                        newCristal.transform.localPosition = new Vector3(step.x * x , step.y * y , 0);
                        newCristal.transform.localScale = Vector3.one;
                        
                        _crystals.Add(newCristal);
                    }
                }
            
           // ResizeCollider(grid.width , grid.height);
        }
        
        private void CreateCrystals()
        {
            CreateCrystals(_crystalPool, _boardGridSettings , _indexColor);
        }

        private void ResizeCollider(
            int width, int height )
        {
            var x =  (width - 1) * 0.5f;
            var y =  (height - 1) * 0.5f;
            
            Vector2 offset = new Vector2(x > 0 ?x : 0, y > 0 ? y : 0);
            collider2d.size = new Vector2(width, height);
            collider2d.offset = offset;
        }

        public void ReleaseCrystals(CrystalPool crystalPool)
        {
            foreach (var crystal in _crystals)
            {
                crystalPool.Release(crystal);
            }
            
            _crystals.Clear();
        }

        public void SetMatrix(int[,] matrix)
        {
            grid.SetMatrix(matrix);
        }

        public void RotateShapeCounterClockwise()
        {
            var matrix = grid.GetMatrix();
            var newMatrix = RotateMatrixCounterClockwise(matrix);
            grid.SetMatrix(newMatrix);
            
            CreateCrystals();
        }

        private static int[,] RotateMatrixCounterClockwise(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            int[,] rotated = new int[cols, rows];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    rotated[cols - 1 - j, i] = matrix[i, j];
                }
            }

            return rotated;
        }
    }
}