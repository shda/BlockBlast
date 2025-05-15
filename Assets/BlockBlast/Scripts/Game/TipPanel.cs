using UnityEngine;

namespace BlockBlast.Scripts.Game
{
    public class TipPanel : MonoBehaviour
    {
        [SerializeField] private Transform rootPanel;
        [SerializeField] private float timeDelayBeforeShow = 0;

        private bool _isStartTimer;
        private float _currentTime;
        
        public void OnEnterTrigger()
        {
            _isStartTimer = true;
        }

        public void OnExitTrigger()
        {
            _isStartTimer = false;
        }

        private void Update()
        {
            if (_isStartTimer)
            {
                _currentTime -= Time.deltaTime;
                rootPanel.gameObject.SetActive(_currentTime <= 0);
            }
            else
            {
                _currentTime = timeDelayBeforeShow;
                rootPanel.gameObject.SetActive(false);
            }
            
            _currentTime -= Time.deltaTime;
        }
    }
}