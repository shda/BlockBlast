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
        public Vector2 Size => collider2d.size;
        
        public int CrystalColor { get; private set; }

        public void CreateCrystals(
            CrystalPool crystalPool,
            GameBoardGridSettings boardGridSettings , int indexColor = 0)
        {
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

            ResizeCollider(grid.width , grid.height);
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
    }
}