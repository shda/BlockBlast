using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class ShapeCollection : MonoBehaviour
    {
        [SerializeField] private Shape[] shapePrefabs;

        public Shape CrateRandomShape()
        {
            return Instantiate(shapePrefabs[Random.Range(0, shapePrefabs.Length)]);
        }

        public void RemoveShape(Shape shape)
        {
            
        }
    }
}