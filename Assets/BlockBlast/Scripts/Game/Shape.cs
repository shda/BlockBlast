using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class Shape : MonoBehaviour
    {
        [SerializeField] private IntGrid grid;
        [SerializeField] private BoxCollider2D collider2d;

        public IntGrid Grid => grid;
        
        public Vector2 Size => collider2d.size;

        public void CreateCrystals(
            CrystalPool crystalPool,
            GameBoardGridSettings boardGridSettings)
        {
            var step = boardGridSettings.GridStep;
            
            for (int y = 0; y < grid.height; y++)
                for (int x = 0; x < grid.width; x++)
                {
                    var point = grid[x , y];
                    if (point > 0)
                    {
                        var newCristal = crystalPool.Get();
                        newCristal.transform.parent = transform;
                        newCristal.transform.localPosition = new Vector3(step.x * x , step.y * y , 0);
                    }
                }

            ResizeCollider(grid.width , grid.height , boardGridSettings);
        }

        private void ResizeCollider(
            int width, int height , 
            GameBoardGridSettings boardGridSettings)
        {
            var x =  (width - 1) * 0.5f;
            var y =  (height - 1) * 0.5f;
            
            
            Vector2 offset = new Vector2(x > 0 ?x : 0, y > 0 ? y : 0);
            collider2d.size = new Vector2(width, height);
            collider2d.offset = offset;
        }
    }
}