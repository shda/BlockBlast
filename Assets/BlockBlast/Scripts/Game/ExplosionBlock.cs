using System;
using BlockBlast.Scripts.Common;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class ExplosionBlock : MonoBehaviour
    {
        [SerializeField] private CompleteAnimation verticalAnimation;
        [SerializeField] private CompleteAnimation horizontalAnimation;

        [SerializeField] private SpriteRenderer verticalSpriteRenderer;
        [SerializeField] private SpriteRenderer horizontalSpriteRenderer;
        
        [SerializeField] private Color[] colors;
        
        public Action OnComplete;
        
        private void Awake()
        {
            verticalAnimation.OnCompleteAnimation = OnCompleteAnimation;
            horizontalAnimation.OnCompleteAnimation = OnCompleteAnimation;
        }


        public void PlayVerticalAnimation()
        {
            horizontalAnimation.gameObject.SetActive(false);
            verticalAnimation.gameObject.SetActive(true);
        }
        
        public void PlayHorizontalAnimation()
        {
            horizontalAnimation.gameObject.SetActive(true);
            verticalAnimation.gameObject.SetActive(false);
        }

        private void OnCompleteAnimation()
        {
            OnComplete?.Invoke();
            gameObject.SetActive(false);
        }

        public void SetColor(int colorIndex)
        {
            verticalSpriteRenderer.color = colors[colorIndex];
            horizontalSpriteRenderer.color = colors[colorIndex];
        }
    }
}