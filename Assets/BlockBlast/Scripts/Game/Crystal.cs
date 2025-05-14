using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class Crystal : MonoBehaviour
    {
        public Vector2Int Coordinates { get; set; }
        
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] sprites;
        
        public int ColorIndex { get; private set; }

        public void SetSprite(int colorIndex)
        {
            ColorIndex = colorIndex;
            spriteRenderer.sprite = sprites[colorIndex];
        }

        public int GetSpriteIndex()
        {
            return ColorIndex;
        }
    }
}