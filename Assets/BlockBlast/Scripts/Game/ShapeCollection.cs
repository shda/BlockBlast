using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class ShapeCollection : MonoBehaviour
    {
        [SerializeField] private Shape[] shapePrefabs;

        public Shape CreateRandomShape()
        {
            return Instantiate(shapePrefabs[Random.Range(0, shapePrefabs.Length)]);
        }
    }
}