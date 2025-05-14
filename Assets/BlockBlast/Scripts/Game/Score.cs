using TMPro;
using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class Score : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI bestScoreText;

        private int _score;
        private int _bestScore;

        private void Awake()
        {
            _bestScore = PlayerPrefs.GetInt("BestScore" , 0);
            UpdateScore();
        }

        public void ResetScore()
        {
            _score = 0;
            UpdateScore();
        }
        public void AddScore(int addScore)
        {
            _score += addScore;
            UpdateScore();
        }

        public void UpdateBestScore()
        {
            if (_score > _bestScore)
            {
                _bestScore = _score;
                UpdateScore();
                PlayerPrefs.SetInt("BestScore", _bestScore);
            }
        }

        private void UpdateScore()
        {
            scoreText.text = _score.ToString();
            bestScoreText.text = _bestScore.ToString();
        }
    }
}