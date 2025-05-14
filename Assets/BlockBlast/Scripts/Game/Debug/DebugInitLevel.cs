using TriInspector;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class DebugInitLevel : MonoBehaviour
    {
        [SerializeField] private IntGrid grid;
        [SerializeField] private Game game;

        [SerializeField] private Shape[] setShapes;

        [Button]
        public void InitGame()
        {
            game.DebugInitLevel(grid);
            if (setShapes.Length > 0)
            {
                game.DebugInitShapes(setShapes);
            }
        }
    }
}