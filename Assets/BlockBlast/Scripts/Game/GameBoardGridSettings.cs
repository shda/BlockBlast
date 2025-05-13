using System;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class GameBoardGridSettings : MonoBehaviour
    {
        [SerializeField] private float gizmoPointSize;
        [SerializeField] private Vector2Int gridSize;
        [SerializeField] private Vector2 gridStep;
        
        public Vector2 GridStep => gridStep;
        
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            
            for(int y = 0; y < gridSize.y; y++)
                for (int x = 0; x < gridSize.x; x++)
            {
                Gizmos.DrawSphere(new Vector3(
                    transform.position.x + x * gridStep.x, 
                    transform.position.y + y * gridStep.y, 0), 
                    gizmoPointSize);
            }
        }
    }
}