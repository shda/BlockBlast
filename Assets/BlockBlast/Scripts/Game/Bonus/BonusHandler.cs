using System.Linq;
using BlockBlast.Scripts.Game.Bonus.Rotate;
using UnityEngine;
using UnityEngine.UI;

namespace BlockBlast.Scripts.Game.Bonus
{
    public class BonusHandler : MonoBehaviour
    {
        [SerializeField] private BonusCountData bonusCountData;
        [SerializeField] private Toggle[] togges;

        private BonusBase _activeBonus;

        public void OnPressBonus(BonusBase bonusBase)
        {
            if (_activeBonus)
            {
                _activeBonus.StopBonus();
            }
            
            if (!togges.Any(t => t.isOn))
            {
                foreach (var toggle in togges)
                {
                    toggle.SetIsOnWithoutNotify(false);
                }

                _activeBonus = null;
                return;
            }
            
            if (!bonusBase.IsCanUsingBonus())
            {
                foreach (var toggle in togges)
                {
                    toggle.SetIsOnWithoutNotify(false);
                }
                return;
            }
            
            _activeBonus = bonusBase;
            _activeBonus.StartBonus();
            
            _activeBonus.OnComplete = () =>
            {
                foreach (var toggle in togges)
                {
                    toggle.SetIsOnWithoutNotify(false);
                }
            };
        }

        public void FullLineRemove(int lines)
        {
            if(lines == 0)
                return;

            if (lines == 2)
            {
                bonusCountData.CountRotateBonus.Value++;
            }
            
            if (lines == 3)
            {
                bonusCountData.CountDestroyBonus.Value++;
            }
            
            if (lines > 3)
            {
                bonusCountData.CountChangeShapeBonus.Value++;
            }
        }
    }
}