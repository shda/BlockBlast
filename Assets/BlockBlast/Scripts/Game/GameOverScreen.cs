using System;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class GameOverScreen : MonoBehaviour
    {
        public Action OnRestartGame;
        
        public void OnPressRestart()
        {
            OnRestartGame?.Invoke();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}