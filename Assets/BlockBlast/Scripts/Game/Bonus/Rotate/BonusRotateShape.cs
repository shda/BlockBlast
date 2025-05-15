using TriInspector;
using UnityEngine;

namespace BlockBlast.Scripts.Game.Bonus.Rotate
{
    public class BonusRotateShape : BonusBase
    {
        [SerializeField] private BonusRotateUi  bonusRotateUi;
        
        [Button]
        public override void StartBonus()
        {
            bonusRotateUi.ShowBonusUi();
            bonusRotateUi.OnComplete = () =>
            {
                OnComplete?.Invoke();
            };
        }

        [Button]
        public override void StopBonus()
        {
            bonusRotateUi.HideBonusUi();
        }
    }
}