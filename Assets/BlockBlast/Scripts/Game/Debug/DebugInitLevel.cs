using System;
using TriInspector;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class DebugInitLevel : MonoBehaviour
    {
        [SerializeField] private IntGrid grid;
        [SerializeField] private Game game;

        [SerializeField] private Shape[] setShapes;

        [SerializeField] private bool craeateByStart;


        private void Start()
        {
            if (craeateByStart)
            {
                InitGame();
            }
        }

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